using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Kinect;
using System.Linq;

namespace KinectWallboard
{
    public partial class Main : Form//, IRequestHandler
    {
        private readonly WebView webView;

        private KinectSensor _kinectSensor;

        private Skeleton _lastClosestSkeleton;

        private RoundRobinPosition position;
        private WebPage _currentWebPage;
        private string _jqueryUrl;
        private bool _waitingForTimer = true;
        private bool _debugMode = false;
        private GestureController _gestureController = new GestureController();
        private string _configFilename = "WebPageConfig.xml";

        private UpdateData _updateData;
        private Thread _loopThread;

        private string _injectJQuery = 
            "(function(d, script) {{ " +
                "script = d.createElement('script'); " +
                "script.type = 'text/javascript'; " +
                "script.async = true; " +
                "script.src = '{0}'; " +
                "script.onload = function() {{ $=jQuery; {1} }}; " +
                "d.getElementsByTagName('head')[0].appendChild(script); " +
            "}}(document));";

        private List<WebPage> _addressList;

        private Direction _skipDirection = Direction.None;

        public Main(bool debugMode, bool startFullscreen)
        {
            _debugMode = debugMode;
            InitializeComponent();

            if (startFullscreen)
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
                Cursor.Position = new Point(0, 0);
            }

            if (!debugMode)
            {
                tableLayoutPanel.ColumnCount = 1;                
                tableLayoutPanel.Controls.Remove(panel1);
            }

            _jqueryUrl = ConfigurationManager.AppSettings["JQueryUrl"];

            LoadWebPageConfig();
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            if (path != null)
            {
                var fileSystemWatch = new FileSystemWatcher(path, "*" + _configFilename + "*");
                fileSystemWatch.Changed += (sender, args) =>
                    {
                        if (args.ChangeType == WatcherChangeTypes.Changed)
                            LoadWebPageConfig();
                    };
                fileSystemWatch.EnableRaisingEvents = true;
            }

            //**** setup Chrome
            /*var settings = new CefSharp.Settings()
                {
                    CachePath = @".\cache"
                };
            
            if (!CEF.Initialize(settings))
                throw new ApplicationException("Failed to initialize CEF");
            
            webView = new WebView()
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0)
            };

            webView.PropertyChanged += WebViewPropertyChanged;
            webView.LoadCompleted += WebViewLoadCompleted;
            if (debugMode)
            {
                webView.ConsoleMessage += WebViewOnConsoleMessage;
            }

            tableLayoutPanel.Controls.Add(webView, 0, 0);     
       
            _updateData = new UpdateData();

            //**** setup kinect
            foreach (var kinectSensor in KinectSensor.KinectSensors)
            {
                if (kinectSensor.Status == KinectStatus.Connected)
                {
                    _kinectSensor = kinectSensor;
                    break;
                }
            }

            if (_kinectSensor != null)
            {
                _kinectSensor.SkeletonStream.Enable();
                _kinectSensor.SkeletonFrameReady += KinectSensorOnSkeletonFrameReady;

                // setup gestures
                _gestureController.AddGesture(GestureType.SwipeRight, new IGestureSegment[2]
                    {
                        new SwipeRightStartGesture(), 
                        new SwipeRightEndGesture()
                    });

                _gestureController.AddGesture(GestureType.SwipeLeft, new IGestureSegment[2]
                    {
                        new SwipeLeftStartGesture(), 
                        new SwipeLeftEndGesture()
                    });

                _gestureController.GestureRecognized += GestureControllerOnGestureRecognized;

                if (_debugMode)
                {
                    _kinectSensor.ColorStream.Enable();
                    _kinectSensor.ColorFrameReady += KinectSensorOnColorFrameReady;
                }

                try
                {
                    _kinectSensor.Start();
                    lblKinectStatus.Text = "Found Kinect";
                }
                catch (IOException)
                {
                    _kinectSensor = null;
                }
            }*/
        }

        private void WebViewOnConsoleMessage(object sender, ConsoleMessageEventArgs consoleMessageEventArgs)
        {
            Invoke((MethodInvoker) delegate
                {
                    txtConsole.AppendText(consoleMessageEventArgs.Message + Environment.NewLine);
                });
        }

        private Thread _notificationThread;

        private void OpenNotification(string message, int duration)
        {
            if (_notificationThread != null && _notificationThread.ThreadState != ThreadState.Stopped && lblNotification.Text == message)
                return;

            if (_notificationThread != null)
            {
                _notificationThread.Abort();
                _notificationThread = null;
            }

            if (IsHandleCreated)
            {
                Invoke((MethodInvoker) delegate
                    {
                        pnlNotification.Visible = true;
                        pnlNotification.Left = (ClientSize.Width/2) - (pnlNotification.Width/2);
                        pnlNotification.Top = ClientSize.Height - pnlNotification.Height;
                        pnlNotification.BringToFront();
                        lblNotification.Text = message;
                    });
            }
            else
            {
                pnlNotification.Visible = true;
                pnlNotification.Left = (ClientSize.Width / 2) - (pnlNotification.Width / 2);
                pnlNotification.Top = ClientSize.Height - pnlNotification.Height;
                pnlNotification.BringToFront();
                lblNotification.Text = message;
            }

            _notificationThread = new Thread(delegate()
                {
                    Thread.Sleep(duration);
                    if (IsHandleCreated)
                    {
                        Invoke((MethodInvoker) delegate
                            {
                                pnlNotification.Visible = false;
                            });
                    }
                    else
                    {
                        pnlNotification.Visible = false;
                    }
                });
            _notificationThread.Start();
        }

        private void LoadWebPageConfig()
        {
            try
            {
                var serializer = new XmlSerializer(typeof (List<WebPage>));

                using (FileStream stream = File.OpenRead(_configFilename))
                {

                    var addresslist = (List<WebPage>) serializer.Deserialize(stream);

                    if (addresslist != null)
                    {
                        _addressList = addresslist;
                        position = new RoundRobinPosition(_addressList.Count);
                        int pos = position.Next();
                        _currentWebPage = _addressList[pos];

                        OpenNotification("New config loaded", 2000);
                    }
                }
            }
            catch (Exception e)
            {
                OpenNotification("Failed to load config!", 5000);
            }
        }

        private void GestureControllerOnGestureRecognized(object sender, GestureRecognizedEvent e)
        {
            switch (e.Type)
            {
                case GestureType.SwipeLeft:
                    if (_skipDirection == Direction.None || _skipDirection == Direction.Hold)
                        _skipDirection = Direction.Previous;
                    break;
                case GestureType.SwipeRight:
                    if (_skipDirection == Direction.None || _skipDirection == Direction.Hold)
                        _skipDirection = Direction.Next;
                    break;
            }
        }

        #region Browser crap
        private void WebViewLoadCompleted(object sender, LoadCompletedEventArgs url)
        {
            Console.WriteLine(url.Url);

            if (url.Url != "about:blank")
            {
                if (!String.IsNullOrWhiteSpace(_currentWebPage.JavaScript))
                {
                    if (_currentWebPage.InjectJQuery)
                    {
                        webView.ExecuteScript(String.Format(_injectJQuery, _jqueryUrl, _currentWebPage.JavaScript));
                    }
                    else
                    {
                        webView.ExecuteScript(_currentWebPage.JavaScript);
                    }

                    if (_currentWebPage.AllowUpdates)
                    {
                        webView.ExecuteScript(String.Format("updateData({0});", _updateData.NumberOfSkeletons));
                    }
                }

                _waitingForTimer = false;
            }
        }

        private void WebViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsBrowserInitialized")
            {
                if (_loopThread == null && _addressList != null && _addressList.Count > 0)
                {
                    _loopThread = new Thread(StartLoop);
                    _loopThread.Start();
                }
            }
        }

        private void StartLoop()
        {
            while (true)
            {
                Invoke((MethodInvoker) delegate
                    {
                        lblState.Text = "Loading...";
                        txtAddress.Text = _currentWebPage.Address;
                    });

                webView.Load(_currentWebPage.Address);
                _waitingForTimer = true;
                int timeout = _currentWebPage.Timeout*1000;

                while (_waitingForTimer)
                {
                    Thread.Sleep(10);
                }

                Invoke((MethodInvoker) delegate
                    {
                        lblState.Text = String.Format("Loaded, starting timer {0}s", timeout/1000);
                    });

                while (timeout > 0 && (_skipDirection == Direction.None || _skipDirection == Direction.Hold))
                {
                    // if on pause the timer and wait
                    while (_skipDirection == Direction.Hold)
                    {
                        Thread.Sleep(10);
                    }

                    Thread.Sleep(10);
                    timeout -= 10;

                    Invoke((MethodInvoker) delegate
                        {
                            lblNextPageTimer.Text = String.Format("{0}s", timeout/1000);
                        });
                }

                int pos = _skipDirection == Direction.Previous ? position.Previous() : position.Next();
                _currentWebPage = _addressList[pos];
                _skipDirection = Direction.None;
            }
        }


        /*public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            throw new NotImplementedException();
        }

        public bool GetDownloadHandler(IWebBrowser browser, string mimeType, string fileName, long contentLength, ref IDownloadHandler handler)
        {
            return false;
        }

        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, NavigationType naigationvType, bool isRedirect)
        {
            var authDigest = Convert.ToBase64String(Encoding.Default.GetBytes(String.Format("{0}:{1}", _currentWebPage.Username, _currentWebPage.Password)));
            var headers = request.GetHeaders();
            headers["Authorization"] = "Basic " + authDigest;
            request.SetHeaders(headers);

            return false;
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
        {
            return false;
        }

        public void OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType, System.Net.WebHeaderCollection headers)
        {
            
        }*/

        #endregion

        #region Kinect crap
        private void KinectSensorOnColorFrameReady(object sender, ColorImageFrameReadyEventArgs colorImageFrameReadyEventArgs)
        {
            using (var frame = colorImageFrameReadyEventArgs.OpenColorImageFrame())
            {
                if (frame != null && frame.Format == ColorImageFormat.RgbResolution640x480Fps30)
                {
                    Bitmap bmp = new Bitmap(ImageToBitmap(frame));
                    
                        if (_lastClosestSkeleton != null)
                        {
                            DrawSkeleton(_lastClosestSkeleton, bmp);
                        }

                        pictureBox.Image = bmp;
                    
                }
            }
        }

        private void DrawSkeleton(Skeleton skeleton, Bitmap bmp)
        {
            using (Pen orange = new Pen(Color.Orange, 5))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                DrawBone(skeleton, JointType.HandLeft, JointType.WristLeft, g, orange);
                DrawBone(skeleton, JointType.WristLeft, JointType.ElbowLeft, g, orange);
                DrawBone(skeleton, JointType.ElbowLeft, JointType.ShoulderLeft, g, orange);
                DrawBone(skeleton, JointType.ShoulderLeft, JointType.ShoulderCenter, g, orange);

                DrawBone(skeleton, JointType.HandRight,JointType.WristRight, g, orange);
                DrawBone(skeleton, JointType.WristRight, JointType.ElbowRight, g, orange);
                DrawBone(skeleton, JointType.ElbowRight, JointType.ShoulderRight, g, orange);
                DrawBone(skeleton, JointType.ShoulderRight, JointType.ShoulderCenter, g, orange);

                DrawBone(skeleton, JointType.ShoulderCenter, JointType.Head, g, orange);
                DrawBone(skeleton, JointType.ShoulderCenter, JointType.Spine, g, orange);
                DrawBone(skeleton, JointType.Spine, JointType.HipCenter, g, orange);

                DrawBone(skeleton, JointType.FootLeft, JointType.AnkleLeft, g, orange);
                DrawBone(skeleton, JointType.AnkleLeft, JointType.KneeLeft, g, orange);
                DrawBone(skeleton, JointType.KneeLeft, JointType.HipLeft, g, orange);
                DrawBone(skeleton, JointType.HipLeft, JointType.HipCenter, g, orange);

                DrawBone(skeleton, JointType.FootRight, JointType.AnkleRight, g, orange);
                DrawBone(skeleton, JointType.AnkleRight, JointType.KneeRight, g, orange);
                DrawBone(skeleton, JointType.KneeRight, JointType.HipRight, g, orange);
                DrawBone(skeleton, JointType.HipRight, JointType.HipCenter, g, orange);

                foreach (Joint joint in skeleton.Joints)
                {
                    var p = SkeletonPointToScreen(joint.Position);
                    g.FillRectangle(Brushes.Yellow, p.X, p.Y, 10, 10);
                }
            }
        }

        private void DrawBone(Skeleton skeleton, JointType j1, JointType j2, Graphics g, Pen orange)
        {
            if (skeleton.Joints[j1].TrackingState == JointTrackingState.NotTracked
                || skeleton.Joints[j1].TrackingState == JointTrackingState.Inferred
                || skeleton.Joints[j2].TrackingState == JointTrackingState.NotTracked
                || skeleton.Joints[j2].TrackingState == JointTrackingState.Inferred)
                return;

            var p1 = SkeletonPointToScreen(skeleton.Joints[j1].Position);
            var p2 = SkeletonPointToScreen(skeleton.Joints[j2].Position);
            g.DrawLine(orange, p1, p2);
        }

        private Bitmap ImageToBitmap(ColorImageFrame image)
        {
            byte[] pixeldata = new byte[image.PixelDataLength];
            image.CopyPixelDataTo(pixeldata);
            Bitmap bmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppRgb);
            BitmapData bmapdata = bmap.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.WriteOnly,
                bmap.PixelFormat);
            IntPtr ptr = bmapdata.Scan0;
            Marshal.Copy(pixeldata, 0, ptr, image.PixelDataLength);
            bmap.UnlockBits(bmapdata);
            return bmap;
        }

        private void KinectSensorOnSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs skeletonFrameReadyEventArgs)
        {
            var skeletons = new Skeleton[0];

            using (var frame = skeletonFrameReadyEventArgs.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    skeletons = new Skeleton[frame.SkeletonArrayLength];
                    frame.CopySkeletonDataTo(skeletons);
                }
            }

            if (skeletons.Length == 0)
                return;

            var closestSkeleton = skeletons.Where(s =>
                                                    s.TrackingState == SkeletonTrackingState.Tracked &&
                                                    s.Joints[JointType.Head].TrackingState == JointTrackingState.Tracked &&
                                                    (s.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked ||
                                                    s.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked)
                ).OrderBy(s => s.Joints[JointType.Head].Position.Z).FirstOrDefault();

            _lastClosestSkeleton = closestSkeleton;

            if (closestSkeleton != null)
            {
                _gestureController.UpdateAllGestures(closestSkeleton);
                
                if ( _skipDirection == Direction.None)
                    _skipDirection = Direction.Hold;

                OpenNotification("Hello!", 4000);
            }
            else
            {
                _skipDirection = Direction.None;
            }

            int numberOfSkeletons = skeletons.Count(s => s.TrackingState == SkeletonTrackingState.Tracked || s.TrackingState == SkeletonTrackingState.PositionOnly);

            if (_updateData.NumberOfSkeletons != numberOfSkeletons)
            {
                _updateData.NumberOfSkeletons = numberOfSkeletons;
                if (_currentWebPage.AllowUpdates)
                {
                    webView.ExecuteScript(String.Format("updateData({0});", _updateData.NumberOfSkeletons));
                }
            }

            if (IsHandleCreated)
            {
                var message = (closestSkeleton == null) ? "No gesture skeleton" : "Found gesture skeleton";

                Invoke((MethodInvoker) delegate
                    {
                        lblGestureSkeletonState.Text = message;
                        lblNumSkeletons.Text = _updateData.NumberOfSkeletons.ToString();
                    });
            }
        }

        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            DepthImagePoint depthPoint = _kinectSensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        #endregion

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_loopThread != null && _loopThread.IsAlive)
                _loopThread.Abort();

            if (_kinectSensor != null)
                _kinectSensor.Stop();

            if (_debugMode && webView != null)
            {
                webView.ConsoleMessage -= WebViewOnConsoleMessage;
            }

            CEF.Shutdown();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_skipDirection == Direction.None || _skipDirection == Direction.Hold)
                _skipDirection = Direction.Next;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (_skipDirection == Direction.None || _skipDirection == Direction.Hold)
                _skipDirection = Direction.Previous;
        }
    }

    public class UpdateData
    {
        public int NumberOfSkeletons { get; set; }
    }

    public enum Direction
    {
        None,
        Previous,
        Hold,
        Next
    }
    
    public class RoundRobinPosition
    {
        private int _position = -1;
        private readonly int _length = 0;

        public RoundRobinPosition(int lenght)
        {
            _length = lenght;
        }

        public int Next()
        {
            _position = _position == _length - 1 ? 0 : _position + 1;
            return _position;
        }

        public int Previous()
        {
            _position = _position == 0 ? _length - 1 : _position - 1;
            return _position;
        }
    }
}

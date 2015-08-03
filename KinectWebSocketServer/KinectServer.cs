using Fleck;
using KinectWallboard.Gesture;
using KinectWallboard.Gesture.Gestures;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KinectWebSocketServer
{
    public class KinectServer
    {
        private List<IWebSocketConnection> _allSockets = new List<IWebSocketConnection>();
        private WebSocketServer _server;
        private KinectSensor _kinectSensor;
        private GestureController _gestureController;

        public KinectServer()
        {
            _server = new WebSocketServer("ws://localhost:8181");

            _server.Start(socket =>
            {
                socket.OnOpen = () => _allSockets.Add(socket);
                socket.OnClose = () => _allSockets.Remove(socket);
            });

            foreach (var kinectSensor in KinectSensor.KinectSensors)
            {
                if (kinectSensor.Status == KinectStatus.Connected)
                {
                    _kinectSensor = kinectSensor;
                    break;
                }
            }

            if (_kinectSensor == null)
                throw new Exception("Failed to initialized Kinect sensor");

            _kinectSensor.ColorStream.Enable();
            _kinectSensor.ColorFrameReady += KinectSensorOnColorFrameReady;

            _kinectSensor.SkeletonStream.Enable();
            _kinectSensor.SkeletonFrameReady += KinectSensorOnSkeletonFrameReady;

            // setup gestures
             _gestureController = new GestureController();
            _gestureController.AddGesture(new SwipeLeftGesture());
            _gestureController.AddGesture(new SwipeRightGesture());
            _gestureController.GestureRecognized += GestureControllerOnGestureRecognized;
        }

        private void GestureControllerOnGestureRecognized(object sender, GestureRecognizedEvent e)
        {
            throw new NotImplementedException();
        }

        private void KinectSensorOnSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            var skeletons = new Skeleton[0];

            using (var frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    skeletons = new Skeleton[frame.SkeletonArrayLength];
                    frame.CopySkeletonDataTo(skeletons);
                }
            }

            if (skeletons.Length == 0)
                return;

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
        }

        public void Start()
        {
            _kinectSensor.Start();
        }

        public void Stop()
        {
            foreach (var socket in _allSockets)
            {
                socket.Close();
            }
        }

        private void KinectSensorOnColorFrameReady(object sender, ColorImageFrameReadyEventArgs colorImageFrameReadyEventArgs)
        {
            using (var frame = colorImageFrameReadyEventArgs.OpenColorImageFrame())
            {
                if (frame != null && frame.Format == ColorImageFormat.RgbResolution640x480Fps30)
                {
                    Bitmap bmp = new Bitmap(ImageToBitmap(frame));

                    foreach (var socket in _allSockets)
                    {
                        if (socket.IsAvailable)
                            socket.Send(BitmapToMessage(bmp));                        
                    }
                }
            }
        }

        /*private string BitmapToBase64String(Bitmap bmp)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Png);
                return Convert.ToBase64String(stream.ToArray());
            }
        }*/

        private byte[] BitmapToMessage(Bitmap bmp)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
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

            return (new Bitmap(bmap, 320,240));
        }
    }
}

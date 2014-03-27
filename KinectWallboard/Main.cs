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

namespace KinectWallboard
{
    public partial class Main : Form
    {
        private readonly WebView webView;
        private const int cycleTimeout = 10;

        private int position = 0;
        private string _jqueryUrl;
        private bool _waitingForTimer = true;

        private Thread _loopThread;

        private string _injectJQuery = 
            "(function(d, script) {{ " +
                "script = d.createElement('script'); " +
                "script.type = 'text/javascript'; " +
                "script.async = true; " +
                /*"script.onload = function(){{ " +
                " {0} " +
                "}}; " +*/
                "script.src = '{0}'; " +
                "d.getElementsByTagName('head')[0].appendChild(script); " +
            "}}(document));";


        private List<WebPage> addressList = new List<WebPage>
        {
            new WebPage()
            {
                Address = "https://www.google.nl",
                OnReady = new JavaScriptAction() {
                    InjectJQuery = true,
                    JavaScript = "alert($('hplogo'))"
                },
                Timeout = 5
            },
            new WebPage()
            {
                Address = "http://www.funda.nl",
                OnReady = new JavaScriptAction() {
                    JavaScript = "alert($('div.box-search h1').html())"
                },
                Timeout = 10
            }
        };

        public Main()
        {
            InitializeComponent();
            _jqueryUrl = ConfigurationManager.AppSettings["JQueryUrl"];
            
            var settings = new CefSharp.Settings();

            if (!CEF.Initialize(settings))
                throw new ApplicationException("Failed to initialize CEF");
            
            webView = new WebView()
            {
                Dock = DockStyle.Fill
            };

            //webView.Address = addressList[position].Address;            

            webView.PropertyChanged += webView_PropertyChanged;
            webView.LoadCompleted += webView_LoadCompleted;

            tableLayoutPanel.Controls.Add(webView, 0, 0);            
        }
        
        private void webView_LoadCompleted(object sender, LoadCompletedEventArgs url)
        {
            Console.WriteLine(url.Url);

            if (url.Url != "about:blank")
            {
                if (addressList[position].OnReady != null)
                {
                    if (addressList[position].OnReady.InjectJQuery)
                    {
                        webView.EvaluateScript(String.Format(_injectJQuery, _jqueryUrl));
                        webView.ExecuteScript(addressList[position].OnReady.JavaScript);
                    }
                    else
                    {
                        webView.ExecuteScript(addressList[position].OnReady.JavaScript);
                    }
                }

                position = (position == addressList.Count - 1) ? 0 : position + 1;
                _waitingForTimer = false;
            }
        }

        private void webView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsBrowserInitialized")
            {
                if (_loopThread == null)
                {
                    _loopThread = new Thread(new ThreadStart(StartLoop));
                    _loopThread.Start();
                }
            }
        }

        private void StartLoop()
        {
            while (true)
            {
                lblState.Text = "Loading...";

                webView.Load(addressList[position].Address);
                _waitingForTimer = true;

                while (_waitingForTimer) { Thread.Sleep(10); }

                int timeout = addressList[position].Timeout * 1000;
                lblState.Text = String.Format("Loaded, starting timer {0}s", timeout);

                Thread.Sleep(timeout);
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_loopThread != null && _loopThread.IsAlive)
                _loopThread.Abort();
        }

    }
}

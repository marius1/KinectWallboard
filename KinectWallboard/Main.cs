using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KinectWallboard
{
    public partial class Main : Form
    {
        private readonly WebView webView;
        private const int cycleTimeout = 10;

        private int position = 0;

        private List<WebPage> addressList = new List<WebPage>
        {
            new WebPage()
            {
                Address = "http://www.google.com"
            },
            new WebPage()
            {
                Address = "http://funda.nl"
            }
        };

        public Main()
        {
            InitializeComponent();

            cycleTimer.Interval = cycleTimeout * 1000;

            var settings = new CefSharp.Settings()
            {
                
            };

            if (CEF.Initialize(settings))
            {

                webView = new WebView()
                {
                    Dock = DockStyle.Fill
                };

                webView.Address = addressList[position].Address;
                position++;

                webView.PropertyChanged += webView_PropertyChanged;

                Controls.Add(webView);
            }
        }

        void webView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsBrowserInitialized")
            {
                cycleTimer.Start();
            }
        }

        void CycleTimerTick(object sender, EventArgs e)
        {            
            webView.Load(addressList[position].Address);
            
            position = (position == addressList.Count - 1) ? 0 : position + 1;
        }
    }
}

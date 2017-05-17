using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ttpim.gamemodule.games.models.lotto
{
    public class WebProcessingData
    {
        private object _obj;
        private WebBrowserDocumentCompletedEventArgs _args;
        private WebBrowser _browser;

        public WebProcessingData(object Sender, WebBrowserDocumentCompletedEventArgs Args, WebBrowser Browser)
        {
            _obj = Sender;
            _args = Args;
            _browser = Browser;
        }

        public object Sender
        {
            get
            {
                return _obj;
            }
            set
            {
                _obj = value;
            }
        }

        public WebBrowserDocumentCompletedEventArgs Arguments
        {
            get
            {
                return _args;
            }
            set
            {
                _args = value;
            }
        }

        public WebBrowser Browser
        {
            get
            {
                return _browser;
            }
            set
            {
                _browser = value;
            }
        }
    }
}

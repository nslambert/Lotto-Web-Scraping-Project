using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ttpim.gamemodule.games.models.lotto
{
    public class WebPageSnapshotData
    {
        private string _uri;
        private string _dataName;

        public WebPageSnapshotData(string uri, string name)
        {
            _uri = uri;
            _dataName = name;
        }

        public string Uri
        {
            get
            {
                return _uri;
            }
            set
            {
                _uri = value;
            }
        }

        public string DataName
        {
            get
            {
                return _dataName;
            }
            set
            {
                _dataName = value;
            }
        }
    }
}

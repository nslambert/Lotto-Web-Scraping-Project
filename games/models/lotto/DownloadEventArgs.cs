using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.lotto.models;

namespace ttpim.gamemodule.games.models.lotto
{
    public class DownloadEventArgs : EventArgs
    {
        private HTMLParsedResults _results;

        public DownloadEventArgs()
        {
        }

        public DownloadEventArgs(HTMLParsedResults Results)
        {
            _results = Results;
        }

        public HTMLParsedResults Results
        {
            get 
            {
                return _results; 
            }
            set 
            {
                _results = value; 
            }
        }
    }
}

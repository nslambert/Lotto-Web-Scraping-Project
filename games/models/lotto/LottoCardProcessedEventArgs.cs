using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ttpim.gamemodule.games.models.lotto
{
    public class LottoCardProcessedEventArgs : EventArgs
    {
        private LottoGameResults _results;

        public LottoCardProcessedEventArgs()
        {
        }

        public LottoCardProcessedEventArgs(LottoGameResults Results)
        {
            _results = Results;
        }

        public LottoGameResults Results
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

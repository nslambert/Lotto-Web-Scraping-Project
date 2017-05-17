using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ttpim.gamemodule.database;

namespace ttpim.gamemodule.games.models.lotto
{
    public class LottoWinEventArgs : EventArgs
    {
        private Win _winning;
        
        public LottoWinEventArgs(Win Winning)
        {
            _winning = Winning;
        }

        public Win Winning
        {
            get
            {
                return _winning;
            }
            set
            {
                _winning = value;
            }
        }
    }
}

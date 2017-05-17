using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ttpim.gamemodule.games.models.lotto
{
    public class LottoCardSavedWinningsEventArgs : EventArgs
    {
        private int _numAffectedRecords;
        private bool _saved;
        private string _message;
        private string _game;

        public int NumOfAffectedRecords
        {
            get
            {
                return _numAffectedRecords;
            }
            set
            {
                _numAffectedRecords = value;
            }
        }

        public bool Saved
        {
            get
            {
                return _saved;
            }
            set
            {
                _saved = value;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        public string Game
        {
            get
            {
                return _game;
            }
            set
            {
                _game = value;
            }
        }
    }
}

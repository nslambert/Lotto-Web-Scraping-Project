using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ttpim.gamemodule.lotto.models
{
    public class GameBoard
    {
        private int _boardId;
        private int _boardnr;
        private int _subscriptionId;
        private string _subscriptionNr;
        private string _subscriptionType;
        private Boolean _active;
        private int[] _numbers;

        public GameBoard(int BoardSize)
        {
            _numbers = new int[BoardSize];
        }


        public int BoardId
        {
            get { return _boardId; }
            set { _boardId = value; }
        }

        public int BoardNumber
        {
            get { return _boardnr; }
            set { _boardnr = value; }
        }

        public int[] Numbers
        {
            get { return _numbers; }
            set { _numbers = value; }
        }

        public int SubscriptionId
        {
            get { return _subscriptionId; }
            set { _subscriptionId = value; }
        }

        public string SubscriptionNr
        {
            get { return _subscriptionNr; }
            set { _subscriptionNr = value; }
        }

        public string SubscriptionType
        {
            get { return _subscriptionType; }
            set { _subscriptionType = value; }
        }

        public Boolean Active
        {
            get { return _active; }
            set { _active = value; }
        }
    }
}

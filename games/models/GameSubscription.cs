using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.lotto.models;
using ttpim.gamemodule.games.models.lotto;

namespace ttpim.gamemodule.games.models
{
    public class GameSubscription
    {
        private string _subscriptionNr;
        private int _subscriptionId;
        private string _subscriptionType;
        private Boolean _active;
        private DateTime _startDate;
        private DateTime _endDate;
        private List<GameBoard> _boards;

        public GameSubscription()
        {
            _boards = new List<GameBoard>();
        }

        public int SubscriptionId
        {
            get { return _subscriptionId; }
            set { _subscriptionId = value; }
        }

        public string SubscriptionNumber
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

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public List<GameBoard> Boards
        {
            get { return _boards; }
            set { _boards = value; }
        }

    }
}

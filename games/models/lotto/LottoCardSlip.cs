using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ttpim.gamemodule.lotto.models
{
    public class LottoCardSlip
    {
        private DateTime  _gameDate;
        private ArrayList _numbers;
        private int _bonus;
        private Boolean   _includeLottoPlus;

        public LottoCardSlip()
        {
            _numbers = new ArrayList(6);
            _bonus = 0;
        }

        public LottoCardSlip(DateTime GameDate)
        {
            _gameDate = GameDate;
            _numbers = new ArrayList(6);
            _bonus = 0;
        }

        public DateTime GameDate
        {
            get { return _gameDate; }
            set { _gameDate = value; }
        }

        public ArrayList Numbers
        {
            get { return _numbers; }
            set { _numbers = value; }
        }

        public int Bonus
        {
            get { return _bonus; }
            set { _bonus = value; }
        }

        public Boolean IncludeLottoPlus
        {
            get { return _includeLottoPlus; }
            set { _includeLottoPlus = value; }
        }

    }
}

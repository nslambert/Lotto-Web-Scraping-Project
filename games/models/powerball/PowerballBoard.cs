using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ttpim.gamemodule.lotto.models.powerball
{
    public class PowerballBoard
    {
        private string cardname;
        private ArrayList numbers;
        private int bonus;

        public string CardName
        {
            get
            {
                return cardname;
            }
            set
            {
                cardname = value;
            }
        }

        public ArrayList PowerballNuimbers
        {
            get
            {
                return numbers;
            }
            set
            {
                numbers = value;
            }
        }

        public int Bonus
        {
            get
            {
                return bonus;
            }
            set
            {
                bonus = value;
            }
        }
    }
}

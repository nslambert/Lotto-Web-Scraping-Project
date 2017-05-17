using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ttpim.gamemodule.games.models.lotto
{
    public class LottoNumbers
    {
        private int bonus;
        private int[] numbers;

        public LottoNumbers()
        {
            numbers = new Int32[6];
        }

        public int Bonus
        {
            get { return bonus; }
            set { bonus = value; }
        }

        public int[] Numbers
        {
            get { return numbers; }
            set { numbers = value; }
        }
    }
}

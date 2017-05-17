using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ttpim.gamemodule.games.models.lotto
{
    public static class LottoMachine
    {
        private static LottoGameResults _results;
        private static Boolean _validLottoCard;

        private static Boolean IsValidLottoCard
        {
            get { return _validLottoCard; }
            set { _validLottoCard = value; }
        }


        public static void EnterDraw(LottoCard GameCard)
        {
            _results = null;
            ValidateLottoCard(GameCard);
            if (IsValidLottoCard)
            {
                _results = LottoEngine.ProcessLottoCard(GameCard);
            }
        }
        private static void ValidateLottoCard(LottoCard GameCard)
        {
        }
    }
}

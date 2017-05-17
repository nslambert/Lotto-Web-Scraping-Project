using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ttpim.gamemodule.games.models.lotto
{
    public struct LottoGameBoardSummary
    {
        private int _numLottoWins;
        private int _numLottoPlusWins;
        private int _numWinBoards;
        private string _message;

        public int NumLottoWins
        {
            get 
            { 
                return _numLottoWins; }
            set 
            { 
                _numLottoWins = value; 
            }
        }

        public int NumLottoPlusWins
        {
            get 
            { 
                return _numLottoPlusWins; 
            }
            set 
            { 
                _numLottoPlusWins = value; 
            }

        }

        public int NumWinBoards
        {
            get 
            { 
                return _numWinBoards; 
            }
            set 
            { 
                _numWinBoards = value; 
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
    }

    public class LottoGameResults
    {
        private List<LottoGameBoard> _gameBoards;
        private LottoGameBoardSummary _summary;
        private int _winningsAdded;

        public LottoGameResults(List<LottoGameBoard> GameBoards)
        {
            _gameBoards = GameBoards;
            _summary = new LottoGameBoardSummary();
            CalculateGameSummary();
        }

        private void CalculateGameSummary()
        {
            String lottowinning = String.Empty;
            String lottopluswinning = String.Empty;

            foreach (LottoGameBoard board in _gameBoards)
            {
                if (board.WinningBoard) 
                    _summary.NumWinBoards = _summary.NumWinBoards + 1;

                if ((board.WinningBoard) && (board.LottoWinCount > 0))
                    _summary.NumLottoWins = _summary.NumLottoWins + 1;

                if ((board.WinningBoard) && (board.LottoPlusWinCount > 0))
                    _summary.NumLottoPlusWins = _summary.NumLottoPlusWins + 1;
            }

            if (_summary.NumWinBoards > 0)
            {
                if (_summary.NumLottoWins > 1)
                {
                    lottowinning = " Lotto winnings";
                }
                else
                {
                    lottowinning = " Lotto winning";
                }

                if (_summary.NumLottoPlusWins > 1)
                {
                    lottopluswinning = " LottoPlus winnings";
                }
                else
                {
                    lottopluswinning = " LottoPlus winning";
                }
                if ((_summary.NumLottoWins > 0) && (_summary.NumLottoPlusWins > 0))
                {
                    _summary.Message = "Congratulations! " + "You have " + Convert.ToString(_summary.NumLottoWins) + lottowinning + " and " +
                    Convert.ToString(_summary.NumLottoPlusWins) + lottopluswinning + ".";
                }
                if ((_summary.NumLottoWins == 0) && (_summary.NumLottoPlusWins > 0))
                    _summary.Message = "Congratulations! " + "You have " + Convert.ToString(_summary.NumLottoPlusWins) + lottopluswinning + ".";
                if ((_summary.NumLottoWins > 0) && (_summary.NumLottoPlusWins == 0))
                    _summary.Message = "Congratulations! " + "You have " + Convert.ToString(_summary.NumLottoWins) + lottowinning + ".";
                if ((_summary.NumLottoWins == 0) && (_summary.NumLottoPlusWins == 0))
                    _summary.Message = "Sorry. " + "You have no Lotto winnings.";
            }

            if ((_gameBoards.Count > 0) && (_summary.NumWinBoards == 0))
            {
                _summary.Message = "Sorry. " + "You have no winnings.";
            }
            else if ((_gameBoards.Count == 0) && (_summary.NumWinBoards > 0))
            {
                _summary.Message = "Sorry. " + "No matching numbers found in subscription boards. No winnings recorded.";
            }
        }

        public List<LottoGameBoard> GameBoards
        {
            get 
            { 
                return _gameBoards; 
            }
            set 
            { 
                _gameBoards = value; 
            }
        }

        public LottoGameBoardSummary Summary
        {
            get 
            { 
                return _summary; 
            }
            set 
            { 
                _summary = value; 
            }
        }

        public int WinningsAdded
        {
            get 
            { 
                return _winningsAdded; 
            }
            set 
            { 
                _winningsAdded = value; 
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.lotto.models;
using ttpim.gamemodule.games.models.lotto;

namespace ttpim.gamemodule.games.models.powerball
{
    public struct PowerballGameBoardSummary
    {
        private int _numPowerballWins;
        private int _numWinBoards;
        private string _message;

        public int NumPowerballWins
        {
            get { return _numPowerballWins; }
            set { _numPowerballWins = value; }
        }

        public int NumWinBoards
        {
            get { return _numWinBoards; }
            set { _numWinBoards = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }

    public class PowerballGameResults
    {
        private List<PowerballGameBoard> _gameBoards;
        private PowerballGameBoardSummary _summary;

        private void CalculateGameSummary()
        {
            String powerwinning = String.Empty;

            foreach (PowerballGameBoard board in _gameBoards)
            {
                if (board.WinningBoard) 
                    _summary.NumWinBoards = _summary.NumWinBoards + 1;

                if ((board.WinningBoard) && (board.PowerballWinCount > 0))
                    _summary.NumPowerballWins = _summary.NumPowerballWins + 1;
            }

            if (_summary.NumWinBoards > 0)
            {
                if (_summary.NumWinBoards > 1)
                {
                    powerwinning = " Powerball winnings";
                }
                else
                {
                    powerwinning = " Powerball winning";
                }

                if (_summary.NumPowerballWins > 0)
                {
                    _summary.Message = "Congratulations! " + "You have " + Convert.ToString(_summary.NumPowerballWins) + powerwinning + ".";
                }
            }

            if ((_gameBoards.Count > 0) && (_summary.NumWinBoards == 0))
            {
                _summary.Message = "Sorry. " + "You have no Powerball winnings.";
            }
            else if ((_gameBoards.Count == 0) && (_summary.NumWinBoards == 0))
            {
                _summary.Message = "Sorry. " + "No matching subscription boards found. No winnings recorded.";
            }

        }

        public PowerballGameResults(List<PowerballGameBoard> GameBoards)
        {
            _gameBoards = GameBoards;
            _summary = new PowerballGameBoardSummary();
            CalculateGameSummary();
        }

        public List<PowerballGameBoard> GameBoards
        {
            get { return _gameBoards; }
            set { _gameBoards = value; }
        }

        public PowerballGameBoardSummary Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }
    }
}

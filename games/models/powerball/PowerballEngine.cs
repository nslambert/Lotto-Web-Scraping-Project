using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.games.controllers.database;
using ttpim.gamemodule.games.models;
using ttpim.gamemodule.games.models.powerball;
using ttpim.gamemodule.lotto.models;
using ttpim.gamemodule.controllers;

namespace ttpim.gamemodule.games.models.lotto
{
    public class PowerballGameBoard
    {
        private int[] _subscriptionBoard = new int[6];
        private int[] _powerballMatches = new int[6];
        private int[] _gameNumbers = new int[6];
        private int _powerballBonus;
        private int _subscriptionId;
        private int _boardNumber;
        private int _boardId;
        private int _powerballWinCount;
        private int _numPowerballMatches;
        private string _game;
        private DateTime _gameDate;
        private string _subscriptionNr;
        private string _subscriptionType;
        private Boolean _powerMatch;
        private Boolean _powerJackpotMatch;
        private Boolean _winningBoard;
        private Boolean _active;
        private int _gameBonus;
        private int _gameSortedBonus;
        private string _powerballDivision;
        private int _powerballDivisionNumber;
        private double _powerballEarning;


        private Dictionary<string, string> _powerballDivReport;

        public PowerballGameBoard()
        {
            for (int i = 0; i < this.PowerballMatches.Length; i++)
            {
                this.PowerballMatches[i] = 0;
            }
            this.PowerballBonus = 0;
            this.PowerballMatch = false;
        }

        public Boolean Active
        {
            get 
            { 
                return _active; 
            }
            set 
            { 
                _active = value; 
            }
        }

        public int[] GameNumbers
        {
            get 
            { 
                return _gameNumbers; 
            }
            set 
            { 
                _gameNumbers = value; 
            }
        }

        public int BoardNumber
        {
            get { return _boardNumber; }
            set { _boardNumber = value; }
        }

        public int BoardId
        {
            get { return _boardId; }
            set { _boardId = value; }
        }

        public int SubscriptionId
        {
            get { return _subscriptionId; }
            set { _subscriptionId = value; }
        }

        public string SubscriptionType
        {
            get { return _subscriptionType; }
            set { _subscriptionType = value; }
        }

        public string SubscriptionNr
        {
            get { return _subscriptionNr; }
            set { _subscriptionNr = value; }
        }

        public int[] SubcriptionBoard
        {
            get { return _subscriptionBoard; }
            set { _subscriptionBoard = value; }
        }

        public int[] PowerballMatches
        {
            get { return _powerballMatches; }
            set { _powerballMatches = value; }
        }

        public string Game
        {
            get { return _game; }
            set { _game = value; }
        }

        public DateTime GameDate
        {
            get { return _gameDate; }
            set { _gameDate = value; }
        }

        public int GameBonus
        {
            get { return _gameBonus; }
            set { _gameBonus = value; }
        }

        public int GameSortedBonus
        {
            get 
            { 
                return _gameSortedBonus; 
            }
            set 
            {
                _gameSortedBonus = value; 
            }
        }

        public string PowerballDivision
        {
            get { return _powerballDivision; }
            set { _powerballDivision = value; }
        }

        public int PowerballDivisionNumber
        {
            get { return _powerballDivisionNumber; }
            set { _powerballDivisionNumber = value; }
        }

        public double PowerballEarning
        {
            get
            {
                return _powerballEarning;
            }
            set
            {
                _powerballEarning = value;
            }
        }

        public int[] PowerballMatchedNumbers
        {
            get 
            { 
                return _powerballMatches; 
            }
            set 
            {
                _powerballMatches = value; 
            }
        }

        public int NumberPowerballMatches
        {
            get { return _numPowerballMatches; }
            set { _numPowerballMatches = value; }
        }

        public int PowerballBonus
        {
            get { return _powerballBonus; }
            set { _powerballBonus = value; }
        }

        public Boolean PowerballMatch
        {
            get { return _powerMatch; }
            set { _powerMatch = value; }
        }

        public Boolean PowerballJackpotMatch
        {
            get 
            { 
                return _powerJackpotMatch; 
            }
            set 
            {
                _powerJackpotMatch = value; 
            }
        }

        public Boolean WinningBoard
        {
            get { return _winningBoard; }
            set { _winningBoard = value; }
        }

        public int PowerballWinCount
        {
            get { return _powerballWinCount; }
            set { _powerballWinCount = value; }
        }

        public Dictionary<string, string> PowerballDivisionReport
        {
            get { return _powerballDivReport; }
            set { _powerballDivReport = value; }
        }
    }

    public class PowerballEngine
    {
        private static List<GameSubscription> _subscriptions;
        private static List<GameBoard> _boards = new List<GameBoard>();
        private static SetupData _setupData;

        public static SetupData EngineSetup
        {
            get
            {
                return _setupData;
            }
            set
            {
                _setupData = value;
            }
        }

        public static List<GameBoard> Boards
        {
            get { return _boards; }
            set { _boards = value; }
        }

        public static List<GameSubscription> Subscriptions
        {
            get { return _subscriptions; }
            set { _subscriptions = value; }
        }

        public static void InitialisePowerballEngine()
        {
            if (Subscriptions == null)
            {
                Subscriptions = new List<GameSubscription>();
            }

            LottoSubscriptionServiceManager controller;
            if (!String.IsNullOrWhiteSpace(EngineSetup.DatasourcePath))
            {
                controller = new LottoSubscriptionServiceManager(EngineSetup);
            }
            else
            {
                controller = new LottoSubscriptionServiceManager();
            }
            Subscriptions = controller.GetLottoSubscriptions("Powerball").ToList();
        }

        public static PowerballGameResults ProcessPowerballCard(PowerballCard GameCard)
        {
            List<PowerballGameBoard> Boards = new List<PowerballGameBoard>();
            InitialisePowerballEngine();
            foreach (GameSubscription LottoSubscription in Subscriptions)
            {
                List<PowerballGameBoard> boards = ProcessSubscription(LottoSubscription, GameCard);
                Boards.AddRange(boards);
            }

            PowerballGameResults _results = new PowerballGameResults(Boards);
            return _results;
        }

        public static List<PowerballGameBoard> ProcessSubscription(GameSubscription PowerballSubscription, PowerballCard Power)
        {
            List<PowerballGameBoard> GameBoards = new List<PowerballGameBoard>();
            foreach (GameBoard SubscriptionBoard in PowerballSubscription.Boards)
            {
                PowerballGameBoard gameBoard = ProcessBoard(SubscriptionBoard, Power);
                GameBoards.Add(gameBoard);
            }
            return GameBoards;
        }

        public static PowerballGameBoard ProcessBoard(GameBoard Board, PowerballCard PowerCard)
        {
            int index = -1;
            PowerballGameBoard gameboard = new PowerballGameBoard();
            gameboard.SubcriptionBoard = Board.Numbers;
            gameboard.PowerballWinCount = 0;
            gameboard.PowerballBonus = 0;
            gameboard.PowerballMatch = false;
            gameboard.PowerballWinCount = 0;
            gameboard.SubscriptionId = Board.SubscriptionId;
            gameboard.SubscriptionType = Board.SubscriptionType;
            gameboard.SubscriptionNr = Board.SubscriptionNr;
            gameboard.BoardId = Board.BoardId;
            gameboard.BoardNumber = Board.BoardNumber;
            gameboard.GameNumbers = PowerCard.Numbers;

            if ((PowerCard != null) && (PowerCard.Numbers != null))
            {
                gameboard.Game = "Powerball";
                gameboard.GameDate = PowerCard.GameDate;
                gameboard.GameNumbers = PowerCard.Numbers;
                gameboard.GameBonus = PowerCard.PowerballBonus;
                gameboard.GameSortedBonus = PowerCard.SortedPowerball;
                gameboard.Active = true;

                for (int i = 0; i < PowerCard.Numbers.Length; i++)
                {
                    index = -1;
                    foreach (int number in Board.Numbers)
                    {
                        index = index + 1;
                        if (number == PowerCard.Numbers[i])
                        {
                            gameboard.PowerballMatches[index] = PowerCard.Numbers[i];
                            gameboard.NumberPowerballMatches = gameboard.NumberPowerballMatches + 1;
                        }

                        if (number == PowerCard.PowerballBonus)
                        {
                            gameboard.PowerballMatches[index] = PowerCard.PowerballBonus;
                            gameboard.PowerballBonus = PowerCard.PowerballBonus;
                            gameboard.PowerballMatch = true;
                        }
                    }
                }
            }

            if (((gameboard.NumberPowerballMatches >= 1) && (gameboard.PowerballMatch == true)) || (gameboard.NumberPowerballMatches >= 3))
            {
                gameboard.WinningBoard = true;
                gameboard.PowerballWinCount = gameboard.PowerballWinCount  + 1;
            }
            else
                gameboard.WinningBoard = false;

            if ((gameboard.NumberPowerballMatches == 5)  && (gameboard.PowerballMatch == true))
            {
                gameboard.PowerballJackpotMatch = true;
            }
            else
            {
                gameboard.PowerballJackpotMatch = false;
            }

            gameboard.PowerballDivisionReport = GetPowerballDivisionReport(gameboard.NumberPowerballMatches, gameboard.PowerballMatch, PowerCard.DivPayouts);

            if ((gameboard.PowerballDivisionReport != null) && (gameboard.PowerballDivisionReport.Count > 0))
            {
                gameboard.PowerballDivision = Convert.ToString(gameboard.PowerballDivisionReport.Keys.FirstOrDefault());
                gameboard.PowerballDivisionNumber = GetPowerballDivisionNumber(gameboard.NumberPowerballMatches, gameboard.PowerballMatch);
                gameboard.PowerballEarning = Convert.ToDouble(gameboard.PowerballDivisionReport.Values.FirstOrDefault().Replace("R", "").Replace(" ", "").Trim());
            }

            return gameboard;
        }

        public static int GetPowerballDivisionNumber(int NumMatches, Boolean PowerballMatch)
        {
            int divNumber = 0;
            if ((NumMatches == 1) && (PowerballMatch == true))
            {
                divNumber = 8;
            }
            if ((NumMatches == 2) && (PowerballMatch == true))
            {
                divNumber = 7;
            }
            if ((NumMatches == 3) && (PowerballMatch == false))
            {
                divNumber = 6;
            }
            if ((NumMatches == 3) && (PowerballMatch == true))
            {
                divNumber = 5;
            }
            if ((NumMatches == 4) && (PowerballMatch == false))
            {
                divNumber = 4;
            }

            if ((NumMatches == 4) && (PowerballMatch == true))
            {
                divNumber = 3;
            }
            if ((NumMatches == 5) && (PowerballMatch == false))
            {
                divNumber = 2;
            }
            if ((NumMatches == 5) && (PowerballMatch == true))
            {
                divNumber = 1;
            }
            return divNumber;
        }


        public static Dictionary<string, string> GetPowerballDivisionReport(int NumMatches, Boolean PowerballMatch, DivisionalPayout DivionalPayouts)
        {
            Dictionary<string, string> divReport = new Dictionary<string, string>();
            if ((NumMatches == 1) && (PowerballMatch == true))
            {
                divReport.Add(DivionalPayouts.PayoutList[7].Name, DivionalPayouts.PayoutList[7].PayoutPerWinner);
            }
            if ((NumMatches == 2) && (PowerballMatch == true))
            {
                divReport.Add(DivionalPayouts.PayoutList[6].Name, DivionalPayouts.PayoutList[6].PayoutPerWinner);
            }
            if ((NumMatches == 3) && (PowerballMatch == false))
            {
                divReport.Add(DivionalPayouts.PayoutList[5].Name, DivionalPayouts.PayoutList[5].PayoutPerWinner);
            }
            if ((NumMatches == 3) && (PowerballMatch == true))
            {
                divReport.Add(DivionalPayouts.PayoutList[4].Name, DivionalPayouts.PayoutList[4].PayoutPerWinner);
            }
            if ((NumMatches == 4) && (PowerballMatch == false))
            {
                divReport.Add(DivionalPayouts.PayoutList[3].Name, DivionalPayouts.PayoutList[3].PayoutPerWinner);
            }

            if ((NumMatches == 4) && (PowerballMatch == true))
            {
                divReport.Add(DivionalPayouts.PayoutList[2].Name, DivionalPayouts.PayoutList[2].PayoutPerWinner);
            }
            if ((NumMatches == 5) && (PowerballMatch == false))
            {
                divReport.Add(DivionalPayouts.PayoutList[1].Name, DivionalPayouts.PayoutList[1].PayoutPerWinner);
            }
            if ((NumMatches == 5) && (PowerballMatch == true))
            {
                divReport.Add(DivionalPayouts.PayoutList[0].Name, DivionalPayouts.PayoutList[0].PayoutPerWinner);
            }
            return divReport;
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using ttpim.gamemodule.lotto.models;
using ttpim.gamemodule.controllers;
using ttpim.gamemodule.database;
using ttpim.gamemodule.lotto.controllers;

namespace ttpim.gamemodule.games.models.lotto
{
    public static class LottoEngine
    {
        private static List<GameSubscription> _subscriptions;
        private static List<Board> _boards = new List<Board>();
        private static LottoWinNotification _winNotifyDelegate;
        private static Thread _databaseThread;
        private static int _winsRecorded;
        private static SetupData _setupData;

        static LottoEngine()
        {
            _winNotifyDelegate = new LottoWinNotification(SaveLottoWinningEventHandler);
        }

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

        public static Thread DatabaseThread 
        {
            get 
            {
                return _databaseThread;
            }
            set 
            {
                _databaseThread = value;
            }
        }

        public static LottoWinNotification WinningNotificationDelegate
        {
            get 
            {
                return _winNotifyDelegate;
            }
            set 
            {
                _winNotifyDelegate = value;
            }
        }

        public static List<Board> Boards
        {
            get 
            { 
                return _boards; 
            }
            
            set 
            { 
                _boards = value; 
            }
        }

        public static List<GameSubscription> Subscriptions
        {
            get 
            { 
                return _subscriptions; 
            }
            set 
            { 
                _subscriptions = value; 
            }
        }

        public static int NumWinsRecorded
        {
            get 
            { 
                return _winsRecorded; 
            }
            set 
            { 
                _winsRecorded = value; 
            }
        }

        private static void InitialiseLottoEngine()
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
            Subscriptions = controller.GetLottoSubscriptions("Lotto").ToList();
        }

        public static LottoGameResults ProcessLottoCard(LottoCard GameCard)
        {
            List<LottoGameBoard> Boards = new List<LottoGameBoard>();
            InitialiseLottoEngine();
            foreach (GameSubscription LottoSubscription in Subscriptions)
            {
                List<LottoGameBoard> boards = ProcessSubscription(LottoSubscription, GameCard);
                Boards.AddRange(boards);
            }

            LottoGameResults results = new LottoGameResults(Boards);
            return results;
        }

        public static List<LottoGameBoard> ProcessSubscription(GameSubscription LottoSubscription, LottoCard Lotto)
        {
            List<LottoGameBoard> GameBoards = new List<LottoGameBoard>();
            foreach (GameBoard SubscriptionBoard in LottoSubscription.Boards)
            {
                LottoGameBoard gameBoard = ProcessBoard(SubscriptionBoard, Lotto);
                GameBoards.Add(gameBoard);
            }
            return GameBoards;
        }

        public static LottoGameBoard ProcessBoard(GameBoard Board, LottoCard Lotto)
        {
            int index = -1;
            LottoGameBoard gameboard = new LottoGameBoard();
            gameboard.SubcriptionBoard = Board.Numbers;
            gameboard.NumberLottoMatches = 0;
            gameboard.NumberLottoPlusMatches = 0;
            gameboard.LottoBonus = 0;
            gameboard.LottoPlusBonus = 0;
            gameboard.LottoBonusMatch = false;
            gameboard.LottoPlusBonusMatch = false;
            gameboard.LottoWinCount = 0;
            gameboard.LottoPlusWinCount = 0;
            gameboard.SubscriptionId = Board.SubscriptionId;
            gameboard.SubscriptionType = Board.SubscriptionType;
            gameboard.SubscriptionNr = Board.SubscriptionNr;
            gameboard.BoardId = Board.BoardId;
            gameboard.BoardNumber = Board.BoardNumber;

            if (Lotto.Numbers != null)
            {
                gameboard.Game = "Lotto";
                gameboard.GameDate = Lotto.GameDate;
                gameboard.GameNumbers = Lotto.Numbers;
                gameboard.GameBonus = Lotto.Bonus;
                gameboard.Active = true;
                for (int i = 0; i < Lotto.Numbers.Length; i++)
                {
                    index = -1;
                    foreach (int number in Board.Numbers)
                    {
                        index = index + 1;
                        if (number == Lotto.Numbers[i])
                        {
                            gameboard.LottoMatchedNumbers[index] = Lotto.Numbers[i];
                            gameboard.NumberLottoMatches = gameboard.NumberLottoMatches + 1;
                        }

                        if (number == Lotto.Bonus)
                        {
                            gameboard.LottoMatchedNumbers[index] = Lotto.Bonus;
                            gameboard.LottoBonus = Lotto.Bonus;
                            gameboard.LottoBonusMatch = true;
                        }
                    }
                }
            }

            if (Lotto.IncludeLottoPlus)
            {
                gameboard.Game = "LottoPlus";
                gameboard.GameDate = Lotto.LottoPlus.GameDate;
                gameboard.GameNumbers = Lotto.LottoPlus.Numbers;
                gameboard.GameBonus = Lotto.LottoPlus.GameBonus;
                gameboard.GameSortedBonus = Lotto.LottoPlus.GameBonus;
                gameboard.Active = true;
                for (int i = 0; i < Lotto.LottoPlus.Numbers.Length; i++)
                {
                    index = -1;
                    foreach (int number in Board.Numbers)
                    {
                        index = index + 1;
                        if (number == Lotto.LottoPlus.Numbers[i])
                        {
                            gameboard.LottoPlusMatchedNumbers[index] = Lotto.LottoPlus.Numbers[i];
                            gameboard.NumberLottoPlusMatches = gameboard.NumberLottoPlusMatches + 1;
                        }

                        if (number == Lotto.LottoPlus.GameBonus)
                        {
                            gameboard.LottoPlusMatchedNumbers[index] = Lotto.LottoPlus.GameBonus;
                            gameboard.LottoPlusBonus = Lotto.LottoPlus.GameBonus;
                            gameboard.LottoPlusBonusMatch = true;
                        }
                    }
                }
            }

            if (gameboard.NumberLottoMatches >= 3)
            {
                gameboard.WinningBoard = true;
                gameboard.LottoWinCount = gameboard.LottoWinCount + 1;
            }
            else
                gameboard.WinningBoard = false;

            if (gameboard.NumberLottoPlusMatches >= 3)
            {
                gameboard.WinningBoard = true;
                gameboard.LottoPlusWinCount = gameboard.LottoPlusWinCount + 1;
            }

            if (gameboard.NumberLottoMatches == 6)
            {
                gameboard.LottoJackpotMatch = true;
            }
            else
            {
                gameboard.LottoJackpotMatch = false;
            }

            if (gameboard.NumberLottoPlusMatches == 6)
            {
                gameboard.LottoPlusJackpotMatch = true;
            }
            else
            {
                gameboard.LottoPlusJackpotMatch = false;
            }

            gameboard.LottoDivisionReport = GetLottoDivisionReport(gameboard.NumberLottoMatches, gameboard.LottoBonusMatch, Lotto.DivionalPayouts);
            if ((gameboard.LottoDivisionReport != null) && (gameboard.LottoDivisionReport.Count > 0))
            {
                gameboard.LottoDivision = Convert.ToString(gameboard.LottoDivisionReport.Keys.FirstOrDefault());
                gameboard.LottoDivisionNumber = GetLottoDivisionNumber(gameboard.NumberLottoMatches, gameboard.LottoBonusMatch);
                gameboard.LottoEarning = Convert.ToDouble(gameboard.LottoDivisionReport.Values.FirstOrDefault().Replace("R","").Replace(" ", "").Trim());
            }

            if (Lotto.IncludeLottoPlus)
            {
                gameboard.LottoPlusDivisionReport = GetLottoPlusDivisionReport(gameboard.NumberLottoPlusMatches, gameboard.LottoPlusBonusMatch, Lotto.LottoPlus.DivisionalPayouts);
                if ((gameboard.LottoPlusDivisionReport != null) && (gameboard.LottoPlusDivisionReport.Count > 0))
                {
                    gameboard.LottoPlusDivision = Convert.ToString(gameboard.LottoPlusDivisionReport.Keys.FirstOrDefault());
                    gameboard.LottoPlusDivisionNumber = GetLottoDivisionNumber(gameboard.NumberLottoPlusMatches,  gameboard.LottoPlusBonusMatch);
                    gameboard.LottoPlusEarning = Convert.ToDouble(gameboard.LottoPlusDivisionReport.Values.FirstOrDefault().Replace("R","").Replace(" ", "").Trim());
                }
            }
            return gameboard;
        }

        private static int GetLottoDivisionNumber(int NumMatches, Boolean BonusMatch)
        {
            int divNumber = 0;
            if ((NumMatches == 3) && (BonusMatch == false))
            {
                divNumber = 7;
            }

            if ((NumMatches == 3) && (BonusMatch == true))
            {
                divNumber = 6;
            }

            if ((NumMatches == 4) && (BonusMatch == false))
            {
                divNumber = 5;
            }

            if ((NumMatches == 4) && (BonusMatch == true))
            {
                divNumber = 4;
            }

            if ((NumMatches == 5) && (BonusMatch == false))
            {
                divNumber = 3;
            }
            if ((NumMatches == 5) && (BonusMatch == true))
            {
                divNumber = 2;
            }
            if (NumMatches == 6)
            {
                divNumber = 1;
            }
            return divNumber;
        }

        public static Dictionary<string, string> GetLottoDivisionReport(int NumMatches, Boolean BonusMatch, DivisionalPayout DivionalPayouts)
        {
            Dictionary<string, string> divReport = new Dictionary<string, string>();
            if ((NumMatches == 3) && (BonusMatch == false))
            {
                divReport.Add(DivionalPayouts.PayoutList[6].Name, DivionalPayouts.PayoutList[6].PayoutPerWinner);
            }

            if ((NumMatches == 3) && (BonusMatch == true))
            {
                divReport.Add(DivionalPayouts.PayoutList[5].Name, DivionalPayouts.PayoutList[5].PayoutPerWinner);
            }

            if ((NumMatches == 4) && (BonusMatch == false))
            {
                divReport.Add(DivionalPayouts.PayoutList[4].Name, DivionalPayouts.PayoutList[4].PayoutPerWinner);
            }

            if ((NumMatches == 4) && (BonusMatch == true))
            {
                divReport.Add(DivionalPayouts.PayoutList[3].Name, DivionalPayouts.PayoutList[3].PayoutPerWinner);
            }

            if ((NumMatches == 5) && (BonusMatch == false))
            {
                divReport.Add(DivionalPayouts.PayoutList[2].Name, DivionalPayouts.PayoutList[2].PayoutPerWinner);
            }
            if ((NumMatches == 5) && (BonusMatch == true))
            {
                divReport.Add(DivionalPayouts.PayoutList[1].Name, DivionalPayouts.PayoutList[1].PayoutPerWinner);
            }
            if (NumMatches == 6)
            {
                divReport.Add(DivionalPayouts.PayoutList[0].Name, DivionalPayouts.PayoutList[0].PayoutPerWinner);
            }
            return divReport;
        }

        public static Dictionary<string, string> GetLottoPlusDivisionReport(int NumMatches, Boolean BonusMatch, DivisionalPayout DivionalPayouts)
        {
            Dictionary<string, string> divReport = new Dictionary<string, string>();
            if ((NumMatches == 3) && (BonusMatch == false))
            {
                divReport.Add(DivionalPayouts.PayoutList[6].Name, DivionalPayouts.PayoutList[6].PayoutPerWinner);
            }

            if ((NumMatches == 3) && (BonusMatch == true))
            {
                divReport.Add(DivionalPayouts.PayoutList[5].Name, DivionalPayouts.PayoutList[5].PayoutPerWinner);
            }

            if ((NumMatches == 4) && (BonusMatch == false))
            {
                divReport.Add(DivionalPayouts.PayoutList[4].Name, DivionalPayouts.PayoutList[4].PayoutPerWinner);
            }

            if ((NumMatches == 4) && (BonusMatch == true))
            {
                divReport.Add(DivionalPayouts.PayoutList[3].Name, DivionalPayouts.PayoutList[3].PayoutPerWinner);
            }

            if ((NumMatches == 5) && (BonusMatch == false))
            {
                divReport.Add(DivionalPayouts.PayoutList[2].Name, DivionalPayouts.PayoutList[2].PayoutPerWinner);
            }

            if ((NumMatches == 5) && (BonusMatch == true))
            {
                divReport.Add(DivionalPayouts.PayoutList[1].Name, DivionalPayouts.PayoutList[1].PayoutPerWinner);
            }

            if (NumMatches == 6)
            {
                divReport.Add(DivionalPayouts.PayoutList[0].Name, DivionalPayouts.PayoutList[0].PayoutPerWinner);
            }
            return divReport;
        }

        public static void SaveLottoWinningEventHandler(object sender, LottoWinEventArgs args)
        {
            ExecuteDatabaseThread(sender, args);
        }

        private static void ExecuteDatabaseThread(object sender, LottoWinEventArgs args)
        {
            ThreadStart thread = delegate() { SaveLottoWinning(sender, args); };
            if (DatabaseThread == null)
                DatabaseThread = new Thread(thread);

            DatabaseThread.IsBackground = true;
            DatabaseThread.Name = "DatabaseThread";
            DatabaseThread.Start();
        }

        private static void SaveLottoWinning(object sender, LottoWinEventArgs args)
        {
            LottoWinsServiceManager controller;
            if (!String.IsNullOrEmpty(EngineSetup.DatasourcePath))
            {
                controller = new LottoWinsServiceManager(EngineSetup);
            }
            else
            {
                controller = new LottoWinsServiceManager();
            }

            int result = 0;
            if ((sender is LottoEngine) && (args != null) && ((args.Winning != null)))
            {
                result = controller.SaveLottoWinning(args.Winning);
            }
            LottoEngine.NumWinsRecorded = LottoEngine.NumWinsRecorded + result;
        }
    }
}

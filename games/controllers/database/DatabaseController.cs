using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.lotto.controllers;
using ttpim.gamemodule.games.models;
using ttpim.gamemodule.lotto.models;
using ttpim.gamemodule.controllers;

namespace ttpim.gamemodule.games.controllers.database
{
    public static class DatabaseController
    {
        public static void LoadGameSetting()
        {
            CommonController.StartLottoParseToken = "./Lotto - National Lottery_files/ball_";
            CommonController.StartLottoPlusParseToken = "./Lotto Plus - National Lottery_files/ball_";
            CommonController.StartPowerballParseToken = "./Powerball- National Lottery_files/ball_";
            CommonController.BonusPowerballParseToken = "./Powerball- National Lottery_files/power_";
            CommonController.EndLottoParseToken = ".gif";
            CommonController.EndLottoPlusParseToken = ".gif";
            CommonController.EndPowerballParseToken = ".gif";
        }

        public static List<GameSubscription> LoadSubscriptions(string SubscriptionType)
        {
            List<GameSubscription> subscriptions = new List<GameSubscription>();
            switch (SubscriptionType)
            {
                case "Lotto":
                    subscriptions = LottoSubscriptions();
                    break;
                case "Powerball":

                    break;
                default:
                    break;
            }
            return subscriptions; 
        }

        private static List<GameSubscription> LottoSubscriptions()
        {
            List<GameSubscription> subscriptions = new List<GameSubscription>();
            List<GameBoard> BoardList1 = new List<GameBoard>();
            GameBoard b1 = new GameBoard(6);
   
            b1.Active = true;
            b1.Numbers[0] = 9;
            b1.Numbers[1] = 12;
            b1.Numbers[2] = 21;
            b1.Numbers[3] = 30;
            b1.Numbers[4] = 42;
            b1.Numbers[5] = 45;
            b1.SubscriptionNr = "0000009671";
            b1.SubscriptionId = 1;
            b1.BoardNumber = 1;
            b1.BoardId = 1;
            BoardList1.Add(b1);


            GameBoard b2 = new GameBoard(6);
            b2.Active = true;
            b2.Numbers[0] = 6;
            b2.Numbers[1] = 14;
            b2.Numbers[2] = 24;
            b2.Numbers[3] = 30;
            b2.Numbers[4] = 36;
            b2.Numbers[5] = 41;
            b2.SubscriptionNr = "0000009671";
            b2.SubscriptionId = 1;
            b2.BoardNumber = 2;
            b2.BoardId = 2;
            BoardList1.Add(b2);

            GameBoard b3 = new GameBoard(6);
            b3.Active = true;
            b3.Numbers[0] = 10;
            b3.Numbers[1] = 29;
            b3.Numbers[2] = 44;
            b3.Numbers[3] = 21;
            b3.Numbers[4] = 36;
            b3.Numbers[5] = 41;
            b3.SubscriptionNr = "0000009671";
            b3.SubscriptionId = 1;
            b3.BoardNumber = 3;
            b3.BoardId = 3;
            BoardList1.Add(b3);

            GameBoard b4 = new GameBoard(6);
            b4.Active = true;
            b4.Numbers[0] = 6;
            b4.Numbers[1] = 43;
            b4.Numbers[2] = 47;
            b4.Numbers[3] = 23;
            b4.Numbers[4] = 28;
            b4.Numbers[5] = 41;
            b4.SubscriptionNr = "0000009671";
            b4.SubscriptionId = 1;
            b4.BoardNumber = 4;
            b4.BoardId = 4;
            BoardList1.Add(b4);

            GameSubscription subscription = new GameSubscription
            {
                SubscriptionNumber = "0000009671",
                SubscriptionType = "Lotto",
                StartDate = Convert.ToDateTime("2010-01-01"),
                EndDate = Convert.ToDateTime("2015-01-01"),
                Active = true,
                Boards = BoardList1
            };
            subscriptions.Add(subscription);
            return subscriptions;
        }
    }
}

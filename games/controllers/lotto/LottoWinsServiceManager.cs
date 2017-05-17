using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.database;
using ttpim.gamemodule.games.models;
using ttpim.gamemodule.games.models.lotto;
using ttpim.gamemodule.common;
using System.Drawing;

namespace ttpim.gamemodule.lotto.controllers
{
    public class LottoWinsServiceManager
    {
        private ILottoWinsRepository _repository;
        public LottoWinsServiceManager() : this(new LottoWinsRepository()) { }

        public LottoWinsServiceManager(ILottoWinsRepository repository)
        {
            _repository = repository;
        }

        public LottoWinsServiceManager(SetupData setup)
        {
            _repository = new LottoWinsRepository(setup.DatasourcePath);
        }

        public List<Win> GetLottoWins()
        {
            IEnumerable<Win> wins = _repository.List();
            return wins.ToList();
        }

        public List<Win> GetLottoWins(long SubscriptionID, long BoardId)
        {
            IEnumerable<Win> wins = _repository.ListBySubscriptionBoard(SubscriptionID, BoardId);
            return wins.ToList();
        }

        public List<Win> GetLottoWins(string Game)
        {
            IEnumerable<Win> wins = _repository.ListByGame(Game);
            return wins.ToList();
        }

        public int SaveLottoWinning(Win LottoWinningToCreate)
        {
            int result = 0;
            result = _repository.Create(LottoWinningToCreate);
            return result;
        }

        public int SaveLottoWinnings(List<LottoGameBoard> Boards, string Game, Bitmap LottoBitmap)
        {
            int result = 0;
            List<Win> LottoWinningsToCreate = new List<Win>();
            foreach (LottoGameBoard board in Boards)
            {
                Win winning = new Win();
                winning.scriptid = board.SubscriptionId;
                winning.scriptnr = board.SubscriptionNr;
                winning.game = board.Game;
                winning.gamedate = board.GameDate;
                winning.boardnr = board.BoardNumber;
                winning.w1 = board.GameNumbers[0];
                winning.w2 = board.GameNumbers[1];
                winning.w3 = board.GameNumbers[2];
                winning.w4 = board.GameNumbers[3];
                winning.w5 = board.GameNumbers[4];
                winning.w6 = board.GameNumbers[5];
                winning.b = board.GameBonus;
                
                switch (Game)
                {
                    case "Lotto":
                        winning.p1 = board.LottoMatchedNumbers[0] == 0 ? -1 : board.LottoMatchedNumbers[0];
                        winning.p2 = board.LottoMatchedNumbers[1] == 0 ? -1 : board.LottoMatchedNumbers[1];
                        winning.p3 = board.LottoMatchedNumbers[2] == 0 ? -1 : board.LottoMatchedNumbers[2];
                        winning.p4 = board.LottoMatchedNumbers[3] == 0 ? -1 : board.LottoMatchedNumbers[3];
                        winning.p5 = board.LottoMatchedNumbers[4] == 0 ? -1 : board.LottoMatchedNumbers[4];
                        winning.p6 = board.LottoMatchedNumbers[5] == 0 ? -1 : board.LottoMatchedNumbers[5];
                        winning.division = board.LottoDivision;
                        winning.earning = board.LottoEarning;
                        if (LottoBitmap != null)
                        {
                            winning.webimage = CommonWebController.ConvertImageToBytes(LottoBitmap);
                        }
                        
                        winning.createdon = DateTime.Now;
                        winning.modifiedon = DateTime.Now;
                        winning.createdby = "sysadmin";
                        winning.modifiedby = "sysadmin";
                        winning.active = board.Active;
              
                        break;
                    case "LottoPlus":
                        winning.p1 = board.LottoPlusMatchedNumbers[0] == 0 ? -1 : board.LottoPlusMatchedNumbers[0];
                        winning.p2 = board.LottoPlusMatchedNumbers[1] == 0 ? -1 : board.LottoPlusMatchedNumbers[1];
                        winning.p3 = board.LottoPlusMatchedNumbers[2] == 0 ? -1 : board.LottoPlusMatchedNumbers[2];
                        winning.p4 = board.LottoPlusMatchedNumbers[3] == 0 ? -1 : board.LottoPlusMatchedNumbers[3];
                        winning.p5 = board.LottoPlusMatchedNumbers[4] == 0 ? -1 : board.LottoPlusMatchedNumbers[4];
                        winning.p6 = board.LottoPlusMatchedNumbers[5] == 0 ? -1 : board.LottoPlusMatchedNumbers[5];
                        winning.division = board.LottoPlusDivision;
                        winning.earning = board.LottoPlusEarning;
                        if (LottoBitmap != null)
                        {
                            winning.webimage = CommonWebController.ConvertImageToBytes(LottoBitmap);
                        }
                        winning.createdon = DateTime.Now;
                        winning.modifiedon = DateTime.Now;
                        winning.createdby = "sysadmin";
                        winning.modifiedby = "sysadmin";
                        winning.active = board.Active;
                        break;
                }
                LottoWinningsToCreate.Add(winning);
            }
            result = _repository.Save(LottoWinningsToCreate);
            return result;
        }

        public int SavePowerWinnings(List<PowerballGameBoard> Boards, string Game, Bitmap PowerballBitmap)
        {
            int result = 0;
            List<Win> PowerWinningsToCreate = new List<Win>();
            foreach (PowerballGameBoard board in Boards)
            {
                Win winning = new Win();
                winning.scriptid = board.SubscriptionId;
                winning.scriptnr = board.SubscriptionNr;
                winning.game = board.Game;
                winning.gamedate = board.GameDate;
                winning.boardnr = board.BoardNumber;
                winning.w1 = board.GameNumbers[0];
                winning.w2 = board.GameNumbers[1];
                winning.w3 = board.GameNumbers[2];
                winning.w4 = board.GameNumbers[3];
                winning.w5 = board.GameNumbers[4];
     
                winning.b = board.GameBonus;
                switch (Game)
                {
                    case "Powerball":
                        winning.p1 = board.PowerballMatchedNumbers[0] == 0 ? -1 : board.PowerballMatchedNumbers[0];
                        winning.p2 = board.PowerballMatchedNumbers[1] == 0 ? -1 : board.PowerballMatchedNumbers[1];
                        winning.p3 = board.PowerballMatchedNumbers[2] == 0 ? -1 : board.PowerballMatchedNumbers[2];
                        winning.p4 = board.PowerballMatchedNumbers[3] == 0 ? -1 : board.PowerballMatchedNumbers[3];
                        winning.p5 = board.PowerballMatchedNumbers[4] == 0 ? -1 : board.PowerballMatchedNumbers[4];
                        winning.p6 = board.PowerballMatchedNumbers[5] == 0 ? -1 : board.PowerballMatchedNumbers[5];
                        winning.division = board.PowerballDivision;
                        winning.earning = board.PowerballEarning;
                        if (PowerballBitmap != null)
                        {
                            winning.webimage = CommonWebController.ConvertImageToBytes(PowerballBitmap);
                        }
                        winning.createdon = DateTime.Now;
                        winning.modifiedon = DateTime.Now;
                        winning.createdby = "sysadmin";
                        winning.modifiedby = "sysadmin";
                        winning.active = board.Active;
                        break;
                }
                PowerWinningsToCreate.Add(winning);
            }
            result = _repository.Save(PowerWinningsToCreate);
            return result;
        }

        public int EditLottoWinning(Win LottoWinningToEdit)
        {
            int result = 0;
            _repository.Edit(LottoWinningToEdit);
            return result;
        }

        public int DeleteLottoWinning(Win LottoWinningToDelete)
        {
            int result = 0;
            _repository.Delete(LottoWinningToDelete);
            return result;
        }

        public DateTime GetLastSavedGameDate(string Game)
        {
            DateTime result = _repository.GetLastSavedGameDate(Game);
            return result;
        }

        public bool ContinueSaveAction(DateTime GameDate, string Game)
        {
            bool result = false;
            DateTime lastSavedGameDate = GetLastSavedGameDate(Game);
            if ((lastSavedGameDate == GameDate) || (lastSavedGameDate > GameDate))
            {
                result = false;
            }
            else
                result = true;
            return result;
        }
    }
}

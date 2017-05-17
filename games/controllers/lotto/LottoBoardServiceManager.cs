using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.database;
using ttpim.gamemodule.games.models;

namespace ttpim.gamemodule.controllers
{
    public class LottoBoardServiceManager
    {
        private ILottoBoardRepository _repository;
        public LottoBoardServiceManager() : this(new LottoBoardRepository()) { }
        public LottoBoardServiceManager(ILottoBoardRepository repository)
        {
            _repository = repository;
        }

        public LottoBoardServiceManager(SetupData setup)
        {
            _repository = new LottoBoardRepository(setup.DatasourcePath);
        }

        public List<Board> GetLottoBoards()
        {
            IEnumerable<Board> boards = _repository.List();
            return boards.ToList();
        }

        public List<Board> GetLottoBoardRegister(long SubscriptionID)
        {
            IEnumerable<Board> boards  = _repository.ListBySubscription(SubscriptionID);
            return boards.ToList();
        }
    }
}

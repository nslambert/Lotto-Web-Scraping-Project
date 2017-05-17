using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.database;
using ttpim.gamemodule.games.models;
using ttpim.gamemodule.lotto.models;

namespace ttpim.gamemodule.controllers
{
    public class LottoSubscriptionServiceManager
    {
        private ILottoSubscriptionRepository _repository;
        private SetupData _setup;

        public LottoSubscriptionServiceManager(ILottoSubscriptionRepository repository)
        {
            _repository = repository;
        }

        public LottoSubscriptionServiceManager(SetupData setup)
        {
            _repository = new LottoSubscriptionRepository(setup.DatasourcePath);
            _setup = setup;
        }

        public SetupData ConnectionSetup
        {
            get
            {
                return _setup;

            }
            set
            {
                _setup = value;
            }
        }

        public LottoSubscriptionServiceManager() : this(new LottoSubscriptionRepository()) { }

        private string GetLottoSubscriptionNumber(int SubscriptionId)
        {
            string subscriptnr = _repository.GetSubscriptionNumber(SubscriptionId);
            return subscriptnr;
        }

        private string GetLottoSubscriptionType(int SubscriptionId)
        {
            string subscriptnr = _repository.GetSubscriptionType(SubscriptionId);
            return subscriptnr;
        }

        private List<Subscription> GetLottoSubscriptionRegister()
        {
            var subscriptions = _repository.List();
            return subscriptions.ToList();
        }

        private List<Subscription> GetLottoSubscriptionRegister(string Game)
        {
            var subscriptions = _repository.List(Game);
            return subscriptions.ToList();
        }

        private List<GameSubscription> ProcessSubscriptionRegister(List<Subscription> SubscriptionRegister)
        {
            List<GameSubscription> subscriptions = new List<GameSubscription>();
            foreach (Subscription entry in SubscriptionRegister)
            {
                GameSubscription subscript = new GameSubscription();
                subscript.Active = (bool)entry.active;
                subscript.StartDate= (DateTime)entry.startdate;
                subscript.EndDate = (DateTime)entry.enddate;
                subscript.SubscriptionId = (int)entry.id;
                subscript.SubscriptionNumber = entry.scriptnr;
                subscript.SubscriptionType = entry.game;
                subscript.Boards = ProcessSubscriptionBoardRegister((long)entry.id);
                subscriptions.Add(subscript);
            }
            return subscriptions;
        }

        private List<GameBoard> ProcessSubscriptionBoardRegister(long SubscriptionId)
        {
            LottoBoardServiceManager controller;
            if (!String.IsNullOrEmpty(this.ConnectionSetup.DatasourcePath))
            {
                controller = new LottoBoardServiceManager(this.ConnectionSetup);
            }
            else
            {
                controller = new LottoBoardServiceManager();
            }
             
            List<Board> register = controller.GetLottoBoardRegister(SubscriptionId);
            List<GameBoard> boards = ProcessBoardRegister(register);
            return boards;
        }

        private List<GameBoard> ProcessBoardRegister(List<Board> BoardRegisters)
        {
            List<GameBoard> boards = new List<GameBoard>();
            foreach (Board entry in BoardRegisters)
            {
                GameBoard boardItem = new GameBoard(6);
                boardItem.Active = (bool)entry.active;
                boardItem.BoardId = (int)entry.id;
                boardItem.BoardNumber = (int)entry.boardnr;
                boardItem.Numbers[0] = (int)entry.b1;
                boardItem.Numbers[1] = (int)entry.b2;
                boardItem.Numbers[2] = (int)entry.b3;
                boardItem.Numbers[3] = (int)entry.b4;
                boardItem.Numbers[4] = (int)entry.b5;
                boardItem.Numbers[5] = (int)entry.b6;
                boardItem.SubscriptionId = (int)entry.scriptid;
                boardItem.SubscriptionNr = GetLottoSubscriptionNumber((int)entry.scriptid);
                boardItem.SubscriptionType = GetLottoSubscriptionType((int)entry.scriptid);
                boards.Add(boardItem);
            }
            return boards;
        }

        public List<GameSubscription> GetLottoSubscriptions()
        {
            List<Subscription> register = GetLottoSubscriptionRegister();
            List<GameSubscription> subscriptions = ProcessSubscriptionRegister(register);
            return subscriptions;
        }

        public List<GameSubscription> GetLottoSubscriptions(string Game)
        {
            List<Subscription> register = GetLottoSubscriptionRegister(Game);
            List<GameSubscription> subscriptions = ProcessSubscriptionRegister(register);
            return subscriptions;
        }
    }
}

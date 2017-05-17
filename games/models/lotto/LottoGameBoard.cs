using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using ttpim.gamemodule.database;


namespace ttpim.gamemodule.games.models.lotto
{
    public class LottoGameBoard
    {
        private int[] _gameNumbers = new int[6];
        private int[] _subscriptionBoard = new int[6];
        private int[] _lottoMatches = new int[6];
        private int[] _lottoPlusMatches = new int[6];
        private int[] _powerballMatches = new int[6];
        private int _gameBonus;
        private int _gameSortedBonus;
        private int _lottoBonus;
        private int _lottoPlusBonus;
        private int _powerballBonus;
        private int _numLottoMatches;
        private int _numLottoPlusMatches;
        private int _numPowerballMatches;
        private int _subscriptionId;
        private int _boardNumber;
        private int _boardId;
        private int _lottoWinCount;
        private int _lottoPlusWinCount;
        private string _game;
        private DateTime _gameDate;
        private string _subscriptionNr;
        private string _subscriptionType;
        private Boolean _active;
        private Boolean _lottoBonusMatch;
        private Boolean _lottoPlusBonusMatch;
        private Boolean _powerballBonusMatch;
        private Boolean _lottoJackpotMatch;
        private Boolean _lottoPlusJackpotMatch;
        private Boolean _powerballJackpotMatch;
        private Boolean _winningBoard;
        private string _lottoDivision;
        private int _lottoDivisionNumber;
        private int _lottoPlusDivisionNumber;
        private int _powerballDivisionNumber;
        private double _lottoEarning;
        private string _lottoPlusDivision;
        private double _lottoPlusEarning;
        private string _powerballDivision;
        private double _powerballEarning;
        private Image _webimage;

        private Dictionary<string, string> _lottoDivReport;
        private Dictionary<string, string> _lottoPlusDivReport;

        public LottoGameBoard()
        {
            for (int i = 0; i < this.GameNumbers.Length; i++)
            {
                this.GameNumbers[i] = 0;
            }
            for (int i = 0; i < this.LottoPlusMatchedNumbers.Length; i++)
            {
                this.LottoPlusMatchedNumbers[i] = 0;
            }
            for (int i = 0; i < this.LottoPlusMatchedNumbers.Length; i++)
            {
                this.LottoPlusMatchedNumbers[i] = 0;
            }
            for (int i = 0; i < this.PowerballMatchedNumbers.Length; i++)
            {
                this.PowerballMatchedNumbers[i] = 0;
            }
            this.LottoBonus = 0;
            this.LottoPlusBonus = 0;
            this.PowerballBonus = 0;
            this.LottoBonusMatch = false;
            this.LottoPlusBonusMatch = false;
            this.PowerballBonusMatch = false;
        }

        public Image WebImage
        {
            get { return _webimage; }
            set { _webimage = value; }
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

        public string Game
        {
            get { return _game; }
            set { _game = value; }
        }

        public DateTime GameDate
        {
            get 
            { 
                return _gameDate; 
            }
            set 
            { 
                _gameDate = value; 
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

        public int[] SubcriptionBoard
        {
            get { return _subscriptionBoard; }
            set { _subscriptionBoard = value; }
        }

        public int[] LottoMatchedNumbers
        {
            get { return _lottoMatches; }
            set { _lottoMatches = value; }
        }

        public int[] LottoPlusMatchedNumbers
        {
            get { return _lottoPlusMatches; }
            set { _lottoPlusMatches = value; }
        }

        public int[] PowerballMatchedNumbers
        {
            get { return _powerballMatches; }
            set { _powerballMatches = value; }
        }

        public int NumberLottoMatches
        {
            get { return _numLottoMatches; }
            set { _numLottoMatches = value; }
        }

        public int NumberLottoPlusMatches
        {
            get { return _numLottoPlusMatches; }
            set { _numLottoPlusMatches = value; }
        }

        public int NumberPowerballMatches
        {
            get 
            { 
                return _numPowerballMatches; 
            }
            set 
            {
                _numPowerballMatches = value; 
            }
        }

        public int GameBonus
        {
            get { return _gameBonus; }
            set { _gameBonus = value; }
        }

        public int GameSortedBonus
        {
            get { return _gameSortedBonus; }
            set { _gameSortedBonus = value; }
        }

        public int LottoBonus
        {
            get { return _lottoBonus; }
            set { _lottoBonus = value; }
        }

        public int LottoPlusBonus
        {
            get { return _lottoPlusBonus; }
            set { _lottoPlusBonus = value; }
        }

        public int PowerballBonus
        {
            get 
            { 
                return _powerballBonus; 
            }
            set 
            {
                _powerballBonus = value; 
            }
        }

        public Boolean LottoBonusMatch
        {
            get { return _lottoBonusMatch; }
            set { _lottoBonusMatch = value; }
        }

        public Boolean LottoPlusBonusMatch
        {
            get { return _lottoPlusBonusMatch; }
            set { _lottoPlusBonusMatch = value; }
        }

        public Boolean PowerballBonusMatch
        {
            get 
            { 
                return _powerballBonusMatch; }
            set 
            {
                _powerballBonusMatch = value; 
            }
        }

        public Boolean LottoJackpotMatch
        {
            get 
            { 
                return _lottoJackpotMatch; 
            }
            set 
            { 
                _lottoJackpotMatch = value; 
            }
        }

        public Boolean LottoPlusJackpotMatch
        {
            get
            {
                return _lottoPlusJackpotMatch;
            }
            set
            {
                _lottoPlusJackpotMatch = value;
            }
        }

        public Boolean PowerballJackpotMatch
        {
            get
            {
                return _powerballJackpotMatch;
            }
            set
            {
                _powerballJackpotMatch = value;
            }
        }

        public Boolean WinningBoard
        {
            get { return _winningBoard; }
            set { _winningBoard = value; }
        }

        public Boolean Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public int LottoWinCount
        {
            get { return _lottoWinCount; }
            set { _lottoWinCount = value; }
        }

        public int LottoPlusWinCount
        {
            get { return _lottoPlusWinCount; }
            set { _lottoPlusWinCount = value; }
        }

        public string LottoDivision
        {
            get 
            { 
                return _lottoDivision; 
            }
            set 
            { 
                _lottoDivision = value; 
            }
        }

        public int LottoDivisionNumber
        {
            get
            {
                return _lottoDivisionNumber;
            }
            set
            {
                _lottoDivisionNumber = value;
            }
        }

        public string LottoPlusDivision
        {
            get { return _lottoPlusDivision; }
            set { _lottoPlusDivision = value; }
        }

        public int LottoPlusDivisionNumber
        {
            get
            {
                return _lottoPlusDivisionNumber;
            }
            set
            {
                _lottoPlusDivisionNumber = value;
            }
        }

        public string PowerballDivision
        {
            get { return _powerballDivision; }
            set { _powerballDivision = value; }
        }

        public int PowerballDivisionNumber
        {
            get
            {
                return _powerballDivisionNumber;
            }
            set
            {
                _powerballDivisionNumber = value;
            }
        }

        public double LottoEarning
        {
            get 
            { 
                return _lottoEarning; 
            }
            set 
            { 
                _lottoEarning = value; 
            }
        }

        public double LottoPlusEarning
        {
            get { return _lottoPlusEarning; }
            set { _lottoPlusEarning = value; }
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

        public Dictionary<string, string> LottoDivisionReport
        {
            get { return _lottoDivReport; }
            set { _lottoDivReport = value; }
        }

        public Dictionary<string, string> LottoPlusDivisionReport
        {
            get { return _lottoPlusDivReport; }
            set { _lottoPlusDivReport = value; }
        }

        public event LottoWinNotification LottoWinEvent;

        public void RaiseLottoWinEvent()
        {
            Win winning = new Win();
            winning.scriptid = this.SubscriptionId;
            winning.scriptnr = this.SubscriptionNr;
            winning.game = this.Game;
            winning.gamedate = this.GameDate;
            winning.boardnr = this.BoardNumber;
            winning.id = -1;
            winning.w1 = this.GameNumbers[0];
            winning.w2 = this.GameNumbers[1];
            winning.w3 = this.GameNumbers[2];
            winning.w4 = this.GameNumbers[3];
            winning.w5 = this.GameNumbers[4];
            winning.w6 = this.GameNumbers[5];
            winning.b = this.GameBonus;
            winning.p1 = this.LottoMatchedNumbers[0];
            winning.p2 = this.LottoMatchedNumbers[1];
            winning.p3 = this.LottoMatchedNumbers[2];
            winning.p4 = this.LottoMatchedNumbers[3];
            winning.p5 = this.LottoMatchedNumbers[4];
            winning.p6 = this.LottoMatchedNumbers[5];
            winning.division = this.LottoDivision;
            winning.earning = this.LottoEarning;
            winning.active = this.Active;

            if (LottoWinEvent != null)
                LottoWinEvent(this, new LottoWinEventArgs(winning) { });
            Thread.Sleep(10);
        }

        public void RaiseLottoPlusWinEvent()
        {
            Win winning = new Win();
            winning.scriptid = this.SubscriptionId;
            winning.scriptnr = this.SubscriptionNr;
            winning.boardnr = this.BoardNumber;
            winning.division = this.LottoDivision;
            winning.earning = this.LottoEarning;
            winning.game = this.Game;
            winning.gamedate = this.GameDate;
            winning.id = -1;
            winning.w1 = this.GameNumbers[0];
            winning.w2 = this.GameNumbers[1];
            winning.w3 = this.GameNumbers[2];
            winning.w4 = this.GameNumbers[3];
            winning.w5 = this.GameNumbers[4];
            winning.w6 = this.GameNumbers[5];
            winning.b = this.GameBonus;
            winning.p1 = this.LottoPlusMatchedNumbers[0];
            winning.p2 = this.LottoPlusMatchedNumbers[1];
            winning.p3 = this.LottoPlusMatchedNumbers[2];
            winning.p4 = this.LottoPlusMatchedNumbers[3];
            winning.p5 = this.LottoPlusMatchedNumbers[4];
            winning.p6 = this.LottoPlusMatchedNumbers[5];
            winning.active = this.Active;

            if (LottoWinEvent != null)
                LottoWinEvent(this, new LottoWinEventArgs(winning) { });
            Thread.Sleep(10);
        }
    }
}

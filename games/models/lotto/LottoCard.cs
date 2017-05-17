using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ttpim.gamemodule.games.models.lotto
{
    public class LottoDivision
    {
        private string _division;
        private int _nrofwinners;
        private string _payoutperwinner;

        public string Division
        {
            get { return _division; }
            set { _division = value; }
        }

        public int NumberOfWinners
        {
            get { return _nrofwinners; }
            set { _nrofwinners = value; }
        }

        public string PayoutPerWinner
        {
            get { return _payoutperwinner; }
            set { _payoutperwinner = value; }
        }
    }

    public class LottoDivisionalPayout
    {
        private List<LottoDivision> _payouts;
        public LottoDivisionalPayout()
        {
            _payouts = new List<LottoDivision>();
        }

        public List<LottoDivision> PayoutList
        {
            get { return _payouts; }
            set { _payouts = value; }
        }

        public void AddPayout(LottoDivision divpayout)
        {
            _payouts.Add(divpayout);
        }

        public void RemovePayout(LottoDivision divpayout)
        {
            _payouts.Remove(divpayout);
        }
    }

    public class LottoDrawStatistics
    {
        private string _rolloveramount;
        private string _rolloveramountdesc;
        private string _rollovernumberdesc;
        private string _rollovernumber;
        private string _totalprizepooldesc;
        private string _totalprizepool;
        private string _totalsalesdesc;
        private string _totalsales;
        private string _nextestimatedjackpotdesc;
        private string _nextestimatedjackpot;
        private string _drawmachineuseddesc;
        private string _drawmachineused;
        private string _ballsetuseddesc;
        private string _ballsetused;
        private string _drawnumberdesc;
        private string _drawnumber;

        public string RollOverAmountDescr
        {
            get { return _rolloveramountdesc; }
            set { _rolloveramountdesc = value; }
        }

        public string RollOverAmount
        {
            get { return _rolloveramount; }
            set { _rolloveramount = value; }
        }

        public string RollOverNumberDesc
        {
            get { return _rollovernumberdesc; }
            set { _rollovernumberdesc = value; }
        }

        public string RollOverNumber
        {
            get { return _rollovernumber; }
            set { _rollovernumber = value; }
        }

        public string TotalPrizePoolDesc
        {
            get { return _totalprizepooldesc; }
            set { _totalprizepooldesc = value; }
        }

        public string TotalPrizePool
        {
            get { return _totalprizepool; }
            set { _totalprizepool = value; }
        }

        public string TotalSalesDesc
        {
            get { return _totalsalesdesc; }
            set { _totalsalesdesc = value; }
        }

        public string TotalSales
        {
            get { return _totalsales; }
            set { _totalsales = value; }
        }

        public string NextEstimatedJackpotDesc
        {
            get { return _nextestimatedjackpotdesc; }
            set { _nextestimatedjackpotdesc = value; }
        }

        public string NextEstimatedJackpot
        {
            get { return _nextestimatedjackpot; }
            set { _nextestimatedjackpot = value; }
        }

        public string DrawMachineUsedDesc
        {
            get { return _drawmachineuseddesc; }
            set { _drawmachineuseddesc = value; }
        }

        public string DrawMachineUsed
        {
            get { return _drawmachineused; }
            set { _drawmachineused = value; }
        }

        public string BallSetUsedDesc
        {
            get { return _ballsetuseddesc; }
            set { _ballsetuseddesc = value; }
        }

        public string BallSetUsed
        {
            get { return _ballsetused; }
            set { _ballsetused = value; }
        }

        public string DrawNumberDesc
        {
            get { return _drawnumberdesc; }
            set { _drawnumberdesc = value; }
        }

        public string DrawNumber
        {
            get { return _drawnumber; }
            set { _drawnumber = value; }
        }
    }

    public class LottoCard
    {
        private int _bonus;
        private int _sortedbonus;
        private int[] _numbers;
        private int[] _sortednumbers;
        private DateTime _gamedate;
        private string _gamedatestr;
        private string _headinglatestresults;
        private string _headingwinnumbers;
        private string _headingsortedwinnumbers;
        private string _headingbonus;
        private string _headingsortedbonus;
        private LottoPlus _lottoPlus;
        private bool _includeLottoPlus;

        private DivisionalPayout _payout;
        private DrawStatistics _stats;

        public LottoCard(Boolean IncludeLottoPlus)
        {
            Initialise(IncludeLottoPlus);
        }

        public LottoCard(HTMLParsedResults ParsedHTML, string CardType)
        {
            if (CardType == "Lotto")
            {
                InitialiseLotto();
                LoadLottoFromParsedHTML(ParsedHTML);

            }
            else if (CardType == "LottoPlus")
            {
                InitialiseLottoPlus();
                LoadLottoPlusFromParsedHTML(ParsedHTML);
            }
            else
                Initialise(IncludeLottoPlus);
        }

        public LottoCard(HTMLParsedResults ParsedLottoHTML, HTMLParsedResults ParsedLottoPlusHTML, Boolean IncludeLottoPlus)
        {
            Initialise(IncludeLottoPlus);
            LoadFromParsedHTML(ParsedLottoHTML, ParsedLottoPlusHTML, IncludeLottoPlus);
        }

        public Boolean IncludeLottoPlus
        {
            get { return _includeLottoPlus; }
            set { _includeLottoPlus = value; }
        }

        public LottoPlus LottoPlus
        {
            get { return _lottoPlus; }
            set { _lottoPlus = value; }
        }
        
        public DateTime GameDate
        {
            get { return _gamedate; }
            set { _gamedate = value; }
        }

        public string GameDateStr
        {
            get { return _gamedatestr; }
            set { _gamedatestr = value; }
        }
        public string HeadingLastestResults
        {
            get { return _headinglatestresults; }
            set { _headinglatestresults = value; }
        }

        public string HeadingWinningNumbers
        {
            get { return _headingwinnumbers; }
            set { _headingwinnumbers = value; }
        }

        public string HeadingSortedWinningNumbers
        {
            get { return _headingsortedwinnumbers; }
            set { _headingsortedwinnumbers = value; }
        }

        public string HeadingBonus
        {
            get { return _headingbonus; }
            set { _headingbonus = value; }
        }

        public string HeadingSortedBonus
        {
            get { return _headingsortedbonus; }
            set { _headingsortedbonus = value; }
        }

        private void SetLottoCard(HTMLParsedResults ParsedLottoHTML)
        {
            this.Bonus = ParsedLottoHTML.Bonus;
            this.SortedBonus = ParsedLottoHTML.SortedBonus;
            this.GameDateStr = ParsedLottoHTML.GameDate;
            this.GameDate = Convert.ToDateTime(ParsedLottoHTML.GameDate);
            this._headingbonus = ParsedLottoHTML.HeadingBonus;
            this._headinglatestresults = ParsedLottoHTML.HeadingLastestResults;
            this._headingsortedbonus = ParsedLottoHTML.HeadingSortedBonus;
            this._headingsortedwinnumbers = ParsedLottoHTML.HeadingSortedWinningNumbers;
            this._headingwinnumbers = ParsedLottoHTML.HeadingWinningNumbers;
            this._numbers = (int[])ParsedLottoHTML.Numbers.ToArray(typeof(int));
            this._sortedbonus = ParsedLottoHTML.SortedBonus;
            this._sortednumbers = (int[])ParsedLottoHTML.SortedNumbers.ToArray(typeof(int));
            this._payout = ParsedLottoHTML.DivPayouts;
            this._stats = ParsedLottoHTML.DrawStatistics;
            this._includeLottoPlus = IncludeLottoPlus;
        }

        private void SetLottoPlusCard(HTMLParsedResults ParsedLottoPlusHTML)
        {
            this._lottoPlus.GameBonus = ParsedLottoPlusHTML.Bonus;
            this._lottoPlus.GameSortedBonus = ParsedLottoPlusHTML.SortedBonus;
            this._lottoPlus.GameDateStr = ParsedLottoPlusHTML.GameDate;
            this._lottoPlus.GameDate = Convert.ToDateTime(ParsedLottoPlusHTML.GameDate);
            this._lottoPlus.HeadingBonus = ParsedLottoPlusHTML.HeadingBonus;
            this._lottoPlus.HeadingLastestResults = ParsedLottoPlusHTML.HeadingLastestResults;
            this._lottoPlus.HeadingSortedBonus = ParsedLottoPlusHTML.HeadingSortedBonus;
            this._lottoPlus.HeadingSortedWinningNumbers = ParsedLottoPlusHTML.HeadingSortedWinningNumbers;
            this._lottoPlus.HeadingWinningNumbers = ParsedLottoPlusHTML.HeadingWinningNumbers;
            this._lottoPlus.Numbers = (int[])ParsedLottoPlusHTML.Numbers.ToArray(typeof(int));
            this._lottoPlus.SortedNumbers = (int[])ParsedLottoPlusHTML.SortedNumbers.ToArray(typeof(int));
            this.LottoPlus.DivisionalPayouts = ParsedLottoPlusHTML.DivPayouts;
            this.LottoPlus.DrawStatistics = ParsedLottoPlusHTML.DrawStatistics;
        }

        private void LoadLottoFromParsedHTML(HTMLParsedResults ParsedLottoHTML)
        {
            SetLottoCard(ParsedLottoHTML);
        }

        private void LoadLottoPlusFromParsedHTML(HTMLParsedResults ParsedLottoPlusHTML)
        {
            SetLottoPlusCard(ParsedLottoPlusHTML);
        }


        private void LoadFromParsedHTML(HTMLParsedResults ParsedLottoHTML, HTMLParsedResults ParsedLottoPlusHTML, Boolean IncludeLottoPlus)
        {
            SetLottoCard(ParsedLottoHTML);

            if (IncludeLottoPlus)
            {
                SetLottoPlusCard(ParsedLottoPlusHTML);
            }
        }

        private void InitialiseLotto()
        {
            _numbers = new Int32[6];
            _sortednumbers = new Int32[6];
            _bonus = 0;
            _sortedbonus = 0;
            _gamedatestr = "";
            _headinglatestresults = "";
            _headingwinnumbers = "";
            _headingsortedwinnumbers = "";
            _headingbonus = "";
            _headingsortedbonus = "";
            _payout = new DivisionalPayout();
            _stats = new DrawStatistics();
        }

        private void InitialiseLottoPlus()
        {
            _includeLottoPlus = true;
            _lottoPlus = new LottoPlus();
        }

        private void Initialise(Boolean IncludeLottoPlus)
        {
            InitialiseLotto();
            if (IncludeLottoPlus)
            {
                InitialiseLottoPlus();
            }
        }

        public int Bonus
        {
            get 
            { 
                return _bonus; 
            }
            set 
            { 
                _bonus = value;
            }
        }

        public int SortedBonus
        {
            get { return _sortedbonus; }
            set { _sortedbonus = value; }
        }

        public int[] Numbers
        {
            get { return _numbers; }
            set { _numbers = value; }
        }

        public int[] SortedNumbers
        {
            get { return _sortednumbers; }
            set { _sortednumbers = value; }
        }

        public DivisionalPayout DivionalPayouts
        {
            get { return _payout; }
            set { _payout = value; }
        }

        public DrawStatistics DrawStatistics
        {
            get { return _stats; }
            set { _stats = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ttpim.gamemodule.games.models.lotto
{
    public class LottoPlusDivision
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

    public class LottoPlusDivisionalPayout
    {
        private List<LottoPlusDivision> _payouts;
        public LottoPlusDivisionalPayout()
        {
            _payouts = new List<LottoPlusDivision>();
        }

        public List<LottoPlusDivision> PayoutList
        {
            get { return _payouts; }
            set { _payouts = value; }
        }

        public void AddPayout(LottoPlusDivision divpayout)
        {
            _payouts.Add(divpayout);
        }

        public void RemovePayout(LottoPlusDivision divpayout)
        {
            _payouts.Remove(divpayout);
        }
    }

    public class LottoPlusDrawStatistics
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

    public class LottoPlus
    {
        private int _bonus;
        private int _sortedbonus;
        private int[] _numbers;
        private int[] _sortednumbers;
        private Image _gameimage;
        private string _gamedatestr;
        private DateTime _gamedate;
        private string _gamename;
        private string _headinglatestresults;
        private string _headingwinnumbers;
        private string _headingsortedwinnumbers;
        private string _headingbonus;
        private string _headingsortedbonus;
        private DivisionalPayout _payout;
        private DrawStatistics _stats;

        public LottoPlus()
        {
            Initialise();
        }

        public Image GameImage
        {
            get { return _gameimage; }
            set { _gameimage = value; }
        }

        public string GameName
        {
            get 
            { 
                return _gamename; 
            }
            set 
            { 
                _gamename = value; 
            }
        }

        public string GameDateStr
        {
            get 
            { 
                return _gamedatestr; 
            }
            set 
            { 
                _gamedatestr = value; 
            }
        }

        public DateTime GameDate
        {
            get
            {
                return _gamedate;
            }
            set
            {
                _gamedate = value;
            }
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

        private void Initialise()
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

        public int GameBonus
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

        public int GameSortedBonus
        {
            get 
            { 
                return _sortedbonus; 
            }
            set 
            { 
                _sortedbonus = value; 
            }
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

        public DivisionalPayout DivisionalPayouts
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

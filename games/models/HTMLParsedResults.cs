using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;

namespace ttpim.gamemodule.games.models
{
    public class Division
    {
        private string _division;
        private int _nrofwinners;
        private string _payoutperwinner;

        public string Name
        {
            get 
            { 
                return _division; 
            }
            set 
            { 
                _division = value; 
            }
        }

        public int NumberOfWinners
        {
            get 
            { 
                return _nrofwinners; 
            }
            set 
            {
                _nrofwinners = value; 
            }
        }

        public string PayoutPerWinner
        {
            get 
            { 
                return _payoutperwinner; 
            }
            set
            { 
                _payoutperwinner = value; 
            }
        }
    }

    public class DivisionalPayout
    {
        private List<Division> _payouts;
        private int _divisionNumber;

        public DivisionalPayout()
        {
            _payouts = new List<Division>();
        }

        public List<Division> PayoutList
        {
            get 
            { 
                return _payouts; 
            }
            set 
            { 
                _payouts = value; 
            }
        }

        public void AddPayout(Division divpayout)
        {
            _payouts.Add(divpayout);
        }

        public void RemovePayout(Division divpayout)
        {
            _payouts.Remove(divpayout);
        }

        public int DivisionNumber
        {
            get
            {
                return _divisionNumber;
            }
            set
            {
                _divisionNumber = value;
            }
        }
    }

    public class DrawStatistics
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

    public class HTMLParsedResults
    {
        private int _bonus;
        private int _sortedbonus;
        private ArrayList _numbers;
        private ArrayList _sortednumbers;
        private Image _gameimage;
        private Image _logoimage;
        private string _gamedate;
        private string _gamename;
        private string _headinglatestresults;
        private string _headingwinnumbers;
        private string _headingsortedwinnumbers;
        private string _headingbonus;
        private string _headingsortedbonus;
        private DivisionalPayout _payout;
        private DrawStatistics _stats;
        private Dictionary<string, Image> _winimages;
        private Dictionary<string, Image> _sortedwinimages;
        private Dictionary<string, string> _errors;

        public HTMLParsedResults()
        {
            Initialise();
        }

        private void Initialise()
        {
            _numbers = new ArrayList();
            _sortednumbers = new ArrayList();
            _gamename = String.Empty;
            _gameimage = null;
            _logoimage = null;
            _bonus = 0;
            _sortedbonus = 0;
            _gamedate = String.Empty;
            _headinglatestresults = String.Empty;
            _headingwinnumbers = String.Empty;
            _headingsortedwinnumbers = String.Empty;
            _headingbonus = String.Empty;
            _headingsortedbonus = String.Empty;
            _payout = new DivisionalPayout();
            _stats = new DrawStatistics();
        }

        public Image GameImage
        {
            get { return _gameimage; }
            set { _gameimage = value; }
        }

        public Image LogoImage
        {
            get 
            { 
                return _logoimage; 
            }
            set 
            { 
                _logoimage = value; 
            }
        }

        public string GameName
        {
            get { return _gamename; }
            set { _gamename = value; }
        }

        public string GameDate
        {
            get { return _gamedate; }
            set { _gamedate = value; }
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

        public int Bonus
        {
            get { return _bonus; }
            set { _bonus = value; }
        }

        public int SortedBonus
        {
            get { return _sortedbonus; }
            set { _sortedbonus = value; }
        }

        public ArrayList Numbers
        {
            get { return _numbers; }
            set { _numbers = value; }
        }

        public ArrayList SortedNumbers
        {
            get { return _sortednumbers; }
            set { _sortednumbers = value; }
        }

        public DivisionalPayout DivPayouts
        {
            get { return _payout; }
            set { _payout = value; }
        }

        public DrawStatistics DrawStatistics
        {
            get { return _stats; }
            set { _stats = value; }
        }

        public Dictionary<string, Image> WinImages
        {
            get { return _winimages; }
            set { _winimages = value; }
        }

        public Dictionary<string, Image> SortedWinImages
        {
            get { return _sortedwinimages; }
            set { _sortedwinimages = value; }
        }

        public Dictionary<string, string> Errors
        {
            get 
            { 
                return _errors; 
            }
            set 
            { 
                _errors = value; 
            }
        }
    }
}

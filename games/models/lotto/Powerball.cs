﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ttpim.gamemodule.games.models.powerball
{
    public class PowerballDivision
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

    public class PowerballDivisionalPayout
    {
        private List<PowerballDivision> _payouts;
        public PowerballDivisionalPayout()
        {
            _payouts = new List<PowerballDivision>();
        }

        public List<PowerballDivision> PayoutList
        {
            get { return _payouts; }
            set { _payouts = value; }
        }

        public void AddPayout(PowerballDivision divpayout)
        {
            _payouts.Add(divpayout);
        }

        public void RemovePayout(PowerballDivision divpayout)
        {
            _payouts.Remove(divpayout);
        }
    }

    public class PowerballDrawStatistics
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

    public class Powerball
    {
        private int _powerball;
        private int _sortedpowerball;
        private int[] _numbers;
        private int[] _sortednumbers;
        private string _gamedate;
        private string _headinglatestresults;
        private string _headingwinnumbers;
        private string _headingsortedwinnumbers;
        private string _headingbonus;
        private string _headingsortedbonus;

        private DivisionalPayout _payout;
        private DrawStatistics _stats;

        public Powerball()
        {
            Initialise();
        }

        public int SortedPowerball
        {
            get { return _sortedpowerball; }
            set { _sortedpowerball = value; }
        }

        public int PowerballBonus
        {
            get { return _powerball; }
            set { _powerball = value; }
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

        private void Initialise()
        {
            _numbers = new Int32[5];
            _sortednumbers = new Int32[5];
            _powerball = 0;
            _sortedpowerball = 0;
            _gamedate = "";
            _headinglatestresults = "";
            _headingwinnumbers = "";
            _headingsortedwinnumbers = "";
            _headingbonus = "";
            _headingsortedbonus = "";
            _payout = new DivisionalPayout();
            _stats = new DrawStatistics();
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ttpim.gamemodule.lotto.controllers
{
    public static class CommonController
    {
        private static string _escapeToken = String.Empty;
        private static string _startLottoParseToken = String.Empty;
        private static string _endLottoParseToken = String.Empty;
        private static string _startLottoPlusParseToken = String.Empty;
        private static string _endLottoPlusParseToken = String.Empty;
        private static string _startPowerballParseToken = String.Empty;
        private static string _endPowerballParseToken = String.Empty;
        private static string _bonusPowerballParseToken = String.Empty;

        

        public static string EscapeToken
        {
            get 
            {
                return @"\\";
            }
        }

        public static string DotToken
        {
            get
            {
                return @".";
            }
        }


        public static string DefaultApplicationBriefcaseDirName
        {
            get
            {
                return @"Nerdi.Briefcase";
            }
        }

        public static string LottoURL
        {
            get
            {
                return @"https:///www.nationallottery.co.za//lotto_home//results.asp?type=1";
            }
        }

        public static string LottoPlusURL
        {
            get
            {
                return @"https:///www.nationallottery.co.za//lotto_home//results.asp?type=2";
            }
        }

        public static string PowerballURL
        {
            get
            {
                return @"https:///www.nationallottery.co.za//powerball_home//results.asp?type=1";
            }
        }

        public static string PowerballFilePreFix
        {
            get
            {
                return @"Powerball_";
            }
        }

        public static string UserFirstName
        {
            get
            {
                return @"Neil";
            }
        }

        public static string UserLastName
        {
            get
            {
                return @"Slambert";
            }
        }
        public static string LottoDownloadBaseDirName
        {
            get
            {
                return @"Lotto\\LottoWebPages";
            }
        }

        public static string StartLottoParseToken
        {
            get 
            {
                return _startLottoParseToken;
            }
            set 
            {
                _startLottoParseToken = value;
            }
        }

        public static string EndLottoParseToken
        {
            get
            {
                return _endLottoParseToken;
            }
            set
            {
                _endLottoParseToken = value;
            }
        }

        public static string StartLottoPlusParseToken
        {
            get
            {
                return _startLottoPlusParseToken;
            }
            set
            {
                _startLottoPlusParseToken = value;
            }
        }

        public static string EndLottoPlusParseToken
        {
            get
            {
                return _endLottoPlusParseToken;
            }
            set
            {
                _endLottoPlusParseToken = value;
            }
        }

        public static string StartPowerballParseToken
        {
            get
            {
                return _startPowerballParseToken;
            }
            set
            {
                _startPowerballParseToken = value;
            }
        }

        public static string EndPowerballParseToken
        {
            get
            {
                return _endPowerballParseToken;
            }
            set
            {
                _endPowerballParseToken = value;
            }
        }

        public static string BonusPowerballParseToken
        {
            get
            {
                return _bonusPowerballParseToken;
            }
            set
            {
                _bonusPowerballParseToken = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using System.Net;
using System.Data;
using ttpim.gamemodule.common;
using ttpim.gamemodule.database;
using ttpim.gamemodule.games.models;
using ttpim.gamemodule.games.views;
using ttpim.gamemodule.games.controllers;
using ttpim.gamemodule.games.models.lotto;
using ttpim.gamemodule.games.controllers.lotto;
using ttpim.gamemodule.games.models.powerball;
using System.Drawing;

namespace ttpim.gamemodule.lotto.controllers
{
    public class LottoCardServiceManager
    {
        private LottoSetupServiceManager _servicemanager;
        private string _lottoURL;
        private string _lottoPlusURL;
        private string _powerballURL;
        private string _lottoBaseDirName;
        private string _lottoPlusBaseDirName;
        private string _powerballBaseDirName;
        
        private string _lottoBallsDirName;
        private string _lottoPlusBallsDirName;
        private string _powerballBallsDirName;

        private string _lottoImagesDirName;
        private string _lottoPlusImagesDirName;
        private string _powerballImagesDirName;

        private string _lottoCSSDirName;
        private string _lottoPlusCSSDirName;
        private string _powerballCSSDirName;

        private string _lottoScriptsDirName;
        private string _lottoPlusScriptsDirName;
        private string _powerballScriptsDirName;

        private string _lottoJavascriptDirName;
        private string _lottoPlusJavascriptDirName;
        private string _powerballJavascriptDirName;

        private string _lottoIncDirName;
        private string _lottoPlusIncDirName;
        private string _powerballIncDirName;

        private string _lottoFilePrefix;
        private string _lottoPlusFilePrefix;
        private string _powerballFilePrefix;

        private string _lottoSavePath;
        private string _lottoPlusSavePath;
        private string _lottoDownloadDirPath;
        private string _lottoPlusDownloadDirPath;
        private string _powerballDownloadDirPath;
        private string _lottoDownloadedFilePath;
        private string _lottoPlusDownloadedFilePath;
        private string _powerballDownloadedFilePath;
        private string _lottoFolderDir;
        private string _lottoPlusFolderDir;
        private string _powerballFolderDir;
        private string _lottoImagesDir;
        private string _lottoPlusImagesDir;
        private string _powerballImagesDir;
        private string _lottoBallsDir;
        private string _lottoPlusBallsDir;
        private string _powerballBallsDir;
        private string _lottoCSSDir;
        private string _lottoPlusCSSDir;
        private string _powerballCSSDir;
        
        private string _lottoScriptsDir;
        private string _lottoPlusScriptsDir;
        private string _powerballScriptsDir;

        private string _lottoJSDir;
        private string _lottoPlusJSDir;
        private string _powerballJSDir;

        private string _lottoIncDir;
        private string _lottoPlusIncDir;
        private string _powerballIncDir;
        private string _appDataRoot;
        private string _appBriefcasePath;
        private string _appBriefcaseDirName;
        private string _userFirstName;
        private string _userLastName;


        private Boolean _includeLottoPlus;
        private Boolean _lottoPlusDownloadCompleted;
        private Boolean _lottoDownloadCompleted;
        private Boolean _powerballDownloadCompleted;
        private Boolean _winLottoBoardsSaved;
        private Boolean _winLottoPlusBoardsSaved;
        private Boolean _winPowerballBoardsSaved;

        private int _numLottoWinsSaved;
        private int _numLottoPlusWinsSaved;
        private int _numPowerballWinsSaved;

        private string _lottoWinsSavedMessage;
        private string _lottoPlusWinsSavedMessage;
        private string _powerballWinsSavedMessage;

        private HTMLParser _lottoParser;
        private HTMLParser _lottoPlusParser;
        private HTMLParser _powerballParser;
        private HTMLParsedResults _lottoResults;
        private HTMLParsedResults _lottoPlusResults;
        private HTMLParsedResults _powerballResults;
        private LottoGameResults _lottoGameResults;
        private LottoGameResults _lottoPlusGameResults;
        private PowerballGameResults _powerballGameResults;
        private Thread _lottoThread;
        private Thread _lottoPlusThread;
        private Thread _powerballThread;
        private Thread _lottoWebpageCaptureThread;
        private Thread _lottoPlusWebpageCaptureThread;
        private Thread _powerballWebpageCaptureThread;

        private Uri _baseUri;
        private Uri _lottoUri;
        private Uri _lottoPlusUri;
        private Uri _powerballUri;
        private Bitmap _lottoBitmap;
        private Bitmap _lottoPlusBitmap;
        private Bitmap _powerballBitmap;
        private DataGridViewStyles _lottoGridViewStyle;
        private DataGridViewStyles _lottoPlusGridViewStyle;
        private DataGridViewStyles _powerballGridViewStyle;
        private List<string> _lottoPresentationDataList;
        private List<string> _lottoPlusPresentationDataList;
        private List<string> _powerballPresentationDataList;

        public event LottoStartDownloadNotification LottoStartDownloadEvent;
        public event LottoFinishDownloadNotification LottoFinishDownloadEvent;
        public event LottoPlusStartDownloadNotification LottoPlusStartDownloadEvent;
        public event LottoPlusFinishDownloadNotification LottoPlusFinishDownloadEvent;
        public event LottoCardProcessedNotification LottoCardProcessedEvent;
        public event LottoCardInProcessingNotification LottoCardInProcessingEvent;

        public event LottoParseExceptionNotification LottoParseExceptionEvent;
        public event LottoPlusParseExceptionNotification LottoPlusParseExceptionEvent;
        public event LottoNavigationNotification LottoNavigationEvent;
        public event LottoPlusNavigationNotification LottoPlusNavigationEvent;
        public event PowerballStartDownloadNotification PowerballStartDownloadEvent;
        public event PowerballFinishDownloadNotification PowerballFinishDownloadEvent;
        public event LottoCardSavedWinningsNotification LottoSavedWinningsEvent;
        public event LottoCardSavedWinningsNotification LottoPlusSavedWinningsEvent;
        public event LottoCardSavedWinningsNotification PowerballSavedWinningsEvent;

        private delegate int SaveLottoWinningDelegate(List<LottoGameBoard> Winnings, string Game);
        private delegate int SaveLottoPlusWinningDelegate(List<LottoGameBoard> Winnings, string Game);
        private delegate int SavePowerballWinningDelegate(List<PowerballGameBoard> Winnings, string Game);

        private bool LottoWebpageBitmapGenerated;

        private delegate Bitmap GenerateWebpageBitmapDelegate(string data);
        private Boolean _isDataProviderAvailable;
        private string _datasourcePath;
        private SetupData _connectionSetup;

        public string DatasourcePath
        {
            get
            {
                return _datasourcePath;
            }
            set
            {
                _datasourcePath = value;
            }
        }

        public Boolean IsDataProviderAvailable
        {
            get
            {
                return _isDataProviderAvailable;
            }
            set
            {
                _isDataProviderAvailable = value;
            }
        }

        public SetupData ConnectionSetup
        {
            get
            {
                return _connectionSetup;
            }
            set
            {
                _connectionSetup = value;
            }
        }

        public LottoSetupServiceManager SetupServiceManager
        {
            get
            {
                return _servicemanager;
            }
            set
            {
                _servicemanager = value;
            }
        }

        #region Constructors
        public LottoCardServiceManager()
        {
            EstablishDataProviderConnection();
        }

        public void IntialiseSetup(SetupData setup)
        {
            _connectionSetup = setup;
            _servicemanager = null;
            _servicemanager = new LottoSetupServiceManager(setup);
            IntialiseSetup();
        }

        private void EstablishDataProviderConnection()
        {
            this.SetupServiceManager = new LottoSetupServiceManager();
            if (_servicemanager.IsDataProviderAvailable)
            {
                _isDataProviderAvailable = true;
            }
            else
            {
                _isDataProviderAvailable = false;
            }
        }

        public void IntialiseSetup()
        {
            LottoURL = _servicemanager.GetURL("Lotto");
            if (String.IsNullOrWhiteSpace(LottoURL))
            {
                LottoURL = CommonController.LottoURL;
                _servicemanager.UpdateSetup("Lotto", CommonController.LottoURL);
            }

            LottoPlusURL = _servicemanager.GetURL("LottoPlus");
            if (String.IsNullOrWhiteSpace(LottoURL))
            {
                LottoPlusURL = CommonController.LottoPlusURL;
                _servicemanager.UpdateSetup("LottoPlus", CommonController.LottoPlusURL);
            }

            PowerballURL = _servicemanager.GetURL("Powerball");
            if (String.IsNullOrWhiteSpace(PowerballURL))
            {
                PowerballURL = CommonController.PowerballURL;
                _servicemanager.UpdateSetup("Powerball", CommonController.PowerballURL);
            }

            this.LottoURI = new Uri(this.LottoURL);
            this.LottoPlusURI = new Uri(this.LottoPlusURL);
            this.PowerballURI = new Uri(this.PowerballURL);
            this.BaseURI = _servicemanager.GetBaseURI("BaseUri");

            LottoDownloadDirName = _servicemanager.GetSetting("LottoDownloadBaseDirName");
            if (String.IsNullOrWhiteSpace(LottoDownloadDirName))
            {
                LottoDownloadDirName = CommonController.LottoDownloadBaseDirName;
                _servicemanager.UpdateSetup("LottoDownloadBaseDirName", CommonController.LottoDownloadBaseDirName);
            }

            this.LottoPlusDownloadDirName = _servicemanager.GetSetting("LottoPlusDownloadBaseDirName");
            this.PowerballDownloadDirName = _servicemanager.GetSetting("PowerballDownloadBaseDirName");

            this.LottoBallsDirName = _servicemanager.GetSetting("BallsDirName");
            this.LottoPlusBallsDirName = _servicemanager.GetSetting("BallsDirName");
            this.PowerballBallsDirName = _servicemanager.GetSetting("PowerballDirName");

            this.LottoImagesDirName = _servicemanager.GetSetting("ImagesDirName");
            this.LottoPlusImagesDirName = _servicemanager.GetSetting("ImagesDirName");
            this.PowerballImagesDirName = _servicemanager.GetSetting("ImagesDirName");

            this.LottoCSSDirName = _servicemanager.GetSetting("CSSDirName");
            this.LottoPlusCSSDirName = _servicemanager.GetSetting("CSSDirName");
            this.PowerballCSSDirName = _servicemanager.GetSetting("CSSDirName");

            this.LottoScriptsDirName = _servicemanager.GetSetting("ScriptsDirName");
            this.LottoPlusScriptsDirName = _servicemanager.GetSetting("ScriptsDirName");
            this.PowerballScriptsDirName = _servicemanager.GetSetting("ScriptsDirName");

            this.LottoJavascriptDirName = _servicemanager.GetSetting("JavascriptDirName");
            this.LottoPlusJavascriptDirName = _servicemanager.GetSetting("JavascriptDirName");
            this.PowerballJavascriptDirName = _servicemanager.GetSetting("JavascriptDirName");

            this.LottoIncDirName = _servicemanager.GetSetting("IncDirName");
            this.LottoPlusIncDirName = _servicemanager.GetSetting("IncDirName");
            this.PowerballIncDirName = _servicemanager.GetSetting("IncDirName");

            this.LottoFilePrefix = _servicemanager.GetSetting("LottoFilePreFix");
            this.LottoPlusFilePrefix = _servicemanager.GetSetting("LottoPlusFilePreFix");

            PowerballFilePrefix = _servicemanager.GetSetting("PowerballFilePreFix");
            if (String.IsNullOrWhiteSpace(PowerballFilePrefix))
            {
                PowerballFilePrefix = CommonController.PowerballFilePreFix;
                _servicemanager.UpdateSetting("PowerballFilePreFix", CommonController.PowerballFilePreFix);
            }

            if (String.IsNullOrWhiteSpace(_servicemanager.GetSetting("ApplicationDataRoot")))
            {
                ApplicationDataRoot = Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + CommonController.EscapeToken;
                _servicemanager.UpdateSetting("ApplicationDataRoot", ApplicationDataRoot);
            }
            else
            {
                ApplicationDataRoot = _servicemanager.GetSetting("ApplicationDataRoot");
            }

            UserFirstName = _servicemanager.GetSetting("UserFirstName");
            if (String.IsNullOrWhiteSpace(UserFirstName))
            {
                UserFirstName = CommonController.UserFirstName;
                _servicemanager.UpdateSetting("UserFirstName", CommonController.UserFirstName);
            }

            UserLastName = _servicemanager.GetSetting("UserLastName");
            if (String.IsNullOrWhiteSpace(UserLastName))
            {
                UserLastName = CommonController.UserLastName;
                _servicemanager.UpdateSetting("UserLastName", CommonController.UserLastName);
            }

            if (String.IsNullOrWhiteSpace(_servicemanager.GetSetting("ApplicationBriefcaseDirName")))
            {
                ApplicationBriefcaseDirName = CommonController.DefaultApplicationBriefcaseDirName + CommonController.DotToken + CommonController.UserFirstName[0] + CommonController.UserLastName;
                _servicemanager.UpdateSetting("ApplicationBriefcaseDirName", ApplicationBriefcaseDirName);
            }
            else
            {
                ApplicationBriefcaseDirName = _servicemanager.GetSetting("ApplicationBriefcaseDirName");
            }

            if (String.IsNullOrWhiteSpace(_servicemanager.GetSetting("ApplicationBriefcasePath")))
            {
                ApplicationBriefcasePath = ApplicationDataRoot + ApplicationBriefcaseDirName + CommonController.EscapeToken;
                _servicemanager.UpdateSetting("ApplicationBriefcasePath", ApplicationBriefcasePath);
            }
            else
            {
                ApplicationBriefcasePath = _servicemanager.GetSetting("ApplicationBriefcasePath");
            }

            CreateHTMLParsers();
        }

        private void CreateHTMLParsers()
        {
            LottoParser = new HTMLParser();
            LottoPlusParser = new HTMLParser();
            PowerballParser = new HTMLParser();
        }

        public LottoCardServiceManager(Boolean IncludeLottoPlus)
        {
            _includeLottoPlus = IncludeLottoPlus;
        }
        #endregion

        #region Properties

        public string UserFirstName
        {
            get
            {
                return _userFirstName;
            }
            set
            {
                _userFirstName = value;
            }
        }

        public string UserLastName
        {
            get
            {
                return _userLastName;
            }
            set
            {
                _userLastName = value;
            }
        }

        public string ApplicationDataRoot
        {
            get 
            {
                return _appDataRoot;
            }
            set 
            {
                _appDataRoot = value;
            }
        }

        public string ApplicationBriefcaseDirName
        {
            get
            {
                return _appBriefcaseDirName;
            }
            set
            {
                _appBriefcaseDirName = value;
            }
        }

        public string ApplicationBriefcasePath
        {
            get
            {
                return _appBriefcasePath;
            }
            set
            {
                _appBriefcasePath = value;
            }
        }

        public string LottoBallsDirName
        {
            get
            {
                return _lottoBallsDirName;
            }
            set
            {
                _lottoBallsDirName = value;
            }
        }

        public string LottoPlusBallsDirName
        {
            get
            {
                return _lottoPlusBallsDirName;
            }
            set
            {
                _lottoPlusBallsDirName = value;
            }
        }

        public string PowerballBallsDirName
        {
            get
            {
                return _powerballBallsDirName;
            }
            set
            {
                _powerballBallsDirName = value;
            }
        }

        public string LottoImagesDirName
        {
            get
            {
                return _lottoImagesDirName;
            }
            set
            {
                _lottoImagesDirName = value;
            }
        }

        public string LottoPlusImagesDirName
        {
            get
            {
                return _lottoPlusImagesDirName;
            }
            set
            {
                _lottoPlusImagesDirName = value;
            }
        }

        public string PowerballImagesDirName
        {
            get
            {
                return _powerballImagesDirName;
            }
            set
            {
                _powerballImagesDirName = value;
            }
        }

        public string LottoCSSDirName
        {
            get
            {
                return _lottoCSSDirName;
            }
            set
            {
                _lottoCSSDirName = value;
            }
        }

        public string LottoPlusCSSDirName
        {
            get
            {
                return _lottoPlusCSSDirName;
            }
            set
            {
                _lottoPlusCSSDirName = value;
            }
        }

        public string PowerballCSSDirName
        {
            get
            {
                return _powerballCSSDirName;
            }
            set
            {
                _powerballCSSDirName = value;
            }
        }

        public string LottoJavascriptDirName
        {
            get
            {
                return _lottoJavascriptDirName;
            }
            set
            {
                _lottoJavascriptDirName = value;
            }
        }

        public string LottoPlusJavascriptDirName
        {
            get
            {
                return _lottoPlusJavascriptDirName;
            }
            set
            {
                _lottoPlusJavascriptDirName = value;
            }
        }

        public string PowerballJavascriptDirName
        {
            get
            {
                return _powerballJavascriptDirName;
            }
            set
            {
                _powerballJavascriptDirName = value;
            }
        }

        public string LottoScriptsDirName
        {
            get
            {
                return _lottoScriptsDirName;
            }
            set
            {
                _lottoScriptsDirName = value;
            }
        }

        public string LottoPlusScriptsDirName
        {
            get
            {
                return _lottoPlusScriptsDirName;
            }
            set
            {
                _lottoPlusScriptsDirName = value;
            }
        }

        public string PowerballScriptsDirName
        {
            get
            {
                return _powerballScriptsDirName;
            }
            set
            {
                _powerballScriptsDirName = value;
            }
        }

        public string LottoIncDirName
        {
            get
            {
                return _lottoIncDirName;
            }
            set
            {
                _lottoIncDirName = value;
            }
        }

        public string LottoPlusIncDirName
        {
            get
            {
                return _lottoPlusIncDirName;
            }
            set
            {
                _lottoPlusIncDirName = value;
            }
        }

        public string PowerballIncDirName
        {
            get
            {
                return _powerballIncDirName;
            }
            set
            {
                _powerballIncDirName = value;
            }
        }

        public Boolean IncludeLottoPlus
        {
            get
            {
                return _includeLottoPlus;
            }
            set
            {
                _includeLottoPlus = value;
            }
        }

        public Boolean LottoPlusDownloadCompleted
        {
            get
            {
                return _lottoPlusDownloadCompleted;
            }
            set
            {
                _lottoPlusDownloadCompleted = value;
            }
        }

        public Boolean LottoDownloadCompleted
        {
            get
            {
                return _lottoDownloadCompleted;
            }
            set
            {
                _lottoDownloadCompleted = value;
            }
        }

        public Boolean PowerballDownloadCompleted
        {
            get
            {
                return _powerballDownloadCompleted;
            }
            set
            {
                _powerballDownloadCompleted = value;
            }
        }

        public string LottoURL
        {
            get 
            {
                return _lottoURL;
            }
            set
            {
                _lottoURL = value;
            }
        }

        public string PowerballURL
        {
            get
            {
                return _powerballURL;
            }
            set
            {
                _powerballURL = value;
            }
        }

        public string LottoSavePath
        {
            get
            {
                return _lottoSavePath;
            }
            set
            {
                _lottoSavePath = value;
            }
        }

        public string LottoPlusURL
        {
            get
            {
                return _lottoPlusURL;
            }
            set
            {
                _lottoPlusURL = value;
            }
        }

        public string LottoPlusSavePath
        {
            get
            {
                return _lottoPlusSavePath;
            }
            set
            {
                _lottoPlusSavePath = value;
            }
        }

        public HTMLParser LottoParser
        {
            get
            {
                return _lottoParser;
            }
            set
            {
                _lottoParser = value;
            }
        }

        public HTMLParser LottoPlusParser
        {
            get
            {
                return _lottoPlusParser;
            }
            set
            {
                _lottoPlusParser = value;
            }
        }

        public HTMLParser PowerballParser
        {
            get
            {
                return _powerballParser;
            }
            set
            {
                _powerballParser = value;
            }
        }

        public HTMLParsedResults LottoResults
        {
            get
            {
                return _lottoResults;
            }
            set
            {
                _lottoResults = value;
            }
        }

        public HTMLParsedResults LottoPlusResults
        {
            get
            {
                return _lottoPlusResults;
            }
            set
            {
                _lottoPlusResults = value;
            }
        }

        public HTMLParsedResults PowerballResults
        {
            get
            {
                return _powerballResults;
            }
            set
            {
                _powerballResults = value;
            }
        }

        public LottoGameResults LottoGameResults
        {
            get
            {
                return _lottoGameResults;
            }
            set
            {
                _lottoGameResults = value;
            }
        }

        public LottoGameResults LottoPlusGameResults
        {
            get
            {
                return _lottoPlusGameResults;
            }
            set
            {
                _lottoPlusGameResults = value;
            }
        }

        public PowerballGameResults PowerballGameResults
        {
            get
            {
                return _powerballGameResults;
            }
            set
            {
                _powerballGameResults = value;
            }
        }

        public Thread LottoThread
        {
            get
            {
                return _lottoThread;
            }
            set
            {
                _lottoThread = value;
            }
        }

        public Thread LottoPlusThread
        {
            get
            {
                return _lottoPlusThread;
            }
            set
            {
                _lottoPlusThread = value;
            }
        }

        public Thread PowerballThread
        {
            get
            {
                return _powerballThread;
            }
            set
            {
                _powerballThread = value;
            }
        }

        public Thread LottoWebPageCaptureThread
        {
            get
            {
                return _lottoWebpageCaptureThread;
            }
            set
            {
                _lottoWebpageCaptureThread = value;
            }
        }

        public Thread LottoPlusWebPageCaptureThread
        {
            get
            {
                return _lottoPlusWebpageCaptureThread;
            }
            set
            {
                _lottoPlusWebpageCaptureThread = value;
            }
        }

        public Thread PowerballWebPageCaptureThread
        {
            get
            {
                return _powerballWebpageCaptureThread;
            }
            set
            {
                _powerballWebpageCaptureThread = value;
            }
        }

        public Uri BaseURI
        {
            get
            {
                return _baseUri;
            }
            set
            {
                _baseUri = value;
            }
        }

        public Uri LottoURI
        {
            get
            {
                return _lottoUri;
            }
            set
            {
                _lottoUri = value;
            }
        }

        public Uri LottoPlusURI
        {
            get
            {
                return _lottoPlusUri;
            }
            set
            {
                _lottoPlusUri = value;
            }
        }

        public Uri PowerballURI
        {
            get
            {
                return _powerballUri;
            }
            set
            {
                _powerballUri = value;
            }
        }

        public string LottoDownloadDirName
        {
            get
            {
                return _lottoBaseDirName;
            }
            set
            {
                _lottoBaseDirName = value;
            }
        }

        public string LottoPlusDownloadDirName
        {
            get
            {
                return _lottoPlusBaseDirName;
            }
            set
            {
                _lottoPlusBaseDirName = value;
            }
        }

        public string PowerballDownloadDirName
        {
            get
            {
                return _powerballBaseDirName;
            }
            set
            {
                _powerballBaseDirName = value;
            }
        }

        public string LottoDownloadDirPath
        {
            get
            {
                return _lottoDownloadDirPath;
            }
            set
            {
                _lottoDownloadDirPath = value;
            }
        }

        public string LottoDownloadedFilePath
        {
            get
            {
                return _lottoDownloadedFilePath;
            }
            set
            {
                _lottoDownloadedFilePath = value;
            }
        }

        public string LottoPlusDownloadDirPath
        {
            get
            {
                return _lottoPlusDownloadDirPath;
            }
            set
            {
                _lottoPlusDownloadDirPath = value;
            }
        }

        public string LottoPlusDownloadedFilePath
        {
            get
            {
                return _lottoPlusDownloadedFilePath;
            }
            set
            {
                _lottoPlusDownloadedFilePath = value;
            }
        }

        public string PowerballDownloadDirPath
        {
            get
            {
                return _powerballDownloadDirPath;
            }
            set
            {
                _powerballDownloadDirPath = value;
            }
        }

        public string PowerballDownloadedFilePath
        {
            get
            {
                return _powerballDownloadedFilePath;
            }
            set
            {
                _powerballDownloadedFilePath = value;
            }
        }

        public string LottoFolderDir
        {
            get
            {
                return _lottoFolderDir;
            }
            set
            {
                _lottoFolderDir = value;
            }
        }

        public string LottoPlusFolderDir
        {
            get
            {
                return _lottoPlusFolderDir;
            }
            set
            {
                _lottoPlusFolderDir = value;
            }
        }

        public string PowerballFolderDir
        {
            get
            {
                return _powerballFolderDir;
            }
            set
            {
                _powerballFolderDir = value;
            }
        }

        public string LottoImagesDir
        {
            get
            {
                return _lottoImagesDir;
            }
            set
            {
                _lottoImagesDir = value;
            }
        }

        public string LottoPlusImagesDir
        {
            get
            {
                return _lottoPlusImagesDir;
            }
            set
            {
                _lottoPlusImagesDir = value;
            }
        }

        public string PowerballImagesDir
        {
            get
            {
                return _powerballImagesDir;
            }
            set
            {
                _powerballImagesDir = value;
            }
        }

        public string LottoBallsDir
        {
            get
            {
                return _lottoBallsDir;
            }
            set
            {
                _lottoBallsDir = value;
            }
        }

        public string LottoPlusBallsDir
        {
            get
            {
                return _lottoPlusBallsDir;
            }
            set
            {
                _lottoPlusBallsDir = value;
            }
        }

        public string PowerballBallsDir
        {
            get
            {
                return _powerballBallsDir;
            }
            set
            {
                _powerballBallsDir = value;
            }
        }

        public string LottoCSSDir
        {
            get
            {
                return _lottoCSSDir;
            }
            set
            {
                _lottoCSSDir = value;
            }
        }

        public string LottoPlusCSSDir
        {
            get
            {
                return _lottoPlusCSSDir;
            }
            set
            {
                _lottoPlusCSSDir = value;
            }
        }

        public string PowerballCSSDir
        {
            get
            {
                return _powerballCSSDir;
            }
            set
            {
                _powerballCSSDir = value;
            }
        }

        public string LottoScriptsDir
        {
            get
            {
                return _lottoScriptsDir;
            }
            set
            {
                _lottoScriptsDir = value;
            }
        }

        public string LottoPlusScriptsDir
        {
            get
            {
                return _lottoPlusScriptsDir;
            }
            set
            {
                _lottoPlusScriptsDir = value;
            }
        }

        public string PowerballScriptsDir
        {
            get
            {
                return _powerballScriptsDir;
            }
            set
            {
                _powerballScriptsDir = value;
            }
        }

        public string LottoJavaScriptDir
        {
            get
            {
                return _lottoJSDir;
            }
            set
            {
                _lottoJSDir = value;
            }
        }

        public string LottoPlusJavaScriptDir
        {
            get
            {
                return _lottoPlusJSDir;
            }
            set
            {
                _lottoPlusJSDir = value;
            }
        }

        public string PowerballJavaScriptDir
        {
            get
            {
                return _powerballJSDir;
            }
            set
            {
                _powerballJSDir = value;
            }
        }

        public string LottoIncDir
        {
            get
            {
                return _lottoIncDir;
            }
            set
            {
                _lottoIncDir = value;
            }
        }

        public string LottoPlusIncDir
        {
            get
            {
                return _lottoPlusIncDir;
            }
            set
            {
                _lottoPlusIncDir = value;
            }
        }

        public string PowerballIncDir
        {
            get
            {
                return _powerballIncDir;
            }
            set
            {
                _powerballIncDir = value;
            }
        }

        public string LottoFilePrefix
        {
            get
            {
                return _lottoFilePrefix;
            }
            set
            {
                _lottoFilePrefix = value;
            }
        }

        public string LottoPlusFilePrefix
        {
            get
            {
                return _lottoPlusFilePrefix;
            }
            set
            {
                _lottoPlusFilePrefix = value;
            }
        }

        public string PowerballFilePrefix
        {
            get
            {
                return _powerballFilePrefix;
            }
            set
            {
                _powerballFilePrefix = value;
            }
        }

        public bool LottoWinningBoardsSaved
        {
            get 
            {
                return _winLottoBoardsSaved;
            }
            set 
            {
                _winLottoBoardsSaved = value;
            }
        }

        public bool LottoPlusWinningBoardsSaved
        {
            get
            {
                return _winLottoPlusBoardsSaved;
            }
            set
            {
                _winLottoPlusBoardsSaved = value;
            }
        }

        public bool PowerballWinningBoardsSaved
        {
            get
            {
                return _winPowerballBoardsSaved;
            }
            set
            {
                _winPowerballBoardsSaved = value;
            }
        }

        public int NumLottoWinsSaved
        {
            get
            {
                return _numLottoWinsSaved;
            }
            set
            {
                _numLottoWinsSaved = value;
            }
        }

        public int NumLottoPlusWinsSaved
        {
            get
            {
                return _numLottoPlusWinsSaved;
            }
            set
            {
                _numLottoPlusWinsSaved = value;
            }
        }

        public int NumPowerballWinsSaved
        {
            get
            {
                return _numPowerballWinsSaved;
            }
            set
            {
                _numPowerballWinsSaved = value;
            }
        }

        public string LottoWinsSavedMessage
        {
            get 
            {
                return _lottoWinsSavedMessage;
            }
            set 
            {
                _lottoWinsSavedMessage = value;
            }
        }

        public string LottoPlusWinsSavedMessage
        {
            get
            {
                return _lottoPlusWinsSavedMessage;
            }
            set
            {
                _lottoPlusWinsSavedMessage = value;
            }
        }

        public string PowerballWinsSavedMessage
        {
            get
            {
                return _powerballWinsSavedMessage;
            }
            set
            {
                _powerballWinsSavedMessage = value;
            }
        }

        public Bitmap LottoWebPageBitmap
        {
            get 
            {
                return _lottoBitmap;
            }
            set 
            {
                _lottoBitmap = value;
            }
        }

        public Bitmap LottoPlusWebPageBitmap
        {
            get
            {
                return _lottoPlusBitmap;
            }
            set
            {
                _lottoPlusBitmap = value;
            }
        }

        public Bitmap PowerballWebPageBitmap
        {
            get
            {
                return _powerballBitmap;
            }
            set
            {
                _powerballBitmap = value;
            }
        }

        public DataGridViewStyles LottoGridViewStyles
        {
            get 
            { 
                return _lottoGridViewStyle;
            }
            set 
            {
                _lottoGridViewStyle = value;
            }
            
        }

        public DataGridViewStyles LottoPlusGridViewStyles
        {
            get
            {
                return _lottoPlusGridViewStyle;
            }
            set
            {
                _lottoPlusGridViewStyle = value;
            }

        }

        public DataGridViewStyles PowerballGridViewStyles
        {
            get
            {
                return _powerballGridViewStyle;
            }
            set
            {
                _powerballGridViewStyle = value;
            }

        }

        public List<string> LottoPresentationDataList
        {
            get
            {
                return _lottoPresentationDataList;
            }
            set
            {
                _lottoPresentationDataList = value;
            }
        }
        private List<string> LottoPlusPresentationDataList
        {
            get
            {
                return _lottoPlusPresentationDataList;
            }
            set
            {
                _lottoPlusPresentationDataList = value;
            }
        }
        private List<string> PowerballPresentationDataList
        {
            get
            {
                return _powerballPresentationDataList;
            }
            set
            {
                _powerballPresentationDataList = value;
            }
        }

        #endregion

        private void CreateLottoDirectories()
        {
            LottoDownloadDirPath = ApplicationBriefcasePath + LottoDownloadDirName + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
            LottoFolderDir = ApplicationBriefcasePath + LottoDownloadDirName + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
            LottoImagesDir = LottoFolderDir + LottoImagesDirName + "\\";
            LottoBallsDir = LottoImagesDir + LottoBallsDirName + "\\";
            LottoCSSDir = LottoFolderDir + LottoCSSDirName + "\\";
            LottoScriptsDir = LottoFolderDir + LottoScriptsDirName + "\\";
            LottoJavaScriptDir = LottoFolderDir + LottoJavascriptDirName + "\\";
            LottoIncDir = LottoFolderDir + LottoIncDirName + "\\";

            if (!Directory.Exists(LottoDownloadDirPath))
            {
                Directory.CreateDirectory(LottoDownloadDirPath);
            }

            if (!Directory.Exists(LottoImagesDir))
            {
                Directory.CreateDirectory(LottoImagesDir);
            }

            if (!Directory.Exists(LottoBallsDir))
            {
                Directory.CreateDirectory(LottoBallsDir);
            }

            if (!Directory.Exists(LottoCSSDir))
            {
                Directory.CreateDirectory(LottoCSSDir);
            }

            if (!Directory.Exists(LottoScriptsDir))
            {
                Directory.CreateDirectory(LottoScriptsDir);
            }

            if (!Directory.Exists(LottoJavaScriptDir))
            {
                Directory.CreateDirectory(LottoJavaScriptDir);
            }

            if (!Directory.Exists(LottoIncDir))
            {
                Directory.CreateDirectory(LottoIncDir);
            }
        }

        private void CreateLottoPlusDirectories()
        {
            LottoPlusDownloadDirPath = ApplicationBriefcasePath + LottoPlusDownloadDirName + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
            LottoPlusFolderDir = ApplicationBriefcasePath + LottoPlusDownloadDirName + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
            LottoPlusImagesDir = LottoPlusFolderDir + LottoPlusImagesDirName + "\\";
            LottoPlusBallsDir = LottoPlusImagesDir + LottoPlusBallsDirName + "\\";
            LottoPlusCSSDir = LottoPlusFolderDir +  LottoPlusCSSDirName + "\\";
            LottoPlusScriptsDir = LottoPlusFolderDir +  LottoPlusScriptsDirName + "\\";
            LottoPlusJavaScriptDir = LottoPlusFolderDir + LottoPlusJavascriptDirName + "\\";
            LottoPlusIncDir = LottoPlusFolderDir + LottoPlusIncDirName + "\\";


            if (!Directory.Exists(LottoPlusDownloadDirPath))
            {
                Directory.CreateDirectory(LottoPlusDownloadDirPath);
            }

            if (!Directory.Exists(LottoPlusImagesDir))
            {
                Directory.CreateDirectory(LottoPlusImagesDir);
            }

            if (!Directory.Exists(LottoPlusBallsDir))
            {
                Directory.CreateDirectory(LottoPlusBallsDir);
            }

            if (!Directory.Exists(LottoPlusCSSDir))
            {
                Directory.CreateDirectory(LottoPlusCSSDir);
            }

            if (!Directory.Exists(LottoPlusScriptsDir))
            {
                Directory.CreateDirectory(LottoPlusScriptsDir);
            }

            if (!Directory.Exists(LottoPlusJavaScriptDir))
            {
                Directory.CreateDirectory(LottoPlusJavaScriptDir);
            }

            if (!Directory.Exists(LottoPlusIncDir))
            {
                Directory.CreateDirectory(LottoPlusIncDir);
            }
        }

        private void CreatePowerballDirectories()
        {
            PowerballDownloadDirPath = ApplicationBriefcasePath + PowerballDownloadDirName + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
            PowerballFolderDir = ApplicationBriefcasePath + PowerballDownloadDirName + "\\" + DateTime.Now.ToString("yyyyMMdd") + "\\";
            PowerballImagesDir = PowerballFolderDir + PowerballImagesDirName + "\\";
            PowerballBallsDir = PowerballImagesDir + PowerballBallsDirName + "\\";
            PowerballCSSDir = PowerballFolderDir + PowerballCSSDirName + "\\";
            PowerballScriptsDir = PowerballFolderDir + PowerballScriptsDirName + "\\";
            PowerballJavaScriptDir = PowerballFolderDir + PowerballJavascriptDirName + "\\";
            PowerballIncDir = PowerballFolderDir + PowerballIncDirName + "\\";

            if (!Directory.Exists(PowerballDownloadDirPath))
            {
                Directory.CreateDirectory(PowerballDownloadDirPath);
            }

            if (!Directory.Exists(PowerballImagesDir))
            {
                Directory.CreateDirectory(PowerballImagesDir);
            }

            if (!Directory.Exists(PowerballBallsDir))
            {
                Directory.CreateDirectory(PowerballBallsDir);
            }

            if (!Directory.Exists(PowerballCSSDir))
            {
                Directory.CreateDirectory(PowerballCSSDir);
            }

            if (!Directory.Exists(PowerballScriptsDir))
            {
                Directory.CreateDirectory(PowerballScriptsDir);
            }

            if (!Directory.Exists(PowerballJavaScriptDir))
            {
                Directory.CreateDirectory(PowerballJavaScriptDir);
            }

            if (!Directory.Exists(PowerballIncDir))
            {
                Directory.CreateDirectory(PowerballIncDir);
            }
        }

        private void SaveLottoWebPage(WebBrowser Browser)
        {
            int count = 0;
            int sleepTimeMiliseconds = 5;
            int waitTime = 6;
            while (Browser.ReadyState != WebBrowserReadyState.Complete)
            {
                Thread.Sleep(sleepTimeMiliseconds);
                Application.DoEvents();
                count++;
                if (count > waitTime / sleepTimeMiliseconds)
                    break;
            }

            MemoryStream memory = new MemoryStream();
            memory = CommonWebController.CopyToMemory(Browser.DocumentStream as Stream);
            LottoDownloadedFilePath = LottoDownloadDirPath + LottoFilePrefix + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".html";

            using (FileStream file = new FileStream(LottoDownloadedFilePath, FileMode.Create))
            {
                Thread.Sleep(1000);
                int buffer;
                while ((buffer = memory.ReadByte()) != -1)
                    file.WriteByte((byte)buffer);
            }
        }

        private void SaveLottoPlusWebPage(WebBrowser Browser)
        {
            int count = 0;
            int sleepTimeMiliseconds = 5;
            int waitTime = 6;
            while (Browser.ReadyState != WebBrowserReadyState.Complete)
            {
                Thread.Sleep(sleepTimeMiliseconds);
                Application.DoEvents();
                count++;
                if (count > waitTime / sleepTimeMiliseconds)
                    break;
            }
            MemoryStream memory = new MemoryStream();
            memory = CommonWebController.CopyToMemory(Browser.DocumentStream as Stream); ;
            LottoPlusDownloadedFilePath = LottoPlusDownloadDirPath + LottoPlusFilePrefix + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".html";
            using (FileStream file = new FileStream(LottoPlusDownloadedFilePath, FileMode.Create))
            {
                int buffer;
                while ((buffer = memory.ReadByte()) != -1)
                    file.WriteByte((byte)buffer);
            }
        }

        private void SavePowerballWebPage(WebBrowser Browser)
        {
            int count = 0;
            int sleepTimeMiliseconds = 5;
            int waitTime = 6;
            while (Browser.ReadyState != WebBrowserReadyState.Complete)
            {
                Thread.Sleep(sleepTimeMiliseconds);
                Application.DoEvents();
                count++;
                if (count > waitTime / sleepTimeMiliseconds)
                    break;
            }
            MemoryStream memory = new MemoryStream();
            memory = CommonWebController.CopyToMemory(Browser.DocumentStream as Stream); ;
            PowerballDownloadedFilePath = PowerballDownloadDirPath + PowerballFilePrefix + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".html";
            using (FileStream file = new FileStream(PowerballDownloadedFilePath, FileMode.Create))
            {
                int buffer;
                while ((buffer = memory.ReadByte()) != -1)
                    file.WriteByte((byte)buffer);
            }
        }

        private void ProcessLottoWebImages(HtmlElementCollection ImageCollection)
        {
            WebClient webClient = new WebClient();
            foreach (HtmlElement img in ImageCollection)
            {
                string url = img.GetAttribute("src");

                if ((url.Contains("https://seal.verisign.com/getseal") == false)  && (url.StartsWith("res://") == false))
                {
                    var vurl = new Uri(BaseURI, url);
                    string path = vurl.AbsoluteUri;
                    string imgpath = url.Substring(url.LastIndexOf('/') + 1);
                    string downloadpath = String.Empty;
                    if (url.Contains("/ball_"))
                    {
                        downloadpath = LottoBallsDir + imgpath;
                        FileInfo fileInfo = new FileInfo(downloadpath);

                        if (!fileInfo.Exists || fileInfo.Length == 0)
                            webClient.DownloadFile(path, downloadpath);
                    }
                    else if (imgpath.Contains("getseal") == false)
                    {
                        downloadpath = LottoImagesDir + imgpath;
                        FileInfo fileInfo = new FileInfo(downloadpath);

                        if (!fileInfo.Exists || fileInfo.Length == 0)
                            webClient.DownloadFile(path, downloadpath);
                    }
                }
            }
        }

        private void ProcessLottoPlusWebImages(HtmlElementCollection ImageCollection)
        {
            WebClient webClient = new WebClient();
            foreach (HtmlElement img in ImageCollection)
            {
                string url = img.GetAttribute("src");

                if ((url.Contains("https://seal.verisign.com/getseal") == false) && (url.StartsWith("res://") == false))
                {
                    var vurl = new Uri(BaseURI, url);
                    string path = vurl.AbsoluteUri;
                    string imgpath = url.Substring(url.LastIndexOf('/') + 1);
                    string downloadpath = String.Empty;
                    if (url.Contains("/ball_"))
                    {
                        downloadpath = LottoPlusBallsDir + imgpath;
                        FileInfo fileInfo = new FileInfo(downloadpath);
                        if (!fileInfo.Exists || fileInfo.Length == 0)
                            webClient.DownloadFile(path, downloadpath);
                    }
                    else if (imgpath.Contains("getseal") == false)
                    {
                        downloadpath = LottoPlusImagesDir + imgpath;
                        FileInfo fileInfo = new FileInfo(downloadpath);
                        if (!fileInfo.Exists || fileInfo.Length == 0)
                            webClient.DownloadFile(path, downloadpath);
                    }
                }
            }
        }

        private void ProcessPowerballWebImages(HtmlElementCollection ImageCollection)
        {
            WebClient webClient = new WebClient();
            foreach (HtmlElement img in ImageCollection)
            {
                string url = img.GetAttribute("src");
                if ((url.Contains("https://seal.verisign.com/getseal") == false) && (url.StartsWith("res://") == false))
                {
                    var vurl = new Uri(BaseURI, url);
                    string path = vurl.AbsoluteUri;
                    string imgpath = url.Substring(url.LastIndexOf('/') + 1);
                    string downloadpath = String.Empty;
                    if ((url.Contains("/ball_")) || (url.Contains("/power_")))
                    {
                        downloadpath = PowerballBallsDir + imgpath;
                        FileInfo fileInfo = new FileInfo(downloadpath);
                        if (!fileInfo.Exists || fileInfo.Length == 0)
                            webClient.DownloadFile(path, downloadpath);
                    }
                    else if (imgpath.Contains("getseal") == false)
                    {
                        downloadpath = PowerballImagesDir + imgpath;
                        FileInfo fileInfo = new FileInfo(downloadpath);
                        if (!fileInfo.Exists || fileInfo.Length == 0)
                            webClient.DownloadFile(path, downloadpath);
                    }
                }
            }
        }

        private void ProcessLottoWebJavaScripts(HtmlElementCollection ScriptsCollection)
        {
            WebClient webClient = new WebClient();
            foreach (HtmlElement script in ScriptsCollection)
            {
                string url = script.GetAttribute("src");
                if (url.Contains("AC_RunActiveContent.js"))
                {
                    var vurl = new Uri(BaseURI, url);
                    string path = vurl.AbsoluteUri;
                    string scriptpath = path.Substring(path.LastIndexOf('/') + 1);
                    string downloadpath = LottoScriptsDir + scriptpath;
                    FileInfo fileInfo = new FileInfo(downloadpath);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                        webClient.DownloadFile(path, downloadpath);
                }

                if (url.Contains("function.js"))
                {
                    var vurl = new Uri(BaseURI, url);
                    string path = vurl.AbsoluteUri;
                    string jspath = path.Substring(path.LastIndexOf('/') + 1);
                    string downloadpath = LottoIncDir + jspath;
                    FileInfo fileInfo = new FileInfo(downloadpath);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                        webClient.DownloadFile(path, downloadpath);
                }

                if (url.Contains("swfobject.js"))
                {
                    var vurl = new Uri(BaseURI, url);
                    string path = vurl.AbsoluteUri;
                    string incpath = path.Substring(path.LastIndexOf('/') + 1);
                    string downloadpath = LottoIncDir + incpath;
                    FileInfo fileInfo = new FileInfo(downloadpath);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                        webClient.DownloadFile(path, downloadpath);
                }
            }
        }

        private void ProcessLottoPlusWebJavaScripts(HtmlElementCollection ScriptsCollection)
        {
            WebClient webClient = new WebClient();
            foreach (HtmlElement script in ScriptsCollection)
            {
                string url = script.GetAttribute("src");
                if (url.Contains("AC_RunActiveContent.js"))
                {
                    var vurl = new Uri(BaseURI, url);
                    string path = vurl.AbsoluteUri;
                    string scriptpath = path.Substring(path.LastIndexOf('/') + 1);
                    string downloadpath = LottoPlusScriptsDir + scriptpath;
                    FileInfo fileInfo = new FileInfo(downloadpath);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                        webClient.DownloadFile(path, downloadpath);
                }

                if (url.Contains("function.js"))
                {
                    var vurl = new Uri(BaseURI, url);
                    string path = vurl.AbsoluteUri;
                    string jspath = path.Substring(path.LastIndexOf('/') + 1);
                    string downloadpath = LottoPlusIncDir + jspath;
                    FileInfo fileInfo = new FileInfo(downloadpath);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                        webClient.DownloadFile(path, downloadpath);
                }

                if (url.Contains("swfobject.js"))
                {
                    var vurl = new Uri(BaseURI, url);
                    string path = vurl.AbsoluteUri;
                    string incpath = path.Substring(path.LastIndexOf('/') + 1);
                    string downloadpath = LottoPlusIncDir + incpath;
                    FileInfo fileInfo = new FileInfo(downloadpath);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                        webClient.DownloadFile(path, downloadpath);
                }
            }
        }

        private void ProcessPowerballWebJavaScripts(HtmlElementCollection ScriptsCollection)
        {
            WebClient webClient = new WebClient();
            foreach (HtmlElement script in ScriptsCollection)
            {
                string url = script.GetAttribute("src");
                if (url.Contains("AC_RunActiveContent.js"))
                {
                    var vurl = new Uri(BaseURI, url);
                    string path = vurl.AbsoluteUri;
                    string scriptpath = path.Substring(path.LastIndexOf('/') + 1);
                    string downloadpath = PowerballScriptsDir + scriptpath;
                    FileInfo fileInfo = new FileInfo(downloadpath);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                        webClient.DownloadFile(path, downloadpath);
                }

                if (url.Contains("function.js"))
                {
                    var vurl = new Uri(BaseURI, url);
                    string path = vurl.AbsoluteUri;
                    string jspath = path.Substring(path.LastIndexOf('/') + 1);
                    string downloadpath = PowerballIncDir + jspath;
                    FileInfo fileInfo = new FileInfo(downloadpath);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                        webClient.DownloadFile(path, downloadpath);
                }

                if (url.Contains("swfobject.js"))
                {
                    var vurl = new Uri(BaseURI, url);
                    string path = vurl.AbsoluteUri;
                    string incpath = path.Substring(path.LastIndexOf('/') + 1);
                    string downloadpath = PowerballIncDir + incpath;
                    FileInfo fileInfo = new FileInfo(downloadpath);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                        webClient.DownloadFile(path, downloadpath);
                }
            }
        }

        private void ProcessLottoWebLinks(HtmlElementCollection LinkCollection)
        {
            WebClient webClient = new WebClient();
            foreach (HtmlElement link in LinkCollection)
            {
                string href = link.GetAttribute("href");
                string rel = link.GetAttribute("rel");
                if (rel == "stylesheet")
                {
                    var vurl = new Uri(BaseURI, href);
                    string path = vurl.AbsoluteUri;
                    string hrefpath = path.Substring(path.LastIndexOf('/') + 1);
                    string downloadpath = LottoCSSDir + hrefpath;
                    FileInfo fileInfo = new FileInfo(downloadpath);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                        webClient.DownloadFile(path, downloadpath);
                }
            }
        }

        private void ProcessLottoPlusWebLinks(HtmlElementCollection LinkCollection)
        {
            WebClient webClient = new WebClient();
            foreach (HtmlElement link in LinkCollection)
            {
                string href = link.GetAttribute("href");
                string rel = link.GetAttribute("rel");
                if (rel == "stylesheet")
                {
                    var vurl = new Uri(BaseURI, href);
                    string path = vurl.AbsoluteUri;
                    string hrefpath = path.Substring(path.LastIndexOf('/') + 1);
                    string downloadpath = LottoPlusCSSDir + hrefpath;
                    FileInfo fileInfo = new FileInfo(downloadpath);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                        webClient.DownloadFile(path, downloadpath);
                }
            }
        }

        private void ProcessPowerballWebLinks(HtmlElementCollection LinkCollection)
        {
            WebClient webClient = new WebClient();
            foreach (HtmlElement link in LinkCollection)
            {
                string href = link.GetAttribute("href");
                string rel = link.GetAttribute("rel");
                if (rel == "stylesheet")
                {
                    var vurl = new Uri(BaseURI, href);
                    string path = vurl.AbsoluteUri;
                    string hrefpath = path.Substring(path.LastIndexOf('/') + 1);
                    string downloadpath = PowerballCSSDir + hrefpath;
                    FileInfo fileInfo = new FileInfo(downloadpath);
                    if (!fileInfo.Exists || fileInfo.Length == 0)
                        webClient.DownloadFile(path, downloadpath);
                }
            }
        }

        public void ProcessLottoWebpage(object Sender, WebBrowserDocumentCompletedEventArgs Args, WebBrowser Browser)
        {
            ParameterizedThreadStart ts = delegate(object o) { DoLottoWebPageProcessing(new WebProcessingData(Sender, Args, Browser)); };
            LottoThread = new Thread(ts);
            LottoThread.IsBackground = true;
            LottoThread.Name = "LottoThread";
            LottoThread.Start();
        }

        public void ProcessLottoPlusWebpage(object Sender, WebBrowserDocumentCompletedEventArgs Args, WebBrowser Browser)
        {
            ParameterizedThreadStart ts = delegate(object o) { DoLottoPlusWebPageProcessing(new WebProcessingData(Sender, Args, Browser)); };
            LottoPlusThread = new Thread(ts);
            LottoPlusThread.IsBackground = true;
            LottoPlusThread.Name = "LottoPlusThread";
            LottoPlusThread.Start();
        }

        public void ProcessPowerballWebpage(object Sender, WebBrowserDocumentCompletedEventArgs Args, WebBrowser Browser)
        {
            ParameterizedThreadStart ts = delegate(object o) { DoPowerballWebPageProcessing(new WebProcessingData(Sender, Args, Browser)); };
            PowerballThread = new Thread(ts);
            PowerballThread.IsBackground = true;
            PowerballThread.Name = "PowerballThread";
            PowerballThread.Start();
        }

        public LottoGameResults GetGameResults(HTMLParsedResults ParsedResults, string Game)
        {
            LottoCard card = new LottoCard(ParsedResults, Game);
            LottoEngine.EngineSetup = this.ConnectionSetup;
            LottoGameResults results = LottoEngine.ProcessLottoCard(card);
            if ((results.Summary.NumWinBoards > 0) && (ContinueSaveAction(card.GameDate, Game)) && (Game == "Lotto"))
            {
                List<LottoGameBoard> winboards = GetWinningBoards(results.GameBoards, Game);
                SaveLottoWinningDelegate del = new SaveLottoWinningDelegate(SaveWinningBoards);
                IAsyncResult iasync = del.BeginInvoke(winboards, Game, CallBackWhenLottoSavedCompleted, Game);
            }

            if (card.LottoPlus != null)
            {
                if ((results.Summary.NumWinBoards > 0) && (ContinueSaveAction(card.LottoPlus.GameDate, Game)) && (Game == "LottoPlus"))
                {
                    List<LottoGameBoard> winboardsx = GetWinningBoards(results.GameBoards, Game);
                    SaveLottoPlusWinningDelegate delx = new SaveLottoPlusWinningDelegate(SaveWinningBoards);
                    IAsyncResult iasyncx = delx.BeginInvoke(winboardsx, Game, CallBackWhenLottoPlusSavedCompleted, Game);
                }
            }
            return results;
        }

        public PowerballGameResults GetPowerGameResults(HTMLParsedResults ParsedResults, string Game)
        {
            PowerballCard powercard = new PowerballCard(ParsedResults);
            PowerballEngine.EngineSetup = this.ConnectionSetup;
            PowerballGameResults results = PowerballEngine.ProcessPowerballCard(powercard);

            if ((results.Summary.NumWinBoards > 0) && (ContinueSaveAction(powercard.GameDate, Game)))
            {
                List<PowerballGameBoard> winboards = GetPowerWinningBoards(results.GameBoards, Game);
                SavePowerballWinningDelegate del = new SavePowerballWinningDelegate(SavePowerWinningBoards);
                IAsyncResult iasync = del.BeginInvoke(winboards, Game, CallBackWhenPowerballSavedCompleted, Game);
            }
            return results;
        }

        private void SaveWebPageCapture(string Game)
        {
            string fileName;
            switch (Game)
            {
                case "Lotto":
                    fileName = LottoDownloadDirPath + LottoFilePrefix + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".bmp";
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                    if (LottoWebPageBitmap != null)
                        LottoWebPageBitmap.Save(fileName);
                    break;
                case "LottoPlus":
                    fileName = LottoPlusDownloadDirPath + LottoPlusFilePrefix + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".bmp";
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                    if (LottoPlusWebPageBitmap != null)
                        LottoPlusWebPageBitmap.Save(fileName);
                    break;
                case "Powerball":
                    fileName = PowerballDownloadDirPath + PowerballFilePrefix + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".bmp";
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                    if (PowerballWebPageBitmap != null)
                        PowerballWebPageBitmap.Save(fileName);
                    break;
                default:
                    break;
            }
        }

        private bool StartLottoWebPageCaptureThread(List<LottoGameBoard> WinningsBoards, string Game)
        {
            bool result = false;
            LottoWinsServiceManager servicemanager;
            if (!String.IsNullOrEmpty(this.ConnectionSetup.DatasourcePath))
            {
                servicemanager = new LottoWinsServiceManager(this.ConnectionSetup);
            }
            else
            {
                servicemanager = new LottoWinsServiceManager();
            }

            ParameterizedThreadStart ts = delegate(object o)
            {
                GenerateWebPageBitmap(new WebPageSnapshotData(LottoDownloadedFilePath, Game));
            };

            LottoWebPageCaptureThread = new Thread(ts);
            LottoWebPageCaptureThread.SetApartmentState(ApartmentState.STA);
            LottoWebPageCaptureThread.IsBackground = true;
            LottoWebPageCaptureThread.Name = "LottoCaptureThread";
            LottoWebPageCaptureThread.Start();

            while (LottoWebPageCaptureThread.IsAlive)
            {
                Application.DoEvents();
                Thread.Sleep(50);
            }

            if (!LottoWebPageCaptureThread.IsAlive)
            {
                if (LottoWebPageBitmap != null)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool StartLottoPlusWebPageCaptureThread(List<LottoGameBoard> WinningsBoards, string Game)
        {
            bool result = false;
            LottoWinsServiceManager servicemanager;
            if (!String.IsNullOrEmpty(this.ConnectionSetup.DatasourcePath))
            {
                servicemanager = new LottoWinsServiceManager(this.ConnectionSetup);
            }
            else
            {
                servicemanager = new LottoWinsServiceManager();
            }

            ParameterizedThreadStart ts = delegate(object o)
            {
                GenerateWebPageBitmap(new WebPageSnapshotData(LottoPlusDownloadedFilePath, Game));
            };

            LottoPlusWebPageCaptureThread = new Thread(ts);
            LottoPlusWebPageCaptureThread.SetApartmentState(ApartmentState.STA);
            LottoPlusWebPageCaptureThread.IsBackground = true;
            LottoPlusWebPageCaptureThread.Name = "LottoPlusCaptureThread";
            LottoPlusWebPageCaptureThread.Start();

            while (LottoPlusWebPageCaptureThread.IsAlive)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            if (!LottoPlusWebPageCaptureThread.IsAlive)
            {
                if (LottoPlusWebPageBitmap != null)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool StartPowerballWebPageCaptureThread(List<PowerballGameBoard> WinningsBoards, string Game)
        {
            bool result = false;
            LottoWinsServiceManager servicemanager;
            if (!String.IsNullOrEmpty(this.ConnectionSetup.DatasourcePath))
            {
                servicemanager = new LottoWinsServiceManager(this.ConnectionSetup);
            }
            else
            {
                servicemanager = new LottoWinsServiceManager();
            }
            ParameterizedThreadStart ts = delegate(object o)
            {
                GenerateWebPageBitmap(new WebPageSnapshotData(PowerballDownloadedFilePath, Game));
            };

            PowerballWebPageCaptureThread = new Thread(ts);
            PowerballWebPageCaptureThread.SetApartmentState(ApartmentState.STA);
            PowerballWebPageCaptureThread.IsBackground = true;
            PowerballWebPageCaptureThread.Name = "PowerballCaptureThread";
            PowerballWebPageCaptureThread.Start();

            while (PowerballWebPageCaptureThread.IsAlive)
            {
                Application.DoEvents();
                Thread.Sleep(150);
            }

            if (!PowerballWebPageCaptureThread.IsAlive)
            {
                if (PowerballWebPageBitmap != null)
                {
                    result = true;
                }
            }
            return result;
        }

        private int SaveWinningBoards(List<LottoGameBoard> WinningsBoards, string Game)
        {
            int result = 0;
            bool captured = false;
            LottoWinsServiceManager servicemanager;
            if (!String.IsNullOrEmpty(this.ConnectionSetup.DatasourcePath))
            {
                servicemanager = new LottoWinsServiceManager(this.ConnectionSetup);
            }
            else
            {
                servicemanager = new LottoWinsServiceManager();
            }
            if (Game == "Lotto")
            {
                captured = StartLottoWebPageCaptureThread(WinningsBoards, Game);
                if (captured)
                    SaveWebPageCapture(Game);
                result = servicemanager.SaveLottoWinnings(WinningsBoards, Game, LottoWebPageBitmap);
            }
            if (Game == "LottoPlus")
            {
                captured = StartLottoPlusWebPageCaptureThread(WinningsBoards, Game);
                if (captured)
                    SaveWebPageCapture(Game);
                result = servicemanager.SaveLottoWinnings(WinningsBoards, Game, LottoPlusWebPageBitmap);
            }
            return result;
        }

        private bool ContinueSaveAction(DateTime GameDate, string Game)
        {
            bool result = false;
            LottoWinsServiceManager servicemanager;
            if (!String.IsNullOrEmpty(this.ConnectionSetup.DatasourcePath))
            {
                servicemanager = new LottoWinsServiceManager(this.ConnectionSetup);
            }
            else
            {
                servicemanager = new LottoWinsServiceManager();
            }
            if (servicemanager.ContinueSaveAction(GameDate, Game))
                result = true;

            return result;
        }

        private int SavePowerWinningBoards(List<PowerballGameBoard> WinningsBoards, string Game)
        {
            int result = 0;
            bool captured = false;
            LottoWinsServiceManager servicemanager;
            if (!String.IsNullOrEmpty(this.ConnectionSetup.DatasourcePath))
            {
                servicemanager = new LottoWinsServiceManager(this.ConnectionSetup);
            }
            else
            {
                servicemanager = new LottoWinsServiceManager();
            }
            captured = StartPowerballWebPageCaptureThread(WinningsBoards, Game);
            if (captured)
                SaveWebPageCapture(Game);
            result = servicemanager.SavePowerWinnings(WinningsBoards, Game, PowerballWebPageBitmap);
            return result;
        }

        public List<LottoGameBoard> GetWinningBoards(List<LottoGameBoard> Boards, string Game)
        {
            List<LottoGameBoard> Winboards = new  List<LottoGameBoard>();
            foreach (LottoGameBoard board in Boards)
            {
                if (board.WinningBoard)
                {
                    Winboards.Add(board);
                }
            }
            return Winboards;
        }

        public List<PowerballGameBoard> GetPowerWinningBoards(List<PowerballGameBoard> Boards, string Game)
        {
            List<PowerballGameBoard> Winboards = new List<PowerballGameBoard>();
            foreach (PowerballGameBoard board in Boards)
            {
                if (board.WinningBoard)
                {
                    Winboards.Add(board);
                }
            }
            return Winboards;
        }

        public void CallBackWhenGenerateWebPageBitmapCompleted(IAsyncResult iar)
        {
            AsyncResult ar = (AsyncResult)iar;
            bool generated = (bool)ar.AsyncState;
            GenerateWebpageBitmapDelegate del = (GenerateWebpageBitmapDelegate)ar.AsyncDelegate;
            if (generated)
                LottoWebPageBitmap = (Bitmap)del.EndInvoke(iar);
            else
                LottoWebPageBitmap = null;
        }

        public void CallBackWhenLottoSavedCompleted(IAsyncResult iar)
        {
            AsyncResult ar = (AsyncResult)iar;
            string Game = (string)ar.AsyncState;
            SaveLottoWinningDelegate del = (SaveLottoWinningDelegate)ar.AsyncDelegate;

            switch (Game)
            {
                case "Lotto":
                    NumLottoWinsSaved = (int)del.EndInvoke(iar);
                    if (NumLottoWinsSaved > 0)
                    {
                        LottoWinsSavedMessage = "Lotto winning records saved: " + Convert.ToString(NumLottoWinsSaved);
                        LottoWinningBoardsSaved = true;
                    }
                    else
                    {
                        LottoWinsSavedMessage = "No Lotto winning records saved.";
                        LottoWinningBoardsSaved = false;
                    }
                    RaiseLottoSavedWinningsEvent();
                    break;
                default:
                    break;
            }
        }

        public void CallBackWhenLottoPlusSavedCompleted(IAsyncResult iar)
        {
            AsyncResult ar = (AsyncResult)iar;
            string Game = (string)ar.AsyncState;
            SaveLottoPlusWinningDelegate del = (SaveLottoPlusWinningDelegate)ar.AsyncDelegate;

            switch (Game)
            {
                case "LottoPlus":
                    NumLottoPlusWinsSaved = (int)del.EndInvoke(iar);
                    if (NumLottoPlusWinsSaved > 0)
                    {
                        LottoPlusWinsSavedMessage = "LottoPlus winning records saved: " + Convert.ToString(NumLottoPlusWinsSaved);
                        LottoPlusWinningBoardsSaved = true;
                    }
                    else
                    {
                        LottoPlusWinsSavedMessage = "No LottoPlus winning records saved.";
                        LottoPlusWinningBoardsSaved = false;
                    }
                    RaiseLottoPlusSavedWinningsEvent();
                    break;
                default:
                    break;
            }
        }

        public void CallBackWhenPowerballSavedCompleted(IAsyncResult iar)
        {
            AsyncResult ar = (AsyncResult)iar;
            string Game = (string)ar.AsyncState;
            SavePowerballWinningDelegate del = (SavePowerballWinningDelegate)ar.AsyncDelegate;
            
            switch (Game)
            {
                case "Powerball":
                    NumPowerballWinsSaved = (int)del.EndInvoke(iar);
                    if (NumPowerballWinsSaved > 0)
                    {
                        PowerballWinsSavedMessage = "Powerball winning records saved: " + Convert.ToString(NumPowerballWinsSaved);
                        PowerballWinningBoardsSaved = true;
                    }
                    else
                    {
                        PowerballWinsSavedMessage = "No Powerball winning records saved.";
                        PowerballWinningBoardsSaved = false;
                    }
                    RaisePowerballSavedWinningsEvent();
                    break;
                default:
                    break;
            }
        }

        public string GetLottoMessage()
        {
            if (LottoGameResults != null)
                return LottoGameResults.Summary.Message;
            else
                return String.Empty;
        }

        public Image GetLottoWinImage()
        {
            Image result = null;
            if (LottoGameResults != null)
            {
                if (LottoGameResults.Summary.NumLottoWins > 0)
                    return global::ttpim.gamemodule.Properties.Resources.youwin;
            }
            return result; 
        }

        public Image GetLottoPlusWinImage()
        {
            Image result = null;
            if (LottoPlusGameResults != null)
            {
                if (LottoPlusGameResults.Summary.NumLottoPlusWins > 0)
                    return global::ttpim.gamemodule.Properties.Resources.youwin;
            }
            return result; 
        }

        public Image GetPowerballWinImage()
        {
            Image result = null;
            if (LottoGameResults != null)
            {
                if (PowerballGameResults.Summary.NumPowerballWins > 0)
                    return global::ttpim.gamemodule.Properties.Resources.youwin;
            }
            return result; 
        }

        public Image GetDivisionImage()
        {
            Image result = null;
            return result;
        }

        public Image GetLottoMessageImage()
        {
            Image result = null;
            string message = GetLottoMessage();
            result = GenerateMessageGraphic(message, String.Empty);
            return result;
        }

        public Image GetLottoPlusMessageImage()
        {
            Image result = null;
            string message = GetLottoPlusMessage();
            result = GenerateMessageGraphic(message, String.Empty);
            return result;
        }

        public Image GetPowerballMessageImage()
        {
            Image result = null;
            string message = GetPowerballMessage();
            result = GenerateMessageGraphic(message, String.Empty);
            return result;
        }

        public string GetLottoPlusMessage()
        {
            if (LottoPlusGameResults != null)
                return LottoPlusGameResults.Summary.Message;
            else
                return String.Empty;
        }

        public string GetPowerballMessage()
        {
            if (PowerballGameResults != null)
                return PowerballGameResults.Summary.Message;
            else
                return String.Empty;
        }

        public DataTable GetLottoTableView()
        {
            if (LottoGameResults != null)
                return GetLottoDataTable(LottoGameResults.GameBoards);
             else
                return new DataTable();
        }

        public DataTable GetLottoPlusTableView()
        {
           if (LottoPlusGameResults != null)
               return GetLottoPlusDataTable(LottoPlusGameResults.GameBoards);
           else
               return new DataTable();
        }

        public DataTable GetPowerballTableView()
        {
            if (PowerballGameResults != null)
                return GetPowerballDataTable(PowerballGameResults.GameBoards);
            else
                return new DataTable();
        }

        private DataTable GetLottoDataTable(List<LottoGameBoard> GameBoards)
        {
            DataTable table = BuildLottoDataTable();
            table = PopulateLottoDataTable(table, GameBoards);
            return table;
        }

        private DataTable GetLottoPlusDataTable(List<LottoGameBoard> GameBoards)
        {
            DataTable table = BuildLottoPlusDataTable();
            table = PopulateLottoPlusDataTable(table, GameBoards);
            return table;
        }

        private DataTable GetPowerballDataTable(List<PowerballGameBoard> GameBoards)
        {
            DataTable table = BuildPowerballDataTable();
            table = PopulatePowerballDataTable(table, GameBoards);
            return table;
        }

        private DataTable BuildLottoPresentationTable()
        {
            DataTable table = new DataTable("LottoPresentationDataTable");
            table.Columns.Add("Row", typeof(object));
            table.Columns.Add("Subscription Nr", typeof(object));
            table.Columns.Add("Game", typeof(object));
            table.Columns.Add("Board", typeof(object));
            table.Columns.Add("Ball 1", typeof(object));
            table.Columns.Add("Ball 2", typeof(object));
            table.Columns.Add("Ball 3", typeof(object));
            table.Columns.Add("Ball 4", typeof(object));
            table.Columns.Add("Ball 5", typeof(object));
            table.Columns.Add("Ball 6", typeof(object));
            table.Columns.Add("Bonus Match", typeof(object));
            table.Columns.Add("Win", typeof(object));
            table.Columns.Add("Division", typeof(object));
            table.Columns.Add("Winnings", typeof(object));
            return table;
        }

        private DataTable BuildLottoDataTable()
        {
            DataTable table = new DataTable("LottoDataTable");
            table.Columns.Add("Subscription Nr", typeof(string));
            table.Columns.Add("Game", typeof(string));
            table.Columns.Add("Board", typeof(string));
            table.Columns.Add("Ball 1", typeof(string));
            table.Columns.Add("Ball 2", typeof(string));
            table.Columns.Add("Ball 3", typeof(string));
            table.Columns.Add("Ball 4", typeof(string));
            table.Columns.Add("Ball 5", typeof(string));
            table.Columns.Add("Ball 6", typeof(string));
            table.Columns.Add("Bonus Match", typeof(Image));
            table.Columns.Add("Win", typeof(Image));
            table.Columns.Add("Division", typeof(string));
            table.Columns.Add("Winnings", typeof(double));
            return table;
        }
 
        private DataTable BuildLottoPlusDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Subscription Nr", typeof(string));
            table.Columns.Add("Game", typeof(string));
            table.Columns.Add("Board", typeof(string));
            table.Columns.Add("Ball 1", typeof(string));
            table.Columns.Add("Ball 2", typeof(string));
            table.Columns.Add("Ball 3", typeof(string));
            table.Columns.Add("Ball 4", typeof(string));
            table.Columns.Add("Ball 5", typeof(string));
            table.Columns.Add("Ball 6", typeof(string));
            table.Columns.Add("Bonus Match", typeof(Image));
            table.Columns.Add("Win", typeof(Image));
            table.Columns.Add("Division", typeof(string));
            table.Columns.Add("Winnings", typeof(double));
            return table;
        }

        private DataTable BuildPowerballDataTable()
        {
            DataTable table = new DataTable("PowerballDataTable");
            table.Columns.Add("Subscription Nr", typeof(string));
            table.Columns.Add("Game", typeof(string));
            table.Columns.Add("Board", typeof(string));
            table.Columns.Add("Ball 1", typeof(string));
            table.Columns.Add("Ball 2", typeof(string));
            table.Columns.Add("Ball 3", typeof(string));
            table.Columns.Add("Ball 4", typeof(string));
            table.Columns.Add("Ball 5", typeof(string));
            table.Columns.Add("Ball 6", typeof(string));
            table.Columns.Add("Powerball Match", typeof(Image));
            table.Columns.Add("Win", typeof(Image));
            table.Columns.Add("Division", typeof(string));
            table.Columns.Add("Winnings", typeof(double));
            return table;
        }

        private DataSet PopulateLottoResultTables(DataTable ResultTable, DataTable PresentationTable, List<LottoGameBoard> GameBoards)
        {
            DataSet dataset = new DataSet("LottoResultsDataSet");
            if ((ResultTable != null) && (PresentationTable != null) && (GameBoards != null) && (GameBoards.Count > 0))
            {
                foreach (LottoGameBoard board in GameBoards)
                {
                    if (board.NumberLottoMatches > 0)
                    {
                        ResultTable.Rows.Add(board.SubscriptionNr,
                                       board.Game,
                                       board.BoardId,
                                       board.SubcriptionBoard[0] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[0]),
                                       board.SubcriptionBoard[1] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[1]),
                                       board.SubcriptionBoard[2] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[2]),
                                       board.SubcriptionBoard[3] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[3]),
                                       board.SubcriptionBoard[4] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[4]),
                                       board.SubcriptionBoard[5] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[5]),
                                       board.LottoBonusMatch ? global::ttpim.gamemodule.Properties.Resources.bonushead : null,
                                       board.WinningBoard ? GetLottoWinDivisionImage(board.LottoDivisionNumber) : null,
                                       board.LottoDivision,
                                       board.LottoEarning);

                        ResultTable.Rows.Add(String.Empty,
                                       String.Empty,
                                       board.BoardId,
                                       board.LottoMatchedNumbers[0] == 0 ? String.Empty : Convert.ToString(board.LottoMatchedNumbers[0]),
                                       board.LottoMatchedNumbers[1] == 0 ? String.Empty : Convert.ToString(board.LottoMatchedNumbers[1]),
                                       board.LottoMatchedNumbers[2] == 0 ? String.Empty : Convert.ToString(board.LottoMatchedNumbers[2]),
                                       board.LottoMatchedNumbers[3] == 0 ? String.Empty : Convert.ToString(board.LottoMatchedNumbers[3]),
                                       board.LottoMatchedNumbers[4] == 0 ? String.Empty : Convert.ToString(board.LottoMatchedNumbers[4]),
                                       board.LottoMatchedNumbers[5] == 0 ? String.Empty : Convert.ToString(board.LottoMatchedNumbers[5]),
                                       board.LottoBonusMatch ? global::ttpim.gamemodule.Properties.Resources.bonus_24x24: null,
                                       board.WinningBoard ? global::ttpim.gamemodule.Properties.Resources.win_24x24 : null, 
                                       board.LottoDivision,
                                       board.LottoEarning);
                        ResultTable.Rows.Add();

                    }
                }
            }

            dataset.Tables.Add(ResultTable);
            dataset.Tables.Add(PresentationTable);
            return dataset;
        }

        private Image GetLottoWinDivisionImage(int division)
        {
            Image img;
            switch(division)
            {
                case 1:
                    img = global::ttpim.gamemodule.Properties.Resources.jackpothead;
                break;
                case 2:
                    img = global::ttpim.gamemodule.Properties.Resources.div2head;
                break;
                case 3:
                    img = global::ttpim.gamemodule.Properties.Resources.div3head;
                break;
                case 4:
                    img = global::ttpim.gamemodule.Properties.Resources.div4head;
                break;
                case 5:
                    img = global::ttpim.gamemodule.Properties.Resources.div5head;
                break;
                case 6:
                    img = global::ttpim.gamemodule.Properties.Resources.div6head;
                break;
                case 7:
                    img = global::ttpim.gamemodule.Properties.Resources.div7head;
                break;
                default:
                    img = null;
                break;
            
            }
            return img;
        }

        private Image GetPowerballWinDivisionImage(int division)
        {
            Image img;
            switch (division)
            {
                case 1:
                    img = global::ttpim.gamemodule.Properties.Resources.jackpothead;
                    break;
                case 2:
                    img = global::ttpim.gamemodule.Properties.Resources.div2head;
                    break;
                case 3:
                    img = global::ttpim.gamemodule.Properties.Resources.div3head;
                    break;
                case 4:
                    img = global::ttpim.gamemodule.Properties.Resources.div4head;
                    break;
                case 5:
                    img = global::ttpim.gamemodule.Properties.Resources.div5head;
                    break;
                case 6:
                    img = global::ttpim.gamemodule.Properties.Resources.div6head;
                    break;
                case 7:
                    img = global::ttpim.gamemodule.Properties.Resources.div7head;
                    break;
                case 8:
                    img = global::ttpim.gamemodule.Properties.Resources.div8head;
                    break;
                default:
                    img = null;
                    break;

            }
            return img;
        }

        private DataTable PopulateLottoDataTable(DataTable PlayTable, List<LottoGameBoard> GameBoards)
        {
            if ((PlayTable != null) && (GameBoards != null) && (GameBoards.Count > 0))
            {
                foreach (LottoGameBoard board in GameBoards)
                {
                    if (board.NumberLottoMatches > 0)
                    {
                        PlayTable.Rows.Add(board.SubscriptionNr,
                                       board.Game,
                                       board.BoardId,
                                       board.SubcriptionBoard[0] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[0]),
                                       board.SubcriptionBoard[1] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[1]),
                                       board.SubcriptionBoard[2] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[2]),
                                       board.SubcriptionBoard[3] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[3]),
                                       board.SubcriptionBoard[4] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[4]),
                                       board.SubcriptionBoard[5] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[5]),
                                       board.LottoBonusMatch ? global::ttpim.gamemodule.Properties.Resources.bonushead : null,
                                       board.WinningBoard ? GetLottoWinDivisionImage(board.LottoDivisionNumber) : null,
                                       null,
                                       null);

                        PlayTable.Rows.Add(String.Empty,
                                       String.Empty,
                                       board.BoardId,
                                       board.LottoMatchedNumbers[0] == 0 ? String.Empty : Convert.ToString(board.LottoMatchedNumbers[0]),
                                       board.LottoMatchedNumbers[1] == 0 ? String.Empty : Convert.ToString(board.LottoMatchedNumbers[1]),
                                       board.LottoMatchedNumbers[2] == 0 ? String.Empty : Convert.ToString(board.LottoMatchedNumbers[2]),
                                       board.LottoMatchedNumbers[3] == 0 ? String.Empty : Convert.ToString(board.LottoMatchedNumbers[3]),
                                       board.LottoMatchedNumbers[4] == 0 ? String.Empty : Convert.ToString(board.LottoMatchedNumbers[4]),
                                       board.LottoMatchedNumbers[5] == 0 ? String.Empty : Convert.ToString(board.LottoMatchedNumbers[5]),
                                       board.LottoBonusMatch ? global::ttpim.gamemodule.Properties.Resources.bonus_24x24 : null,
                                       board.WinningBoard ? global::ttpim.gamemodule.Properties.Resources.win_24x24 : null, 
                                       board.LottoDivision,
                                       board.LottoEarning);
                        PlayTable.Rows.Add();
                    }
                }
            }
            return PlayTable;
        }

        private DataTable PopulateLottoPlusDataTable(DataTable PlayTable, List<LottoGameBoard> GameBoards)
        {
            if ((PlayTable != null) && (GameBoards != null) && (GameBoards.Count > 0))
            {
                foreach (LottoGameBoard board in GameBoards)
                {
                    if (board.NumberLottoPlusMatches > 0)
                    {
                        PlayTable.Rows.Add(board.SubscriptionNr,
                                       board.Game,
                                       board.BoardId,
                                       board.SubcriptionBoard[0] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[0]),
                                       board.SubcriptionBoard[1] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[1]),
                                       board.SubcriptionBoard[2] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[2]),
                                       board.SubcriptionBoard[3] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[3]),
                                       board.SubcriptionBoard[4] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[4]),
                                       board.SubcriptionBoard[5] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[5]),
                                       board.LottoPlusBonusMatch ? global::ttpim.gamemodule.Properties.Resources.bonushead : null,
                                       board.WinningBoard ? GetLottoWinDivisionImage(board.LottoPlusDivisionNumber) : null,
                                       null,
                                       null);

                        PlayTable.Rows.Add(String.Empty,
                                       board.Game,
                                       board.BoardId,
                                       board.LottoPlusMatchedNumbers[0] == 0 ? String.Empty : Convert.ToString(board.LottoPlusMatchedNumbers[0]),
                                       board.LottoPlusMatchedNumbers[1] == 0 ? String.Empty : Convert.ToString(board.LottoPlusMatchedNumbers[1]),
                                       board.LottoPlusMatchedNumbers[2] == 0 ? String.Empty : Convert.ToString(board.LottoPlusMatchedNumbers[2]),
                                       board.LottoPlusMatchedNumbers[3] == 0 ? String.Empty : Convert.ToString(board.LottoPlusMatchedNumbers[3]),
                                       board.LottoPlusMatchedNumbers[4] == 0 ? String.Empty : Convert.ToString(board.LottoPlusMatchedNumbers[4]),
                                       board.LottoPlusMatchedNumbers[5] == 0 ? String.Empty : Convert.ToString(board.LottoPlusMatchedNumbers[5]),
                                       board.LottoPlusBonusMatch ? global::ttpim.gamemodule.Properties.Resources.bonus_24x24 : null,
                                       board.WinningBoard ? global::ttpim.gamemodule.Properties.Resources.win_24x24 : null, 
                                       board.LottoPlusDivision,
                                       board.LottoPlusEarning);
                        PlayTable.Rows.Add();
                    }
                }
            }
            return PlayTable;
        }

        private DataTable PopulatePowerballDataTable(DataTable PlayTable, List<PowerballGameBoard> GameBoards)
        {
            if ((PlayTable != null) && (GameBoards != null) && (GameBoards.Count > 0))
            {
                foreach (PowerballGameBoard board in GameBoards)
                {
                    if (board.NumberPowerballMatches > 0)
                    {
                        PlayTable.Rows.Add(board.SubscriptionNr,
                                           board.Game,
                                           board.BoardId,
                                           board.SubcriptionBoard[0] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[0]),
                                           board.SubcriptionBoard[1] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[1]),
                                           board.SubcriptionBoard[2] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[2]),
                                           board.SubcriptionBoard[3] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[3]),
                                           board.SubcriptionBoard[4] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[4]),
                                           board.SubcriptionBoard[5] == 0 ? String.Empty : Convert.ToString(board.SubcriptionBoard[5]),
                                           board.PowerballMatch ? global::ttpim.gamemodule.Properties.Resources.bonushead : null,
                                           board.WinningBoard ? GetPowerballWinDivisionImage(board.PowerballDivisionNumber) : null,
                                           null,
                                           null);

                        PlayTable.Rows.Add(String.Empty,
                                       board.Game,
                                       board.BoardId,
                                       board.PowerballMatchedNumbers[0] == 0 ? String.Empty : Convert.ToString(board.PowerballMatchedNumbers[0]),
                                       board.PowerballMatchedNumbers[1] == 0 ? String.Empty : Convert.ToString(board.PowerballMatchedNumbers[1]),
                                       board.PowerballMatchedNumbers[2] == 0 ? String.Empty : Convert.ToString(board.PowerballMatchedNumbers[2]),
                                       board.PowerballMatchedNumbers[3] == 0 ? String.Empty : Convert.ToString(board.PowerballMatchedNumbers[3]),
                                       board.PowerballMatchedNumbers[4] == 0 ? String.Empty : Convert.ToString(board.PowerballMatchedNumbers[4]),
                                       board.PowerballMatchedNumbers[5] == 0 ? String.Empty : Convert.ToString(board.PowerballMatchedNumbers[5]),
                                       board.PowerballMatch ? global::ttpim.gamemodule.Properties.Resources.bonus_24x24 : null,
                                       board.WinningBoard ? global::ttpim.gamemodule.Properties.Resources.win_24x24 : null, 
                                       board.PowerballDivision,
                                       board.PowerballEarning);
                        PlayTable.Rows.Add();
                    }
                }
            }
            return PlayTable;
        }

        public Bitmap GenerateWebPageBitmap(string url)
        {
            WebBrowser browser = new WebBrowser();
            Bitmap result = null;
            browser.ScrollBarsEnabled = false;
            browser.ScriptErrorsSuppressed = true;
            browser.Navigate(url);
            Thread.Sleep(200);

            while (browser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            
            if (browser.ReadyState == WebBrowserReadyState.Complete)
            {
                browser.ScrollBarsEnabled = true;
                browser.ScriptErrorsSuppressed = true;

                browser.Width = browser.Document.Body.ScrollRectangle.Width;
                browser.Height = browser.Document.Body.ScrollRectangle.Height;

                Bitmap bitmap = new Bitmap(browser.Width, browser.Height);
                browser.DrawToBitmap(bitmap, new Rectangle(0, 0, browser.Width, browser.Height));
                browser.ScrollBarsEnabled = true;
                result = bitmap;
                return bitmap;
            }
            return result;
        }

        public void GenerateWebPageBitmap(WebPageSnapshotData snapshot)
        {
            WebBrowser browser = new WebBrowser();
            browser.ScrollBarsEnabled = false;
            browser.ScriptErrorsSuppressed = true;
            if (!string.IsNullOrEmpty(snapshot.Uri))
                browser.Navigate(snapshot.Uri);
            else
                return;

            while (browser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }

            if (browser.ReadyState == WebBrowserReadyState.Complete)
            {
                browser.ScrollBarsEnabled = true;
                browser.ScriptErrorsSuppressed = true;

                browser.Width = browser.Document.Body.ScrollRectangle.Width;
                browser.Height = browser.Document.Body.ScrollRectangle.Height;

                Bitmap bitmap = new Bitmap(browser.Width, browser.Height);
                browser.DrawToBitmap(bitmap, new Rectangle(0, 0, browser.Width, browser.Height));
                browser.ScrollBarsEnabled = true;
                if ((!string.IsNullOrEmpty(snapshot.DataName)) && (snapshot.DataName == "Lotto"))
                    LottoWebPageBitmap = bitmap;
                if ((!string.IsNullOrEmpty(snapshot.DataName)) && (snapshot.DataName == "LottoPlus"))
                    LottoPlusWebPageBitmap = bitmap;
                if ((!string.IsNullOrEmpty(snapshot.DataName)) && (snapshot.DataName == "Powerball"))
                    PowerballWebPageBitmap = bitmap;
            }
        }

        public Image GenerateMessageGraphic(string leftText, string rightText)
        {
            DrawingTextParams drawParams = GetMessageGraphicSetupParams(leftText, rightText);
            Image result = GraphicsController.DrawTextToImage(drawParams, 40, 10);
            return result;
        }

        public Image GenerateGraphic(string leftText, string rightText)
        {
            DrawingTextParams drawParams = GetGraphicSetupParams(leftText, rightText);
            Image result = GraphicsController.DrawTextToImage(drawParams) ;
            return result;
        }

        private DrawingTextParams GetMessageGraphicSetupParams(string leftText, string rightText)
        {
            DrawingTextParams drawParams = new DrawingTextParams();
            ColorConverter convertor = new ColorConverter();
            drawParams.BackColor = System.Drawing.ColorTranslator.FromHtml("#fff7c2");
            drawParams.CornerRadius = 20;
            drawParams.LeftFont = new Font("Arial", 14, FontStyle.Bold);
            drawParams.RightFont = new Font("Arial", 14, FontStyle.Regular);
            drawParams.LeftTextColor = Color.Black;
            drawParams.RightTextColor = Color.White;
            StringFormat leftFormat = new StringFormat();
            StringFormat rightFormat = new StringFormat();
            leftFormat.Alignment = StringAlignment.Near;
            leftFormat.LineAlignment = StringAlignment.Center;
            leftFormat.Trimming = StringTrimming.None;
            rightFormat = leftFormat;
            drawParams.LeftStringFormat = leftFormat;
            drawParams.RightStringFormat = rightFormat;
            drawParams.StartImage = global::ttpim.gamemodule.Properties.Resources.messagebase;
            drawParams.LeftText = leftText;
            drawParams.RightText = rightText;
            return drawParams;
        }

        private DrawingTextParams GetGraphicSetupParams(string leftText, string rightText)
        {
            DrawingTextParams drawParams = new DrawingTextParams();
            drawParams.BackColor = Color.ForestGreen;
            drawParams.CornerRadius = 15;
            drawParams.LeftFont = new Font("Arial", 9, FontStyle.Bold);
            drawParams.RightFont = new Font("Arial", 9, FontStyle.Regular);
            drawParams.LeftTextColor = Color.White;
            drawParams.RightTextColor = Color.White;
            StringFormat leftFormat = new StringFormat();
            StringFormat rightFormat = new StringFormat();
            leftFormat.Alignment = StringAlignment.Near;
            leftFormat.LineAlignment = StringAlignment.Center;
            leftFormat.Trimming = StringTrimming.None;
            rightFormat = leftFormat;
            drawParams.LeftStringFormat = leftFormat;
            drawParams.RightStringFormat = rightFormat;
            drawParams.StartImage = global::ttpim.gamemodule.Properties.Resources.greenstrip;
            drawParams.LeftText = leftText;
            drawParams.RightText = rightText;
            return drawParams;
        }

        public delegate void UpdateTextCallback(WebProcessingData data);

        public void DoLottoWebPageProcessing(WebProcessingData data)
        {
            Thread.Sleep(1000);
            CreateLottoDirectories();

            if ((data.Sender as WebBrowser).InvokeRequired)
            {
                (data.Sender as WebBrowser).Invoke(new UpdateTextCallback(DoLottoWebPageProcessing), new object[] { data });
            }
            else
            {
                SaveLottoWebPage((data.Sender as WebBrowser));
                ProcessLottoWebImages((data.Sender as WebBrowser).Document.GetElementsByTagName("img"));
                ProcessLottoWebJavaScripts((data.Sender as WebBrowser).Document.GetElementsByTagName("script"));
                ProcessLottoWebLinks((data.Sender as WebBrowser).Document.GetElementsByTagName("link"));
                LottoParser.Initialise(LottoDownloadedFilePath);
                LottoResults = LottoParser.HTMLParsedResults;
                LottoGameResults = GetGameResults(LottoResults, "Lotto");
                RaiseLottoFinishDownloadEvent();
            }
        }

        public void DoLottoPlusWebPageProcessing(WebProcessingData data)
        {
            Thread.Sleep(1000);
            CreateLottoPlusDirectories();

            if ((data.Sender as WebBrowser).InvokeRequired)
            {
                (data.Sender as WebBrowser).Invoke(new UpdateTextCallback(DoLottoPlusWebPageProcessing), new object[] { data });
            }
            else
            {
                SaveLottoPlusWebPage((data.Sender as WebBrowser));
                ProcessLottoPlusWebImages((data.Sender as WebBrowser).Document.GetElementsByTagName("img"));
                ProcessLottoPlusWebJavaScripts((data.Sender as WebBrowser).Document.GetElementsByTagName("script"));
                ProcessLottoPlusWebLinks((data.Sender as WebBrowser).Document.GetElementsByTagName("link"));
                LottoPlusParser.Initialise(LottoPlusDownloadedFilePath);
                LottoPlusResults = LottoPlusParser.HTMLParsedResults;
                LottoPlusGameResults = GetGameResults(LottoPlusResults, "LottoPlus");
                RaiseLottoPlusFinishDownloadEvent();
            }
        }

        public void DoPowerballWebPageProcessing(WebProcessingData data)
        {
            Thread.Sleep(1000);
            CreatePowerballDirectories();

            if ((data.Sender as WebBrowser).InvokeRequired)
            {
                (data.Sender as WebBrowser).Invoke(new UpdateTextCallback(DoPowerballWebPageProcessing), new object[] { data });
            }
            else
            {
                SavePowerballWebPage((data.Sender as WebBrowser));
                ProcessPowerballWebImages((data.Sender as WebBrowser).Document.GetElementsByTagName("img"));
                ProcessPowerballWebJavaScripts((data.Sender as WebBrowser).Document.GetElementsByTagName("script"));
                ProcessPowerballWebLinks((data.Sender as WebBrowser).Document.GetElementsByTagName("link"));
                PowerballParser.Initialise(PowerballDownloadedFilePath);
                PowerballResults = PowerballParser.HTMLParsedResults;
                PowerballGameResults = GetPowerGameResults(PowerballParser.HTMLParsedResults, "Powerball");
                RaisePowerballFinishDownloadEvent();
            }
        }

        public void RaiseLottoStartDownloadEvent()
        {
            Thread.Sleep(1);
            if (LottoStartDownloadEvent != null)
                LottoStartDownloadEvent(this, new DownloadEventArgs());
            Thread.Sleep(1);
        }

        public void RaiseLottoPlusStartDownloadEvent()
        {
            if (LottoPlusStartDownloadEvent != null)
                LottoPlusStartDownloadEvent(this, new DownloadEventArgs());
            Thread.Sleep(1);
        }

        public void RaisePowerballStartDownloadEvent()
        {
            Thread.Sleep(1);
            if (PowerballStartDownloadEvent != null)
                PowerballStartDownloadEvent(this, new DownloadEventArgs());
            Thread.Sleep(1);
        }

        private void RaiseLottoFinishDownloadEvent()
        {
            Thread.Sleep(1);
            if (LottoFinishDownloadEvent != null)
                LottoFinishDownloadEvent(this, new DownloadEventArgs(LottoResults));
            LottoDownloadCompleted = true;
            Thread.Sleep(1);
        }

        private void RaiseLottoPlusFinishDownloadEvent()
        {
            if (LottoPlusFinishDownloadEvent != null)
                LottoPlusFinishDownloadEvent(this, new DownloadEventArgs(LottoPlusResults));
            LottoDownloadCompleted = true;
            Thread.Sleep(1);
        }

        private void RaisePowerballFinishDownloadEvent()
        {
            if (PowerballFinishDownloadEvent != null)
                PowerballFinishDownloadEvent(this, new DownloadEventArgs(PowerballResults));
            PowerballDownloadCompleted = true;
            Thread.Sleep(1);
        }

        private void RaiseLottoCardProcessedEvent()
        {
            if (LottoCardProcessedEvent != null)
                LottoCardProcessedEvent(this, new LottoCardProcessedEventArgs(LottoGameResults));
            Thread.Sleep(1);
        }

        private void RaiseLottoCardInProcessingEvent()
        {
            if (LottoCardInProcessingEvent != null)
                LottoCardInProcessingEvent(this, new LottoCardInProcessingEventArgs());
            Thread.Sleep(1);
        }

        private void RaiseLottoParseExceptionEvent()
        {
            if (LottoParseExceptionEvent != null)
                LottoParseExceptionEvent(this, new LottoParseExceptionEventArgs());
            Thread.Sleep(1);
        }

        private void RaiseLottoPlusParseExceptionEvent()
        {
            if (LottoPlusParseExceptionEvent != null)
                LottoPlusParseExceptionEvent(this, new LottoPlusParseExceptionEventArgs());
            Thread.Sleep(1);
        }

        private void RaiseLottoNavigationEvent()
        {
            if (LottoNavigationEvent != null)
                LottoNavigationEvent(this, new WebNavigationEventArgs { Url = this.LottoURL });
            Thread.Sleep(1);
        }

        private void RaiseLottoPlusNavigationEvent()
        {
            if (LottoPlusNavigationEvent != null)
                LottoPlusNavigationEvent(this, new WebNavigationEventArgs { Url = this.LottoPlusURL });
            Thread.Sleep(1);
        }

        private void RaiseLottoSavedWinningsEvent()
        {
            if (LottoSavedWinningsEvent != null)
                LottoSavedWinningsEvent(this, new LottoCardSavedWinningsEventArgs { Message = LottoWinsSavedMessage, NumOfAffectedRecords = NumLottoWinsSaved, Saved = LottoWinningBoardsSaved, Game = "Lotto" });
            Thread.Sleep(1);
        }

        private void RaiseLottoPlusSavedWinningsEvent()
        {
            if (LottoPlusSavedWinningsEvent != null)
                LottoPlusSavedWinningsEvent(this, new LottoCardSavedWinningsEventArgs { Message = LottoPlusWinsSavedMessage, NumOfAffectedRecords = NumLottoPlusWinsSaved, Saved = LottoPlusWinningBoardsSaved, Game = "LottoPlus" });
            Thread.Sleep(1);
        }

        private void RaisePowerballSavedWinningsEvent()
        {
            if (PowerballSavedWinningsEvent != null)
                PowerballSavedWinningsEvent(this, new LottoCardSavedWinningsEventArgs { Message = PowerballWinsSavedMessage, NumOfAffectedRecords = NumPowerballWinsSaved, Saved = PowerballWinningBoardsSaved, Game = "Powerball" });
            Thread.Sleep(1);
        }

    }
}

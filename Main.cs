using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Net.Json;
using HtmlAgilityPack;
using ttpim.gamemodule.lotto.models;
using ttpim.gamemodule.games.controllers;
using ttpim.gamemodule.games.models.lotto;
using ttpim.gamemodule.games.controllers.common;
using ttpim.gamemodule.games.controllers.database;
using ttpim.gamemodule.games.models.powerball;
using ttpim.gamemodule.lotto.controllers;
using Infragistics.Win.UltraWinTabControl;
using ttpim.gamemodule.games.controllers.lotto;
using System.Runtime.Remoting.Messaging;
using ttpim.gamemodule.common;
using ttpim.gamemodule.games.views;
using ttpim.gamemodule;
using Telerik.WinControls;
using FormGridViewStyle = System.Windows.Forms.DataGridViewCellStyle;

namespace ttpim.gamemodule
{
    public partial class Main : Form
    {
        private LottoStartDownloadNotification _lottoStartPlusDownload;
        private LottoFinishDownloadNotification _lottoFinishDownload;
        private LottoPlusStartDownloadNotification _lottoPlusStartDownload;
        private LottoPlusFinishDownloadNotification _lottoPlusFinishDownload;
        private LottoNavigationNotification _lottoNavigate;
        private LottoPlusNavigationNotification _lottoPlusNavigate;
        private PowerballStartDownloadNotification _powerballStartDownload;
        private PowerballFinishDownloadNotification _powerballFinishDownload;
        private LottoCardSavedWinningsNotification _lottoWinningsSaved;
        private LottoCardSavedWinningsNotification _lottoplusWinningsSaved;
        private LottoCardSavedWinningsNotification _powerballWinningsSaved;

        private BackgroundWorker _lottoWorker;
        private BackgroundWorker _lottoPlusWorker;
        private BackgroundWorker _powerballWorker;
        private BackgroundWorker _lottoCompletedWorker;
        private BackgroundWorker _lottoPlusCompletedWorker;
        private BackgroundWorker _powerballCompletedWorker;
        private HTMLParser _lottoParser;
        private HTMLParser _lottoPlusParser;
        private HTMLParser _powerballParser;
        private bool _lottoCompletedNormally;
        private bool _lottoPlusCompletedNormally;
        private bool _powerballCompletedNormally;
        private delegate bool LottoDelegate(LottoGameResults Results);
        private LottoCardServiceManager _cardServiceManager;
        private DataGridViewStyles LottoGridViewStyles;
        private DataGridViewStyles LottoPlusGridViewStyles;
        private DataGridViewStyles PowerballGridViewStyles;

        public delegate void UpdateBasePanelCallback(string pnlName, bool status);
        public delegate void UpdateProgressReportPanelCallback(string pnlName, bool status);
        public delegate void UpdateProgressListBoxCallback(string lstBoxName, string message);
        public delegate void UpdateDateConfirmationCallback(Image dateImage);
        public delegate void UpdateGameNameCallback(string controlName, string gameName);
        public delegate void UpdateImageCallback(Image updateImage, string picName);
        public delegate void UpdateLogoImageCallback(Image logoImage);
        public delegate void UpdateGridViewCallback(string gridName);
        public delegate void UpdateControlEnabledStatusCallback(string controlName, bool status);
        public delegate void UpdateLottoGridViewCallback(object sender, DataGridViewCellPaintingEventArgs e);
        public delegate void UpdateLottoPlusGridViewCallback(object sender, DataGridViewCellPaintingEventArgs e);
        public delegate void UpdatePowerballGridViewCallback(object sender, DataGridViewCellPaintingEventArgs e);
        public delegate void UpdateLottoPanelCallback();

        private SetupData _setupData;
        public SetupData SetupData
        {
            get
            {
                return _setupData;
            }
            set
            {
                _setupData = value;
            }
        }

        public Main()
        {
            InitializeComponent();
            CreateBackgroundWorkers();
            CreateServiceManagers();
            SetupBackgroundWorkers();
            SetupServiceNotifications();
            SetupBrowsers();
            SetNerdiForToday();
        }

        public void CreateBackgroundWorkers()
        {
            LottoBackgroundWorker = new BackgroundWorker();
            LottoPlusBackgroundWorker = new BackgroundWorker();
            PowerBallBackgroundWorker = new BackgroundWorker();
            LottoCompletedDownloadBackgroundWorker = new BackgroundWorker();
            LottoPlusCompletedDownloadBackgroundWorker = new BackgroundWorker();
            PowerballCompletedDownloadBackgroundWorker = new BackgroundWorker();
        }

        public void CreateServiceManagers()
        {
            LottoCardService = new LottoCardServiceManager();
            if (LottoCardService.IsDataProviderAvailable)
            {
                LottoCardService.IntialiseSetup();
            }
            else
            {
                DatasourceSetup setupForm = new DatasourceSetup();
                SetupData = setupForm.Execute();
                if (!String.IsNullOrEmpty(SetupData.DatasourcePath) && SetupData.FileExists && SetupData.PathExists)
                {
                    LottoCardService.IntialiseSetup(SetupData);
                }
            }
        }

        public void SetupBrowsers()
        {
            wbLotto.ScriptErrorsSuppressed = true;
            wbLottoPlus.ScriptErrorsSuppressed = true;
            wbPowerball.ScriptErrorsSuppressed = true;
        }

        private void SetNerdiForToday()
        {
            pbxNerdi.Image = ApplicationController.GetNerdiImage(DateTime.Today.DayOfWeek);
        }

        public LottoCardServiceManager LottoCardService
        {
            get
            {
                return _cardServiceManager;
            }
            set
            {
                _cardServiceManager = value;
            }
        }

        public BackgroundWorker LottoBackgroundWorker
        {
            get
            {
                return _lottoWorker;
            }
            set
            {
                _lottoWorker = value;
            }
        }

        public BackgroundWorker LottoPlusBackgroundWorker
        {
            get
            {
                return _lottoPlusWorker;
            }
            set
            {
                _lottoPlusWorker = value;
            }
        }

        public BackgroundWorker PowerBallBackgroundWorker
        {
            get
            {
                return _powerballWorker;
            }
            set
            {
                _powerballWorker = value;
            }
        }

        public BackgroundWorker LottoCompletedDownloadBackgroundWorker
        {
            get
            {
                return _lottoCompletedWorker;
            }
            set
            {
                _lottoCompletedWorker = value;
            }
        }

        public BackgroundWorker LottoPlusCompletedDownloadBackgroundWorker
        {
            get
            {
                return _lottoPlusCompletedWorker;
            }
            set
            {
                _lottoPlusCompletedWorker = value;
            }
        }

        public BackgroundWorker PowerballCompletedDownloadBackgroundWorker
        {
            get
            {
                return _powerballCompletedWorker;
            }
            set
            {
                _powerballCompletedWorker = value;
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

        public LottoStartDownloadNotification LottoDownloadStartDelegate
        {
            get
            {
                return _lottoStartPlusDownload;
            }
            set
            {
                _lottoStartPlusDownload = value;
            }
        }

        public LottoFinishDownloadNotification LottoDownloadFinishDelegate
        {
            get
            {
                return _lottoFinishDownload;
            }
            set
            {
                _lottoFinishDownload = value;
            }
        }

        public LottoPlusStartDownloadNotification LottoPlusDownloadStartDelegate
        {
            get
            {
                return _lottoPlusStartDownload;
            }
            set
            {
                _lottoPlusStartDownload = value;
            }
        }

        public LottoPlusFinishDownloadNotification LottoPlusDownloadFinishDelegate
        {
            get
            {
                return _lottoPlusFinishDownload;
            }
            set
            {
                _lottoPlusFinishDownload = value;
            }
        }

        public PowerballStartDownloadNotification PowerballDownloadStartDelegate
        {
            get
            {
                return _powerballStartDownload;
            }
            set
            {
                _powerballStartDownload = value;
            }
        }

        public PowerballFinishDownloadNotification PowerballDownloadFinishDelegate
        {
            get
            {
                return _powerballFinishDownload;
            }
            set
            {
                _powerballFinishDownload = value;
            }
        }

        public LottoNavigationNotification LottoNavigationDelegate
        {
            get
            {
                return _lottoNavigate;
            }
            set
            {
                _lottoNavigate = value;
            }
        }

        public LottoPlusNavigationNotification LottoPlusNavigationDelegate
        {
            get
            {
                return _lottoPlusNavigate;
            }
            set
            {
                _lottoPlusNavigate = value;
            }
        }

        public LottoCardSavedWinningsNotification LottoCardSavedLottoWinningsDelegate
        {
            get
            {
                return _lottoWinningsSaved;
            }
            set
            {
                _lottoWinningsSaved = value;
            }
        }

        public LottoCardSavedWinningsNotification LottoCardSavedLottoPlusWinningsDelegate
        {
            get
            {
                return _lottoplusWinningsSaved;
            }
            set
            {
                _lottoplusWinningsSaved = value;
            }
        }

        public LottoCardSavedWinningsNotification LottoCardSavedPowerballWinningsDelegate
        {
            get
            {
                return _powerballWinningsSaved;
            }
            set
            {
                _powerballWinningsSaved = value;
            }
        }

        public bool LottoCompletedNormally
        {
            get
            {
                return _lottoCompletedNormally;
            }
            set
            {
                _lottoCompletedNormally = value;
            }
        }

        public bool LottoPlusCompletedNormally
        {
            get
            {
                return _lottoPlusCompletedNormally;
            }
            set
            {
                _lottoPlusCompletedNormally = value;
            }
        }

        public bool PowerballCompletedNormally
        {
            get
            {
                return _powerballCompletedNormally;
            }
            set
            {
                _powerballCompletedNormally = value;
            }
        }

        private void SetupBackgroundWorkers()
        {
            LottoBackgroundWorker.WorkerReportsProgress = true;
            LottoBackgroundWorker.WorkerSupportsCancellation = true;
            LottoPlusBackgroundWorker.WorkerReportsProgress = true;
            LottoPlusBackgroundWorker.WorkerSupportsCancellation = true;
            PowerBallBackgroundWorker.WorkerReportsProgress = true;
            PowerBallBackgroundWorker.WorkerSupportsCancellation = true;
            LottoCompletedDownloadBackgroundWorker.WorkerReportsProgress = true;
            LottoCompletedDownloadBackgroundWorker.WorkerSupportsCancellation = true;
            LottoPlusCompletedDownloadBackgroundWorker.WorkerReportsProgress = true;
            LottoPlusCompletedDownloadBackgroundWorker.WorkerSupportsCancellation = true;
            PowerballCompletedDownloadBackgroundWorker.WorkerReportsProgress = true;
            PowerballCompletedDownloadBackgroundWorker.WorkerSupportsCancellation = true;

            LottoBackgroundWorker.DoWork += DoLottoWork_Handler;
            LottoBackgroundWorker.ProgressChanged += ProgressChangedLotto_Handler;
            LottoBackgroundWorker.RunWorkerCompleted += RunLottoWorkerCompleted_Handler;
            LottoPlusBackgroundWorker.DoWork += DoLottoPlusWork_Handler;
            LottoPlusBackgroundWorker.ProgressChanged += ProgressChangedLottoPlus_Handler;
            LottoPlusBackgroundWorker.RunWorkerCompleted += RunLottoPlusWorkerCompleted_Handler;
            PowerBallBackgroundWorker.DoWork += DoPowerballWork_Handler;
            PowerBallBackgroundWorker.ProgressChanged += ProgressChangedPowerball_Handler;
            PowerBallBackgroundWorker.RunWorkerCompleted += RunPowerballWorkerCompleted_Handler;

            LottoCompletedDownloadBackgroundWorker.DoWork += DoLottoCompletedDownloadWork_Handler;
            LottoCompletedDownloadBackgroundWorker.ProgressChanged += ProgressChangedLottoCompletedDownload_Handler;
            LottoCompletedDownloadBackgroundWorker.RunWorkerCompleted += RunLottoWorkerLottoCompletedDownload_Handler;
        }

        public void DoLottoCompletedDownloadWork_Handler(object sender, DoWorkEventArgs args)
        {
            lblInProgress.Visible = true;
            BackgroundWorker worker = sender as BackgroundWorker;
            DownloadData data = (DownloadData)args.Argument;
            LottoCardServiceManager manager = new LottoCardServiceManager();
            MemoryStream m = new MemoryStream();
        }

        private void ProgressChangedLottoCompletedDownload_Handler(object sender, ProgressChangedEventArgs args)
        {
        }

        private void RunLottoWorkerLottoCompletedDownload_Handler(object sender, RunWorkerCompletedEventArgs args)
        {
            LottoCompletedNormally = !args.Cancelled;
            lblInProgress.Visible = false;
        }

        public void DoLottoWork_Handler(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            wbLotto.Navigate(LottoCardService.LottoURI);
        }

        private void ProgressChangedLotto_Handler(object sender, ProgressChangedEventArgs args)
        {

        }

        private void RunLottoWorkerCompleted_Handler(object sender, RunWorkerCompletedEventArgs args)
        {
            LottoCompletedNormally = !args.Cancelled;
            lblInProgress.Visible = false;
        }


        public void DoLottoPlusWork_Handler(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            wbLottoPlus.Navigate(LottoCardService.LottoPlusURI);
            if (worker.CancellationPending)
            {
                args.Cancel = true;
                worker.ReportProgress(-1);
            }
            else
            {
            }
        }

        private void ProgressChangedLottoPlus_Handler(object sender, ProgressChangedEventArgs args)
        {

        }

        private void RunLottoPlusWorkerCompleted_Handler(object sender, RunWorkerCompletedEventArgs args)
        {
            LottoPlusCompletedNormally = !args.Cancelled;
            lblInProgress.Visible = false;
        }

        public void DoPowerballWork_Handler(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            wbPowerball.Navigate(LottoCardService.PowerballURI);
            if (worker.CancellationPending)
            {
                args.Cancel = true;
                worker.ReportProgress(-1);
            }
            else
            {
            }
        }

        private void ProgressChangedPowerball_Handler(object sender, ProgressChangedEventArgs args)
        {

        }

        private void RunPowerballWorkerCompleted_Handler(object sender, RunWorkerCompletedEventArgs args)
        {
            PowerballCompletedNormally = !args.Cancelled;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            DatabaseController.LoadGameSetting();
            menuMar.Groups[0].Active = true;
            pnlControlPanel.Select();
            if (InternetController.GetConnectionStatus())
            {
                pnlConnectionMessage.Visible = false;
                picBoxConnection.Image = global::ttpim.gamemodule.Properties.Resources.connected_y;
                picBoxConnection.SizeMode = PictureBoxSizeMode.AutoSize;
                picBoxConnection.Visible = true;
                btnWork.Image = global::ttpim.gamemodule.Properties.Resources.getlatest_3;
                btnRetryCancel.Image = global::ttpim.gamemodule.Properties.Resources.cancel;
            }
            else
            {
                pnlConnectionMessage.Visible = true;
                pnlConnectionMessage.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                picBoxAlert.Visible = true;
                picBoxConnectStatus.Image = global::ttpim.gamemodule.Properties.Resources.nointernet2;
                picBoxConnectStatus.SizeMode = PictureBoxSizeMode.AutoSize;
                btnWork.Image = global::ttpim.gamemodule.Properties.Resources.workoffline3;
                btnRetryCancel.Image = global::ttpim.gamemodule.Properties.Resources.retry4;
            }
        }

        private void WorkOffline()
        {
            TopHeaderGrp.Collapsed = true;
            label2.Select();
        }

        private void WorkOnline()
        {
            DisplayOnlineStatus();
            StartLottoWorker();
            StartLottoPlusWorker();
            StartPowerballWorker();
            DisableWebReqests();
        }

        public void DisplayOnlineStatus()
        {
            lblInProgress.Visible = true;
        }

        public void DisableWebReqests()
        {
            if (btnWork.Enabled == true)
                btnWork.Enabled = false;
        }

        public void EnableWebReqests()
        {
            if (btnWork.InvokeRequired)
            {
                btnWork.Invoke(new UpdateControlEnabledStatusCallback(UpdateControlEnabledStatus), new object[] { btnWork.Name, true });
            }
            else
            {
                if (btnWork.Enabled == false)
                    btnWork.Enabled = true;
            }
        }

        public void StartLottoWorker()
        {
            if (!LottoBackgroundWorker.IsBusy)
            {
                LottoBackgroundWorker.RunWorkerAsync();
            }
        }

        public void StartLottoPlusWorker()
        {
            if (!LottoPlusBackgroundWorker.IsBusy)
            {
                LottoPlusBackgroundWorker.RunWorkerAsync();
            }
        }

        public void StartPowerballWorker()
        {
            if (!PowerBallBackgroundWorker.IsBusy)
            {
                PowerBallBackgroundWorker.RunWorkerAsync();
            }
        }

        public void StartLottoCompletedDownloadWorker(DownloadData Data)
        {
            if (!LottoCompletedDownloadBackgroundWorker.IsBusy)
                LottoCompletedDownloadBackgroundWorker.RunWorkerAsync(Data);
        }

        private void SetupServiceNotifications()
        {
            LottoDownloadStartDelegate = new LottoStartDownloadNotification(LottoStartDownloadHandler);
            LottoDownloadFinishDelegate = new LottoFinishDownloadNotification(LottoFinishDownloadHandler);
            LottoPlusDownloadStartDelegate = new LottoPlusStartDownloadNotification(LottoPlusStartDownloadHandler);
            LottoPlusDownloadFinishDelegate = new LottoPlusFinishDownloadNotification(LottoPlusFinishDownloadHandler);
            PowerballDownloadStartDelegate = new PowerballStartDownloadNotification(PowerballStartDownloadHandler);
            PowerballDownloadFinishDelegate = new PowerballFinishDownloadNotification(PowerballFinishDownloadHandler);
            LottoCardSavedLottoWinningsDelegate = new LottoCardSavedWinningsNotification(LottoCardSavedLottoWinningsHandler);
            LottoCardSavedLottoPlusWinningsDelegate = new LottoCardSavedWinningsNotification(LottoCardSavedLottoPlusWinningsHandler);
            LottoCardSavedPowerballWinningsDelegate = new LottoCardSavedWinningsNotification(LottoCardSavedPowerballWinningsHandler);
            LottoCardService.LottoStartDownloadEvent += LottoDownloadStartDelegate;
            LottoCardService.LottoFinishDownloadEvent += LottoDownloadFinishDelegate;
            LottoCardService.LottoPlusStartDownloadEvent += LottoPlusDownloadStartDelegate;
            LottoCardService.LottoPlusFinishDownloadEvent += LottoPlusDownloadFinishDelegate;
            LottoCardService.PowerballStartDownloadEvent += PowerballDownloadStartDelegate;
            LottoCardService.PowerballFinishDownloadEvent += PowerballDownloadFinishDelegate;
            LottoCardService.LottoSavedWinningsEvent += LottoCardSavedLottoWinningsDelegate;
            LottoCardService.LottoPlusSavedWinningsEvent += LottoCardSavedLottoPlusWinningsDelegate;
            LottoCardService.PowerballSavedWinningsEvent += LottoCardSavedPowerballWinningsDelegate;
        }

        public void LottoNavigationHandler(object sender, WebNavigationEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new LottoNavigationNotification(LottoNavigationHandler), new object[] { sender, args });
            }
            else
            {
            }
        }

        public void LottoPlusNavigationHandler(object sender, WebNavigationEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new LottoPlusNavigationNotification(LottoNavigationHandler), new object[] { sender, args });
            }
            else
            {
            }
        }

        public void LottoStartDownloadHandler(object sender, DownloadEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new LottoStartDownloadNotification(LottoStartDownloadHandler), new object[] { sender, args });
            }
            else
            {
                if (pnlProgressReport.Visible == false)
                    pnlProgressReport.Visible = true;
                lstBoxLottoProgress.Items.Add("Starting Lotto webpage download...");
                pbxLottoProgress.Visible = true;
                pbxLottoProgress.Image = global::ttpim.gamemodule.Properties.Resources.animated_loading;
            }
        }

        private delegate int SavePowerballWinningDelegate(List<PowerballGameBoard> Winnings, string Game);

        public void LottoFinishDownloadHandler(object sender, DownloadEventArgs args)
        {
            LottoFinishDownloadNotification del = new LottoFinishDownloadNotification(ProcessLottoFinishDownload);
            IAsyncResult iasync = del.BeginInvoke(sender, args, null, null);
        }

        #region Update GUI Delegate methods
        private void UpdateBasePanel(string pnlName, bool status)
        {
            switch (pnlName)
            {
                case "pnlLotto":
                    pnlLotto.Visible = status;
                    break;
                case "pnlLottoPlus":
                    pnlLottoPlus.Visible = status;
                    break;
                case "pnlPowerball":
                    pnlPowerball.Visible = status;
                    break;
                default:
                    break;
            }
        }

        private void UpdateProgressReportPanel(string pnlName, bool status)
        {
            switch (pnlName)
            {
                case "pnlProgressReport":
                    if (pnlProgressReport.Visible != status)
                        pnlProgressReport.Visible = status;
                    break;
                default:
                    break;
            }
        }

        private void UpdateProgressListBox(string lstBoxName, string message)
        {
            switch (lstBoxName)
            {
                case "lstBoxLottoProgress":
                    lstBoxLottoProgress.Items.Add(message);
                    break;
                case "lstBoxLottoPlusProgress":
                    lstBoxLottoPlusProgress.Items.Add(message);
                    break;
                case "lstBoxPowerballProgress":
                    lstBoxPowerballProgress.Items.Add(message);
                    break;
                default:
                    break;
            }
        }

        private void UpdateGameName(string controlName, string gameName)
        {
            switch (controlName)
            {
                case "lblGame_lotto":
                    lblGame_lotto.Text = gameName;
                    break;
                case "lblGame_lottoPlus":
                    lblGame_lottoPlus.Text = gameName;
                    break;
                case "lblGame_powerball":
                    lblGame_powerball.Text = gameName;
                    break;
                default:
                    break;
            }
        }

        private void UpdateLogoImage(Image logoImage)
        {
            pbxGame_lotto.Image = logoImage;
        }

        private void UpdateImage(Image updatedImage, string picName)
        {
            switch (picName)
            {
                case "pbx1_lotto":
                    pbx1_lotto.Image = updatedImage;
                    break;
                case "pbx2_lotto":
                    pbx2_lotto.Image = updatedImage;
                    break;
                case "pbx3_lotto":
                    pbx3_lotto.Image = updatedImage;
                    break;
                case "pbx4_lotto":
                    pbx4_lotto.Image = updatedImage;
                    break;
                case "pbx5_lotto":
                    pbx5_lotto.Image = updatedImage;
                    break;
                case "pbx6_lotto":
                    pbx6_lotto.Image = updatedImage;
                    break;
                case "pbxBonus_lotto":
                    pbxBonus_lotto.Image = updatedImage;
                    break;
                case "pbxGame_lotto":
                    pbxGame_lotto.Image = updatedImage;
                    break;

                case "pbxLottoProgress":
                    pbxLottoProgress.Image = updatedImage;
                    break;
                case "picBoxLottoMessage":
                    picBoxLottoMessage.Image = updatedImage;
                    break;
                case "picBoxLottoWin":
                    picBoxLottoWin.Image = updatedImage;
                    picBoxLottoWin.SizeMode = PictureBoxSizeMode.AutoSize;
                    picBoxLottoWin.SizeMode = PictureBoxSizeMode.CenterImage;
                    break;

                case "pbx1_lottoplus":
                    pbx1_lottoplus.Image = updatedImage;
                    break;
                case "pbx2_lottoplus":
                    pbx2_lottoplus.Image = updatedImage;
                    break;
                case "pbx3_lottoplus":
                    pbx3_lottoplus.Image = updatedImage;
                    break;
                case "pbx4_lottoplus":
                    pbx4_lottoplus.Image = updatedImage;
                    break;
                case "pbx5_lottoplus":
                    pbx5_lottoplus.Image = updatedImage;
                    break;
                case "pbx6_lottoplus":
                    pbx6_lottoplus.Image = updatedImage;
                    break;
                case "pbxBonus_lottoplus":
                    pbxBonus_lottoplus.Image = updatedImage;
                    break;
                case "pbxLottoPlusProgress":
                    pbxLottoPlusProgress.Image = updatedImage;
                    break;
                case "picBoxLottoPlusMessage":
                    picBoxLottoPlusMessage.Image = updatedImage;
                    break;
                case "picBoxLottoPlusWin":
                    picBoxLottoPlusWin.Image = updatedImage;
                    break;
                case "pbxGame_lottoplus":
                    pbxGame_lottoplus.Image = updatedImage;
                    break;


                case "pbx1_powerball":
                    pbx1_powerball.Image = updatedImage;
                    break;
                case "pbx2_powerball":
                    pbx2_powerball.Image = updatedImage;
                    break;
                case "pbx3_powerball":
                    pbx3_powerball.Image = updatedImage;
                    break;
                case "pbx4_powerball":
                    pbx4_powerball.Image = updatedImage;
                    break;
                case "pbx5_powerball":
                    pbx5_powerball.Image = updatedImage;
                    break;
                case "pbxBonus_powerball":
                    pbxBonus_powerball.Image = updatedImage;
                    break;
                case "pbxPowerballProgress":
                    pbxPowerballProgress.Image = updatedImage;
                    break;
                case "picBoxPowerballMessage":
                    picBoxPowerballMessage.Image = updatedImage;
                    break;
                case "picBoxPowerballWin":
                    picBoxPowerballWin.Image = updatedImage;
                    break;

                case "picBoxLottoDateConfirm":
                    picBoxLottoDateConfirm.Image = updatedImage;
                    break;
                case "picBoxLottoPlusDateConfirm":
                    picBoxLottoPlusDateConfirm.Image = updatedImage;
                    break;
                case "picBoxPowerballDateConfirm":
                    picBoxPowerballDateConfirm.Image = updatedImage;
                    break;

                case "pbxGame_powerball":
                    pbxGame_powerball.Image = updatedImage;
                    break;

                default:
                    break;
            }
        }

        private void UpdateGridView(string gridName)
        {
            switch (gridName)
            {
                case "grdViewLotto":
                    grdViewLotto.Columns.Clear();
                    grdViewLotto.Rows.Clear();
                    grdViewLotto.DataSource = LottoCardService.GetLottoTableView();
                    break;
                case "grdViewLottoPlus":
                    grdViewLottoPlus.Columns.Clear();
                    grdViewLottoPlus.Rows.Clear();
                    grdViewLottoPlus.DataSource = LottoCardService.GetLottoPlusTableView();
                    break;
                case "grdViewPowerball":
                    grdViewPowerball.Columns.Clear();
                    grdViewPowerball.Rows.Clear();
                    grdViewPowerball.DataSource = LottoCardService.GetPowerballTableView();
                    break;
                default:
                    break;
            }
        }

        private void UpdateControlEnabledStatus(string controlName, bool status)
        {
            switch (controlName)
            {
                case "btnWork":
                    btnWork.Enabled = status;
                    break;
                default:
                    break;
            }
        }
        #endregion

        public void ProcessLottoFinishDownload(object sender, DownloadEventArgs args)
        {

            if (pnlProgressReport.InvokeRequired)
            {
                pnlProgressReport.Invoke(new UpdateProgressReportPanelCallback(this.UpdateProgressReportPanel), new object[] { pnlProgressReport.Name, true });
            }
            else
            {
                pnlProgressReport.Visible = true;
            }

            if (lstBoxLottoProgress.InvokeRequired)
            {
                lstBoxLottoProgress.Invoke(new UpdateProgressListBoxCallback(this.UpdateProgressListBox), new object[] { lstBoxLottoProgress.Name, "Lotto webpage downloaded." });
            }
            else
            {
                lstBoxLottoProgress.Items.Add("Lotto webpage downloaded.");
            }
            if (pnlLotto.InvokeRequired)
            {
                pnlLotto.Invoke(new UpdateBasePanelCallback(UpdateBasePanel), new object[] { pnlLotto.Name, true });
            }
            else
            {
                pnlLotto.Visible = true;
            }

            if (picBoxLottoDateConfirm.InvokeRequired)
            {
                picBoxLottoDateConfirm.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { LottoCardService.GenerateGraphic(args.Results.GameDate, args.Results.HeadingLastestResults), picBoxLottoDateConfirm.Name });
            }
            else
            {
                picBoxLottoDateConfirm.Image = LottoCardService.GenerateGraphic(args.Results.GameDate, args.Results.HeadingLastestResults);
            }

            if (lblGame_lotto.InvokeRequired)
            {
                lblGame_lotto.Invoke(new UpdateGameNameCallback(this.UpdateGameName), new object[] { lblGame_lotto.Name, args.Results.GameName });
            }
            else
            {
                lblGame_lotto.Text = args.Results.GameName;
            }

            if (pbxGame_lotto.InvokeRequired)
            {
                pbxGame_lotto.Invoke(new UpdateLogoImageCallback(this.UpdateLogoImage), new object[] { (Image)args.Results.LogoImage });
            }
            else
            {
                pbxGame_lotto.Image = (Image)args.Results.LogoImage;
            }

            if (pbx1_lotto.InvokeRequired)
            {
                pbx1_lotto.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-1"], pbx1_lotto.Name });
            }
            else
            {
                pbx1_lotto.Image = (Image)args.Results.WinImages["ball-1"];
            }

            if (pbx2_lotto.InvokeRequired)
            {
                pbx2_lotto.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-2"], pbx2_lotto.Name });
            }
            else
            {
                pbx2_lotto.Image = (Image)args.Results.WinImages["ball-2"];
            }

            if (pbx3_lotto.InvokeRequired)
            {
                pbx3_lotto.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-3"], pbx3_lotto.Name });
            }
            else
            {
                pbx3_lotto.Image = (Image)args.Results.WinImages["ball-3"];
            }

            if (pbx4_lotto.InvokeRequired)
            {
                pbx4_lotto.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-4"], pbx4_lotto.Name });
            }
            else
            {
                pbx4_lotto.Image = (Image)args.Results.WinImages["ball-4"];
            }

            if (pbx5_lotto.InvokeRequired)
            {
                pbx5_lotto.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-5"], pbx5_lotto.Name });
            }
            else
            {
                pbx5_lotto.Image = (Image)args.Results.WinImages["ball-5"];
            }

            if (pbx6_lotto.InvokeRequired)
            {
                pbx6_lotto.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-6"], pbx6_lotto.Name });
            }
            else
            {
                pbx6_lotto.Image = (Image)args.Results.WinImages["ball-6"];
            }

            if (pbxBonus_lotto.InvokeRequired)
            {
                pbxBonus_lotto.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["bonusball"], pbxBonus_lotto.Name });
            }
            else
            {
                pbxBonus_lotto.Image = (Image)args.Results.WinImages["bonusball"];
            }

            if (pbxLottoProgress.InvokeRequired)
            {
                pbxLottoProgress.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { global::ttpim.gamemodule.Properties.Resources.ok, pbxLottoProgress.Name });
            }
            else
            {
                pbxLottoProgress.Image = global::ttpim.gamemodule.Properties.Resources.ok;
            }


            if (lstBoxLottoProgress.InvokeRequired)
            {
                lstBoxLottoProgress.Invoke(new UpdateProgressListBoxCallback(this.UpdateProgressListBox), new object[] { lstBoxLottoProgress.Name, "Lotto processing completed successfully." });
            }
            else
            {
                lstBoxLottoProgress.Items.Add("Lotto processing completed successfully.");
            }

            if (picBoxLottoMessage.InvokeRequired)
            {
                picBoxLottoMessage.Invoke(new UpdateImageCallback(UpdateImage), new object[] { LottoCardService.GetLottoMessageImage(), picBoxLottoMessage.Name });
            }
            else
            {
                picBoxLottoMessage.Image = LottoCardService.GetLottoMessageImage();
            }

            if (picBoxLottoWin.InvokeRequired)
            {
                picBoxLottoWin.Invoke(new UpdateImageCallback(UpdateImage), new object[] { LottoCardService.GetLottoWinImage(), picBoxLottoWin.Name });
            }
            else
            {
                picBoxLottoWin.Image = LottoCardService.GetLottoWinImage();
                picBoxLottoWin.SizeMode = PictureBoxSizeMode.AutoSize;
                picBoxLottoWin.SizeMode = PictureBoxSizeMode.CenterImage;
            }

            DisplayLottoResults();

            if (btnWork.InvokeRequired)
            {
                btnWork.Invoke(new UpdateControlEnabledStatusCallback(UpdateControlEnabledStatus), new object[] { btnWork.Name, true });
            }
            else
            {
                if (btnWork.Enabled == false)
                    btnWork.Enabled = true;
            }

        }

        public void ProcessLottoPlusFinishDownload(object sender, DownloadEventArgs args)
        {
            if (pnlProgressReport.InvokeRequired)
            {
                pnlProgressReport.Invoke(new UpdateProgressReportPanelCallback(this.UpdateProgressReportPanel), new object[] { pnlProgressReport.Name, true });
            }
            else
            {
                pnlProgressReport.Visible = true;
            }

            if (lstBoxLottoPlusProgress.InvokeRequired)
            {
                lstBoxLottoPlusProgress.Invoke(new UpdateProgressListBoxCallback(this.UpdateProgressListBox), new object[] { lstBoxLottoPlusProgress.Name, "LottoPlus webpage downloaded." });
            }
            else
            {
                lstBoxLottoPlusProgress.Items.Add("LottoPlus webpage downloaded.");
            }

            if (picBoxLottoPlusDateConfirm.InvokeRequired)
            {
                picBoxLottoPlusDateConfirm.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { LottoCardService.GenerateGraphic(args.Results.GameDate, args.Results.HeadingLastestResults), picBoxLottoPlusDateConfirm.Name });
            }
            else
            {
                picBoxLottoPlusDateConfirm.Image = LottoCardService.GenerateGraphic(args.Results.GameDate, args.Results.HeadingLastestResults);
            }

            if (lblGame_lottoPlus.InvokeRequired)
            {
                lblGame_lottoPlus.Invoke(new UpdateGameNameCallback(this.UpdateGameName), new object[] { lblGame_lottoPlus.Name, args.Results.GameName });
            }
            else
            {
                lblGame_lottoPlus.Text = args.Results.GameName;
            }

            if (pbxGame_lottoplus.InvokeRequired)
            {
                pbxGame_lottoplus.Invoke(new UpdateLogoImageCallback(this.UpdateLogoImage), new object[] { (Image)args.Results.LogoImage });
            }
            else
            {
                pbxGame_lottoplus.Image = (Image)args.Results.LogoImage;
            }

            if (pbx1_lottoplus.InvokeRequired)
            {
                pbx1_lottoplus.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-1"], pbx1_lottoplus.Name });
            }
            else
            {
                pbx1_lottoplus.Image = (Image)args.Results.WinImages["ball-1"];
            }

            if (pbx2_lottoplus.InvokeRequired)
            {
                pbx2_lottoplus.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-2"], pbx2_lottoplus.Name });
            }
            else
            {
                pbx2_lottoplus.Image = (Image)args.Results.WinImages["ball-2"];
            }

            if (pbx3_lottoplus.InvokeRequired)
            {
                pbx3_lottoplus.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-3"], pbx3_lottoplus.Name });
            }
            else
            {
                pbx3_lotto.Image = (Image)args.Results.WinImages["ball-3"];
            }

            if (pbx4_lottoplus.InvokeRequired)
            {
                pbx4_lottoplus.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-4"], pbx4_lottoplus.Name });
            }
            else
            {
                pbx4_lottoplus.Image = (Image)args.Results.WinImages["ball-4"];
            }

            if (pbx5_lottoplus.InvokeRequired)
            {
                pbx5_lottoplus.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-5"], pbx5_lottoplus.Name });
            }
            else
            {
                pbx5_lottoplus.Image = (Image)args.Results.WinImages["ball-5"];
            }

            if (pbx6_lottoplus.InvokeRequired)
            {
                pbx6_lottoplus.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-6"], pbx6_lottoplus.Name });
            }
            else
            {
                pbx6_lottoplus.Image = (Image)args.Results.WinImages["ball-6"];
            }

            if (pbxBonus_lottoplus.InvokeRequired)
            {
                pbxBonus_lottoplus.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["bonusball"], pbxBonus_lottoplus.Name });
            }
            else
            {
                pbxBonus_lottoplus.Image = (Image)args.Results.WinImages["bonusball"];
            }

            if (pbxLottoPlusProgress.InvokeRequired)
            {
                pbxLottoPlusProgress.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { global::ttpim.gamemodule.Properties.Resources.ok, pbxLottoPlusProgress.Name });
            }
            else
            {
                pbxLottoPlusProgress.Image = global::ttpim.gamemodule.Properties.Resources.ok;
            }


            if (lstBoxLottoPlusProgress.InvokeRequired)
            {
                lstBoxLottoPlusProgress.Invoke(new UpdateProgressListBoxCallback(this.UpdateProgressListBox), new object[] { lstBoxLottoPlusProgress.Name, "Lotto processing completed successfully." });
            }
            else
            {
                lstBoxLottoPlusProgress.Items.Add("Lotto processing completed successfully.");
            }
            DisplayLottoPlusResults();
            EnableWebReqests();
        }

        public void ProcessPowerballFinishDownload(object sender, DownloadEventArgs args)
        {
            if (pnlProgressReport.InvokeRequired)
            {
                pnlProgressReport.Invoke(new UpdateProgressReportPanelCallback(this.UpdateProgressReportPanel), new object[] { pnlProgressReport.Name, true });
            }
            else
            {
                pnlProgressReport.Visible = true;
            }

            if (lstBoxPowerballProgress.InvokeRequired)
            {
                lstBoxPowerballProgress.Invoke(new UpdateProgressListBoxCallback(this.UpdateProgressListBox), new object[] { lstBoxPowerballProgress.Name, "Powerball webpage downloaded." });
            }
            else
            {
                lstBoxPowerballProgress.Items.Add("Powerball webpage downloaded.");
            }

            if (picBoxPowerballDateConfirm.InvokeRequired)
            {
                picBoxPowerballDateConfirm.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { LottoCardService.GenerateGraphic(args.Results.GameDate, args.Results.HeadingLastestResults), picBoxPowerballDateConfirm.Name });
            }
            else
            {
                picBoxPowerballDateConfirm.Image = LottoCardService.GenerateGraphic(args.Results.GameDate, args.Results.HeadingLastestResults);
            }

            if (lblGame_powerball.InvokeRequired)
            {
                lblGame_powerball.Invoke(new UpdateGameNameCallback(this.UpdateGameName), new object[] { lblGame_powerball.Name, args.Results.GameName });
            }
            else
            {
                lblGame_powerball.Text = args.Results.GameName;
            }

            if (pbxGame_powerball.InvokeRequired)
            {
                pbxGame_powerball.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.LogoImage, pbxGame_powerball.Name });
            }
            else
            {
                pbxGame_powerball.Image = (Image)args.Results.LogoImage;
            }

            if (pbx1_powerball.InvokeRequired)
            {
                pbx1_powerball.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-1"], pbx1_powerball.Name });
            }
            else
            {
                pbx1_powerball.Image = (Image)args.Results.WinImages["ball-1"];
            }

            if (pbx2_powerball.InvokeRequired)
            {
                pbx2_powerball.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-2"], pbx2_powerball.Name });
            }
            else
            {
                pbx2_lotto.Image = (Image)args.Results.WinImages["ball-2"];
            }

            if (pbx3_powerball.InvokeRequired)
            {
                pbx3_powerball.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-3"], pbx3_powerball.Name });
            }
            else
            {
                pbx3_powerball.Image = (Image)args.Results.WinImages["ball-3"];
            }

            if (pbx4_powerball.InvokeRequired)
            {
                pbx4_powerball.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-4"], pbx4_powerball.Name });
            }
            else
            {
                pbx4_powerball.Image = (Image)args.Results.WinImages["ball-4"];
            }

            if (pbx5_powerball.InvokeRequired)
            {
                pbx5_powerball.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["ball-5"], pbx5_powerball.Name });
            }
            else
            {
                pbx5_powerball.Image = (Image)args.Results.WinImages["ball-5"];
            }

            if (pbxBonus_powerball.InvokeRequired)
            {
                pbxBonus_powerball.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { (Image)args.Results.WinImages["bonusball"], pbxBonus_powerball.Name });
            }
            else
            {
                pbxBonus_powerball.Image = (Image)args.Results.WinImages["bonusball"];
            }

            if (pbxLottoProgress.InvokeRequired)
            {
                pbxPowerballProgress.Invoke(new UpdateImageCallback(this.UpdateImage), new object[] { global::ttpim.gamemodule.Properties.Resources.ok, pbxPowerballProgress.Name });
            }
            else
            {
                pbxPowerballProgress.Image = global::ttpim.gamemodule.Properties.Resources.ok;
            }


            if (lstBoxPowerballProgress.InvokeRequired)
            {
                lstBoxPowerballProgress.Invoke(new UpdateProgressListBoxCallback(this.UpdateProgressListBox), new object[] { lstBoxPowerballProgress.Name, "Powerball processing completed successfully." });
            }
            else
            {
                lstBoxPowerballProgress.Items.Add("Powerball processing completed successfully.");
            }
            DisplayPowerballResults();
            EnableWebReqests();
        }

        public void LottoPlusStartDownloadHandler(object sender, DownloadEventArgs args)
        {
            if (InvokeRequired)
            {
                Invoke(new LottoPlusStartDownloadNotification(LottoPlusStartDownloadHandler), new object[] { sender, args });
            }
            else
            {
                if (pnlProgressReport.Visible == false)
                    pnlProgressReport.Visible = true;
                lstBoxLottoPlusProgress.Items.Add("Starting LottoPlus webpage download...");
                pbxLottoPlusProgress.Visible = true;
                pbxLottoPlusProgress.Image = global::ttpim.gamemodule.Properties.Resources.animated_loading;
            }
        }

        public void LottoPlusFinishDownloadHandler(object sender, DownloadEventArgs args)
        {
            LottoPlusStartDownloadNotification del = new LottoPlusStartDownloadNotification(ProcessLottoPlusFinishDownload);
            IAsyncResult iasync = del.BeginInvoke(sender, args, null, null);
        }

        public void PowerballStartDownloadHandler(object sender, DownloadEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new PowerballStartDownloadNotification(PowerballStartDownloadHandler), new object[] { sender, args });
            }
            else
            {
                if (pnlProgressReport.Visible == false)
                    pnlProgressReport.Visible = true;
                lstBoxPowerballProgress.Items.Add("Starting Powerball webpage download...");
                pbxPowerballProgress.Visible = true;
                pbxPowerballProgress.Image = global::ttpim.gamemodule.Properties.Resources.animated_loading;
            }
        }

        public void PowerballFinishDownloadHandler(object sender, DownloadEventArgs args)
        {
            PowerballFinishDownloadNotification del = new PowerballFinishDownloadNotification(ProcessPowerballFinishDownload);
            IAsyncResult iasync = del.BeginInvoke(sender, args, null, null);
        }

        public void LottoCardSavedLottoWinningsHandler(object sender, LottoCardSavedWinningsEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new LottoCardSavedWinningsNotification(LottoCardSavedLottoWinningsHandler), new object[] { sender, args });
            }
            else
            {
                if (args.Game == "Lotto")
                {
                    if (pnlProgressReport.Visible == false)
                        pnlProgressReport.Visible = true;

                    lstBoxLottoProgress.Items.Add(args.Message);
                    pbxLottoDb.Visible = true;
                    if (args.Saved)
                        pbxLottoDb.Image = global::ttpim.gamemodule.Properties.Resources.database_ok;
                    else
                        pbxLottoDb.Image = global::ttpim.gamemodule.Properties.Resources.error_database_24;
                }
            }
        }

        public void LottoCardSavedLottoPlusWinningsHandler(object sender, LottoCardSavedWinningsEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new LottoCardSavedWinningsNotification(LottoCardSavedLottoPlusWinningsHandler), new object[] { sender, args });
            }
            else
            {
                if (args.Game == "LottoPlus")
                {
                    if (pnlProgressReport.Visible == false)
                        pnlProgressReport.Visible = true;

                    lstBoxLottoPlusProgress.Items.Add(args.Message);
                    pbxLottoPlusDb.Visible = true;
                    if (args.Saved)
                        pbxLottoPlusDb.Image = global::ttpim.gamemodule.Properties.Resources.database_ok;
                    else
                        pbxLottoPlusDb.Image = global::ttpim.gamemodule.Properties.Resources.error_database_24;
                }
            }
        }

        public void LottoCardSavedPowerballWinningsHandler(object sender, LottoCardSavedWinningsEventArgs args)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new LottoCardSavedWinningsNotification(LottoCardSavedPowerballWinningsHandler), new object[] { sender, args });
            }
            else
            {
                if (args.Game == "Powerball")
                {
                    if (pnlProgressReport.Visible == false)
                        pnlProgressReport.Visible = true;

                    lstBoxPowerballProgress.Items.Add(args.Message);
                    pbxPowerballDb.Visible = true;
                    if (args.Saved)
                        pbxPowerballDb.Image = global::ttpim.gamemodule.Properties.Resources.database_ok;
                    else
                        pbxPowerballDb.Image = global::ttpim.gamemodule.Properties.Resources.error_database_24;
                }
            }
        }

        private void buttonSpecHeaderGroup1_Click(object sender, EventArgs e)
        {
            if (TopHeaderGrp.Collapsed)
                TopHeaderGrp.Collapsed = false;
            else
                TopHeaderGrp.Collapsed = true;
        }

        private void btnWork_Click(object sender, EventArgs e)
        {
            Connect();
        }


        private void Connect()
        {
            if (InternetController.GetConnectionStatus())
            {
                WorkOnline();
            }
            else
            {
                WorkOffline();
            }
        }

        #region WebBrowserDocumentCompleted Events

        private void wbLotto_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            LottoCardService.ProcessLottoWebpage(sender, e, this.wbLotto);
        }

        private void wbLottoPlus_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            LottoCardService.ProcessLottoPlusWebpage(sender, e, this.wbLottoPlus);
        }

        private void wbPowerball_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            LottoCardService.ProcessPowerballWebpage(sender, e, this.wbPowerball);
        }

        #endregion
        private void DisplayLottoResults()
        {
            if (picBoxLottoMessage.InvokeRequired)
            {
                picBoxLottoMessage.Invoke(new UpdateImageCallback(UpdateImage), new object[] { LottoCardService.GetLottoMessageImage(), picBoxLottoMessage.Name });
            }
            else
            {
                picBoxLottoMessage.Image = LottoCardService.GetLottoMessageImage();
            }

            if (picBoxLottoWin.InvokeRequired)
            {
                picBoxLottoWin.Invoke(new UpdateImageCallback(UpdateImage), new object[] { LottoCardService.GetLottoWinImage(), picBoxLottoWin.Name });
            }
            else
            {
                picBoxLottoWin.Image = LottoCardService.GetLottoWinImage();
                picBoxLottoWin.SizeMode = PictureBoxSizeMode.AutoSize;
                picBoxLottoWin.SizeMode = PictureBoxSizeMode.CenterImage;
            }

            if (grdViewLotto.InvokeRequired)
            {
                grdViewLotto.Invoke(new UpdateGridViewCallback(UpdateGridView), new object[] { grdViewLotto.Name });
            }
            else
            {
                SetLottoGridViewPresentationTemplate();
                grdViewLotto.Columns.Clear();
                grdViewLotto.Rows.Clear();
                grdViewLotto.DataSource = LottoCardService.GetLottoTableView();
                grdViewLotto.ReadOnly = true;
            }

            if (pnlLotto.InvokeRequired)
            {
                pnlLotto.Invoke(new UpdateBasePanelCallback(UpdateBasePanel), new object[] { pnlLotto.Name, true });
            }
            else
            {
                pnlLotto.Visible = true;
                pnlLotto.Refresh();
            }
        }

        private void DisplayLottoPlusResults()
        {
            if (picBoxLottoPlusMessage.InvokeRequired)
            {
                picBoxLottoPlusMessage.Invoke(new UpdateImageCallback(UpdateImage), new object[] { LottoCardService.GetLottoPlusMessageImage(), picBoxLottoPlusMessage.Name });
            }
            else
            {
                picBoxLottoPlusMessage.Image = LottoCardService.GetLottoMessageImage();
            }

            if (picBoxLottoPlusWin.InvokeRequired)
            {
                picBoxLottoPlusWin.Invoke(new UpdateImageCallback(UpdateImage), new object[] { LottoCardService.GetLottoPlusWinImage(), picBoxLottoPlusWin.Name });
            }
            else
            {
                picBoxLottoPlusWin.Image = LottoCardService.GetLottoPlusWinImage();
                picBoxLottoPlusWin.SizeMode = PictureBoxSizeMode.AutoSize;
                picBoxLottoPlusWin.SizeMode = PictureBoxSizeMode.CenterImage;
            }

            if (grdViewLottoPlus.InvokeRequired)
            {
                grdViewLottoPlus.Invoke(new UpdateGridViewCallback(UpdateGridView), new object[] { grdViewLottoPlus.Name });
            }
            else
            {
                ApplyLottoPlusGridViewFormatTemplate(LottoCardService.GetLottoPlusTableView());
                grdViewLottoPlus.Columns.Clear();
                grdViewLottoPlus.Rows.Clear();
                grdViewLottoPlus.DataSource = LottoCardService.GetLottoPlusTableView();
            }
            if (pnlLottoPlus.InvokeRequired)
            {
                pnlLottoPlus.Invoke(new UpdateBasePanelCallback(UpdateBasePanel), new object[] { pnlLottoPlus.Name, true });
            }
            else
            {
                pnlLottoPlus.Visible = true;
            }
        }

        private void DisplayPowerballResults()
        {
            if (picBoxPowerballMessage.InvokeRequired)
            {
                picBoxPowerballMessage.Invoke(new UpdateImageCallback(UpdateImage), new object[] { LottoCardService.GetPowerballMessageImage(), picBoxPowerballMessage.Name });
            }
            else
            {
                picBoxPowerballMessage.Image = LottoCardService.GetPowerballMessageImage();
            }

            if (picBoxPowerballWin.InvokeRequired)
            {
                picBoxPowerballWin.Invoke(new UpdateImageCallback(UpdateImage), new object[] { LottoCardService.GetPowerballWinImage(), picBoxPowerballWin.Name });
            }
            else
            {
                picBoxPowerballWin.Image = LottoCardService.GetPowerballWinImage();
                picBoxPowerballWin.SizeMode = PictureBoxSizeMode.AutoSize;
                picBoxPowerballWin.SizeMode = PictureBoxSizeMode.CenterImage;
            }

            if (grdViewPowerball.InvokeRequired)
            {
                grdViewPowerball.Invoke(new UpdateGridViewCallback(UpdateGridView), new object[] { grdViewPowerball.Name });
            }
            else
            {
                ApplyPowerballGridViewFormatTemplate(LottoCardService.GetPowerballTableView());
                grdViewPowerball.Columns.Clear();
                grdViewPowerball.Rows.Clear();
                grdViewPowerball.DataSource = LottoCardService.GetPowerballTableView();
            }

            if (pnlPowerball.InvokeRequired)
            {
                pnlPowerball.Invoke(new UpdateBasePanelCallback(UpdateBasePanel), new object[] { pnlPowerball.Name, true });
            }
            else
            {
                pnlPowerball.Visible = true;
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            //DisplayLottoPlusResults();
            // pnlProgressReport.Visible = true;
            //InitialiseDisplay(this.tabLottoPlus);
            //Image img = LottoCardService.GenerateGraphic("20 August 2012", "Latest results");
            Image img = LottoCardService.GetLottoMessageImage();
            picBoxLottoMessage.Image = img;
        }


        private void UpdateLottoPanel()
        {
            pnlLotto.Update();
        }

        private void SetLottoGridViewPresentationTemplate()
        {
            LottoCardService.LottoGridViewStyles = new DataGridViewStyles();
            LottoCardService.LottoGridViewStyles.DefaultRowStyle = new System.Windows.Forms.DataGridViewCellStyle { BackColor = Color.White, ForeColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleCenter };
            this.grdViewLotto.RowsDefaultCellStyle = LottoCardService.LottoGridViewStyles.DefaultRowStyle;

            FormGridViewStyle subscriptionNrStyle = new FormGridViewStyle();
            subscriptionNrStyle.BackColor = Color.White;
            subscriptionNrStyle.ForeColor = Color.Purple;
            subscriptionNrStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            subscriptionNrStyle.Font = new Font("Arial", 8, FontStyle.Regular);
            LottoCardService.LottoGridViewStyles.ViewStyles.Add("SubscriptionNumberStyle", subscriptionNrStyle);

            FormGridViewStyle subscriptionRowStyle = new FormGridViewStyle();
            LottoCardService.LottoGridViewStyles.ViewStyles.Add("SubscriptionRowStyle", subscriptionRowStyle);
            FormGridViewStyle gameNameStyle = new FormGridViewStyle();
            LottoCardService.LottoGridViewStyles.ViewStyles.Add("GameNameStyle", gameNameStyle);
            FormGridViewStyle boardNumberStyle = new FormGridViewStyle();
            LottoCardService.LottoGridViewStyles.ViewStyles.Add("BoardNumberStyle", boardNumberStyle);

            FormGridViewStyle ballNumberStyle = new FormGridViewStyle();
            LottoCardService.LottoGridViewStyles.ViewStyles.Add("BallNumberStyle", ballNumberStyle);
            FormGridViewStyle bonusNumberStyle = new FormGridViewStyle();
            LottoCardService.LottoGridViewStyles.ViewStyles.Add("BonusNumberStyle", bonusNumberStyle);

            FormGridViewStyle bonusMatchStyle = new FormGridViewStyle();
            bonusMatchStyle.BackColor = Color.White;
            bonusMatchStyle.ForeColor = Color.LimeGreen;
            bonusMatchStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            bonusMatchStyle.Font = new Font("Arial", 8, FontStyle.Bold);
            LottoCardService.LottoGridViewStyles.ViewStyles.Add("BonusNumberMatchStyle", bonusMatchStyle);

            FormGridViewStyle winningStyle = new FormGridViewStyle();
            winningStyle.BackColor = Color.White;
            winningStyle.ForeColor = Color.Purple;
            winningStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            winningStyle.Font = new Font("Arial", 8, FontStyle.Regular);
            LottoCardService.LottoGridViewStyles.ViewStyles.Add("WinningStyle", winningStyle);

            FormGridViewStyle divsionStyle = new FormGridViewStyle();
            LottoCardService.LottoGridViewStyles.ViewStyles.Add("DivisionStyle", divsionStyle);
            FormGridViewStyle earningStyle = new FormGridViewStyle();
            LottoCardService.LottoGridViewStyles.ViewStyles.Add("EarningStyle", earningStyle);

        }

        private void ApplyLottoPlusGridViewFormatTemplate(DataTable Table)
        {
            //grdViewLottoPlusPresenter = new DataGridViewPresenter(Table);
        }

        private void ApplyPowerballGridViewFormatTemplate(DataTable Table)
        {
            PowerballGridViewStyles = new DataGridViewStyles();
        }

        private void wbLotto_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            LottoCardService.RaiseLottoStartDownloadEvent();
        }

        private void wbLottoPlus_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            LottoCardService.RaiseLottoPlusStartDownloadEvent();
        }

        private void wbPowerball_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            LottoCardService.RaisePowerballStartDownloadEvent();
        }

        private void btnCancelRequest_Click(object sender, EventArgs e)
        {

        }

        private void menuMar_ItemClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
        {
            switch (e.Item.Key)
            {
                case "LatestLotto":
                    tabControlResults.SelectedTab = tabControlResults.Tabs[1];
                    break;
                case "LatestLottoPlus":
                    tabControlResults.SelectedTab = tabControlResults.Tabs[2];
                    break;
                case "LatestPowerball":
                    tabControlResults.SelectedTab = tabControlResults.Tabs[3];
                    break;
                case "Subscription":
                    tabControlResults.SelectedTab = tabControlResults.Tabs[7];
                    break;
                default:
                    break;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.BringToFront();
        }

        public delegate void FormatGridCellsCallback(object sender, DataGridViewCellFormattingEventArgs e);
        public void FormatGridCells(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }


        private ComponentFactory.Krypton.Toolkit.DataGridViewStyle GetSubscriptionStyle()
        {
            ComponentFactory.Krypton.Toolkit.DataGridViewStyle subscriptionRowCellStyle = new ComponentFactory.Krypton.Toolkit.DataGridViewStyle
            {
            };
            return subscriptionRowCellStyle;
        }

        private DataGridViewCellStyle GetSubscriptionRowStyle()
        {
            DataGridViewCellStyle subscriptionRowCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.Silver,
                ForeColor = Color.Black,
                Font = new Font("Arial", 8, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };
            return subscriptionRowCellStyle;
        }


        private DataGridViewCellStyle GetBonusCellStyle()
        {
            System.Windows.Forms.DataGridViewCellStyle bonusRowCellStyle = new DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.LimeGreen,
                ForeColor = Color.White,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
            };
            return bonusRowCellStyle;
        }

        private DataGridViewCellStyle GetResultRowStyle()
        {
            System.Windows.Forms.DataGridViewCellStyle resultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.LemonChiffon,
                ForeColor = Color.DarkGray,
                Font = new Font("Arial", 8, FontStyle.Regular),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };
            return resultCellStyle;
        }

        private DataGridViewCellStyle GetHeaderRowStyle()
        {
            System.Windows.Forms.DataGridViewCellStyle resultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.LemonChiffon,
                ForeColor = Color.DarkGray,
                Font = new Font("Arial", 8, FontStyle.Regular),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };
            return resultCellStyle;
        }

        private void grdViewLotto_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
            {
                if ((e.ColumnIndex == 0) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    (sender as DataGridView).Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    (sender as DataGridView).Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Bold);
                    (sender as DataGridView).Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }

                if ((e.ColumnIndex == 3) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 4) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 5) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 6) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 7) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 8) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 1) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value == DBNull.Value) || (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) == String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Style.BackColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Style.Font = new Font("Arial", 8, FontStyle.Regular);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Style.ForeColor = Color.Purple;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 2) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value == DBNull.Value) || (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) == String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Style.BackColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Style.Font = new Font("Arial", 8, FontStyle.Regular);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Style.ForeColor = Color.DarkOrange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if (((e.ColumnIndex >= 3) && (e.ColumnIndex <= 8)) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value == DBNull.Value) || (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) == String.Empty)))
                {
                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.BackColor = Color.White;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.BackColor = Color.White;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.BackColor = Color.White;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.BackColor = Color.White;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.BackColor = Color.White;
                    }
                }

                if (((sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Value) != String.Empty)))
                {
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.BackColor = Color.White;
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.ForeColor = Color.Red;
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.BackColor = Color.White;
                }

                if (((sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Value) != String.Empty)))
                {
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.BackColor = Color.White;
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.ForeColor = Color.Blue;
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.BackColor = Color.White;
                }

                if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && ((sender as DataGridView).Columns[e.ColumnIndex].Name == "Win") && (e.FormattedValue == e.CellStyle.NullValue))
                {
                    (sender as DataGridView).Columns[e.ColumnIndex].DefaultCellStyle.NullValue = null;
                }

                if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && ((sender as DataGridView).Columns[e.ColumnIndex].Name == "Bonus Match") && (e.FormattedValue == e.CellStyle.NullValue))
                {
                    (sender as DataGridView).Columns[e.ColumnIndex].DefaultCellStyle.NullValue = null;
                }

                if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && ((e.RowIndex >= (sender as DataGridView).Rows.Count - 1)))
                {
                    (sender as DataGridView).Columns[e.ColumnIndex].DefaultCellStyle.NullValue = null;
                    DataGridViewPaintParts pp = e.PaintParts & ~DataGridViewPaintParts.ContentForeground;
                    e.Paint(e.ClipBounds, pp);
                    e.Handled = true;
                }
            }
        }

        private void grdViewLottoPlus_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
            {
                if ((e.ColumnIndex == 0) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    (sender as DataGridView).Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    (sender as DataGridView).Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Bold);
                    (sender as DataGridView).Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }

                if ((e.ColumnIndex == 3) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 4) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 5) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 6) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 7) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 8) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 1) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value == DBNull.Value) || (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) == String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Style.BackColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Style.Font = new Font("Arial", 8, FontStyle.Regular);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Style.ForeColor = Color.Purple;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 2) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value == DBNull.Value) || (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) == String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Style.BackColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Style.Font = new Font("Arial", 8, FontStyle.Regular);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Style.ForeColor = Color.DarkOrange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if (((e.ColumnIndex >= 3) && (e.ColumnIndex <= 8)) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value == DBNull.Value) || (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) == String.Empty)))
                {
                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.BackColor = Color.White;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.BackColor = Color.White;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.BackColor = Color.White;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.BackColor = Color.White;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.BackColor = Color.White;
                    }
                }

                if (((sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Value) != String.Empty)))
                {
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.BackColor = Color.White;
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.ForeColor = Color.Red;
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.BackColor = Color.White;
                }

                if (((sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Value) != String.Empty)))
                {
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.BackColor = Color.White;
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.ForeColor = Color.Blue;
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.BackColor = Color.White;
                }

                if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && ((sender as DataGridView).Columns[e.ColumnIndex].Name == "Win") && (e.FormattedValue == e.CellStyle.NullValue))
                {
                    (sender as DataGridView).Columns[e.ColumnIndex].DefaultCellStyle.NullValue = null;
                }

                if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && ((sender as DataGridView).Columns[e.ColumnIndex].Name == "Bonus Match") && (e.FormattedValue == e.CellStyle.NullValue))
                {
                    (sender as DataGridView).Columns[e.ColumnIndex].DefaultCellStyle.NullValue = null;
                }

                if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && ((e.RowIndex >= (sender as DataGridView).Rows.Count - 1)))
                {
                    (sender as DataGridView).Columns[e.ColumnIndex].DefaultCellStyle.NullValue = null;
                    DataGridViewPaintParts pp = e.PaintParts & ~DataGridViewPaintParts.ContentForeground;
                    e.Paint(e.ClipBounds, pp);
                    e.Handled = true;
                }
            }
        }

        private void grdViewPowerball_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if ((e.RowIndex > -1) && (e.ColumnIndex > -1))
            {

                if ((e.ColumnIndex == 0) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    (sender as DataGridView).Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    (sender as DataGridView).Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Bold);
                    (sender as DataGridView).Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }

                if ((e.ColumnIndex == 3) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 4) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 5) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 6) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 7) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 8) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value != DBNull.Value) && (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) != String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.BackColor = Color.Orange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.ForeColor = Color.Black;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 1) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value == DBNull.Value) || (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) == String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Style.BackColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Style.ForeColor = Color.Purple;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Game"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if ((e.ColumnIndex == 2) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value == DBNull.Value) || (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) == String.Empty)))
                {
                    if ((sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Value != DBNull.Value)
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Style.BackColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Style.ForeColor = Color.DarkOrange;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Board"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if (((e.ColumnIndex >= 3) && (e.ColumnIndex <= 8)) && (((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value == DBNull.Value) || (Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Subscription Nr"].Value) == String.Empty)))
                {
                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 1"].Style.BackColor = Color.White;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 2"].Style.BackColor = Color.White;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 3"].Style.BackColor = Color.White;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    else
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 4"].Style.BackColor = Color.White;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 5"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }

                    if (((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Value) != String.Empty)))
                    {
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.BackColor = Color.Red;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.ForeColor = Color.White;
                        (sender as DataGridView).Rows[e.RowIndex].Cells["Ball 6"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }

                if (((sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Value) != String.Empty)))
                {
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.BackColor = Color.White;
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.ForeColor = Color.Red;
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Style.BackColor = Color.White;
                }

                if (((sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Value != DBNull.Value) && ((Convert.ToString((sender as DataGridView).Rows[e.RowIndex].Cells["Division"].Value) != String.Empty)))
                {
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.BackColor = Color.White;
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.Font = new Font("Arial", 8, FontStyle.Bold);
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.ForeColor = Color.Blue;
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    (sender as DataGridView).Rows[e.RowIndex].Cells["Winnings"].Style.BackColor = Color.White;
                }

                if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && ((sender as DataGridView).Columns[e.ColumnIndex].Name == "Win") && (e.FormattedValue == e.CellStyle.NullValue))
                {
                    (sender as DataGridView).Columns[e.ColumnIndex].DefaultCellStyle.NullValue = null;
                }

                if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && ((sender as DataGridView).Columns[e.ColumnIndex].Name == "Powerball Match") && (e.FormattedValue == e.CellStyle.NullValue))
                {
                    (sender as DataGridView).Columns[e.ColumnIndex].DefaultCellStyle.NullValue = null;
                }

                if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && ((e.RowIndex >= (sender as DataGridView).Rows.Count - 1)))
                {
                    (sender as DataGridView).Columns[e.ColumnIndex].DefaultCellStyle.NullValue = null;
                    DataGridViewPaintParts pp = e.PaintParts & ~DataGridViewPaintParts.ContentForeground;
                    e.Paint(e.ClipBounds, pp);
                    e.Handled = true;
                }
            }
        }

        private void btnRetryCancel_Click(object sender, EventArgs e)
        {
            if (btnRetryCancel.Text == "Retry")
            {
                Connect();
            }
        }

        private void grdViewLotto_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridViewColumn subscription = (sender as DataGridView).Columns[0];
            subscription.Width = 70;
            DataGridViewColumn game = (sender as DataGridView).Columns[1];
            game.Width = 70;
            DataGridViewColumn board = (sender as DataGridView).Columns[2];
            board.Width = 55;
            DataGridViewColumn ball1 = (sender as DataGridView).Columns[3];
            ball1.Width = 40;
            DataGridViewColumn ball2 = (sender as DataGridView).Columns[4];
            ball2.Width = 40;
            DataGridViewColumn ball3 = (sender as DataGridView).Columns[5];
            ball3.Width = 40;
            DataGridViewColumn ball4 = (sender as DataGridView).Columns[6];
            ball4.Width = 40;
            DataGridViewColumn ball5 = (sender as DataGridView).Columns[7];
            ball5.Width = 40;
            DataGridViewColumn ball6 = (sender as DataGridView).Columns[8];
            ball6.Width = 40;
            DataGridViewColumn bonus = (sender as DataGridView).Columns[9];
            bonus.Width = 60;
            DataGridViewColumn win = (sender as DataGridView).Columns[10];
            win.Width = 80;
            DataGridViewColumn division = (sender as DataGridView).Columns[11];
            division.MinimumWidth = 120;
            division.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DataGridViewColumn winnings = (sender as DataGridView).Columns[12];
            winnings.Width = 50;

        }

        private void grdViewLottoPlus_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridViewColumn subscription = (sender as DataGridView).Columns[0];
            subscription.Width = 70;
            DataGridViewColumn game = (sender as DataGridView).Columns[1];
            game.Width = 70;
            DataGridViewColumn board = (sender as DataGridView).Columns[2];
            board.Width = 55;
            DataGridViewColumn ball1 = (sender as DataGridView).Columns[3];
            ball1.Width = 40;
            DataGridViewColumn ball2 = (sender as DataGridView).Columns[4];
            ball2.Width = 40;
            DataGridViewColumn ball3 = (sender as DataGridView).Columns[5];
            ball3.Width = 40;
            DataGridViewColumn ball4 = (sender as DataGridView).Columns[6];
            ball4.Width = 40;
            DataGridViewColumn ball5 = (sender as DataGridView).Columns[7];
            ball5.Width = 40;
            DataGridViewColumn ball6 = (sender as DataGridView).Columns[8];
            ball6.Width = 40;
            DataGridViewColumn bonus = (sender as DataGridView).Columns[9];
            bonus.Width = 60;
            DataGridViewColumn win = (sender as DataGridView).Columns[10];
            win.Width = 80;
            DataGridViewColumn division = (sender as DataGridView).Columns[11];
            division.MinimumWidth = 120;
            division.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DataGridViewColumn winnings = (sender as DataGridView).Columns[12];
            winnings.Width = 50;
        }

        private void grdViewPowerball_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridViewColumn subscription = (sender as DataGridView).Columns[0];
            subscription.Width = 85;
            DataGridViewColumn game = (sender as DataGridView).Columns[1];
            game.Width = 70;
            DataGridViewColumn board = (sender as DataGridView).Columns[2];
            board.Width = 60;
            DataGridViewColumn ball1 = (sender as DataGridView).Columns[3];
            ball1.Width = 40;
            DataGridViewColumn ball2 = (sender as DataGridView).Columns[4];
            ball2.Width = 40;
            DataGridViewColumn ball3 = (sender as DataGridView).Columns[5];
            ball3.Width = 40;
            DataGridViewColumn ball4 = (sender as DataGridView).Columns[6];
            ball4.Width = 40;
            DataGridViewColumn ball5 = (sender as DataGridView).Columns[7];
            ball5.Width = 40;
            DataGridViewColumn bonus = (sender as DataGridView).Columns[8];
            bonus.Width = 60;
            DataGridViewColumn win = (sender as DataGridView).Columns[9];
            win.MinimumWidth = 80;
            DataGridViewColumn division = (sender as DataGridView).Columns[10];
            division.MinimumWidth = 120;
            division.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DataGridViewColumn winnings = (sender as DataGridView).Columns[11];
            winnings.Width = 50;

        }
    }
}

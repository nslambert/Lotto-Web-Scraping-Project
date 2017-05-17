
namespace ttpim.gamemodule.games.models.lotto
{
    public delegate void LottoWinNotification(object sender, LottoWinEventArgs e);
    public delegate void LottoPlusWinNotification(object sender, LottoWinEventArgs e);
    public delegate void LottoStartDownloadNotification(object sender, DownloadEventArgs e);
    public delegate void LottoFinishDownloadNotification(object sender, DownloadEventArgs e);
    public delegate void LottoPlusStartDownloadNotification(object sender, DownloadEventArgs e);
    public delegate void LottoPlusFinishDownloadNotification(object sender, DownloadEventArgs e);
    public delegate void LottoCardProcessedNotification(object sender, LottoCardProcessedEventArgs e);
    public delegate void LottoCardInProcessingNotification(object sender, LottoCardInProcessingEventArgs e);

    public delegate void LottoPlusCardInProcessingNotification(object sender, LottoCardInProcessingEventArgs e);
    public delegate void LottoParseExceptionNotification(object sender, LottoParseExceptionEventArgs e);
    public delegate void LottoPlusParseExceptionNotification(object sender, LottoPlusParseExceptionEventArgs e);
    public delegate void LottoNavigationNotification(object sender, WebNavigationEventArgs e);
    public delegate void LottoPlusNavigationNotification(object sender, WebNavigationEventArgs e);
    public delegate void PowerballStartDownloadNotification(object sender, DownloadEventArgs e);
    public delegate void PowerballFinishDownloadNotification(object sender, DownloadEventArgs e);
    public delegate void LottoCardSavedWinningsNotification(object sender, LottoCardSavedWinningsEventArgs e);

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.database;

namespace ttpim.gamemodule.games.models.lotto
{
    public interface ILottoSetupDataProvider
    {
        Setting Get(string setting);
        string GetURL(string GameName);
        Uri GetBaseURI(string URISetting);
        string DownloadBaseDir(string BaseDirSetting);
        string GetSetting(string SettingName);
        int UpdateSetting(string settingName, string settingValue);
        int UpdateSetup(string setupName, string setupValue);
        List<Setup> List();
        Boolean IsDatasourceAvailable();
    }
}

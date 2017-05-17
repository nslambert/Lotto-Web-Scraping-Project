using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.games.models.lotto;
using ttpim.gamemodule.database;

namespace ttpim.gamemodule.games.controllers.lotto
{
    public class LottoSetupServiceManager
    {
        private ILottoSetupDataProvider _dataprovider;
        private Boolean _isDataProviderAvailable;
        private string _applicationDataRoot;
        private string _applicationBriefcasePath;

        public LottoSetupServiceManager() : this(new LottoSetupDataProvider()) { }

        public LottoSetupServiceManager(SetupData setup)
        {
            _dataprovider = new LottoSetupDataProvider(setup.DatasourcePath);
            _applicationDataRoot = setup.ApplicationDataRoot;
            _applicationBriefcasePath = setup.ApplicationBriefcasePath;
        }

        public LottoSetupServiceManager(ILottoSetupDataProvider repository)
        {
            _dataprovider = repository;
            _isDataProviderAvailable = _dataprovider.IsDatasourceAvailable();
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

        public ILottoSetupDataProvider DataProvider
        {
            get 
            {
                return _dataprovider;
            }
            set 
            {
                _dataprovider = value;
            }
        }

        public List<Setup> GetSetups()
        {
            var wins = DataProvider.List();
            return wins.ToList();
        }

        public string GetURL(string Name)
        {
            string url = DataProvider.GetURL(Name);
            return url;
        }

        public Uri GetBaseURI(string URISetting)
        {
            string setting = GetSetting(URISetting);
            Uri uri = new Uri(setting);
            return uri;
        }

        public string GetSetting(string SettingName)
        {
            string setting = String.Empty;
            if ((SettingName == "ApplicationDataRoot") && (!String.IsNullOrEmpty(_applicationDataRoot)))
            {
                setting = _applicationDataRoot;
            }
            else if ((SettingName == "ApplicationBriefcasePath") && (!String.IsNullOrEmpty(_applicationBriefcasePath)))
            {
                setting = _applicationBriefcasePath;
            }
            else
            {
                setting = DataProvider.GetSetting(SettingName);
            }
            return setting;
        }

        public int UpdateSetting(string settingName, string settingValue)
        {
            return DataProvider.UpdateSetting(settingName, settingValue);
        }

        public int UpdateSetup(string setupName, string setupValue)
        {
            return DataProvider.UpdateSetup(setupName, setupValue);
        }

    }
}

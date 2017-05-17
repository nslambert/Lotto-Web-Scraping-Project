using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.database;
using System.Data.SQLite;
using System.Data.EntityClient;
using System.Configuration;
using System.IO;

namespace ttpim.gamemodule.games.models.lotto
{
    public class LottoSetupDataProvider : ILottoSetupDataProvider
    {
        private LottoDBEntities _entities;

        #region Constructors
        private SQLiteConnection _connection;
        private Boolean _isAvailable;

        private Boolean IsAvailable
       {
            get
            {
                return _isAvailable;
            }
            set
            {
                _isAvailable = value;
            }
       }

        public LottoDBEntities DataContext
        {
            get
            {
                return _entities;
            }
            set
            {
                _entities = value;
            }
        }

        public SQLiteConnection DataConnection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        public LottoSetupDataProvider (string dataSourcePath)
        {
            ConnectToDataSource(dataSourcePath);
        }

        public LottoSetupDataProvider()
        {
            string dataSourcePath = ConfigurationManager.AppSettings["DefaultDBPath"].ToString();
            if (File.Exists(dataSourcePath))
            {
                IsAvailable = true;
                ConnectToDataSource(dataSourcePath);
            }
            else
            {
                IsAvailable = false;
            }
        }

        public void ConnectToDataSource(string dataSourcePath)
        {
            var entityConnectionStringBuilder = new EntityConnectionStringBuilder();
            entityConnectionStringBuilder.Provider = @"System.Data.SQLite";
            entityConnectionStringBuilder.Metadata = @"res://*/database.LottoDataModel.csdl|res://*/database.LottoDataModel.ssdl|res://*/database.LottoDataModel.msl";

            entityConnectionStringBuilder.ProviderConnectionString = new SQLiteConnectionStringBuilder()
            {
                DataSource = dataSourcePath
            }.ConnectionString;
            this.DataContext = new LottoDBEntities(entityConnectionStringBuilder.ToString());
            DataConnection = (this.DataContext.Connection as EntityConnection).StoreConnection as SQLiteConnection;
        }

        public Boolean IsDatasourceAvailable()
        {
            return IsAvailable;
        }

        #endregion

        public Setting Get(string setting)
        {
            var result = (from settingitem in this.DataContext.Settings
                          where settingitem.name == setting
                          select settingitem).FirstOrDefault();
            return result;
        }

        public Setup GetSetup(string setup)
        {
            var result = (from setupitem in this.DataContext.Setups
                          where setupitem.game == setup
                          select setupitem).FirstOrDefault();
            return result;
        }

        public string GetURL(string GameName)
        {
            var result = (from page in this.DataContext.Setups
                          where page.game == GameName
                          select page.webpage).FirstOrDefault();
            return Convert.ToString(result);
        }

        public Uri GetBaseURI(string URISetting)
        {
            var result = (from uri_setting in this.DataContext.Settings
                          where uri_setting.name == URISetting
                          select uri_setting.value).FirstOrDefault();
            return new Uri(Convert.ToString(result));
        }

        public List<Setup> List()
        {
            var result = this.DataContext.Setups;
            return result.ToList();
        }

        public string DownloadBaseDir(string BaseDirSetting)
        {
            var result = (from dir_setting in this.DataContext.Settings
                          where dir_setting.name == BaseDirSetting
                          select dir_setting.value).FirstOrDefault();
            return Convert.ToString(result);
        }

        public string GetSetting(string SettingName)
        {
            var result = (from setname in this.DataContext.Settings
                          where setname.name == SettingName
                          select setname).FirstOrDefault();
            return Convert.ToString(result.value);
        }

        public int UpdateSetting(string settingName, string settingValue)
        {
            int result = 0;
            Setting originalSetting = Get(settingName);
            Setting settingToEdit = originalSetting;
            settingToEdit.value = settingValue;
            settingToEdit.modifiedby = "sysadmin";
            settingToEdit.modifiedon = DateTime.Now;
            this.DataContext.ApplyCurrentValues(originalSetting.EntityKey.EntitySetName, settingToEdit);
            result = this.DataContext.SaveChanges();
            return result;
        }

        public int UpdateSetup(string setupName, string setupValue)
        {
            int result = 0;
            Setup originalSetup = GetSetup(setupName);
            Setup setupToEdit = originalSetup;
            setupToEdit.webpage = setupValue;
            setupToEdit.modifiedby = "sysadmin";
            setupToEdit.modifiedon = DateTime.Now;
            this.DataContext.ApplyCurrentValues(originalSetup.EntityKey.EntitySetName, setupToEdit);
            result = this.DataContext.SaveChanges();
            return result;
        }
    }
}

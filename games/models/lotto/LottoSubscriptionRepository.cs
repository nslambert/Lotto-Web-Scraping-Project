using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.database;
using System.Data.SQLite;
using System.Data.EntityClient;
using System.Configuration;

namespace ttpim.gamemodule.database
{
    public class LottoSubscriptionRepository : ILottoSubscriptionRepository
    {
       private LottoDBEntities _entities;

       #region Constructors
       private SQLiteConnection _connection;

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

        public LottoSubscriptionRepository(string dataSourcePath)
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

        public LottoSubscriptionRepository()
        {
            var entityConnectionStringBuilder = new EntityConnectionStringBuilder();
            entityConnectionStringBuilder.Provider = @"System.Data.SQLite";
            entityConnectionStringBuilder.Metadata = @"res://*/database.LottoDataModel.csdl|res://*/database.LottoDataModel.ssdl|res://*/database.LottoDataModel.msl";
        
            string dataSourcePath = ConfigurationManager.AppSettings["DefaultDBPath"].ToString();
            entityConnectionStringBuilder.ProviderConnectionString = new SQLiteConnectionStringBuilder()
            {
                DataSource = dataSourcePath
            }.ConnectionString;
            this.DataContext = new LottoDBEntities(entityConnectionStringBuilder.ToString());
            DataConnection = (this.DataContext.Connection as EntityConnection).StoreConnection as SQLiteConnection;
        }
        #endregion

        #region ILottoSubscriptionRepository Members

        public IEnumerable<Subscription> List()
        {
            return this.DataContext.Subscriptions.ToList();
        }

        public IEnumerable<Subscription> List(string Game)
        {
            var result = (from subscription in this.DataContext.Subscriptions
                          where subscription.game == Game
                          select subscription);
            return result.ToList();
        }

        public Subscription Get(long id)
        {
            var result = (from subscription in this.DataContext.Subscriptions
                         where subscription.id == id
                         select subscription).FirstOrDefault();
           return result;
        }

        public string GetSubscriptionNumber(long id)
        {
            var result = (from subscription in this.DataContext.Subscriptions
                          where subscription.id == id
                          select subscription.scriptnr).FirstOrDefault();
            return result.ToString();
        }

        public string GetSubscriptionType(long id)
        {
            var result = (from subscription in this.DataContext.Subscriptions
                          where subscription.id == id
                          select subscription.game).FirstOrDefault();
            return result.ToString();
        }

        public void Create(Subscription LottoSubscriptionToCreate)
        {
            this.DataContext.AddToSubscriptions(LottoSubscriptionToCreate);
            this.DataContext.SaveChanges();
        }

        public void Edit(Subscription LottoSubscriptionToEdit)
        {
            var originalLottoSubscription = Get(LottoSubscriptionToEdit.id);
            this.DataContext.ApplyCurrentValues(originalLottoSubscription.EntityKey.EntitySetName, LottoSubscriptionToEdit);
            this.DataContext.SaveChanges();
        }

        public void Delete(Subscription LottoSubscriptionToDelete)
        {
            var originalLottoSubscription = Get(LottoSubscriptionToDelete.id);
            this.DataContext.DeleteObject(originalLottoSubscription);
            this.DataContext.SaveChanges();
        }
        #endregion

    }
}

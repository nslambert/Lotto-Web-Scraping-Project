using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.games.models;
using System.Data.EntityClient;
using System.Configuration;
using System.Data.SQLite;

namespace ttpim.gamemodule.database
{
    public class LottoWinsRepository : ILottoWinsRepository
    {
        private LottoDBEntities _entities;
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

        #region ILottoWinningRepository Members

        public LottoWinsRepository(string dataSourcePath)
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

        public LottoWinsRepository()
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

        public IEnumerable<Win> List()
        {
            return this.DataContext.Wins.ToList();
        }

        public Win Get(long id)
        {
            var result = (from win in this.DataContext.Wins
                          where win.id == id
                          select win).FirstOrDefault();
            return result;
        }

        public int Create(Win LottoWinningToCreate)
        {
            int result = 0;

            this.DataContext.AddToWins(LottoWinningToCreate);
            result = this.DataContext.SaveChanges();
            return result;
        }

        public int Save(List<Win> LottoWinningsToSave)
        {
            int result = 0;
            foreach(Win winning in LottoWinningsToSave)
            {
                result = result + Create(winning);
            }
            return result;
        }

        public int SaveWebpage(Document DocumentToSave)
        {
            int result = 0;
            this.DataContext.AddToDocuments(DocumentToSave);
            result = this.DataContext.SaveChanges();
            return result;
        }

        public int Edit(Win LottoWinningToEdit)
        {
            int result = 0;
            var originalLottoWinning = Get(LottoWinningToEdit.id);
            this.DataContext.ApplyCurrentValues(originalLottoWinning.EntityKey.EntitySetName, LottoWinningToEdit);
            this.DataContext.SaveChanges();
            return result;
        }

        public int Delete(Win LottoWinningToDelete)
        {
            int result = 0;
            var originalLottoWinning = Get(LottoWinningToDelete.id);
            this.DataContext.DeleteObject(originalLottoWinning);
            this.DataContext.SaveChanges();
            return result;
        }

        public IEnumerable<Win> ListByGame(string Game)
        {
            var result = (from win in this.DataContext.Wins
                          where win.game == Game
                          select win);
            return result.ToList();
        }

        public IEnumerable<Win> ListByPeriod(DateTime StartDate, DateTime Enddate)
        {
            var result = (from win in this.DataContext.Wins
                          where win.gamedate >= StartDate && win.gamedate <= Enddate
                          select win);
            return result.ToList();
        }

        public IEnumerable<Win> ListBySubscriptionBoard(long SubscriptionId, long BoardId)
        {
            var result = (from win in this.DataContext.Wins
                          where win.scriptid == SubscriptionId && win.boardnr == BoardId
                          select win);
            return result.ToList();
        }

        public DateTime GetLastSavedGameDate(string Game)
        {
            var result = (from win in this.DataContext.Wins
                          where win.game == Game
                          select win.gamedate).Max().GetValueOrDefault();
            return (DateTime)result;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data.SQLite;
using System.Configuration;

namespace ttpim.gamemodule.database
{
    public class LottoBoardRepository : ILottoBoardRepository
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
        public LottoBoardRepository (string dataSourcePath)
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

        public LottoBoardRepository()
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

        #region ILottoBoardRepository Members


        public IEnumerable<Board> List()
        {
            return this.DataContext.Boards.ToList();
        }

        public IEnumerable<Board> ListBySubscription(long SubscriptionID)
        {
            var result = (from board in this.DataContext.Boards
                          where board.scriptid == (long)SubscriptionID
                          select board);
            return result.ToList();
        }

        public Board Get(long id)
        {
            var result = (from board in this.DataContext.Boards
                          where board.id == id
                          select board).FirstOrDefault();
            return result;
        }

        public void Create(Board BoardRegisterToCreate)
        {
            this.DataContext.AddToBoards(BoardRegisterToCreate);
            this.DataContext.SaveChanges();
        }

        public void Edit(Board BoardRegisterToEdit)
        {
            var originalBoardRegister = Get(BoardRegisterToEdit.id);
            this.DataContext.ApplyCurrentValues(originalBoardRegister.EntityKey.EntitySetName, BoardRegisterToEdit);
            this.DataContext.SaveChanges();
        }

        public void Delete(Board BoardRegisterToDelete)
        {
            var originalBoardRegister = Get(BoardRegisterToDelete.id);
            this.DataContext.DeleteObject(originalBoardRegister);
            this.DataContext.SaveChanges();
        }
        #endregion
    }
}

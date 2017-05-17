using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.games.models;

namespace ttpim.gamemodule.database
{
    public interface ILottoWinsRepository
    {
        IEnumerable<Win> List();
        IEnumerable<Win> ListByPeriod(DateTime StartDate, DateTime Enddate);
        IEnumerable<Win> ListBySubscriptionBoard(long SubscriptionId, long BoardId);
        IEnumerable<Win> ListByGame(string Game);

        Win Get(long id);
        int Create(Win LottoWinningToCreate);
        int Save(List<Win> LottoWinningsToSave);
        int Edit(Win LottoWinningToEdit);
        int Delete(Win LottoWinningToDelete);
        DateTime GetLastSavedGameDate(string Game);
    }
}

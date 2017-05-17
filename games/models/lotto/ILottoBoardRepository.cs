using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.games.models;

namespace ttpim.gamemodule.database
{
    public interface ILottoBoardRepository
    {
        IEnumerable<Board> List();
        IEnumerable<Board> ListBySubscription(long SubscriptionID);
        Board Get(long id);
        void Create(Board LottoBoardToCreate);
        void Edit(Board LottoBoardToEdit);
        void Delete(Board LottoBoardToDelete);
    }
}

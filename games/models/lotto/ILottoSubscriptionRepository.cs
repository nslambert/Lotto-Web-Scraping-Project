using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ttpim.gamemodule.games.models;

namespace ttpim.gamemodule.database
{
    public interface ILottoSubscriptionRepository
    {
        IEnumerable<Subscription> List();
        IEnumerable<Subscription> List(string Game);
        Subscription Get(long id);
        void Create(Subscription LottoSubscriptionToCreate);
        void Edit(Subscription LottoSubscriptionToEdit);
        void Delete(Subscription LottoSubscriptionToDelete);
        string GetSubscriptionNumber(long id);
        string GetSubscriptionType(long id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace ttpim.gamemodule.games.controllers.lotto
{
    public class LottoBackgroundWorker
    {
        private BackgroundWorker bgWorker = new BackgroundWorker();
        public LottoBackgroundWorker()
        {
            bgWorker.WorkerReportsProgress      = true; 
            bgWorker.WorkerSupportsCancellation = true;
        }

        public BackgroundWorker Worker
        {
            get
            {
                return bgWorker;
            }
            set
            {
                bgWorker = value;
            }
        }
    }
}

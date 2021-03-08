using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeStats.Services.Interfaces
{
    interface IContextFactory<T>
    {
        T CreateDbContext();
        void SwitchDatabase(string dbName);
    }
}

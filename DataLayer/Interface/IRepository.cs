using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IRepository
    {
        Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, object parameters = null);

        Task<IEnumerable<T>> QueryTextAsync<T>(string sql, object parameters = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string storedProcedure, object parameters = null);
        Task<int> ExecuteAsync(string storedProcedure, object parameters = null);
    }
}

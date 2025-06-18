using Dapper;
using DataLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.RN
{
    public class Repository : IRepository
    {

        private readonly IDbConnection _db;

        public Repository(IDbConnection db)
        {
            _db = db;
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, object parameters = null)
        {
            return await _db.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> QueryTextAsync<T>(string sql, object parameters = null)
        {
            return await _db.QueryAsync<T>(sql, parameters, commandType: CommandType.Text);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string storedProcedure, object parameters = null)
        {
            return await _db.QueryFirstOrDefaultAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ExecuteAsync(string storedProcedure, object parameters = null)
        {
            return await _db.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

    }
}

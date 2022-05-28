using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using FireWolf.Data;
namespace DtmCommon
{
    public class DbUtils
    {
        private readonly DtmOptions _options;
        private readonly DbSpecialDelegate _specialDelegate;

        public DbUtils(IOptions<DtmOptions> optionsAccs, DbSpecialDelegate specialDelegate)
        {
            _options = optionsAccs.Value;
            _specialDelegate = specialDelegate;
        }
        public async Task<(int, string)> InsertBarrier(DbContext db, string transType, string gid, string branchID, string op, string barrierID, string reason)
        {
            if (db == null) return (-1, string.Empty);
            if (string.IsNullOrWhiteSpace(op)) return (0, string.Empty);
            int result = 0;
            string err = string.Empty;
            string sql = $"insert into {_options.BarrierTableName} (trans_type, gid, branch_id, op, barrier_id, reason) values(#trans_type#,#gid#,#branch_id#,#op#,#barrier_id#,#reason#)";
            try
            {
                result = await db.ExecuteNonQueryAsync(sql, new { trans_type = transType, gid = gid, branch_id = branchID, op = op, barrier_id = barrierID, reason = reason });
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            return (result, err);
        }
        public async Task<(int, string)> InsertBarrier(DbConnection db, string transType, string gid, string branchID, string op, string barrierID, string reason, DbTransaction tx = null)
        {
            if (db == null) return (-1, string.Empty);
            if (string.IsNullOrWhiteSpace(op)) return (0, string.Empty);

            int result = 0;
            string err = string.Empty;

            try
            {
                var str = string.Concat(_options.BarrierTableName, "(trans_type, gid, branch_id, op, barrier_id, reason) values(@trans_type,@gid,@branch_id,@op,@barrier_id,@reason)");
                var sql = _specialDelegate.GetDbSpecial().GetInsertIgnoreTemplate(str, Constant.Barrier.PG_CONSTRAINT);

                sql = _specialDelegate.GetDbSpecial().GetPlaceHoldSQL(sql);

                result = await db.ExecuteAsync(
                    sql,
                    new { trans_type = transType, gid = gid, branch_id = branchID, op = op, barrier_id = barrierID, reason = reason },
                    transaction: tx);
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }

            return (result, err);
        }
    }

}
using async.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
namespace async.Services;

public class WarehouseDb : IWarehouseDb
{
    private readonly string _sqlConn = "Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True";

    public async Task<int> PostWarehouse(Warehouse warehouse)
    {
        const string procedure = "AddProductToWarehouse";
        int result = 0;

        using (var conn = new SqlConnection(_sqlConn))
        {
            await conn.OpenAsync();
            using (var transaction = conn.BeginTransaction())
            {
                using (var comm = new SqlCommand(procedure, conn, transaction))
                {
                    comm.CommandType = System.Data.CommandType.StoredProcedure;
                    comm.Parameters.Add("@IdProduct", System.Data.SqlDbType.Int).Value = warehouse.IdProduct;
                    comm.Parameters.Add("@IdWarehouse", System.Data.SqlDbType.Int).Value = warehouse.IdWarehouse;
                    comm.Parameters.Add("@Amount", System.Data.SqlDbType.Int).Value = warehouse.Amount;
                    comm.Parameters.Add("@CreatedAt", System.Data.SqlDbType.DateTime).Value = DateTime.Parse(warehouse.CreatedAt);
                    try
                    {
                        result = await comm.ExecuteNonQueryAsync();
                        await transaction.CommitAsync();
                    }
                    catch (SqlException exception)
                    {
                        await transaction.RollbackAsync();
                        throw new InvalidOperationException("Error processing request!");
                    }
                }
            }

        }
        return result;
    }
}



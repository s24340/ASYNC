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
            if (conn.State != System.Data.ConnectionState.Closed)
            {
                await conn.CloseAsync();
            }
        }
        return result;
    } 
    public async Task<int> PostWarehouseSelect (Warehouse warehouse)
     {
         var order = 0;
         var price = 0;
         using ( var conn = new SqlConnection(_sqlConn) )
         {
             conn.Open();
             using ( SqlTransaction transaction = conn.BeginTransaction() )
             {
                 using ( var comm = new SqlCommand() )
                 {
                     comm.Connection = conn;
                     comm.Transaction = transaction;
                     comm.CommandText = "SELECT * FROM WAREHOUSE WHERE IDWAREHOUSE = " + warehouse.IdProduct;
                 }

                 using ( var comm = new SqlCommand() )
                 {
                     comm.CommandText = "SELECT * FROM ORDER WHERE WHERE IDPRODUCT = " + warehouse.IdProduct + " AND AMOUNT = " + warehouse.Amount;
                     using ( SqlDataReader dr = await comm.ExecuteReaderAsync() )
                     { 
                         order = int.Parse( dr["IdOrder"].ToString() );
                     }
                 }

                 using ( var comm = new SqlCommand() )
                 {
                     comm.CommandText = "SELECT * FROM PRODUCT_WAREHOUSE WHERE IDORDER = " + order;
                 }

                 using ( var comm = new SqlCommand() )
                 {
                     comm.CommandText = "update order set FulfilledAt = " + DateTime.Now + " where idorder = " + order;
                     comm.ExecuteNonQuery();
                 }

                 using ( var comm = new SqlCommand() )
                 {
                     comm.CommandText = "Select price from product where idproduct = " + warehouse.IdProduct;
                     await comm.ExecuteNonQueryAsync();
                     SqlDataReader dr = await comm.ExecuteReaderAsync();
                     price = int.Parse(dr["price"].ToString());
                     comm.CommandText = $"INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES( {warehouse.IdWarehouse}, {warehouse.IdProduct}, {order}, {warehouse.Amount}, {warehouse.Amount * price}, {warehouse.CreatedAt} );";
                     await comm.ExecuteNonQueryAsync();
                 }
                 await transaction.CommitAsync();
                 await conn.CloseAsync();
             }
         }
         return 1;
     }
}
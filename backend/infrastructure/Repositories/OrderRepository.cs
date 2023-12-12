using Dapper;
using infrastructure.Models;
using Npgsql;

namespace infrastructure.Repositories;

public class OrderRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public OrderRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
    }

    public async Task<Order> CreateOrder(Order order)
    {
        const string sql = @"
        INSERT INTO userorders (userid, totalprice, orderdate) 
        VALUES (@UserId, @TotalPrice, @OrderDate) 
        RETURNING *;"; 

        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QuerySingleAsync<Order>(sql, order);
        }
    }
    
    public async Task<IEnumerable<OrderDetail>> GetOrderDetails(int orderId)
    {
        const string sql = "SELECT * FROM orderdetails WHERE orderid = @orderId;";
        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QueryAsync<OrderDetail>(sql, new { orderId });
        }
    }
    
    public async Task<OrderDetail> CreateOrderDetail(OrderDetail orderDetail)
    {
        const string sql = @"
    INSERT INTO orderdetails (orderid, itemid, quantity, itemtype) 
    VALUES (@OrderId, @ItemId, @Quantity, @ItemType) 
    RETURNING *;";
        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QuerySingleAsync<OrderDetail>(sql, orderDetail);
        }
    }
}
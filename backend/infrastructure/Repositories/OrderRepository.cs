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
    
    public async Task<IEnumerable<Order>> GetAllUserOrders()
    {
        const string sql = "SELECT * FROM userorders;";
        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QueryAsync<Order>(sql);
        }
    }
    
    public async Task<IEnumerable<OrderDetail>> GetAllOrderDetails()
    {
        const string sql = "SELECT * FROM orderdetails;";
        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QueryAsync<OrderDetail>(sql);
        }
    }

    public async Task<Order> GetOrderById(int orderId)
    {
        const string sql = "SELECT * FROM userorders WHERE orderid = @OrderId;";
        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QuerySingleOrDefaultAsync<Order>(sql, new { OrderId = orderId });
        }
    }
}
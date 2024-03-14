using project.entity;

namespace project.dao;

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class OrderDao
{
    private readonly string connectionString;

    public OrderDao(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void AddOrder(Order order)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "INSERT INTO Orders (product_id, quantity, order_date, order_type) VALUES (@ProductId, @Quantity, @OrderDate, @OrderType)";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ProductId", order.ProductId);
        command.Parameters.AddWithValue("@Quantity", order.Quantity);
        command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
        command.Parameters.AddWithValue("@OrderType", order.OrderType);
        command.ExecuteNonQuery();
    }

    public List<Order> GetAllOrders()
    {
        List<Order> orders = new List<Order>();
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "SELECT * FROM Orders";
        using var command = new MySqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            orders.Add(new Order
            {
                Id = Convert.ToInt32(reader["order_id"]),
                ProductId = Convert.ToInt32(reader["product_id"]),
                Quantity = Convert.ToInt32(reader["quantity"]),
                OrderDate = Convert.ToDateTime(reader["order_date"]),
                OrderType = reader["order_type"].ToString()
            });
        }
        return orders;
    }

    public void UpdateOrder(Order order)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "UPDATE Orders SET product_id = @ProductId, quantity = @Quantity, order_date = @OrderDate, order_type = @OrderType WHERE order_id = @OrderId";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ProductId", order.ProductId);
        command.Parameters.AddWithValue("@Quantity", order.Quantity);
        command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
        command.Parameters.AddWithValue("@OrderType", order.OrderType);
        command.Parameters.AddWithValue("@OrderId", order.Id);
        command.ExecuteNonQuery();
    }
    public Order GetById(int orderId)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "SELECT * FROM Orders WHERE order_id = @OrderId";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@OrderId", orderId);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Order
            {
                Id = Convert.ToInt32(reader["order_id"]),
                ProductId = Convert.ToInt32(reader["product_id"]),
                Quantity = Convert.ToInt32(reader["quantity"]),
                OrderDate = Convert.ToDateTime(reader["order_date"]),
                OrderType = reader["order_type"].ToString()
            };
        }
        return null;
    }

    public void DeleteOrder(int orderId)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "DELETE FROM Orders WHERE order_id = @OrderId";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@OrderId", orderId);
        command.ExecuteNonQuery();
    }
}

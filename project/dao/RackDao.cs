using project.entity;

namespace project.dao;

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class RackDao
{
    private readonly string connectionString;

    public RackDao(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void AddRack(Rack rack)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "INSERT INTO Racks (location, capacity) VALUES (@Location, @Capacity)";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Location", rack.Location);
        command.Parameters.AddWithValue("@Capacity", rack.Capacity);
        command.ExecuteNonQuery();
    }
    public Rack GetById(int rackId)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "SELECT * FROM Racks WHERE rack_id = @RackId";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@RackId", rackId);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Rack
            {
                Id = Convert.ToInt32(reader["rack_id"]),
                Location = reader["location"].ToString(),
                Capacity = Convert.ToInt32(reader["capacity"])
            };
        }
        return null;
    }
    public List<Rack> GetAllRacks()
    {
        List<Rack> racks = new List<Rack>();
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "SELECT * FROM Racks";
        using var command = new MySqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            racks.Add(new Rack
            {
                Id = Convert.ToInt32(reader["rack_id"]),
                Location = reader["location"].ToString(),
                Capacity = Convert.ToInt32(reader["capacity"])
            });
        }
        return racks;
    }

    public void UpdateRack(Rack rack)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "UPDATE Racks SET location = @Location, capacity = @Capacity WHERE rack_id = @RackId";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Location", rack.Location);
        command.Parameters.AddWithValue("@Capacity", rack.Capacity);
        command.Parameters.AddWithValue("@RackId", rack.Id);
        command.ExecuteNonQuery();
    }

    public void DeleteRack(int rackId)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "DELETE FROM Racks WHERE rack_id = @RackId";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@RackId", rackId);
        command.ExecuteNonQuery();
    }
    public void PlaceProductOnRack(int productId, int rackId)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "INSERT INTO RackProducts (product_id, rack_id) VALUES (@ProductId, @RackId)";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ProductId", productId);
        command.Parameters.AddWithValue("@RackId", rackId);
        command.ExecuteNonQuery();
    }
}

using MySql.Data.MySqlClient;
using project.entity;

namespace project.dao;

public class JunkyardDao
{
    private readonly string connectionString;

    public JunkyardDao(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void AddToJunkyard(int productId, int quantity)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "INSERT INTO Junkyard (product_id, quantity) VALUES (@ProductId, @Quantity)";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ProductId", productId);
        command.Parameters.AddWithValue("@Quantity", quantity);
        command.ExecuteNonQuery();
    }

    public List<Junkyard> GetAllItems()
    {
        List<Junkyard> items = new List<Junkyard>();
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "SELECT * FROM Junkyard";
        using var command = new MySqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            items.Add(new Junkyard
            {
                JunkId = Convert.ToInt32(reader["junk_id"]),
                ProductId = Convert.ToInt32(reader["product_id"]),
                Quantity = Convert.ToInt32(reader["quantity"])
            });
        }
        return items;
    }

    public void UpdateJunkyardItem(int junkId, int quantity)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "UPDATE Junkyard SET quantity = @Quantity WHERE junk_id = @JunkId";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Quantity", quantity);
        command.Parameters.AddWithValue("@JunkId", junkId);
        command.ExecuteNonQuery();
    }

    public void DeleteFromJunkyard(int junkId)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "DELETE FROM Junkyard WHERE junk_id = @JunkId";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@JunkId", junkId);
        command.ExecuteNonQuery();
    }
}
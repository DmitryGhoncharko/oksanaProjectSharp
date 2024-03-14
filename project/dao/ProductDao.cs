using MySql.Data.MySqlClient;
using project.entity;

namespace project.dao;

public class ProductDao
{
    private readonly string connectionString;

    public ProductDao(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public Product GetById(int productId)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "SELECT * FROM Products WHERE product_id = @ProductId";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ProductId", productId);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Product
            {
                Id = Convert.ToInt32(reader["product_id"]),
                Name = reader["name"].ToString(),
                Category = reader["category"].ToString(),
                Quantity = Convert.ToInt32(reader["quantity"]),
                MinQuantity = Convert.ToInt32(reader["min_quantity"])
            };
        }
        return null;
    }
    public void AddProduct(Product product)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "INSERT INTO Products (name, category, quantity, min_quantity) VALUES (@Name, @Category, @Quantity, @MinQuantity)";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Category", product.Category);
        command.Parameters.AddWithValue("@Quantity", product.Quantity);
        command.Parameters.AddWithValue("@MinQuantity", product.MinQuantity);
        command.ExecuteNonQuery();
    }

    public List<Product> GetAllProducts()
    {
        List<Product> products = new List<Product>();
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "SELECT * FROM Products";
        using var command = new MySqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            products.Add(new Product
            {
                Id = Convert.ToInt32(reader["product_id"]),
                Name = reader["name"].ToString(),
                Category = reader["category"].ToString(),
                Quantity = Convert.ToInt32(reader["quantity"]),
                MinQuantity = Convert.ToInt32(reader["min_quantity"])
            });
        }
        return products;
    }

    public void UpdateProduct(Product product)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "UPDATE Products SET name = @Name, category = @Category, quantity = @Quantity, min_quantity = @MinQuantity WHERE product_id = @ProductId";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Category", product.Category);
        command.Parameters.AddWithValue("@Quantity", product.Quantity);
        command.Parameters.AddWithValue("@MinQuantity", product.MinQuantity);
        command.Parameters.AddWithValue("@ProductId", product.Id);
        command.ExecuteNonQuery();
    }

    public void DeleteProduct(int productId)
    {
        using var connection = new MySqlConnection(connectionString);
        connection.Open();
        string sql = "DELETE FROM Products WHERE product_id = @ProductId";
        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ProductId", productId);
        command.ExecuteNonQuery();
    }
}

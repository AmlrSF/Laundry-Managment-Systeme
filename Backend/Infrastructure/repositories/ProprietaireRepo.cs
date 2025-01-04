// Repository
using System.Data;
using System.Data.SqlClient;
using Laverie.API.Infrastructure.context;
using Laverie.Domain.Entities;
using Laverie.API.Infrastructure.context;
using Microsoft.Data.SqlClient;

namespace Laverie.API.Infrastructure.repositories
{
    public class ProprietaireRepo
    {
        private readonly string _connectionString;

        public ProprietaireRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<User> GetAll()
        {
            var proprietaires = new List<User>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Proprietaire", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    proprietaires.Add(new User
                    {
                        id = (int)reader["Id"],
                        name = reader["Name"].ToString(),
                        email = reader["Email"].ToString(),
                        password = reader["Password"].ToString(),
                        age = (int)reader["Age"]
                    });
                }
            }
            return proprietaires;
        }

        public User GetById(int id)
        {
            User proprietaire = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Proprietaire WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    proprietaire = new User
                    {
                        id = (int)reader["Id"],
                        name = reader["Name"].ToString(),
                        email = reader["Email"].ToString(),
                        password = reader["Password"].ToString(),
                        age = (int)reader["Age"]
                    };
                }
            }
            return proprietaire;
        }

        public void Create(User proprietaire)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Proprietaire (Name, Email, Password, Age) VALUES (@Name, @Email, @Password, @Age)", conn);
                cmd.Parameters.AddWithValue("@Name", proprietaire.name);
                cmd.Parameters.AddWithValue("@Email", proprietaire.email);
                cmd.Parameters.AddWithValue("@Password", proprietaire.password);
                cmd.Parameters.AddWithValue("@Age", proprietaire.age);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(User proprietaire)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Proprietaire SET Name = @Name, Email = @Email, Password = @Password, Age = @Age WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", proprietaire.id);
                cmd.Parameters.AddWithValue("@Name", proprietaire.name);
                cmd.Parameters.AddWithValue("@Email", proprietaire.email);
                cmd.Parameters.AddWithValue("@Password", proprietaire.password);
                cmd.Parameters.AddWithValue("@Age", proprietaire.age);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Proprietaire WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public User Login(string email, string password)
        {
            User proprietaire = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Proprietaire WHERE Email = @Email AND Password = @Password", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    proprietaire = new User
                    {
                        id = (int)reader["Id"],
                        name = reader["Name"].ToString(),
                        email = reader["Email"].ToString(),
                        password = reader["Password"].ToString(),
                        age = (int)reader["Age"]
                    };
                }
            }
            return proprietaire;
        }
    }
}

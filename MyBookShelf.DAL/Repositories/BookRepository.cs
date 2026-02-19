using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MyBookShelf.Common1.Enum;
using MyBookShelf.Common1.Models;


namespace MyBookShelf.DAL.Repositories
{
    public class BookRepository
    {
        private readonly string _connectionString;
        
        public BookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Book> GetAll()
        {
            List<Book> books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id, Title, Author, Status FROM Books";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Book book = new Book 
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.GetString(2),
                            Status = (BookStatus)reader.GetInt32(3)
                        };
                        books.Add(book);
                    }
                }
                
            }
            return books;
            
        }
        public void add(Book book)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Books (Title, Author, Status) VALUES (@Title, @Author, @Status)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@Status", (int)book.Status);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
            {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Books WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdateStatus (int id, BookStatus status)
            {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "UPDATE Books SET Status = @Status WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", (int)status);
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}



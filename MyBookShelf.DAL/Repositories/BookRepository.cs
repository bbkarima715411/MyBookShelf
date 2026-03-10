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
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;

        public BookRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Book> GetAll()
        {
            List<Book> books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id, Title, Author, Status, Rating, Comment FROM Books";
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
                            Status = (BookStatus)reader.GetInt32(3),
                            Rating = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                            Comment = reader.IsDBNull(5) ? null : reader.GetString(5)
                        };
                        books.Add(book);
                    }
                }
                
            }
            return books;
            
        }

        public Book? GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id, Title, Author, Status, Rating, Comment FROM Books WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return new Book
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Author = reader.GetString(2),
                            Status = (BookStatus)reader.GetInt32(3),
                            Rating = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                            Comment = reader.IsDBNull(5) ? null : reader.GetString(5)
                        };
                    }
                }
            }
        }

        public List<Book> GetByStatus(BookStatus status)
        {
            List<Book> books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id, Title, Author, Status, Rating, Comment FROM Books WHERE Status = @status";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@status", (int)status);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Book book = new Book
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Author = reader.GetString(2),
                                Status = (BookStatus)reader.GetInt32(3),
                                Rating = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                                Comment = reader.IsDBNull(5) ? null : reader.GetString(5)
                            };
                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }

        public void Update(Book book)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "UPDATE Books SET Title = @Title, Author = @Author, Status = @Status, Rating = @Rating, Comment = @Comment WHERE id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@Status", (int)book.Status);
                    command.Parameters.AddWithValue("@Rating", (object?)book.Rating ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Comment", (object?)book.Comment ?? DBNull.Value);
                    command.Parameters.AddWithValue("@id", book.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void add(Book book)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Books (Title, Author, Status, Rating, Comment) VALUES (@Title, @Author, @Status, @Rating, @Comment)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@Status", (int)book.Status);
                    command.Parameters.AddWithValue("@Rating", (object?)book.Rating ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Comment", (object?)book.Comment ?? DBNull.Value);
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



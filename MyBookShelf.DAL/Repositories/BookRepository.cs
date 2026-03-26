using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MyBookShelf.Models;


namespace MyBookShelf.DAL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;

        public BookRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Book> GetAll(string userId)
        {
            List<Book> books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id, UserId, Title, Author, Status, Rating, Comment FROM Books WHERE UserId = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Book book = new Book
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetString(1),
                                Title = reader.GetString(2),
                                Author = reader.GetString(3),
                                Status = (BookStatus)reader.GetInt32(4),
                                Rating = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                                Comment = reader.IsDBNull(6) ? null : reader.GetString(6)
                            };
                            books.Add(book);
                        }
                    }
                }
                
            }
            return books;
            
        }

        public Book? GetById(string userId, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id, UserId, Title, Author, Status, Rating, Comment FROM Books WHERE id = @id AND UserId = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@userId", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return new Book
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetString(1),
                            Title = reader.GetString(2),
                            Author = reader.GetString(3),
                            Status = (BookStatus)reader.GetInt32(4),
                            Rating = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                            Comment = reader.IsDBNull(6) ? null : reader.GetString(6)
                        };
                    }
                }
            }
        }

        public List<Book> GetByStatus(string userId, BookStatus status)
        {
            List<Book> books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id, UserId, Title, Author, Status, Rating, Comment FROM Books WHERE UserId = @userId AND Status = @status";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@status", (int)status);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Book book = new Book
                            {
                                Id = reader.GetInt32(0),
                                UserId = reader.GetString(1),
                                Title = reader.GetString(2),
                                Author = reader.GetString(3),
                                Status = (BookStatus)reader.GetInt32(4),
                                Rating = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                                Comment = reader.IsDBNull(6) ? null : reader.GetString(6)
                            };
                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }

        public void Update(string userId, Book book)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "UPDATE Books SET Title = @Title, Author = @Author, Status = @Status, Rating = @Rating, Comment = @Comment WHERE id = @id AND UserId = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@Status", (int)book.Status);
                    command.Parameters.AddWithValue("@Rating", (object?)book.Rating ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Comment", (object?)book.Comment ?? DBNull.Value);
                    command.Parameters.AddWithValue("@id", book.Id);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void add(Book book)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Books (UserId, Title, Author, Status, Rating, Comment) VALUES (@UserId, @Title, @Author, @Status, @Rating, @Comment)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", book.UserId);
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@Status", (int)book.Status);
                    command.Parameters.AddWithValue("@Rating", (object?)book.Rating ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Comment", (object?)book.Comment ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(string userId, int id)
            {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Books WHERE id = @id AND UserId = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void UpdateStatus (string userId, int id, BookStatus status)
            {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "UPDATE Books SET Status = @Status WHERE id = @id AND UserId = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", (int)status);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}



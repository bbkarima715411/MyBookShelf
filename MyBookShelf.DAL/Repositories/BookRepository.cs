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

        private const string SelectColumns = "SELECT id, UserId, Title, Author, Status, IsFavorite, Rating, Comment FROM Books";

        public BookRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private static Book MapBook(SqlDataReader reader)
        {
            return new Book
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetString(1),
                Title = reader.GetString(2),
                Author = reader.GetString(3),
                Status = (BookStatus)reader.GetInt32(4),
                IsFavorite = reader.GetBoolean(5),
                Rating = reader.IsDBNull(6) ? null : reader.GetInt32(6),
                Comment = reader.IsDBNull(7) ? null : reader.GetString(7)
            };
        }

        public List<Book> GetAll(string userId)
        {
            List<Book> books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = SelectColumns + " WHERE UserId = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(MapBook(reader));
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
                string query = SelectColumns + " WHERE id = @id AND UserId = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@userId", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return MapBook(reader);
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
                string query = SelectColumns + " WHERE UserId = @userId AND Status = @status";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@status", (int)status);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(MapBook(reader));
                        }
                    }
                }
            }

            return books;
        }

        public List<Book> Search(string userId, string? title, string? author, BookStatus? status, bool favoritesOnly)
        {
            List<Book> books = new List<Book>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var query = new StringBuilder(SelectColumns);
                query.Append(" WHERE UserId = @userId");

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.Parameters.AddWithValue("@userId", userId);

                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        query.Append(" AND Title LIKE @title");
                        command.Parameters.AddWithValue("@title", "%" + title.Trim() + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(author))
                    {
                        query.Append(" AND Author LIKE @author");
                        command.Parameters.AddWithValue("@author", "%" + author.Trim() + "%");
                    }

                    if (status.HasValue)
                    {
                        query.Append(" AND Status = @status");
                        command.Parameters.AddWithValue("@status", (int)status.Value);
                    }

                    if (favoritesOnly)
                    {
                        query.Append(" AND IsFavorite = 1");
                    }

                    query.Append(" ORDER BY Title");
                    command.CommandText = query.ToString();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(MapBook(reader));
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

                string query = "UPDATE Books SET Title = @Title, Author = @Author, Status = @Status, IsFavorite = @IsFavorite, Rating = @Rating, Comment = @Comment WHERE id = @id AND UserId = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@Status", (int)book.Status);
                    command.Parameters.AddWithValue("@IsFavorite", book.IsFavorite);
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

                string query = "INSERT INTO Books (UserId, Title, Author, Status, IsFavorite, Rating, Comment) VALUES (@UserId, @Title, @Author, @Status, @IsFavorite, @Rating, @Comment)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", book.UserId);
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@Status", (int)book.Status);
                    command.Parameters.AddWithValue("@IsFavorite", book.IsFavorite);
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

        public void UpdateFavorite(string userId, int id, bool isFavorite)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "UPDATE Books SET IsFavorite = @IsFavorite WHERE id = @id AND UserId = @userId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IsFavorite", isFavorite);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}



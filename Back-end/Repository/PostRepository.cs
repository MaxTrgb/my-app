using DENMAP_SERVER.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DENMAP_SERVER.Repository
{
    internal class PostRepository
    {
        public int addPost(MySqlConnection connection, int userId, string title, byte[] image, string content, double rating, DateTime createdAt)
        {
            string query = $"INSERT INTO posts (user_id, title, image, content, rating, created_at) VALUES (@userId, @title, @image, @content, @rating, @createdAt); SELECT LAST_INSERT_ID();";
            int id = 0;
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@image", image);
                cmd.Parameters.AddWithValue("@content", content);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@createdAt", createdAt);
                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return id;
        }


        public List<Post> getPostsByUserID(MySqlConnection connection, int userId)
        {
            List<Post> posts = new List<Post>();
            string query = $"SELECT * " +
                           $"FROM posts " +
                           $"WHERE user_id = @userId";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte[] image = (reader["image"] != DBNull.Value) ? (byte[])reader["image"] : null;

                        Post post = new Post(
                            Convert.ToInt32(reader["id"]),
                            Convert.ToInt32(reader["user_id"]),
                            Convert.ToString(reader["title"]),
                            image,
                            Convert.ToString(reader["content"]),
                            Convert.ToDouble(reader["rating"]),
                            Convert.ToDateTime(reader["created_at"])
                        );

                        posts.Add(post);
                    }
                }
            }
            return posts;
        }

        public Post getPostByID(MySqlConnection connection, int Id)
        {
            Post post = null;
            string query = $"SELECT * " +
                           $"FROM posts " +
                           $"WHERE id = @Id";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Id", Id);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte[] image = (reader["image"] != DBNull.Value) ? (byte[])reader["image"] : null;

                        post = new Post(
                            Convert.ToInt32(reader["id"]),
                            Convert.ToInt32(reader["user_id"]),
                            Convert.ToString(reader["title"]),
                            image,
                            Convert.ToString(reader["content"]),
                            Convert.ToDouble(reader["rating"]),
                            Convert.ToDateTime(reader["created_at"])
                        );

                    }
                }
            }
            return post;
        }

        public List<Post> getAllPosts(MySqlConnection connection)
        {
            List<Post> posts = new List<Post>();
            string query = $"SELECT * " +
                           $"FROM posts ";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte[] image = (reader["image"] != DBNull.Value) ? (byte[])reader["image"] : null;

                        Post post = new Post(
                            Convert.ToInt32(reader["id"]),
                            Convert.ToInt32(reader["user_id"]),
                            Convert.ToString(reader["title"]),
                            image,
                            Convert.ToString(reader["content"]),
                            Convert.ToDouble(reader["rating"]),
                            Convert.ToDateTime(reader["created_at"])
                        );

                        posts.Add(post);
                    }
                }
            }
            return posts;
        }

        public int updatePost(MySqlConnection connection, int id, int userId, string title, byte[] image, string content, double rating, DateTime createdAt)
        {
            string query = $"UPDATE posts " +
                           $"SET user_id = @userId, " +
                               $"title = @title, " +
                               $"image = @image, " +
                               $"content = @content, " +
                               $"rating = @rating, " +
                               $"created_at = @createdAt " +
                           $"WHERE id = @Id";

            int result = 0;
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@image", image);
                cmd.Parameters.AddWithValue("@content", content);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@createdAt", createdAt);
                cmd.Parameters.AddWithValue("@Id", id);

                result = cmd.ExecuteNonQuery();
            }
            return result;
        }

        public int updatePostRating(MySqlConnection connection, int id, double rating)
        {
            string query = $"UPDATE posts " +
                           $"SET rating = @rating " +
                           $"WHERE id = @Id";

            int result = 0;
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@Id", id);

                result = cmd.ExecuteNonQuery();
            }
            return result;
        }

        public double getPostRating(MySqlConnection connection, int id)
        {
            double rating = 0.0;
            string query = $"SELECT rating " +
                           $"FROM posts " +
                           $"WHERE id = @Id";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("rating")))
                        {
                            rating = Convert.ToDouble(reader["rating"]);
                        }
                    }
                }
            }
            return rating;
        }
    }
}

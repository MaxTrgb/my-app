using DENMAP_SERVER.Entity;
using DENMAP_SERVER.Repository;
using MySql.Data.MySqlClient;

namespace DENMAP_SERVER.Service
{
    internal class PostService
    {
        string connectionString = "Server=34.116.253.154;Port=3306;Database=chat_database;Uid=app_user;Pwd=&X9fT#7vYqZ$4LpR;";

        private PostRepository postRepository = new PostRepository();
        private CommentRepository commentRepository = new CommentRepository();

        public int AddPost(int userId, string title, string image, string content, int genreId)
        {
            int id = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    id = postRepository.addPost(connection, userId, title, image, content, 0, DateTime.Now, genreId);
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }
            return id;
        }

        public Post GetPostById(int id)
        {
            Post post = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    post = postRepository.getPostByID(connection, id);
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }

            if (post == null)
            {
                throw new Exception("Post with id: " + id + " not found");
            }

            return post;
        }

        public List<Post> GetPostsByUserId(int userId)
        {
            List<Post> posts = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    posts = postRepository.getPostsByUserID(connection, userId);
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }

            return posts;
        }

        public List<Post> GetAllPosts()
        {
            List<Post> posts = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    posts = postRepository.getAllPosts(connection);
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }

            return posts;
        }

        public int UpdatePost(int id, int userId, string title, string image, string content, double rating, DateTime createdAt)
        {
            int result = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    result = postRepository.updatePost(connection, id, userId, title, image, content, rating, createdAt);
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }

            return result;
        }

        public int UpdatePostRating(int id, double rating)
        {
            int result = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    result = postRepository.updatePostRating(connection, id, rating);
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }

            return result;
        }

        public double GetPostRating(int id)
        {
            double rating = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    rating = postRepository.getPostRating(connection, id);
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }

            return rating;
        }

        public void ReCalculatePostRating(int id)
        {
            double rating = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    List<Comment> comments = commentRepository.getCommentsByPostID(connection, id);
                    foreach (Comment comment in comments)
                    {
                        rating += comment.Rating;
                    }
                    rating /= comments.Count;

                    
                }
                catch (Exception ex)
                {
                    throw new Exception("Database error.\nError:" + ex.Message);
                }
            }

            UpdatePostRating(id, rating); ;
        }
    }
}

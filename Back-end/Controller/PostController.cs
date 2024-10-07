using DENMAP_SERVER.Entity.dto;
using DENMAP_SERVER.Entity;
using DENMAP_SERVER.Service;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.ModelBinding;

namespace DENMAP_SERVER.Controller
{
    internal class PostController : NancyModule
    {
        private UserService _userService = new UserService();
        private PostService _postService = new PostService();
        private CommentService _commentService = new CommentService();

        private readonly string _basePath = "/post";

        public PostController()
        {
            Get(_basePath + "/", _ => 
            {

                try
                {
                    List<Post> posts = _postService.GetAllPosts();
                    List<int> userIds = posts.Select(x => x.UserId).ToList();

                    List<User> users = _userService.GetUsersByIds(userIds);

                    List<PostDTO> postsDTO = new List<PostDTO>();

                    Dictionary<int, Post> userPosts = new Dictionary<int, Post>();
                    posts.ForEach(x => userPosts.Add(x.UserId, x));

                    foreach (Post post in posts)
                    {
                        if (userPosts.ContainsKey(post.UserId))
                        {
                            postsDTO.Add(new PostDTO(post, users.Find(x => x.Id == post.UserId)));
                        }

                    }

                    return Response.AsJson(postsDTO);
                }
                catch (Exception ex)
                {
                    return Response.AsJson(new { message = ex.Message }, HttpStatusCode.NotFound);
                }
            });


            Get(_basePath + "/{id}", parameters =>
            {
                int id = parameters.id;

                try
                {
                    Post post = _postService.GetPostById(id);
                    if (post == null)
                        return Response.AsJson(new { message = "Post with id " + id + " not found" }, HttpStatusCode.NotFound);

                    User user = _userService.GetUserById(post.UserId);
                    if (user == null)
                        return Response.AsJson(new { message = "User with id " + post.UserId + " not found" }, HttpStatusCode.NotFound);

                    List<Comment> comments = _commentService.GetCommentsByPostId(id);

                    List<User> commentUsers = new List<User>();
                    if (comments != null)
                        commentUsers = _userService.GetUsersByIds(comments.Select(x => x.UserId).ToList());

                    Dictionary<int, Comment> userCommentsMap = new Dictionary<int, Comment>();
                    if (comments != null)
                        comments.ForEach(x => userCommentsMap.Add(x.UserId, x));

                    List<CommentDTO> commentDTOs = new List<CommentDTO>();

                    foreach (Comment comment in comments)
                        commentDTOs.Add(new CommentDTO(comment, commentUsers.Find(x => x.Id == comment.UserId)));


                    PostDTO postDTO = new PostDTO(post, user, commentDTOs);

                    return Response.AsJson(postDTO);
                }
                catch (Exception ex)
                {
                    return Response.AsJson(new { message = ex.Message }, HttpStatusCode.NotFound);
                }
            });

            Post(_basePath + "/", args =>
            {
                int userId;
                string title;
                byte[] image;
                string content;

                try
                {
                    userId = this.Bind<int>("userId");
                    title = this.Bind<string>("title");
                    image = this.Bind<byte[]>("image");
                    content = this.Bind<string>("content");
                }
                catch (Exception e)
                {
                    return Response.AsJson(new { message = e.Message }, HttpStatusCode.BadRequest);
                }

                try
                {
                    int postId = _postService.AddPost(userId, title, image, content);
                    return Response.AsJson(new { message = postId }, HttpStatusCode.Created);
                }
                catch (Exception e)
                {
                    return Response.AsJson(new { message = e.Message }, HttpStatusCode.BadRequest);
                }
            });

            Get(_basePath + "/", args =>
            {
                int? id = (int?)this.Request.Query["userId"];

                if (!id.HasValue)
                    return Response.AsJson(new { message = "Missing id parameter" }, HttpStatusCode.BadRequest);

                List<Post> posts = _postService.GetPostsByUserId(id.Value);
                if (posts != null)
                    return Response.AsJson(new { message = "Posts not found" }, HttpStatusCode.NotFound);

                User user = _userService.GetUserById(posts[0].UserId);
                if (user != null)
                    return Response.AsJson(new { message = "User not found" }, HttpStatusCode.NotFound);

                List<PostDTO> postDTOs = new List<PostDTO>();

                foreach (Post post in posts)
                    postDTOs.Add(new PostDTO(post, user));

                return Response.AsJson(user);
            });
        }
    }
}

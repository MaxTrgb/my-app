using DENMAP_SERVER.Controller.request;
using DENMAP_SERVER.Entity;
using DENMAP_SERVER.Entity.dto;
using DENMAP_SERVER.Service;
using Nancy;
using Nancy.ModelBinding;

namespace DENMAP_SERVER.Controller
{
    public class UserController : NancyModule
    {
        private UserService _userService = new UserService();
        private PostService _postService = new PostService();
        private CommentService _commentService = new CommentService();

        private const string _BASE_PATH = "/api/v1/user";

        public UserController()
        {
            //Get(_basePath + "/", _ => "{\"message\": \"Hello World!\"}");

            Options("/*", args =>
            {
                return new Response()
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
                    .WithHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
            });

            After.AddItemToEndOfPipeline(ctx =>
            {
                ctx.Response
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
                    .WithHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
            });

            Get(_BASE_PATH + "/{id}", parameters =>
            {
                int id = parameters.id;

                try
                {
                    User user = _userService.GetUserById(id);
                    Console.WriteLine("user: " + user);

                    List<Comment> comments = _commentService.GetCommentsByUserId(id);
                    Console.WriteLine("comments: " + comments);

                    List<Post> posts = _postService.GetPostsByUserId(id);
                    Console.WriteLine("posts: " + posts);

                    return Response.AsJson(new UserDTO(user, comments, posts));
                }
                catch (Exception ex)
                {
                    return Response.AsJson(new { message = ex.Message }, HttpStatusCode.NotFound);
                }
            });

            Put(_BASE_PATH + "/{id}", parameters =>
            {
                int id = parameters.id;

                UserRequest request = null;
                try
                {
                    request = this.Bind<UserRequest>();
                }
                catch (Exception e)
                {
                    return Response.AsJson(new { message = e.Message }, HttpStatusCode.BadRequest);
                }

                try
                {
                    User user = _userService.GetUserById(id);
                }
                catch (Exception ex)
                {
                    return Response.AsJson(new { message = ex.Message }, HttpStatusCode.NotFound);
                }

                try
                {
                    int userId = _userService.UpdateUser(id, request.Name, request.Password, request.Image, request.Description);

                    return Response.AsJson(new { message = "User updated successfully", userId });
                }
                catch (Exception ex)
                {
                    return Response.AsJson(new { message = ex.Message }, HttpStatusCode.BadRequest);
                }
            });
        }
    }
}

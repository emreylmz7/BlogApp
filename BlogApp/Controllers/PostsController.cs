using System.Globalization;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;
        public PostsController(IPostRepository postRepository,ICommentRepository commentRepository)
        {
            _postRepository = postRepository; 
            _commentRepository = commentRepository;
        }

        public async Task<IActionResult> Index(string tag)
        {
            var claims = User.Claims;
            var posts = _postRepository.Posts;
            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(x => x.Tags.Any(t=> t.Url == tag));
            };
            return View(
                await posts
                        .Include(x => x.Tags)
                        .ToListAsync()
            );
        }

        public async Task<IActionResult> Details(string url)
        {
            return View(await _postRepository
                        .Posts
                        .Include(x => x.Tags)
                        .Include(x => x.Comments)
                        .ThenInclude(x => x.User)
                        .FirstOrDefaultAsync(d => d.Url == url)
            );
        }

        [HttpPost]
        public IActionResult AddComment(int PostId,string UserName,string CommentText,string Url)
        {
            var entity = new Comment {
                CommentText = CommentText,
                PublishedOn = DateTime.Now,
                PostId = PostId,
                User = new User {UserName = UserName, Image= "avatar.jpeg"}
            };
            _commentRepository.CreateComment(entity);
            
            return Redirect("/posts/details/" + Url);
        }  
      
    }
}
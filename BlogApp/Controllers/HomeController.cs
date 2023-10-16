using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private IPostRepository _postRepository;
        public HomeController(IPostRepository postRepository)
        {
            _postRepository = postRepository; 
        }

        public IActionResult Index()
        {
            var claims = User.Claims;
            var posts = _postRepository.Posts;
           
            return View(posts.OrderBy(p => p.PublishedOn).Take(3).ToList());
        }
    }
}
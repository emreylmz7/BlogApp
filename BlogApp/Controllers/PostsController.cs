using System.Globalization;
using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;
        private ITagRepository _tagRepository;
        public PostsController(IPostRepository postRepository,ICommentRepository commentRepository,ITagRepository tagRepository)
        {
            _postRepository = postRepository; 
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
        }

        public async Task<IActionResult> Index(string tag)
        {
            var posts = _postRepository.Posts.Where(i => i.IsActive);
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
                        .Include(x => x.User)
                        .Include(x => x.Comments)
                        .ThenInclude(x => x.User)
                        .FirstOrDefaultAsync(d => d.Url == url)
            );
        }

        [HttpPost]
        public IActionResult AddComment(int PostId,string CommentText,string Url)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);

            var entity = new Comment {
                CommentText = CommentText,
                PublishedOn = DateTime.Now,
                PostId = PostId,
                UserId = int.Parse(userId ?? ""),
            };
            _commentRepository.CreateComment(entity);
            
            return RedirectToRoute("post_details",new {url = Url});
        }  

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Create(PostCreateViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                _postRepository.CreatePost( new Post{
                    Title = model.Title,
                    Content = model.Content,
                    Description = model.Description,
                    Url = model.Url,
                    UserId = int.Parse(userId ?? ""),
                    PublishedOn = DateTime.Now,
                    Image = "b1.jpg",
                    IsActive = false
                }); 
                return RedirectToAction("Index");
            }

            return View(model);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);

            var post = _postRepository.Posts;
            if(string.IsNullOrEmpty(role))
            {
                post = post.Where(i => i.UserId == userId);
            }

            return View(await post.ToListAsync());
        }
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var post = _postRepository.Posts.Include(i => i.Tags).FirstOrDefault(i => i.PostId == id);
            if(post == null)
            {
                return NotFound();
            }

            ViewBag.Tags = _tagRepository.Tags.ToList();
            
            return View(new PostCreateViewModel {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                IsActive = post.IsActive,
                Tags = post.Tags
            });
        }
        [Authorize]
        [HttpPost]
        public IActionResult Edit(PostCreateViewModel model,int[] tagIds)
        {
            if (ModelState.IsValid)
            {
                var entityToUpdate = new Post{
                    PostId = model.PostId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url,
                };

                if (User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    entityToUpdate.IsActive = model.IsActive;
                }

                _postRepository.EditPost(entityToUpdate,tagIds);
                return RedirectToAction("List");
            }
            ViewBag.Tags = _tagRepository.Tags.ToList();
            return View(model);
        }
    }
}
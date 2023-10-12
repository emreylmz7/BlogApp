using Blogapp.Data.Concrete.EfCore;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void FillTestData(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag {Text = "Web Programming" ,Url = "web-programming",Color = TagColors.primary},
                        new Tag {Text = "Frontend",Url = "frontend",Color = TagColors.danger},
                        new Tag {Text = "Backend" ,Url = "backend",Color = TagColors.secondary},
                        new Tag {Text = "Full-Stack",Url = "full-stack",Color = TagColors.success},
                        new Tag {Text = "Data Science",Url = "data-science",Color = TagColors.warning}
                    );
                    context.SaveChanges();
                };

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User {UserName = "emreyilmaz", Image = "p1.jpg",Name = "Emre Yılmaz",Email = "emreyilmaz@gmail.com",Password = "123456"},
                        new User {UserName = "sadikturan", Image = "p2.jpg",Name = "Sadık Turan",Email = "sadikturan@gmail.com",Password = "123456"}
                    );
                    context.SaveChanges();
                };
                
                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post {
                            Title = "ASP.NET Core",
                            Content = "ASP.NET Core Courses for Beginners",
                            Url = "aspnet-core",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            Image = "1.jpg",
                            UserId = 1,
                            Comments = new List<Comment> {
                                new Comment {CommentText = "It was Perfect",PublishedOn = new DateTime(),UserId = 1},
                                new Comment {CommentText = "It was awesome,Its help me a lot!",PublishedOn = new DateTime(),UserId = 2}
                            }
                        },
                        new Post {
                            Title = "Angular",
                            Content = "Angular for Middle Level",
                            Url = "angular",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-5),
                            Tags = context.Tags.Take(2).ToList(),
                            Image = "2.png",
                            UserId = 1,
                        },
                        new Post {
                            Title = "Django",
                            Content = "Django with Pyhton for Professionals",
                            Url = "django",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-20),
                            Tags = context.Tags.Take(4).ToList(),
                            Image = "3.png",
                            UserId = 2,
                        },
                        new Post {
                            Title = "React",
                            Content = "React for Everyone",
                            Url = "react",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-30),
                            Tags = context.Tags.Take(2).ToList(),
                            Image = "4.png",
                            UserId = 1,
                        },
                        new Post {
                            Title = "Php",
                            Content = "Php Courses",
                            Url = "php",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-35),
                            Tags = context.Tags.Take(2).ToList(),
                            Image = "5.png",
                            UserId = 2,
                        },
                        new Post {
                            Title = "C# Web Development",
                            Content = "C# Web Development Courses.",
                            Url = "c-sharp",
                            IsActive = true,
                            PublishedOn = DateTime.Now.AddDays(-50),
                            Tags = context.Tags.Take(2).ToList(),
                            Image = "6.jpeg",
                            UserId = 2,
                        }
                    );
                    context.SaveChanges();
                };
            }
        }
    }
}
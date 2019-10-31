using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            var menu = true;
            while (menu)
            {
                var db = new BloggingContext();
                Console.WriteLine("\n_______________\nBlog Console\n_______________\n1. Display Blogs\n2. Add Blog\n3. Add Post\n4. Remove Blogs\nEnter to Quit");
                switch (Console.ReadLine())
                {
                    case "1":
                        var query = db.Blogs.OrderBy(b => b.Name);
                        Console.WriteLine("\n\n" + query.Count() +" blogs in the database:\n");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                        break;
                    case "2":
                        Console.WriteLine();
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();
                        var blog = new Blog { Name = name };
                        db.AddBlog(blog);
                        logger.Info($"Blog added - {name}");
                        break;
                    case "3":
                        Console.WriteLine();
                        var idQuery = db.Blogs.OrderBy(b => b.BlogId);
                        int id=0;
                        foreach (var item in idQuery)
                        {
                            Console.WriteLine(item.BlogId + " " + item.Name);
                        }
                        Console.WriteLine("Enter the ID of the Blog you are posting to");
                        try {
                            id = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (Exception e) { Console.WriteLine("Please enter a number"); }
                        Blog postBlog = null;
                        foreach (var item in idQuery)
                        {
                            if (id == item.BlogId)
                            {
                                postBlog = item;
                                break;
                            }
                        }
                        if (postBlog == null) Console.WriteLine("Blog not found");
                        else {
                            Console.WriteLine("Enter Post Title:");
                            var title = Console.ReadLine();
                            while (title == "")
                            {
                                Console.WriteLine("Invalid title-\nEnter Post Title:");
                                title = Console.ReadLine();
                            }
                            Console.WriteLine("Content:");
                            var content = Console.ReadLine();
                            Post post = new Post()
                            {
                                Title = title,
                                Blog = postBlog,
                                //BlogId = id,
                                Content = content
                            };
                            db.AddPost(post);
                            logger.Info("Post added to " + post.Blog.Name);
                        }
                        
                        break;
                    case "4":
                        db.Blogs.RemoveRange(db.Blogs);
                        db.SaveChanges();
                        break;
                default:
                        menu = false;
                        break;
                }
            }
            logger.Info("Program ended");
        }
    }
}

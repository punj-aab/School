using StudentTracker.Core.Components.Interfaces;
using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data.Entity.Migrations;
using System.Data.Entity;
namespace StudentTracker.Core.Components
{
    public class BlogComponent : BaseComponent, IBlogComponent
    {
        // NHibernate object


        public BlogComponent()
        {

        }

        public IList<Post> Posts(int pageNo, int pageSize)
        {
            var query = context.Posts
                        .Include("Tags")
                        .Include("Category")
                        .Where(p => p.Published)
                        .OrderByDescending(p => p.PostedOn)
                        .Skip(pageNo * pageSize)
                        .Take(pageSize);

            return query.ToList();
            //var query = _session.Query<Post>()
            //                .Where(p => p.Published)
            //                .OrderByDescending(p => p.PostedOn)
            //                .Skip(pageNo * pageSize)
            //                .Take(pageSize)
            //                .Fetch(p => p.Category);

            //query.FetchMany(p => p.Tags).ToFuture();

            //return query.ToFuture().ToList();
        }

        public int TotalPosts(bool checkIsPublished = true, long userId = -1)
        {
            //return _session.Query<Post>()
            //.Where(p => checkIsPublished || p.Published == true)
            //.Count();
            if (userId == -1)
            {
                return context.Posts
                              .Where(p => checkIsPublished || p.Published)
                              .Count();
            }
            else
            {
                return context.Posts
                              .Where(p => (checkIsPublished || p.Published) && p.PostedBy == userId)
                              .Count();
            }
        }

        public IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize)
        {
            //var query = _session.Query<Post>()
            //                .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
            //                .OrderByDescending(p => p.PostedOn)
            //                .Skip(pageNo * pageSize)
            //                .Take(pageSize)
            //                .Fetch(p => p.Category);

            //query.FetchMany(p => p.Tags).ToFuture();

            var query = context.Posts
                        .Include("Tags")
                        .Include("Category")
                        .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                        .OrderByDescending(p => p.PostedOn)
                        .Skip(pageNo * pageSize)
                        .Take(pageSize);

            return query.ToList();
        }

        public int TotalPostsForCategory(string categorySlug)
        {
            //return _session.Query<Post>()
            //            .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
            //            .Count();
            return context.Posts
                          .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                          .Count();

        }

        public Category Category(string categorySlug)
        {
            //return _session.Query<Category>()
            //            .FirstOrDefault(t => t.UrlSlug.Equals(categorySlug));
            return context.Categories
                       .FirstOrDefault(t => t.UrlSlug.Equals(categorySlug));
        }

        public IList<Post> PostsForTag(string tagSlug, int pageNo, int pageSize)
        {
            var query = context.Posts
                        .Include("Tags")
                        .Include("Category")
                        .Where(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                        .OrderByDescending(p => p.PostedOn)
                        .Skip(pageNo * pageSize)
                        .Take(pageSize);

            return query.ToList();
        }

        public int TotalPostsForTag(string tagSlug)
        {
            //return _session.Query<Post>()
            //            .Where(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
            //            .Count();

            return context.Posts.Where(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                            .Count();
        }

        public Tag Tag(string tagSlug)
        {
            return context.Tags
                        .FirstOrDefault(t => t.UrlSlug.Equals(tagSlug));
        }

        public IList<Post> PostsForSearch(string search, int pageNo, int pageSize)
        {

            var query = context.Posts
                       .Include("Tags")
                       .Include("Category")
                       .Where(p => p.Published && (p.Title.Contains(search) || p.Category.Name.Equals(search) || p.Tags.Any(t => t.Name.Equals(search))))
                       .OrderByDescending(p => p.PostedOn)
                       .Skip(pageNo * pageSize)
                       .Take(pageSize);

            //var query = _session.Query<Post>()
            //                    .Where(p => p.Published && (p.Title.Contains(search) || p.Category.Name.Equals(search) || p.Tags.Any(t => t.Name.Equals(search))))
            //                    .OrderByDescending(p => p.PostedOn)
            //                    .Skip(pageNo * pageSize)
            //                    .Take(pageSize)
            //                    .Fetch(p => p.Category);

            //query.FetchMany(p => p.Tags).ToFuture();

            return query.ToList();
        }

        public int TotalPostsForSearch(string search)
        {
            return context.Posts
                    .Where(p => p.Published && (p.Title.Contains(search) || p.Category.Name.Equals(search) || p.Tags.Any(t => t.Name.Equals(search))))
                    .Count();
        }

        public Post Post(int year, int month, string titleSlug)
        {
            var query = context.Posts
                            .Include("Tags")
                            .Include("Category")
                            .Where(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug.Equals(titleSlug));

            return query.Single();
        }

        public IList<Category> Categories()
        {
            return context.Categories.Where(a => a.IsDeleted != true).OrderBy(p => p.Name).ToList();
        }

        public IList<Tag> Tags()
        {
            return context.Tags.Where(a => a.IsDeleted != true).OrderBy(p => p.Name).ToList();
        }

        public IList<Post> PostsNew(int userId)
        {
            IQueryable<Post> query;
            query = context.Posts
                                      .Where(p => p.PostedBy == userId)
                                      .Include("Tags")
                                      .Include("Category")
                                      .OrderByDescending(p => p.Published);

            return query.ToList();
        }

        public IList<Post> Posts(int pageNo, int pageSize, string sortColumn, bool sortByAscending, long userId)
        {
            IQueryable<Post> query;

            switch (sortColumn)
            {
                case "Title":
                    if (sortByAscending)
                        query = context.Posts
                                        .Where(p => p.PostedBy == userId)
                                        .Include("Tags")
                                        .Include("Category")
                                        .OrderBy(p => p.Title)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize);

                    else
                        query = context.Posts
                                        .Where(p => p.PostedBy == userId)
                                        .Include("Tags")
                                        .Include("Category")
                                        .OrderByDescending(p => p.Title)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize);
                    break;

                case "Published":
                    if (sortByAscending)
                        query = context.Posts
                                        .Where(p => p.PostedBy == userId)
                                        .Include("Tags")
                                        .Include("Category")
                                        .OrderBy(p => p.Published)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize);

                    else
                        query = context.Posts
                                        .Where(p => p.PostedBy == userId)
                                        .Include("Tags")
                                        .Include("Category")
                                        .OrderByDescending(p => p.Published)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize);

                    break;

                case "PostedOn":
                    if (sortByAscending)
                        query = context.Posts
                                        .Where(p => p.PostedBy == userId)
                                        .Include("Tags")
                                        .Include("Category")
                                        .OrderBy(p => p.PostedOn)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize);
                    else
                        query = context.Posts
                                        .Where(p => p.PostedBy == userId)
                                        .Include("Tags")
                                        .Include("Category")
                                        .OrderByDescending(p => p.PostedOn)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize);
                    break;

                case "Modified":
                    if (sortByAscending)
                        query = context.Posts
                                        .Where(p => p.PostedBy == userId)
                                        .Include("Tags")
                                        .Include("Category")
                                        .OrderBy(p => p.Modified)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize);
                    else
                        query = context.Posts
                                        .Where(p => p.PostedBy == userId)
                                        .Include("Tags")
                                        .Include("Category")
                                        .OrderByDescending(p => p.Modified)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize);
                    break;

                case "Category":
                    if (sortByAscending)
                        query = context.Posts
                                         .Where(p => p.PostedBy == userId)
                                        .Include("Tags")
                                        .Include("Category")
                                        .OrderBy(p => p.Category.Name)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize);
                    else
                        query = context.Posts
                                         .Where(p => p.PostedBy == userId)
                                        .Include("Tags")
                                        .Include("Category")
                                        .OrderByDescending(p => p.Category.Name)
                                        .Skip(pageNo * pageSize)
                                        .Take(pageSize);
                    break;

                default:
                    query = context.Posts
                                         .Where(p => p.PostedBy == userId)
                                        .Include("Tags")
                                        .Include("Category")
                                    .OrderByDescending(p => p.PostedOn)
                                    .Skip(pageNo * pageSize)
                                    .Take(pageSize);
                    break;
            }



            return query.ToList();
        }

        public int AddPost(Post post)
        {
            using (var transactionScope = new TransactionScope())
            {
                post.Category = context.Categories.Find(post.CategoryId);
                List <Tag> tags= context.Tags.Where(a => post.TagList.Contains(a.Id)).ToList();
                post.Tags = tags;
                context.Posts.AddOrUpdate(a => a.Id, post);
                context.SaveChanges();
                transactionScope.Complete();
                return post.Id;
            }
        }

        public Category Category(int id)
        {
            return context.Categories.FirstOrDefault(t => t.Id == id);
        }

        public Tag Tag(int id)
        {
            return context.Tags.FirstOrDefault(t => t.Id == id);
        }

        public void EditPost(Post post)
        {
            using (var transactionScope = new TransactionScope())
            {
                context.Posts.AddOrUpdate(p => p.Id, post);
                context.SaveChanges();
                transactionScope.Complete();
            }
        }

        public void DeletePost(int id)
        {
            using (var transactionScope = new TransactionScope())
            {
                var post = context.Posts.Find(id);
                context.Posts.Remove(post);
                context.SaveChanges();
                transactionScope.Complete();
            }
        }

        public int AddCategory(Category category)
        {
            using (var transactionScope = new TransactionScope())
            {
                context.Categories.AddOrUpdate(a => a.Id, category);
                context.SaveChanges();
                transactionScope.Complete();
                return category.Id;
            }
        }

        public void EditCategory(Category category)
        {
            using (var transactionScope = new TransactionScope())
            {
                context.Categories.AddOrUpdate(c => c.Id, category);
                context.SaveChanges();
                transactionScope.Complete();
            }
        }

        public void DeleteCategory(int id)
        {
            using (var transactionScope = new TransactionScope())
            {
                Category category = context.Categories.Find(id);
                category.IsDeleted = true;
                context.SaveChanges();
                transactionScope.Complete();
            }
        }

        public int AddTag(Tag tag)
        {
            using (var transactionScope = new TransactionScope())
            {
                context.Tags.AddOrUpdate(a => a.Id, tag);
                context.SaveChanges();
                transactionScope.Complete();
                return tag.Id;
            }
        }

        public void EditTag(Tag tag)
        {
            using (var transactionScope = new TransactionScope())
            {
                context.Tags.AddOrUpdate(t => t.Id, tag);
                context.SaveChanges();
                transactionScope.Complete();
            }
        }

        public void DeleteTag(int id)
        {
            using (var transactionScope = new TransactionScope())
            {
                Tag tag = context.Tags.Find(id);
                tag.IsDeleted = true;
                context.SaveChanges();
                transactionScope.Complete();
            }
        }


        public IList<Post> GetPosts(long userId)
        {
            var query = context.Posts
                      .Include("Tags")
                      .Include("Category")
                      .Where(p => p.Published && p.PostedBy == userId)
                      .OrderByDescending(p => p.PostedOn);


            return query.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="postedBy"></param>
        /// <returns></returns>
        public Post GetPostbyId(int postId, long postedBy)
        {
            try
            {
                return context.Posts
                     .Include("Tags")
                     .Include("Category")
                     .FirstOrDefault(p => p.Id == postId && p.PostedBy == postedBy);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}

﻿using StudentTracker.Controllers;
using StudentTracker.Core.Components;
using StudentTracker.Core.Components.Interfaces;
using StudentTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Areas.SAS.Controllers
{
    public class BlogController : BaseController
    {
        private readonly IBlogComponent _blogRepository;

        public BlogController()
        {
            _blogRepository = new BlogComponent();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Posts");
        }

        public ViewResult Posts(int p = 1)
        {
            // pick latest 10 posts
            var viewModel = new ListViewModel(_blogRepository, p);

            ViewBag.Title = "Latest Posts";
            return View("List", viewModel);
        }

        public ActionResult List(ListViewModel viewModel)
        {
            return View(viewModel);
        }

        //
        // GET: /Blog/

        public ActionResult Category(string category, int p = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, category, "Category", p);

            if (viewModel.Category == null)
                throw new HttpException(404, "Category not found");

            ViewBag.Title = String.Format(@"Latest posts on category ""{0}""",
                                viewModel.Category.Name);
            return View("List", viewModel);
        }


        public ViewResult Tag(string tag, int p = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, tag, "Tag", p);

            if (viewModel.Tag == null)
                throw new HttpException(404, "Tag not found");

            ViewBag.Title = String.Format(@"Latest posts tagged on ""{0}""",
                viewModel.Tag.Name);
            return View("List", viewModel);
        }

        public ViewResult Search(string s, int p = 1)
        {
            ViewBag.Title = String.Format(@"Lists of posts found 
                        for search text ""{0}""", s);

            var viewModel = new ListViewModel(_blogRepository, s, "Search", p);
            return View("List", viewModel);
        }

        public ViewResult Post(int year, int month, string title)
        {
            var post = _blogRepository.Post(year, month, title);

            if (post == null)
                throw new HttpException(404, "Post not found");

            if (post.Published == false && User.Identity.IsAuthenticated == false)
                throw new HttpException(401, "The post is not published");

            return View(post);
        }

        [ChildActionOnly]
        public PartialViewResult Sidebars()
        {
            var widgetViewModel = new WidgetViewModel(_blogRepository);
            return PartialView("_Sidebars", widgetViewModel);
        }

    }
}

using StudentTracker.Core.Components.Interfaces;
using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentTracker.ViewModels
{
    public class WidgetViewModel
    {
        public WidgetViewModel(IBlogComponent blogRepository)
        {
            Categories = blogRepository.Categories();
            Tags = blogRepository.Tags();
            LatestPosts = blogRepository.Posts(0, 10);
        }

        public IList<Category> Categories { get; private set; }
        public IList<Tag> Tags { get; private set; }

        public IList<Post> LatestPosts { get; private set; }
    }
}
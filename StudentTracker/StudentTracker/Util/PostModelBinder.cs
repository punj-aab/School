using StudentTracker.Core.Components;
using StudentTracker.Core.Components.Interfaces;
using StudentTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Util
{
    public class PostModelBinder : DefaultModelBinder
    {
        private readonly IBlogComponent _blogRepository;

        public PostModelBinder()
        {
            _blogRepository = new BlogComponent();
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var post = (Post)base.BindModel(controllerContext, bindingContext);

           // var _blogRepository = _kernel.Get<IBlogComponent>();

            //if (post.Category != null)
            //    post.Category = _blogRepository.Category(post.Category.Id);

            var tags = bindingContext.ValueProvider.GetValue("Tags").AttemptedValue.Split(',');

            if (tags.Length > 0)
            {
                post.Tags = new List<Tag>();

                foreach (var tag in tags)
                {
                    post.Tags.Add(_blogRepository.Tag(int.Parse(tag.Trim())));
                }
            }

            if (bindingContext.ValueProvider.GetValue("oper").AttemptedValue.Equals("edit"))
                post.Modified = DateTime.UtcNow;
            else
                post.PostedOn = DateTime.UtcNow;

           // _blogRepository.Dispose();
            return post;
        }
    }
}
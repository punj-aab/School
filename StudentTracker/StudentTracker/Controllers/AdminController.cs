using Newtonsoft.Json;
using StudentTracker.Core.Components;
using StudentTracker.Core.Components.Interfaces;
using StudentTracker.Core.Entities;
using StudentTracker.Util;
using StudentTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace StudentTracker.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IBlogComponent _blogRepository;

        public AdminController()
        {
            _blogRepository = new BlogComponent();
        }


        #region Blog1
        /// <summary>
        /// This function return manage blog view.
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageBlog()
        {
            try
            {
                return View();

            }

            catch (Exception ex)
            {
                //CatchException.CatchTheException(ex);
                throw;
            }
            finally
            {

            }


        }

        #region PostBlog

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ContentResult GetPosts()
        {
            try
            {
                var posts = _blogRepository.GetPosts(_userStatistics.UserId);
                return Content(JsonConvert.SerializeObject(new
                {
                    rows = posts,
                }, new CustomDateTimeConverter()), "application/json");
            }

            catch (Exception ex)
            {
                //CatchException.CatchTheException(ex);
                throw;
            }
            finally
            {

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddEditPost(int postId = -1)
        {
            try
            {
                Post model = null;
                if (postId == -1)
                {
                    model = new Post();
                    ViewBag.Categoris = new SelectList(_blogRepository.Categories(), "Id", "Name");
                    ViewBag.Tags = new MultiSelectList(_blogRepository.Tags(), "Id", "Name");
                }
                else
                {
                    model = _blogRepository.GetPostbyId(postId, _userStatistics.UserId);
                    ViewBag.Categoris = new SelectList(_blogRepository.Categories(), "Id", "Name", model.Category.Id);
                    ViewBag.Tags = new MultiSelectList(_blogRepository.Tags(), "Id", "Name", model.Tags.Select(a => a.Id).ToList());
                }


                return PartialView("../Blog/_AddEdit_Post", model);

            }

            catch (Exception ex)
            {
                //CatchException.CatchTheException(ex);
                throw;
            }
            finally
            {

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditPost(Post model)
        {
            try
            {
                model.PostedBy = _userStatistics.UserId;
                model.PostedOn = DateTime.Now;
                int id = _blogRepository.AddPost(model);
                return Json(id, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                //CatchException.CatchTheException(ex);
                throw;
            }
            finally
            {

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public JsonResult DeletePost(int postId)
        {
            try
            {

                _blogRepository.DeletePost(postId);
                return Json(true, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                //CatchException.CatchTheException(ex);
                throw;
            }
            finally
            {

            }
        }
        #endregion

        #region Categories

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ContentResult GetCategories()
        {
            try
            {
                var categories = _blogRepository.Categories();
                return Content(JsonConvert.SerializeObject(new
                {
                    rows = categories,
                }, new CustomDateTimeConverter()), "application/json");
            }

            catch (Exception ex)
            {
                //CatchException.CatchTheException(ex);
                throw;
            }
            finally
            {

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddEditCategory(int categoryId = -1)
        {
            try
            {
                Category model = null;
                if (categoryId == -1)
                {
                    model = new Category();
                }
                else
                {
                    model = _blogRepository.Category(categoryId);
                }
                return PartialView("../Blog/_AddEdit_Category", model);

            }

            catch (Exception ex)
            {
                //CatchException.CatchTheException(ex);
                throw;
            }
            finally
            {

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditCategory(Category model)
        {
            try
            {

                int id = _blogRepository.AddCategory(model);
                return Json(id, JsonRequestBehavior.AllowGet);
                //return PartialView("../Blog/_AddEdit_Category", model);

            }

            catch (Exception ex)
            {
                //CatchException.CatchTheException(ex);
                throw;
            }
            finally
            {

            }

        }

        public JsonResult DeleteCategory(int categoryId)
        {
            try
            {

                _blogRepository.DeleteCategory(categoryId);
                return Json(true, JsonRequestBehavior.AllowGet);

            }

            catch (Exception ex)
            {
                //CatchException.CatchTheException(ex);
                throw;
            }
            finally
            {

            }
        }

        #endregion

        #region Tags

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ContentResult GetTags()
        {
            try
            {
                var tags = _blogRepository.Tags();
                return Content(JsonConvert.SerializeObject(new
                {
                    rows = tags,
                }, new CustomDateTimeConverter()), "application/json");
            }

            catch (Exception ex)
            {
                //CatchException.CatchTheException(ex);
                throw;
            }
            finally
            {

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddEditTag(int tagId = -1)
        {
            try
            {
                Tag model = null;
                if (tagId == -1)
                {
                    model = new Tag();
                }
                else
                {
                    model = _blogRepository.Tag(tagId);
                }
                return PartialView("../Blog/_AddEdit_Tag", model);

            }

            catch (Exception ex)
            {
                //CatchException.CatchTheException(ex);
                throw;
            }
            finally
            {

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditTag(Tag model)
        {
            try
            {
                _blogRepository.AddTag(model);
                return Json("", JsonRequestBehavior.AllowGet);
                //return PartialView("../Blog/_AddEdit_Tag", model);

            }

            catch (Exception ex)
            {
                //CatchException.CatchTheException(ex);
                throw;
            }
            finally
            {

            }

        }

        public JsonResult DeleteTag(int tagId)
        {
            try
            {
                _blogRepository.DeleteTag(tagId);
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                //CatchException.CatchTheException(ex);
                throw;
            }
            finally
            {

            }

        }

        #endregion

        #endregion

    }
}

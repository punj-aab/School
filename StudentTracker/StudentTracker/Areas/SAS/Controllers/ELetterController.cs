using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentTracker.Repository;
using StudentTracker.Core.Entities;
using StudentTracker.Controllers;
using System.Text.RegularExpressions;
namespace StudentTracker.Areas.SAS.Controllers
{
    public class ELetterController : BaseController
    {
        //
        // GET: /ELetter/
        StudentRepository repository = new StudentRepository();
        public ActionResult ViewEletters()
        {
            List<ELetter> objEletterList = repository.GetEletters(_userStatistics.UserId);
            return View(objEletterList);
        }

        public ActionResult UserEletter(long eLetterId)
        {
            ELetter objEletter = repository.GetEletters(_userStatistics.UserId, eLetterId);
            string input = objEletter.TemplateText;
            var regex = new Regex("{.*?}");
            var matches = regex.Matches(input); //your matches: name, name@gmail.com
            //List<FormattingField> formattingFieldList = repository.FormattingFields(objEletter.TemplateId);
            foreach (var match in matches) // e.g. you can loop through your matches like this
            {
                //yourmatch
                objEletter.TemplateText = objEletter.TemplateText.Replace(match.ToString(), User.Identity.Name);
            }
            return View(objEletter);
        }
    }
}

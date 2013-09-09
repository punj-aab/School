using System.Web.Mvc;

namespace StudentTracker.Areas.SAS
{
    public class SASAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SAS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SAS_default",
                "SAS/{controller}/{action}/{id}",
                new {  controller = "sashome", action = "Index", id = UrlParameter.Optional }

            );
        }
    }
}

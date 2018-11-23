using System.Collections.Generic;
using System.Web.Mvc;

namespace Quizzard.Web.App_Data
{
    public class QuestionViewEngine : RazorViewEngine
    {
        public QuestionViewEngine(string controllerName)
        {
            controllerName = controllerName.Replace("Controller", "");
            AddViewLocationFormat("~/Views/" + controllerName + "/{0}.cshtml");
            AddPartialViewLocationFormat("~/Views/Shared/" + controllerName + "/{0}.cshtml");
        }

        public void AddViewLocationFormat(string paths)
        {
            var existingPaths = new List<string>(ViewLocationFormats);
            existingPaths.Add(paths);

            ViewLocationFormats = existingPaths.ToArray();
        }

        public void AddPartialViewLocationFormat(string paths)
        {
            var existingPaths = new List<string>(PartialViewLocationFormats);
            existingPaths.Add(paths);

            PartialViewLocationFormats = existingPaths.ToArray();
        }

    }
}
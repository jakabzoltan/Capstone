using System;
using System.IO;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Quizzard.Web.Models
{
    public class ViewWrapper
    {
        public RazorView View { get; set; }
        public object Model { get; set; }
        public ViewWrapper(RazorView view, object model)
        {
            View = view;
            Model = model;
        }

    }
}
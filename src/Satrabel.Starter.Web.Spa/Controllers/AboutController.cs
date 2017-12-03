using Abp.AspNetCore.Mvc.Authorization;
using Satrabel.OpenApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Abp.AspNetCore.EmbeddedResources;
using Abp.Resources.Embedded;

namespace Satrabel.Starter.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : StarterControllerBase
    {

        private readonly IEmbeddedResourceManager _fileProvider;

        public AboutController(IEmbeddedResourceManager manager)
        {
            _fileProvider = manager;
        }

        public ActionResult Index()
        {
            var contents = _fileProvider.GetResources("/Views/");
            return View(contents);
        }
	}
}
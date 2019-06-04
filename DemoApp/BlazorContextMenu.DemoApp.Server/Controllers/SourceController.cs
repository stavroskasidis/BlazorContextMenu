using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BlazorContextMenu.DemoApp.Server.Controllers
{
    [Route("api/[controller]")]
    public class SourceController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SourceController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("{*path}")]
        public ActionResult<string> Get(string path)
        {
            var mappedPath = Path.Combine(_hostingEnvironment.ContentRootPath, "ClientSources", path.Replace("_dot_", "."));

            if (!System.IO.File.Exists(mappedPath))
            {
                return NotFound();
            }

            var source = System.IO.File.ReadAllText(mappedPath);
            return source;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace web_socket_server.Controllers
{
     
    public class PagesController : Controller
    {
        [Route("mainpage")]
        public  ActionResult pg()
        {
            return View("socketpage");
        }
    }
}

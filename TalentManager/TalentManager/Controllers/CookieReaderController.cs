using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace TalentManager.Controllers
{
    public class CookieReaderController : Controller
    {
        //
        // GET: /CookieReader/

        public ActionResult Read()
        {
            // reading cookie from webclient component
            string url = "http://localhost:5214/api/employees/12345";
            CookieWebClient client = new CookieWebClient()
            {
                Proxy = new WebProxy("localhost", 8888) 
                {
                    Credentials = CredentialCache.DefaultCredentials
                } // Fiddler
            };
            Console.WriteLine(client.DownloadString(url)); // Cookie is created here
            Console.WriteLine(client.DownloadString(url));
            return View();
        }

    }
}

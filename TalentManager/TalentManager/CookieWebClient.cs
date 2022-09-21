using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace TalentManager
{
    public class CookieWebClient : WebClient
    {       
        private CookieContainer jar = new CookieContainer();
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            HttpWebRequest webRequest = request as HttpWebRequest;
            if (webRequest != null)
                webRequest.CookieContainer = jar;
            return request;
        }   
    }
}
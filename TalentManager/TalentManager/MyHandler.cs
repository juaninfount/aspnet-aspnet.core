using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TalentManager
{
    public class MyHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
        {
            // Inspect and do your stuff with request here
            // If you are not happy for any reason,
            // you can reject the request right here like this
            bool isBadRequest = true;
            if (isBadRequest) {             
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("Error en myhandler"));
                //return request.CreateResponse(HttpStatusCode.BadRequest);
            }
                          
            // Inspect and do your stuff with response here
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
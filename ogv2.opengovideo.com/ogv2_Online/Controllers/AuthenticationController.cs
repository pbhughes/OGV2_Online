using ogv2_Online.Models;
using ogv2_Online.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Security;
using System.Security.Principal;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ogv2_Online.Controllers
{
    public class AuthenticationController : ApiController
    {
        IDataRepository repo;
        

        public AuthenticationController()
        {
            repo = new OGVRepository();
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Authenticate([FromBody]AuthenticationRequest authReq)
        {
            try
            {
                if (authReq == null)
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "No Credentials");

                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Credentials");
                }

                User possibleUser = await repo.Authenticate(authReq);

                if (possibleUser == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Credentials");
                }
                
                HttpResponseMessage retVal = Request.CreateResponse<User>(HttpStatusCode.OK, possibleUser);
                retVal.Headers.Add("OGVSession", possibleUser.SessionGuid);
                CookieHeaderValue cookie = new CookieHeaderValue("OGVSession", possibleUser.UserName);
                
                retVal.Headers.AddCookies(new List<CookieHeaderValue>() { cookie });
                return retVal;
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }

        }

        [HttpPost]
        public async Task<HttpResponseMessage> LogOff()
        {
            try
            {
                var result = await repo.LogOff(Request.Headers.GetValues("OGVSession").First());

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }

        [HttpPost]
        public async Task<DateTime> Ping()
        {
            return await Task.Run(  () =>
            {
                return DateTime.Now;
            });
        }
    }
}

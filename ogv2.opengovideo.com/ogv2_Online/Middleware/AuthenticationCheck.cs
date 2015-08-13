using Microsoft.Owin;
using ogv2_Online.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ogv2_Online.Middleware
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class AuthenticationCheck : Microsoft.Owin.OwinMiddleware
    {
        private AppFunc _next = null;
        private IAuthenticationRepository _repo = null;
        private const string OGV_SESSION = "OGVSession";
        private string REQUEST_PATH = "owin.RequestPath";

        public AuthenticationCheck(OwinMiddleware next)
            : base(next)
        {
            _repo = new OGVAuthenticationRepository();
        }

        public async override Task Invoke(IOwinContext context)
        {
            string path = context.Environment[REQUEST_PATH].ToString();
            var response = context.Response;

            if (path == "/api/Authentication/Authenticate")

                await Next.Invoke(context);
            else
            {
                if (!context.Request.Headers.ContainsKey(OGV_SESSION))
                {
                    response.OnSendingHeaders(state =>
                    {
                        var resp = (OwinResponse)state;
                        resp.StatusCode = 406;
                        resp.ReasonPhrase = "Auth token header missing"; 
                    }, response);

                    return;
                }

                if ( string.IsNullOrEmpty(context.Request.Headers["OGVSession"]))
                {
                    response.OnSendingHeaders(state =>
                    {
                        var resp = (OwinResponse)state;
                        resp.StatusCode = 401;
                        resp.ReasonPhrase = "Token is invalid"; 
                    }, response);

                    return;
                }

                string headerValue = context.Request.Headers["OGVSession"];
                Guid key;
                bool canParse = Guid.TryParse(headerValue, out key);
                if (!canParse)
                {
                    response.OnSendingHeaders(state =>
                    {
                        var resp = (OwinResponse)state;
                        resp.StatusCode = 406;
                        resp.ReasonPhrase = "Token is invalid";
                    }, response);
                    return;
                }
                else
                {
                    int valid = await _repo.SessionIsValid(key.ToString());
                    if (valid == -1)
                    {
                        response.OnSendingHeaders(state =>
                        {
                            var resp = (OwinResponse)state;
                            resp.StatusCode = 403;
                            resp.ReasonPhrase = "Token has  expired";
                        }, response);
                        return;
                    }
                    if( valid == 0)
                    {
                        response.OnSendingHeaders(state =>
                        {
                            var resp = (OwinResponse)state;
                            resp.StatusCode = 401;
                            resp.ReasonPhrase = "Must login";
                        }, response);
                        return;
                    }

                    await Next.Invoke(context);
                }
            }
        }
    }
}
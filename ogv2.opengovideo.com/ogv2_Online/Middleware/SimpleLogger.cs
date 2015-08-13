using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ogv2_Online.Repositories;
using System.IO;
using System.Web;
using Microsoft.Owin;
using System.Text;

namespace ogv2_Online.Middleware
{

    using AppFunc = Func<IDictionary<string, object>, Task>;


    public class SimpleLogger : Microsoft.Owin.OwinMiddleware
    {

        AppFunc _next = null;
        IAuthenticationRepository _repo = null;
        string REMOTE_IP = "server.RemoteIpAddress";
        string REQUEST_BODY = "owin.RequestBody";
        string REQUEST_PATH = "owin.RequestPath";
        string REQUEST_METHOD = "owin.RequestMethod";
        string RESPONSE_CODE = "owin.ResponseStatusCode";
        string RESPONSE_BODY = "owin.ResponseBody";
        string RESPONSE_REASON_PHRASE = "owin.ResponseReasonPhrase";

        public SimpleLogger(OwinMiddleware next)
            : base(next)
        {
            _repo = new OGVAuthenticationRepository();
        }


        public override async Task Invoke(Microsoft.Owin.IOwinContext context)
        {
            try
            {
                Stream oldResponseBody;
                IAuthenticationRepository repo = new OGVAuthenticationRepository();
                string ip = context.Request.RemoteIpAddress;

                //get the response body and put it back for the downstream items to read
                string reqBody = new StreamReader(context.Request.Body).ReadToEnd();
                byte[] requestData = Encoding.UTF8.GetBytes(reqBody);
                context.Request.Body = new MemoryStream(requestData);

                //read the path and the request method
                string reqPath = context.Request.Path.ToUriComponent();
                string reqMethod = context.Request.Method;

                //buffer the response stream
                oldResponseBody = context.Response.Body;
                //create a memory stream replacement
                MemoryStream replacement = new MemoryStream();
                //assing the replacement to the resonse body
                context.Response.Body = replacement;

                //Do the next thing
                await Next.Invoke(context);

                //grab the response code
                int responseCode = context.Response.StatusCode;

                //grab the response stream buffer
                //move to the beginning
                //copy it to the old response stream 
                replacement.Seek(0, SeekOrigin.Begin);

                //**********************Put the response back on the request body - this sends it down stream to the caller!
                await replacement.CopyToAsync(oldResponseBody);

                //move back to the head of the memory stream
                replacement.Seek(0, SeekOrigin.Begin);
                //read the response for logging
                StreamReader sr = new StreamReader(replacement);
                string responseBody = sr.ReadToEnd();

                //grab the response phrase
                string responseReasonPhrase = context.Response.ReasonPhrase;
                _repo.LogRequest(ip, reqBody, reqPath, reqMethod, context.Response.StatusCode, responseReasonPhrase, responseBody);
            }
            catch (Exception ex)
            {

                throw;

            }



        }
    }
}
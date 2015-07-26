using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using OGV2_Online.Repositories;

namespace OGV2_Online.Middleware
{
    
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using System.IO;
    using System.Web;
using Microsoft.Owin;

    public class SimpleLogger : Microsoft.Owin.OwinMiddleware
    {

        AppFunc _next = null;
        IDataRepository _repo = null;
        string REMOTE_IP = "server.RemoteIpAddress";
        string REQUEST_BODY = "owin.RequestBody";
        string REQUEST_PATH = "owin.RequestPath";
        string REQUEST_METHOD = "owin.RequestMethod";
        string RESPONSE_CODE = "owin.ResponseStatusCode";
        string RESPONSE_BODY = "owin.ResponseBody";
        string RESPONSE_REASON_PHRASE = "owin.ResponseReasonPhrase";

        public SimpleLogger(OwinMiddleware next):base( next )
        {
            _repo = new OGVRepository();
        }





        public override async Task Invoke(Microsoft.Owin.IOwinContext context)
        {
            IDataRepository repo = new OGVRepository();
            string ip = context.Request.RemoteIpAddress;
            var reqBodyStream = context.Request.Body;
            string reqBody = string.Empty;
            using (StreamReader sr = new StreamReader(reqBodyStream))
            {
                reqBody = sr.ReadToEnd();
            }



            string reqPath = context.Request.Path.ToUriComponent();
            string reqMethod = context.Request.Method;


            // Buffer the response
            var responseBodyStream = context.Response.Body;
            var buffer = new MemoryStream();
            context.Response.Body = buffer;

             await Next.Invoke(context);

            int responseCode = context.Response.StatusCode;

            //grab the response stream buffer and read from the beginning
            var reader = new StreamReader(buffer);
            buffer.Seek(0, SeekOrigin.Begin);
            string responseBody = reader.ReadToEnd();

            //put it all back together for down stream access and client access
            buffer.Seek(0, SeekOrigin.Begin);
            buffer.CopyTo(responseBodyStream);

            string responseReasonPhrase = context.Response.ReasonPhrase;
            _repo.LogRequest(ip, reqBody, reqPath, reqMethod, responseCode, responseReasonPhrase, responseBody);


        }
    }
}
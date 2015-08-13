using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ogv2_Online.Exentions
{
    public static class HttpRequestExtensions
    {
        public static string GetSessionValue(this HttpRequestMessage Request){
            return Request.Headers.GetValues("OGVSession").First();
        }
    }
}
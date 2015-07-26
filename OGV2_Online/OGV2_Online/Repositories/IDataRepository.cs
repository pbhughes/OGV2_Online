using OGV2_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGV2_Online.Repositories
{
    public interface IDataRepository
    {
        void LogRequest(string remoteIP, string requestBody, string requestPath, string requestMethod, int responseCode, string responseReasonPhrase, string responseBody);
        User Authenticate(AuthenticationRequest authRequest);

    }

     
}

using ogv2_Online.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ogv2_Online.Repositories
{
    public interface IDataRepository
    {
        void LogRequest(string remoteIP, string requestBody, string requestPath, string requestMethod, int responseCode, string responseReasonPhrase, string responseBody);
        Task<User> Authenticate(AuthenticationRequest authRequest);
        Task<bool> LogOff(string sessionKey);
        Task<bool> SessionIsValid(string sessionKey);
    }


}

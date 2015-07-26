using OGV2_Online.Models;
using OGV2_Online.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OGV2_Online.Controllers
{
    public class AuthenticationController : ApiController
    {
        IDataRepository repo;

        public AuthenticationController()
        {
            repo = new OGVRepository();
        }

        [HttpPost]
        public User Authenticate(AuthenticationRequest authReq)
        {
            return repo.Authenticate(authReq);
        }
    }
}

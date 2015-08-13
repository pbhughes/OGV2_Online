using ogv2_Online.Models;
using ogv2_Online.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ogv2_Online.Exentions;

namespace ogv2_Online.Controllers
{
    public class BoardController : ApiController
    {
        IDataRepository repo;

        public BoardController()
        {
            repo = new OGVDataRepository();
        }
        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {

            try
            {
                var retval = await repo.GetBoards(Request.GetSessionValue());
                return Request.CreateResponse(HttpStatusCode.OK, retval);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            
        }
    }
}

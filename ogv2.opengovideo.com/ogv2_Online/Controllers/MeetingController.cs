using ogv2_Online.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ogv2_Online.Exentions;
using ogv2_Online.Models;

namespace ogv2_Online.Controllers
{
    public class MeetingController : ApiController
    {
        IDataRepository repo;

        public MeetingController()
        {
            repo = new OGVDataRepository();
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetActiveMeetingList([FromUri] int boardID)
        {
            try
            {
                var sessionKey = Request.GetSessionValue();
                List<Meeting> meetings = new List<Meeting>();
                meetings = await repo.GetActiveMeetingList(sessionKey, boardID);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, meetings);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}

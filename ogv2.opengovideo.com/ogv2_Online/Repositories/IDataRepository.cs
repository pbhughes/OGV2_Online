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
        Task<List<Board>> GetBoards(string sessionKey);

        Task<List<Meeting>> GetActiveMeetingList(string sessionKey, int boardID);
    }
}

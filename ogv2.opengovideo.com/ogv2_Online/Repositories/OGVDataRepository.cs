using ogv2_Online.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace ogv2_Online.Repositories
{
    public class OGVDataRepository : IDataRepository
    {
        private string conString = string.Empty;

        public OGVDataRepository()
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["ogv_data"];
            if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
                throw new Exception("Fatal error: missing connecting string in web.config file");
            conString = mySetting.ConnectionString;
        }

        public async Task<List<Board>> GetBoards(string sessionKey)
        {
            string procName = "proc_Boards_Get";

            Task<List<Board>> t = Task.Run<List<Board>>(async () =>
            {

                SqlCommand cmd = null;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    await con.OpenAsync();
                    using (cmd = new SqlCommand(procName, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sessionKey", sessionKey);
                        List<Board> boards = new List<Board>();
                        var rdr = cmd.ExecuteReader();
                            if(rdr.HasRows){
                                
                                while(rdr.Read()){
                                    Board x = new Board(){
                                        UserID = int.Parse(rdr["UserID"].ToString()),
                                        BoardID = int.Parse(rdr["BoardID"].ToString()),
                                        BoardName = rdr["BoardName"].ToString(),
                                        VideoFolder = rdr["VideoFolder"].ToString()
                                    };
                                    boards.Add(x);
                                }
                            }
                            return boards;
                        
                    }

                }

            });
            return await t;
        }

        public async Task<List<Meeting>> GetActiveMeetingList(string sessionKey, int boardID)
        {
            string procName = "proc_Meetings_Get";

            Task<List<Meeting>> t = Task.Run<List<Meeting>>(async () =>
            {

                SqlCommand cmd = null;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    await con.OpenAsync();
                    using (cmd = new SqlCommand(procName, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sessionKey", sessionKey);
                        cmd.Parameters.AddWithValue("@boardID", boardID);
                        List<Meeting> meetings = new List<Meeting>();
                        var rdr = cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {

                            while (rdr.Read())
                            {
                                Meeting x = new Meeting()
                                {
                                    MeetingID = int.Parse(rdr["MeetingID"].ToString()),
                                    BoardID = int.Parse(rdr["BoardID"].ToString()),
                                    MeetingName = rdr["MeetingName"].ToString(),
                                    MeetingDate = DateTime.Parse(rdr["MeetingDate"].ToString()),
                                    RecordingURL = rdr["RecordingURL"].ToString()
                                };
                                meetings.Add(x);
                            }
                        }
                        return meetings;

                    }

                }

            });
            return await t;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ogv2_Online.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Net.Http;

namespace ogv2_Online.Repositories
{
    public class OGVAuthenticationRepository : IAuthenticationRepository
    {
        private string conString = string.Empty;

        public OGVAuthenticationRepository()
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["ogv_data"];
            if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
                throw new Exception("Fatal error: missing connecting string in web.config file");
            conString = mySetting.ConnectionString;

        }

        public async Task<User> Authenticate(AuthenticationRequest authRequest)
        {
            
            string procName = "proc_Users_Authenticate";

            Task<User> t = Task.Run<User>(async () =>
            {
                User currentUser = null;
                SqlCommand cmd = null;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    await con.OpenAsync();
                    using (cmd = new SqlCommand(procName, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userName", authRequest.UserName);
                        cmd.Parameters.AddWithValue("@password", authRequest.Password);
                        var rdr = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.Default);
                        if (rdr.HasRows)
                        {
                            currentUser = new User();

                            while (rdr.Read())
                            {
                                currentUser.UserID = int.Parse(rdr["UserID"].ToString());
                                currentUser.UserName = rdr["UserName"].ToString();
                                currentUser.SessionGuid = rdr["SessionGuid"].ToString();
                                Board x = new Board()
                                {
                                    BoardID = int.Parse(rdr["BoardID"].ToString()),
                                    BoardName = rdr["BoardName"].ToString(),
                                    UserID = int.Parse(rdr["UserID"].ToString())
                                };
                                currentUser.BoardList.Add(x);
                            }
                            
                        }
                    }
                   
                }

                return currentUser;

            });

            return await t;

        }

        public void LogRequest(string remoteIP, string requestBody, string requestPath, string requestMethod, int responseCode, string responseReasonPhrase, string responseBody)
        {
           

            try
            {
                

                SqlCommand cmd = null;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (cmd = new SqlCommand("proc_RequestMetric_Insert", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@remoteIP", remoteIP);
                        cmd.Parameters.AddWithValue("@requestBody", requestBody);
                        cmd.Parameters.AddWithValue("@requestPath", requestPath);
                        cmd.Parameters.AddWithValue("@requestMethod", requestMethod);
                        cmd.Parameters.AddWithValue("@responseCode", responseCode);
                        cmd.Parameters.AddWithValue("@responseReasonPhrase", responseReasonPhrase);
                        cmd.Parameters.AddWithValue("@responseBody", (responseBody.Length < 4098) ? responseBody : responseBody.Substring(0, 4098));
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<bool> LogOff(string sessionKey)
        {
            string procName = "proc_Sessions_Delete";
            Task<bool> t = Task.Run<bool>(async () =>
            {
                
                SqlCommand cmd = null;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    await con.OpenAsync();
                    using (cmd = new SqlCommand(procName, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sessionKey", sessionKey);
                        int count = await cmd.ExecuteNonQueryAsync();
                        if (count != 0)
                            return true;
                        else
                            return false;

                        
                    }

                }

            });

            return await t;
        }

        public async Task<int> SessionIsValid(string sessionKey)
        {
            string procName = "proc_Sessions_Select";

            Task<int> t = Task.Run<int>(async () =>
            {

                SqlCommand cmd = null;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    await con.OpenAsync();
                    using (cmd = new SqlCommand(procName, con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sessionKey", sessionKey);
                        int count = (int)await cmd.ExecuteScalarAsync();
                        return count;
                    }

                }

            });
            return await t;
        }

        
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VirusBlokAda.CC.DataBase
{
    internal sealed class ScanningObjectManager
    {
        private readonly String connectionString;
		
		#region Constructors
        public ScanningObjectManager(String connectionString)
		{
            this.connectionString = connectionString;
		}
		#endregion
		
		#region Methods

        /// <summary>
        /// Add comment in database
        /// </summary>
        internal void AddComment(ScanningObjectEntity entity)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("AddCommentByIP", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IP", entity.IPAddress);
                cmd.Parameters.AddWithValue("@Comment", entity.Comment);

                con.Open();
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Delete comment in database
        /// </summary>
        internal void DeleteComment(String ip)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("DeleteCommentByIP", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IP", ip);

                con.Open();
                cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Get comment by IPAddress
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        internal String GetComment(String ip)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetCommentByIP", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IP", ip);

                con.Open();
                Object res = cmd.ExecuteScalar();
                return res != null ? res.ToString() : String.Empty;
            }
        }
		
		#endregion
    }
}

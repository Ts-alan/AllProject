using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace VirusBlokAda.Vba32CC.DataBase
{
    public class ScanningObjectManager
    {
        VlslVConnection database; 
		
		#region Constructors
		public ScanningObjectManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        public ScanningObjectManager(VlslVConnection l_database)
		{
			database=l_database;
		}
		#endregion
		
		#region Methods

        /// <summary>
        /// Add comment in database
        /// </summary>
        public void AddComment(ScanningObjectEntity entity)
        {
            IDbCommand command = database.CreateCommand("AddCommentByIP", true);

            database.AddCommandParameter(command, "@IP",
                DbType.String, entity.IPAddress, ParameterDirection.Input);

            database.AddCommandParameter(command, "@Comment",
                DbType.String, entity.Comment, ParameterDirection.Input);
                        
            command.ExecuteScalar();
        }

        /// <summary>
        /// Delete comment in database
        /// </summary>
        public void DeleteComment(String ip)
        {
            IDbCommand command = database.CreateCommand("DeleteCommentByIP", true);

            database.AddCommandParameter(command, "@IP",
                DbType.String, ip, ParameterDirection.Input);

            command.ExecuteScalar();
        }

        /// <summary>
        /// Get comment by IPAddress
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public String GetComment(String ip)
        {
            IDbCommand command = database.CreateCommand("GetCommentByIP", true);

            database.AddCommandParameter(command, "@IP",
                DbType.String, ip, ParameterDirection.Input);
            Object res = command.ExecuteScalar();
            return res != null ? res.ToString() : String.Empty;
        }
		
		#endregion
    }
}

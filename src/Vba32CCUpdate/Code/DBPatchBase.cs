using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Vba32ControlCenterUpdate
{
    internal abstract class DBPatchBase: IPatchUpdate
    {
        #region Fields

        private readonly String _version;
        private readonly IPatchUpdate _previous;
        private readonly IPatchUpdate _next;

        #endregion

        #region Constructors

        internal DBPatchBase(String version)
            : this(version, null)
        {
        }

        internal DBPatchBase(String version, IPatchUpdate previous)
            : this(version, previous, null)
        {
        }

        internal DBPatchBase(String version, IPatchUpdate previous, IPatchUpdate next)
        {
            _version = version;
            _previous = previous;
            _next = next;
        }

        #endregion

        #region IPatchUpdate Members

        public String Version
        {
            get { return _version; } 
        }

        public Boolean Update(String currentVersion, String connectionString, out String errorVersion)
        {
            if (this.Version != currentVersion && _previous != null)
            {
                if (!_previous.Update(currentVersion, connectionString, out errorVersion))
                {
                    return false;
                }
            }

            //Update
            Logger.Debug("Updating DB script. Version: " + _version);

            SqlConnection con = null;
            try
            {
                con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(GetUpdateScript(), con);
                con.Open();

                //раскомментить!!!!!!
                //cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Logger.Error(String.Format("Could not update DB script (Version: {0}, Message: {1}).", _version, ex.Message));
                errorVersion = _version;
                return false;
            }
            finally
            {
                if (con != null)
                    con.Close();
            }

            Logger.Info("Update to version: " + _version);
            errorVersion = null;
            return true;
        }

        public Boolean Rollback(String oldVersion, String connectionString, out String errorVersion)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods

        protected virtual String GetUpdateScript()
        {
            Logger.Debug("GetUpdateScript() :: " + _version);
            return String.Empty;
        }

        protected virtual String GetRollbackScript()
        {
            Logger.Debug("GetRollbackScript() :: " + _version);
            return String.Empty;
        }

        #endregion
    }
}

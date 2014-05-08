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
        private readonly String _previousVersion;
        private readonly String _nextVersion;

        #endregion

        #region Constructors

        internal DBPatchBase(String version)
            : this(version, null)
        {
        }

        internal DBPatchBase(String version, String previousVersion)
            : this(version, previousVersion, null)
        {
        }

        internal DBPatchBase(String version, String previousVersion, String nextVersion)
        {
            _version = version;
            _previousVersion = previousVersion;
            _nextVersion = nextVersion;
        }

        #endregion

        #region IPatchUpdate Members

        public String Version
        {
            get { return _version; } 
        }

        public Boolean Update(String currentVersion, String connectionString, out String errorVersion)
        {
            if (this.Version != currentVersion && !String.IsNullOrEmpty(_previousVersion))
            {
                IPatchUpdate prev = DBPatchFactory.GetPatch(_previousVersion);
                if (_previousVersion != null)
                {
                    if (!prev.Update(currentVersion, connectionString, out errorVersion))
                    {
                        return false;
                    }
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

                cmd.ExecuteNonQuery();
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

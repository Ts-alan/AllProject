using System;
using System.Collections.Generic;
using System.Text;
using Vba32ControlCenterUpdate.DBPatches;

namespace Vba32ControlCenterUpdate
{
    internal static class DBPatchFactory
    {
        /// <summary>
        /// Get patch by version
        /// </summary>
        /// <param name="version">New version</param>
        /// <returns></returns>
        internal static IPatchUpdate GetPatch(String version)
        {
            if(String.IsNullOrEmpty(version))
                return null;

            Type typeDBPatch = Type.GetType("Vba32ControlCenterUpdate.DBPatches.DBPatch_" + version.Replace('.', '_'));
            if (typeDBPatch == null)
            {
                Logger.Error("GetPatch() :: Can't get Type for version: " + version);
                return null;
            }

            IPatchUpdate patch = null;
            try
            {
                patch = (IPatchUpdate)Activator.CreateInstance(typeDBPatch);
            }
            catch (Exception ex)
            {
                Logger.Error(String.Format("GetPatch() :: Can't create instance for version: {0} ({1})", version, ex.Message));
            }

            return patch;
        }
    }
}

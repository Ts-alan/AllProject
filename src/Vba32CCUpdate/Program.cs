using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace Vba32ControlCenterUpdate
{
    class Program
    {
        static Int32 Main(string[] args)
        {
            Logger.User = "Vba32CCUpdate";
            Logger.Path = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName + @"\Vba32CCUpdate.log";

            Logger.Debug("Starting...");

            if (args.Length < 4)
            {
                Logger.Fatal("Wrong count of arguments.");
                return 0;
            }

            Logger.Debug("Arguments(args[0]: " + args[0] + "; args[1]: " + args[1] + "; args[2]: " + args[2] + "; args[3]: " + args[3] + ")");

            #region Get params

            EventEnum eventName;
            String currentVersion;
            String newVersion;
            String[] files;
            try
            {
                eventName = EventEnumExtensions.Get(args[0]);
                currentVersion = ParseVersion(args[1]);
                newVersion = ParseVersion(args[2]);
                files = args[3].Split(new Char[] { '|' });                
            }
            catch(Exception e)
            {
                Logger.Fatal(String.Format("Wrong argument type ({0}).", e.Message));
                return 0;
            }

            #endregion

            switch (eventName)
            {
                case EventEnum.ActionBeforeReplaceFiles:
                    UpdateActions.ActionBeforeReplaceFiles(files);
                    break;
                case EventEnum.ActionAfterReplaceFiles:
                    UpdateActions.ActionAfterReplaceFiles(files, currentVersion, newVersion);
                    break;
                default:
                    return 0;
            }

            Logger.Debug("Finished: success.");
            return 0;
        }

        private static String ParseVersion(String version)
        {
            /*
                    Incorrect work when major version > 9
                    Correct format:
                      *   <version> = Major.Minor.Build.Revision
                      *   Major =[0, 9]
                      *   Minor =[0, 999]
                      *   Build =[0, 900]
                      *   Revision =[0, 900]
                      *   "<version> after"
                      *   "before <version> after"
                      *   "before <version>"
            */
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^.*(?<major>\d{1,3})\.(?<minor>\d{1,3})\.(?<build>\d{1,3})\.(?<revision>\d{1,3}).*$");
            System.Text.RegularExpressions.Match match = reg.Match(version);

            if (!match.Success)
                throw new Exception("Parse version is invalid. Input value: " + version);

            return String.Format("{0}.{1}.{2}.{3}", match.Groups[1].Value, match.Groups[2].Value, match.Groups[3].Value, match.Groups[4].Value);
        }
    }
}

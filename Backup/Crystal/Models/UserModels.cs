using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Profile;

namespace Crystal.Models
{
    public class UserProfile :ProfileBase
    {
        public PersonalData PersonalData
        {
            get { return (PersonalData) GetPropertyValue("PersonalData"); }
        }

        public List<WatchedPart> WatchedPart
        {
            get { return (List<WatchedPart>)GetPropertyValue("WatchedPart"); } 
        }

        /// <summary>
        /// Получаем профиль залогиненого пользователя
        /// </summary>
       public static UserProfile GetProfile()
       {
           return (UserProfile)HttpContext.Current.Profile;
       }

        /// <summary>
        /// Получаем профиль указанного пользователя
        /// </summary>
        public static UserProfile GetProfile(string username)
        {
            return (UserProfile)Create(username);
        }
    }

    [Serializable]
    public class PersonalData
    {
        public string UserName { get; set; }
        public string Fio { get; set; }
        public string Unit { get; set; }
    }

    [Serializable]
    public class WatchedPart
    {
        public string Nprt { get; set; }
        public string Plant { get; set; }

        public WatchedPart(){}

        public WatchedPart(string nprt, string plant)
        {
            Nprt = nprt;
            Plant = plant;
        }
    }

    public class AddWatchedPart
    {
        public string Nprt { get; set; }
        public string Plant { get; set; }
        public string Username { get; set; }

        public AddWatchedPart() { }

        public AddWatchedPart(string nprt, string plant, string username)
        {
            Nprt = nprt;
            Plant = plant;
            Username = username;
        }
    }

    public class WatchedPartData
    {
        public string Nprt { get; set; }
        public string LastNop { get; set; }
        public string Kpls { get; set; }
        public string Plant { get; set; }
    }
}
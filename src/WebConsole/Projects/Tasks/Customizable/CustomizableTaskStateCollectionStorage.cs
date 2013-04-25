using System;
using System.Collections.Generic;
using System.Text;
using Common;
using System.Web;

namespace Tasks.Customizable
{
    internal class CustomizableTaskStateCollectionStorage
    {
        #region Settings
        private static readonly StorageTypeEnum storageType = StorageTypeEnum.Session;
        private static readonly string storageName = "TasksList";
        private static readonly string profileKey = "TasksList";
        #endregion

        #region Helper Properties
        private static string SessionStorageKey
        {
            get
            {
                return storageName;
            }
        }

        private static string ApplicationStorageKey
        {
            get
            {
                return String.Format("{0}_{1}", HttpContext.Current.User.Identity.Name,
                    storageName);
            }
        }
        #endregion

        #region Storage
        private static object Storage
        {
            get
            {
                if (storageType == StorageTypeEnum.Session)
                {
                    return HttpContext.Current.Session[SessionStorageKey];
                }
                else if (storageType == StorageTypeEnum.Application)
                {
                    return HttpContext.Current.Application[ApplicationStorageKey];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (storageType == StorageTypeEnum.Session)
                {
                    HttpContext.Current.Session[SessionStorageKey] = value;
                }
                else if (storageType == StorageTypeEnum.Application)
                {
                    HttpContext.Current.Application[ApplicationStorageKey] = value;
                }

            }
        }
        #endregion

        #region CompositeFilterStateCollection        
        private static CustomizableTaskStateCollection Collection
        {
            get
            {
                CustomizableTaskStateCollection collection = Storage as CustomizableTaskStateCollection;
                if (collection == null)
                {
                    string xml = HttpContext.Current.Profile.GetPropertyValue(profileKey) as String;
                    collection = CustomizableTaskStateCollection.Deserialize(xml);
                    Storage = collection;
                }
                return collection;
            }
        }

        private static void UpdateProfile(CustomizableTaskStateCollection collection)
        {
            HttpContext.Current.Profile.SetPropertyValue(profileKey, collection.Serialize());
        }
        #endregion

        #region Get/Update
        public static CustomizableTaskState Get(string taskType)
        {
            return Collection.Get(taskType);
        }

        public static void Update(CustomizableTaskState state)
        {
            Collection.Update(state);
            UpdateProfile(Collection);
        }
        #endregion
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CCP.WebApi.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Res {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Res() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CCP.WebApi.Resources.Res", typeof(Res).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user name or password is incorrect.
        /// </summary>
        internal static string AuthorizationError {
            get {
                return ResourceManager.GetString("AuthorizationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can only edit contracts in Draft status!.
        /// </summary>
        internal static string ContractEdition {
            get {
                return ResourceManager.GetString("ContractEdition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can&apos;t delete Sales Person who has attached contracts.
        /// </summary>
        internal static string DeletingSalesPersonWithCPR {
            get {
                return ResourceManager.GetString("DeletingSalesPersonWithCPR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client secret should be sent.
        /// </summary>
        internal static string EmptyClientSecret {
            get {
                return ResourceManager.GetString("EmptyClientSecret", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client is inactive.
        /// </summary>
        internal static string InactiveClient {
            get {
                return ResourceManager.GetString("InactiveClient", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client &apos;{0}&apos; is not registered in the system.
        /// </summary>
        internal static string InvalidClientId {
            get {
                return ResourceManager.GetString("InvalidClientId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client secret is invalid..
        /// </summary>
        internal static string InvalidClientSecret {
            get {
                return ResourceManager.GetString("InvalidClientSecret", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Model is invalid.
        /// </summary>
        internal static string InvalidModel {
            get {
                return ResourceManager.GetString("InvalidModel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Refresh token is issued to a different clientId.
        /// </summary>
        internal static string InvalidRefreshToken {
            get {
                return ResourceManager.GetString("InvalidRefreshToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Contract does not exist.
        /// </summary>
        internal static string NonexistentContract {
            get {
                return ResourceManager.GetString("NonexistentContract", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Token Id does not exist.
        /// </summary>
        internal static string TokenIdDoesNotExist {
            get {
                return ResourceManager.GetString("TokenIdDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sales person must have 3 different approvers.
        /// </summary>
        internal static string UserSaveErrorApprovers {
            get {
                return ResourceManager.GetString("UserSaveErrorApprovers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User with this Email is already exist.
        /// </summary>
        internal static string UserSaveErrorEmail {
            get {
                return ResourceManager.GetString("UserSaveErrorEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User was succesfuly saved.
        /// </summary>
        internal static string UserSaveOk {
            get {
                return ResourceManager.GetString("UserSaveOk", resourceCulture);
            }
        }
    }
}

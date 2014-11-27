using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.Profile;
using System.Reflection;
using System.Diagnostics;

/// <summary>
/// Summary description for UsersDataContainer
/// </summary>
public static class UsersDataContainer
{
    #region Data
    /// <summary>
    /// Получение списка пользователей
    /// </summary>
    /// <param name="SortExpression">выражение сортировки</param>
    /// <param name="maximumRows">максимальное количество рядов</param>
    /// <param name="startRowIndex">индекс начального ряда</param>
    /// <returns>список пользователей</returns>
    public static List<UserEntity> Get(string sortExpression, int maximumRows, int startRowIndex)
    {
        UpdateCollection();

        List<UserEntity> list = new List<UserEntity>();
        sortExpression += "";
        string[] parts = sortExpression.Split(' ');
        bool descending = false;
        string property = "";

        if (parts.Length > 0 && parts[0] != "")
        {
            property = parts[0];

            if (parts.Length > 1)
            {
                descending = parts[1].ToLower().Contains("esc");
            }
            PropertyInfo prop = typeof(UserEntity).GetProperty(property);
            if (prop == null)
            {
                throw new Exception("No property '" + property + "' in UserEntity");
            }
            if (descending)
            {
                listFull.Sort(delegate(UserEntity a, UserEntity b)
                {
                    return (prop.GetValue(b, null) as IComparable).CompareTo(prop.GetValue(a, null));
                });
            }
            else
            {
                listFull.Sort(delegate(UserEntity a, UserEntity b)
                {                    
                    return (prop.GetValue(a, null) as IComparable).CompareTo(prop.GetValue(b, null));
                });
            }
        }


        for (int i = startRowIndex; i < startRowIndex + maximumRows; i++)
        {
            if (i >= listFull.Count) break;
            list.Add(listFull[i]);
        }
        return list;      
    }
    /// <summary>
    /// Обновление коллекции
    /// </summary>
    private static void UpdateCollection()
    {
        listFull = new List<UserEntity>();
        foreach (MembershipUser next in Membership.GetAllUsers())
        {
            UserEntity user = new UserEntity();
            user.Login = next.UserName;
            ProfileCommon profile = defaultProfile.GetProfile(next.UserName);
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.Email = next.Email;
            user.LastLoginDate = next.LastLoginDate;
            user.CreationDate = next.CreationDate;
            if (Roles.GetRolesForUser(next.UserName).Length == 0)
                user.Role = "Not defined";
            else
            {
                user.Role = Roles.GetRolesForUser(next.UserName)[0];
            }
            listFull.Add(user);
        }
    }

    private static List<UserEntity> listFull = null;
    /// <summary>
    /// Количество пользователей
    /// </summary>
    /// <returns></returns>
    public static int Count()
    {
        return listFull.Count;
    }
    #endregion

    #region Create
    private static string GetMembershipCreateUserExceptionMessage(MembershipCreateStatus status)
    {
        switch (status)
        {
            case MembershipCreateStatus.DuplicateUserName:
                return Resources.Resource.DuplicateUserName;

            case MembershipCreateStatus.DuplicateEmail:
                return Resources.Resource.DuplicateEmail;

            case MembershipCreateStatus.InvalidPassword:
                return Resources.Resource.InvalidPassword;

            case MembershipCreateStatus.InvalidEmail:
                return Resources.Resource.InvalidEmail;

            case MembershipCreateStatus.InvalidUserName:
                return Resources.Resource.InvalidUserName;

            case MembershipCreateStatus.ProviderError:
                return Resources.Resource.ProviderError;

            case MembershipCreateStatus.UserRejected:
                return Resources.Resource.UserRejected;

            default:
                return Resources.Resource.UnknownError;
        }
    }
    /// <summary>
    /// Создание пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="password">пароль</param>
    /// <param name="email">email</param>
    /// <param name="error">сообщение об ошибке</param>
    /// <returns>успешность создания</returns>
    private static bool MembershipCreateUser(string username, string password, string email, out string error)
    {
        try
        {
            Membership.CreateUser(username, password, email);
            error = String.Empty;
            return true;
        }
        catch (MembershipCreateUserException ex)
        {
            error = GetMembershipCreateUserExceptionMessage(ex.StatusCode);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error creating user with membership: " + ex.Message);
            error = Resources.Resource.MembershipCreateUserFailed +
                Resources.Resource.UserRejected;
        }
        return false;
    }
    /// <summary>
    /// Добавление пользователя к роли
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="role">роль</param>
    /// <param name="error">сообщение об ошибке</param>
    /// <returns>успешность добавления</returns>
    private static bool AddUserToRole(string username, string role, out string error)
    {
        try
        {
            Roles.AddUserToRole(username, role);
            error = String.Empty;
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error adding user to role: " + ex.Message);
            error = Resources.Resource.CannotAddUserToRole +
                Resources.Resource.UserRejected;
        }
        return false;
    }
    /// <summary>
    /// удаление пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    private static void MembershipDeleteUser(string username)
    {
        try
        {
            Membership.DeleteUser(username);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error deleting user: " + ex.Message);
        }
    }
    /// <summary>
    /// Создание профиля пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="firstname">имя</param>
    /// <param name="lastname">фамилия</param>
    /// <param name="error">сообщение об ошибке</param>
    /// <returns>успешность создания</returns>
    private static bool CreateUserProfile(string username, string firstname, string lastname, out string error)
    {
        try
        {
            ProfileCommon profile = defaultProfile.GetProfile(username);
            profile.FirstName = firstname;
            profile.LastName = lastname;
            profile.Save();
            error = String.Empty;
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error setting profile: " + ex.Message);
            error = Resources.Resource.CannotSetUserProfile +
                Resources.Resource.UserRejected;
        }
        return false;
    }
    /// <summary>
    /// Создание пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="password">пароль</param>
    /// <param name="email">почта</param>
    /// <param name="firstname">имя</param>
    /// <param name="lastname">фамилия</param>
    /// <param name="role">роль</param>
    /// <param name="message">сообщение</param>
    /// <returns>успешность создания</returns>
    public static bool Create(string username, string password, string email, string firstname,
        string lastname, string role, out string message)
    {
        message = String.Empty;
        bool success = false;

        if (MembershipCreateUser(username, password, email, out message))
        {            
            //add user to role
            if (AddUserToRole(username, role, out message))
            {
                //edit profile
                if (CreateUserProfile(username, firstname, lastname, out message))
                {
                    //success
                    message = Resources.Resource.AcountCreate;
                    success = true;
                }
            }
            if (!success)
            {
                //rollback
                MembershipDeleteUser(username);
            }
        }
        return success;
    }
    #endregion

    #region Update
    /// <summary>
    /// обновление информации о пользователе
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="email">почта</param>
    /// <param name="firstname">имя</param>
    /// <param name="lastname">фамилия</param>
    /// <param name="role">роль</param>
    /// <param name="message">сообщение</param>
    /// <returns>успешность обновления</returns>
    public static bool Update (string username, string email, string firstname, string lastname,
        string role, out string message)
    {
        message = String.Empty;
        //get old info
        string emailOld = String.Empty;
        string roleOld = String.Empty;
        string firstnameOld = String.Empty;
        string lastnameOld = String.Empty;

        bool gotInfoSuccessfully = false;
        if (GetUserEmail(username, out emailOld, out message))
        {
            if (GetUserRole(username, out roleOld, out message))
            {
                if (GetUserProfile(username, out firstnameOld, out lastnameOld, out message))
                {
                    gotInfoSuccessfully = true;
                }
            }
        }

        if (!gotInfoSuccessfully)
        {
            return false;
        }

        //edit user

        bool changedMailSuccessfully = true;
        bool changedRoleSuccessfully = true;
        bool changedProfileSuccessfully = true;
        bool needToChangeMail = false;
        bool needToChangeRole = false;
        bool needToChangeProfile = false;

        needToChangeMail = (!email.Equals(emailOld));
        if (needToChangeMail)
        {
            changedMailSuccessfully = ChangeUserEmail(username, email, out message);
        }

        if (changedMailSuccessfully)
        {
            needToChangeRole = (!role.Equals(roleOld));
            if (needToChangeRole)
            {
                changedRoleSuccessfully = ChangeUserRole(username, role, out message);
            }
            if (changedRoleSuccessfully)
            {
                needToChangeProfile = (!firstname.Equals(firstnameOld) || !lastname.Equals(lastnameOld));
                if (needToChangeProfile)
                {
                    changedProfileSuccessfully = ChangeUserProfile(username, firstname, lastname, out message);
                }
            }
        }

        //rollback if subsequent editions failed
        if (!changedProfileSuccessfully)
        {
            if (needToChangeRole)
            {
                ChangeUserRoleSilent(username, roleOld);
            }
            if (needToChangeMail)
            {
                ChangeUserEmailSilent(username, emailOld);
            }
            return false;
        }

        if (!changedRoleSuccessfully)
        {
            if (needToChangeMail)
            {
                ChangeUserEmailSilent(username, emailOld);
            }
            return false;
        }

        if (!changedMailSuccessfully)
        {
            return false;
        }

        //everything went good
        message = Resources.Resource.AcountEdit;
        return true;
    }

    /// <summary>
    /// Смена роли пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="role">роль</param>
    /// <param name="error">сообщение об ошибке</param>
    /// <returns>успешность смены роли</returns>
    private static bool ChangeUserRole(string username, string role ,out string error)
    {
        try
        {
            Roles.RemoveUserFromRoles(username, Roles.GetRolesForUser(username));
            Roles.AddUserToRole(username, role);
            error = String.Empty;
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error changing user role: " + ex.Message);
            error = Resources.Resource.CannotChangeUserRole +
                Resources.Resource.UserEditionRejection;
        }
        return false;
    }
    /// <summary>
    /// смена роли пользователя без сообщения об ошибке
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="role">роль</param>
    private static void ChangeUserRoleSilent(string username, string role)
    {
        try
        {
            Roles.RemoveUserFromRoles(username, Roles.GetRolesForUser(username));
            Roles.AddUserToRole(username, role);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error changing user role: " + ex.Message);
        }
    }
    /// <summary>
    /// получение роли пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="role">роль</param>
    /// <param name="error">сообщение об ошибке</param>
    /// <returns></returns>
    private static bool GetUserRole(string username, out string role, out string error)
    {
        try
        {
            role = Roles.GetRolesForUser(username)[0];
            error = String.Empty;
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error getting user role: " + ex.Message);
            error = Resources.Resource.CannotGetUserRole +
                Resources.Resource.UserEditionRejection;
        }
        role = String.Empty;
        return false;
    }
    /// <summary>
    /// получение почты пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="email">почта</param>
    /// <param name="error">сообщение об ошибке</param>
    /// <returns></returns>
    private static bool GetUserEmail(string username, out string email, out string error)
    {
        try
        {
            MembershipUser user = Membership.GetUser(username);
            email = user.Email;
            error = String.Empty;
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error getting user email: " + ex.Message);
            error = Resources.Resource.CannotGetUserEmail +
                Resources.Resource.UserEditionRejection;
        }
        email = String.Empty;
        return false;
    }
    /// <summary>
    /// смена почты пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="email">почта</param>
    /// <param name="error">сообщение об ошибке</param>
    /// <returns></returns>
    private static bool ChangeUserEmail(string username, string email, out string error)
    {
        try
        {
            MembershipUser user = Membership.GetUser(username);
            user.Email = email;
            Membership.UpdateUser(user);
            error = String.Empty;
            return true;
        }
        catch (System.Configuration.Provider.ProviderException ex)
        {
            Debug.WriteLine("Error changin user email: " + ex.Message);
            error = Resources.Resource.DuplicateEmail;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error changin user email: " + ex.Message);
            error = Resources.Resource.CannotChangeUserEmail +
                Resources.Resource.UserEditionRejection;
        }
        return false;
    }
    /// <summary>
    /// смена почты пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="email">почта</param>
    private static void ChangeUserEmailSilent(string username, string email)
    {
        try
        {
            MembershipUser user = Membership.GetUser(username);
            user.Email = email;
            Membership.UpdateUser(user);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error changin user email: " + ex.Message);            
        }
    }

    private static ProfileCommon defaultProfile = new ProfileCommon();
    /// <summary>
    /// смена профиля пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="firstname">имя</param>
    /// <param name="lastname">фамилия</param>
    /// <param name="error">сообщение об ошибке</param>
    /// <returns></returns>
    private static bool ChangeUserProfile(string username, string firstname, string lastname, out string error)
    {
        try
        {
            ProfileCommon profile = defaultProfile.GetProfile(username);
            profile.FirstName = firstname;
            profile.LastName = lastname;
            profile.Save();
            error = String.Empty;
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error changing profile: " + ex.Message);
            error = Resources.Resource.CannotChangeUserProfile +
                Resources.Resource.UserEditionRejection;
        }
        return false;
    }
    /// <summary>
    /// Получение профиля пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="firstname">имя</param>
    /// <param name="lastname">фамилия</param>
    /// <param name="error">сообщение об ошибке</param>
    /// <returns></returns>
    private static bool GetUserProfile(string username, out string firstname, out string lastname, out string error)
    {
        try
        {
            ProfileCommon profile = defaultProfile.GetProfile(username);
            firstname = profile.FirstName;
            lastname = profile.LastName;
            error = String.Empty;
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error getting profile: " + ex.Message);
            error = Resources.Resource.CannotGetUserProfile +
                Resources.Resource.UserEditionRejection;
        }
        firstname = String.Empty;
        lastname = String.Empty;
        return false;
    }
    #endregion

    #region Delete
    /// <summary>
    /// удаление пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="message">сообщение</param>
    /// <returns>успешность удаления</returns>
    public static bool Delete(string username, out string message)
    {
        message = String.Empty;
        if (RemoveUserRoles(username, out message))
        {
            if (MembershipDeleteUser(username, out message))
            {
                message = Resources.Resource.UserDeleteSuccess;
                return true;
            }
            else if (message == String.Empty)
            {
                message = Resources.Resource.UserDeleteFail;
            }
        }
        return false;
    }
    /// <summary>
    /// удаление пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="error">сообщение об ошибке</param>
    /// <returns>успешность удаления</returns>
    private static bool MembershipDeleteUser(string username, out string error)
    {
        try
        {
            error = String.Empty;
            return Membership.DeleteUser(username, true);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error deleting user: " + ex.Message);
            error = Resources.Resource.CannotDeleteUser +
                Resources.Resource.UserDeletionRejection;
        }
        return false;
    }
    /// <summary>
    /// удаление роли пользователя
    /// </summary>
    /// <param name="username">логин пользователя</param>
    /// <param name="error">сообщение об ошибке</param>
    /// <returns>успешность удаления</returns>
    private static bool RemoveUserRoles(string username,out string error)
    {
        try
        {
            Roles.RemoveUserFromRoles(username, Roles.GetRolesForUser(username));
            error = String.Empty;
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error removing user roles: " + ex.Message);
            error = Resources.Resource.CannotRemoveUserRoles +
                Resources.Resource.UserDeletionRejection;
        }
        return false;
    }
    #endregion
}
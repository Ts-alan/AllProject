using System;
using System.Collections.Generic;
using System.Web;

public class UserEntity
{
	private string _login = String.Empty;
    public string Login
    {
        get 
        {
            return _login;
        }
        set
        {
            _login = value;
        }
    }

    private string _firstName = String.Empty;
    public string FirstName
    {
        get
        {
            return _firstName;
        }
        set
        {
            _firstName = value;
        }
    }

    private string _lastName = String.Empty;
    public string LastName
    {
        get
        {
            return _lastName;
        }
        set
        {
            _lastName = value;
        }
    }

    private DateTime _creationDate = DateTime.MinValue;
    public DateTime CreationDate
    {
        get
        {
            return _creationDate;
        }
        set
        {
            _creationDate = value;
        }
    }

    private string _email = String.Empty;
    public string Email
    {
        get
        {
            return _email;
        }
        set
        {
            _email = value;
        }
    }

    private DateTime _lastLoginDate = DateTime.MinValue;
    public DateTime LastLoginDate
    {
        get
        {
            return _lastLoginDate;
        }
        set
        {
            _lastLoginDate = value;
        }
    }

    private string _role;
    public string Role
    {
        get 
        {
            return _role;
        }
        set 
        {
            _role = value;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerUsers : MonoBehaviour
{
    public string ID_User;
    public string ID_User_Login;
    public bool _isLogin = false;
    private static ManagerUsers _instance;
    public static ManagerUsers Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ManagerUsers>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(ManagerUsers).Name);
                    _instance = singletonObject.AddComponent<ManagerUsers>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }
    public void SetID_User(string value)
    {
        ID_User = value;
    }
    public void SetID_UserLogin(string value)
    {
        ID_User_Login = value;
    }
    public void SetStatusLogin(bool value)
    {
        _isLogin= value;
    }
}

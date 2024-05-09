using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Load_Music_User : MonoBehaviour
{
    private static Load_Music_User _instance;
    public static Load_Music_User Instance { get { return _instance; } }

    public List<Music_User> List_Music_User;
    string link_API = "https://musicgame0911.000webhostapp.com/score.php";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        List_Music_User = new List<Music_User>();
        StartCoroutine(InitializeUserData());
    }

    IEnumerator InitializeUserData()
    {
        yield return StartCoroutine(LoadData(link_API, List_Music_User));
        UpdateTotalStar();
    }

    public void UpdateTotalStar()
    {
        int count = 0;
        foreach (var item in List_Music_User)
        {
            if (ManagerUsers.Instance._isLogin)
            {
                if (item.ID_User == ManagerUsers.Instance.ID_User_Login)
                {
                    count += Convert.ToInt32(item.Stars);
                }
            }
            else
            {
                if (item.ID_User == ManagerUsers.Instance.ID_User)
                {
                    count += Convert.ToInt32(item.Stars);
                }
            }
        }
        GameManager.Instance.SetTotalStar(count);
    }
    
    IEnumerator LoadData<T>(string urlApi, List<T> list)
    {
        UnityWebRequest www = UnityWebRequest.Get(urlApi);
        www.timeout = 3;
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            if (www.error.Contains("timeout"))
            {
                string filePath = Application.persistentDataPath + "/data_Music_User.json";
                if (System.IO.File.Exists(filePath))
                {
                    string jsonAPI = System.IO.File.ReadAllText(filePath);
                    T[] listqs = JsonHelper.GetArray<T>(jsonAPI);
                    if (listqs != null)
                    {
                        list.Clear();
                        list.AddRange(listqs);
                        Debug.Log("User music tải về bằng local!");
                    }
                }
            }
        }
        else
        {
            string jsonAPI = www.downloadHandler.text;
            if (jsonAPI != "Không có dữ liệu về user")
            {
                string filePath = Application.persistentDataPath + "/data_Music_User.json";
                System.IO.File.WriteAllText(filePath, jsonAPI);
                T[] listqs = JsonHelper.GetArray<T>(jsonAPI);
                if (listqs != null)
                {
                    list.Clear();
                    list.AddRange(listqs);
                    Debug.Log("Users music tải về bằng web!");
                }
            }
        }
    }
}

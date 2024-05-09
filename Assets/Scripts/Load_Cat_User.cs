using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Load_Cat_User : MonoBehaviour
{
    private static Load_Cat_User _instance;
    public static Load_Cat_User Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Load_Cat_User>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(Load_Cat_User).Name);
                    _instance = singletonObject.AddComponent<Load_Cat_User>();
                }
            }
            return _instance;
        }
    }

    public List<Cat_User> List_Cat_User;
    string link_API = "https://musicgame0911.000webhostapp.com/cat_unlock.php";
    private void Start()
    {
        List_Cat_User = new List<Cat_User>();
        StartCoroutine(InitializeUserData());
    }
    IEnumerator InitializeUserData()
    {
        yield return StartCoroutine(LoadData(link_API, List_Cat_User));
    }
    IEnumerator LoadData<T>(string urlApi, List<T> list)
    {
        UnityWebRequest www = UnityWebRequest.Get(urlApi);
        www.timeout = 5;
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
                        Debug.Log("Cat _ User tải về bằng local!");
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
                    Debug.Log("Cat _ User tải về bằng web!");
                }
            }
        }
    }
}

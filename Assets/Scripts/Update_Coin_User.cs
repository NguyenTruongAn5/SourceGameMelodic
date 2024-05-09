using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Update_Coin_User : MonoBehaviour
{
    string link_API_UpdateCoin = "https://musicgame0911.000webhostapp.com/update_coin.php";
    public List<Users> List_User;
    string link_API = "https://musicgame0911.000webhostapp.com/user.php";

    private static Update_Coin_User _instance;
    public static Update_Coin_User Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Update_Coin_User>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(Update_Coin_User).Name);
                    _instance = singletonObject.AddComponent<Update_Coin_User>();
                }
            }
            return _instance;
        }
    }
    private void Start()
    {
        List_User = new List<Users>();
        StartCoroutine(InitializeUserData());
    }
    IEnumerator InitializeUserData()
    {
        yield return StartCoroutine(LoadData(link_API, List_User));
        SetTotalCoin();
    }
    public void SetTotalCoin()
    {
        int coin = 0;
        if (ManagerUsers.Instance._isLogin)
        {
            var user = List_User.FirstOrDefault(x => x.User_ID == ManagerUsers.Instance.ID_User_Login);
            if (user != null)
            {
                coin = Convert.ToInt32(user.User_Coin);
            }
        }
        else
        {
            var user = List_User.FirstOrDefault(x => x.User_ID == ManagerUsers.Instance.ID_User);
            if (user != null)
            {
                coin = Convert.ToInt32(user.User_Coin);
            }
        }
        GameManager.Instance.SetTotalCoin(coin);
    }
    public void IncreaseCoin(int coin)
    {
        if (ManagerUsers.Instance._isLogin)
        {
            var user = List_User.FirstOrDefault(x => x.User_ID == ManagerUsers.Instance.ID_User_Login);
            if (user != null)
            {
                int count = Convert.ToInt32(user.User_Coin) + coin;
                StartCoroutine(SendUserDataRoutine(user.User_ID, count));
            }
        }
        else
        {
            var user = List_User.FirstOrDefault(x => x.User_ID == ManagerUsers.Instance.ID_User);
            if (user != null)
            {
                int count = Convert.ToInt32(user.User_Coin) + coin;
                StartCoroutine(SendUserDataRoutine(user.User_ID, count));
            }
        }
    }
    public void DescreaseCoin(int coin)
    {
        if (ManagerUsers.Instance._isLogin)
        {
            var user = List_User.FirstOrDefault(x => x.User_ID == ManagerUsers.Instance.ID_User_Login);
            if (user != null)
            {
                int count = Convert.ToInt32(user.User_Coin) - coin;
                StartCoroutine(SendUserDataRoutine(user.User_ID, count));
                Debug.Log("Đã giảm tiền người chơi đăng nhập");
            }
        }
        else
        {
            var user = List_User.FirstOrDefault(x => x.User_ID == ManagerUsers.Instance.ID_User);
            if (user != null)
            {
                int count = Convert.ToInt32(user.User_Coin) - coin;
                StartCoroutine(SendUserDataRoutine(user.User_ID, count));
                Debug.Log("Đã giảm tiền người chơi KHÔNG đăng nhập");
            }
        }
    }
    IEnumerator SendUserDataRoutine(string id_user, int coin)
    {
        WWWForm form = new WWWForm();
        form.AddField("User_ID", id_user);
        form.AddField("User_Coin", coin);
        using (UnityWebRequest www = UnityWebRequest.Post(link_API_UpdateCoin, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Lỗi: " + www.error);
            }
            else
            {
                Debug.Log("Dữ liệu đã được gửi thành công!");
                Debug.Log("Phản hồi từ server: " + www.downloadHandler.text);
            }
        }
    }
    IEnumerator LoadData<T>(string urlApi, List<T> list)
    {
        UnityWebRequest www = UnityWebRequest.Get(urlApi);
        www.timeout = 2;
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            if (www.error.Contains("timeout"))
            {
                string filePath = Application.persistentDataPath + "/dataUser.json";
                if (System.IO.File.Exists(filePath))
                {
                    string jsonAPI = System.IO.File.ReadAllText(filePath);
                    T[] listqs = JsonHelper.GetArray<T>(jsonAPI);
                    if (listqs != null)
                    {
                        list.Clear();
                        list.AddRange(listqs);
                        Debug.Log("User tải về bằng local!");
                    }
                }
            }
        }
        else
        {
            string jsonAPI = www.downloadHandler.text;
            if (jsonAPI != "Không có dữ liệu về user")
            {
                string filePath = Application.persistentDataPath + "/dataUser.json";
                System.IO.File.WriteAllText(filePath, jsonAPI);
                T[] listqs = JsonHelper.GetArray<T>(jsonAPI);
                if (listqs != null)
                {
                    list.Clear();
                    list.AddRange(listqs);
                    Debug.Log("Users tải về bằng web!");
                }
            }
        }
    }
}

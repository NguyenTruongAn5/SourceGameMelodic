using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CheckUserViral : MonoBehaviour
{
    public List<Users> List_User;
    string link_API = "https://musicgame0911.000webhostapp.com/user.php";
    string link_API_signin = "https://musicgame0911.000webhostapp.com/register.php";
    int current_ID_User;
    private void Start()
    {
        List_User = new List<Users>();
        StartCoroutine(InitializeUserData());
    }
    IEnumerator InitializeUserData()
    {
        yield return StartCoroutine(LoadData(link_API, List_User));
        current_ID_User = List_User.Count;
        CheckSaveLocal();
    }
    private void CheckSaveLocal()
    {
        string filePath = Application.persistentDataPath + "/current_ID_User.json";
        int currentID = current_ID_User + 1;
        if (!File.Exists(filePath))
        {
            StartCoroutine(SendUserDataRoutine("", 0, "", ""));
            File.WriteAllText(filePath, currentID.ToString());
            ManagerUsers.Instance.SetID_User(currentID.ToString());
        }
        else
        {
            string jsonContent = File.ReadAllText(filePath);
            ManagerUsers.Instance.SetID_User(jsonContent);
        }
    }
    IEnumerator SendUserDataRoutine(string name, int coin, string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("User_Name", name);
        form.AddField("User_Email", email);
        form.AddField("User_Coin", coin);
        form.AddField("User_Password", password);
        form.AddField("ID_Left", "0");
        form.AddField("ID_Right", "0");

        using (UnityWebRequest www = UnityWebRequest.Post(link_API_signin, form))
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
    public void LogoutPlayer()
    {
        string filePath = Application.persistentDataPath + "/CheckSaveLoginPlayer.json";
        string filePathID = Application.persistentDataPath + "/current_ID_User.json";
        if (File.Exists(filePath))
        {
            ManagerUsers.Instance.SetStatusLogin(false);
            string jsonContent = File.ReadAllText(filePathID);
            ManagerUsers.Instance.SetID_User(jsonContent);
            File.Delete(filePath);
        }
        SceneManager.LoadScene("Home");
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

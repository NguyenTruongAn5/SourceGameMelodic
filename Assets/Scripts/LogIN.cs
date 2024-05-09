using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LogIN : MonoBehaviour
{
    public List<Users> List_User;
    string link_API = "https://musicgame0911.000webhostapp.com/user.php";
    [SerializeField] TMP_InputField txtEmail;
    [SerializeField] TMP_InputField txtPassword;
    [SerializeField] GameObject dialogNotification;
    [SerializeField] TextMeshProUGUI txtNotification;

    private void Start()
    {
        List_User = new List<Users>();
        StartCoroutine(InitializeUserData());
    }
    IEnumerator InitializeUserData()
    {
        yield return StartCoroutine(LoadData(link_API, List_User));
    }
    public void btnLog_In()
    {
        bool checkLogin = false;
        string email = txtEmail.text;
        string password = txtPassword.text;
        string hashPassword = MD5Hasher.GetMd5Hash(txtPassword.text);
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowNotification("Vui lòng nhập đầy đủ thông tin!");
        }
        else if (!email.Contains("@"))
        {
            ShowNotification("Email chưa đúng định dạng!");
        }
        else
        {
            foreach (var user in List_User)
            {
                if (hashPassword == user.User_Password && email == user.User_Email)
                {
                    checkLogin = true;
                    ManagerUsers.Instance.SetID_UserLogin(user.User_ID);
                    ManagerUsers.Instance.SetStatusLogin(true);
                    int left = Convert.ToInt32(user.ID_Left);
                    int right = Convert.ToInt32(user.ID_Right);
                    ManagerChooseCat.Instance.SetCatLeft(left);
                    ManagerChooseCat.Instance.SetCatLeft(right);
                    GameManager.Instance.SetTotalCoin(Convert.ToInt32(user.User_Coin));
                    string filePath = Application.persistentDataPath + "/CheckSaveLoginPlayer.json";
                    File.WriteAllText(filePath, user.User_ID);
                }
            }
        }
        if (checkLogin)
        {
            ShowNotification("Đăng nhập thành công!");
            LoadScence("Home");
        }
        else
        {
            ShowNotification("Tài khoản hoặc Mật khẩu không chính xác!");
        }
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
    void ShowNotification(string message)
    {
        dialogNotification.SetActive(true);
        txtNotification.text = message;
    }
    public void CloseDialog()
    {
        dialogNotification.SetActive(false);
    }
    public void LoadScence(string name)
    {
        SceneManager.LoadScene(name);
    }
}

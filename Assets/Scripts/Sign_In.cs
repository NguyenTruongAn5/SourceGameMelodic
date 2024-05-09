using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Sign_In : MonoBehaviour
{
    public List<Users> List_User;
    string link_API = "https://musicgame0911.000webhostapp.com/user.php";
    string link_API_signin = "https://musicgame0911.000webhostapp.com/register.php";
    [SerializeField] TMP_InputField txtName;
    [SerializeField] TMP_InputField txtAccount;
    [SerializeField] TMP_InputField txtEmail;
    [SerializeField] TMP_InputField txtPassword;
    [SerializeField] TMP_InputField txtConfirm;
    [SerializeField] GameObject dialogExist;
    [SerializeField] GameObject dialogNotification;
    [SerializeField] TextMeshProUGUI txtNotification;

    private void Start()
    {
        List_User = new List<Users>();
        StartCoroutine(LoadData(link_API, List_User));
    }
    public void btnSignIn()
    {
        bool emailExists = CheckEmailExists(txtEmail.text);
        if (emailExists)
        {
            dialogExist.SetActive(true);
            return; 
        }

        string name = txtName.text;
        string email = txtEmail.text;
        string password = txtPassword.text;
        string hashPassword = MD5Hasher.GetMd5Hash(txtPassword.text);
        string confirm = txtConfirm.text;
        int coin = GameManager.Instance.total_Coin;
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || 
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirm))
        {
            ShowNotification("Vui lòng nhập đầy đủ thông tin!");
        }
        else if (!email.Contains("@"))
        {
            ShowNotification("Email chưa đúng định dạng!");
        }
        else
        {
            if (password == confirm)
            {
                StartCoroutine(SendUserDataRoutine(name, coin, email, hashPassword));
                ShowNotification("Đăng kí thành công!");
            }
            else
            {
                ShowNotification("Mật khẩu xác nhận không khớp!");
            }
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
    bool CheckEmailExists(string email)
    {
        foreach (var user in List_User)
        {
            if (user.User_Email == email)
            {
                return true;
            }
        }
        return false;
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

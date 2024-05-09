using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Information_User : MonoBehaviour
{
    public List<Users> List_User;
    string link_API = "https://musicgame0911.000webhostapp.com/user.php";
    [SerializeField] GameObject panel_User_Login;
    [SerializeField] GameObject panel_User_No_Login;
    [SerializeField] TextMeshProUGUI txtAccount;
    [SerializeField] TextMeshProUGUI txtEmail;
    [SerializeField] TextMeshProUGUI txtTotalStar;
    [SerializeField] TextMeshProUGUI txtTotalCoin;
    [SerializeField] TextMeshProUGUI txtTotalStar_No_Login;
    [SerializeField] TextMeshProUGUI txtTotalCoin_No_Login;
    private void Start()
    {
        List_User = new List<Users>();
        StartCoroutine(InitializeUserData());
    }
    IEnumerator InitializeUserData()
    {
        yield return StartCoroutine(LoadData(link_API, List_User));
    }
    public void btnClickUser()
    {
        var checkUserID = List_User.FirstOrDefault(x => x.User_ID == ManagerUsers.Instance.ID_User_Login);
        if (checkUserID != null && ManagerUsers.Instance._isLogin)
        {
            ShowPanelUserLogin(checkUserID);
        }
        else
        {
            ShowPanelUserNOLogin(checkUserID);
        }
    }
    private void ShowPanelUserLogin(Users user)
    {
        panel_User_Login.SetActive(true);
        txtAccount.text = "Tên: " + user.User_Name;
        txtEmail.text = "Email: " + user.User_Email;
        txtTotalCoin.text = "Vàng: " + user.User_Coin;
        txtTotalStar.text = "Tổng sao: " + GameManager.Instance.countTotalStar;

    }
    private void ShowPanelUserNOLogin(Users user)
    {
        panel_User_No_Login.SetActive(true);
        txtTotalCoin_No_Login.text = "Vàng: " + GameManager.Instance.total_Coin;
        txtTotalStar_No_Login.text = "Tổng sao: " + GameManager.Instance.countTotalStar;
    }
    public void btnClickClodeUserLogin()
    {
        panel_User_Login.SetActive(false);
    }
    public void btnClickClodeUserNOLogin()
    {
        panel_User_No_Login.SetActive(false);
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

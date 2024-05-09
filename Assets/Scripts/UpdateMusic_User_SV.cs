using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UpdateMusic_User_SV : MonoBehaviour
{
    public List<Music_User> List_Music_User;
    string link_API = "https://musicgame0911.000webhostapp.com/score.php";
    string link_API_signin = "https://musicgame0911.000webhostapp.com/up_score.php";
    string link_API_UpScore = "https://musicgame0911.000webhostapp.com/update_score.php";
    bool checkSend = true;
    private static UpdateMusic_User_SV _instance;
    public static UpdateMusic_User_SV Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UpdateMusic_User_SV>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(UpdateMusic_User_SV).Name);
                    _instance = singletonObject.AddComponent<UpdateMusic_User_SV>();
                }
            }
            return _instance;
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
    }
    public void onClickSenData()
    {
        string score = GameManager.Instance.score.ToString();
        string ID_Music = LoadListMusic.Instance.index.ToString();
        string ID_User = ManagerUsers.Instance.ID_User;
        if (ManagerUsers.Instance._isLogin)
        {
            string ID_User_Login = ManagerUsers.Instance.ID_User_Login;
            var music_user = List_Music_User.FirstOrDefault(x => x.ID_User == ID_User_Login && x.ID_Music == ID_Music);
            int star = CreateStar.Instance.count;
            if (music_user != null)
            {
                if (star < Convert.ToInt32(music_user.Stars))
                {
                    star = Convert.ToInt32(music_user.Stars);
                }
                if (Convert.ToInt32(score) > Convert.ToInt32(music_user.Score))
                {
                    StartCoroutine(SendUpdateUserDataRoutine(music_user.ID, score, star.ToString()));
                }
            }
            else
            {
                StartCoroutine(SendUserDataRoutine(ID_User_Login, ID_Music, score, star.ToString()));
            }
        }
        else
        {
            var music_user = List_Music_User.FirstOrDefault(x => x.ID_User == ID_User && x.ID_Music == ID_Music);
            int star = CreateStar.Instance.count;
            if(music_user!=null)
            {
                if (star < Convert.ToInt32(music_user.Stars))
                {
                    star = Convert.ToInt32(music_user.Stars);
                }
            }
            if (music_user != null)
            {
                if (Convert.ToInt32(score) > Convert.ToInt32(music_user.Score))
                {
                    StartCoroutine(SendUpdateUserDataRoutine(music_user.ID, score, star.ToString()));
                }
            }
            else
            {
                StartCoroutine(SendUserDataRoutine(ID_User, ID_Music, score, star.ToString()));
            }
        }

        int coin = GameManager.Instance.coin;
        Update_Coin_User.Instance.IncreaseCoin(coin);
        checkSend = false;
    }
    public void LoadGameScene(string name)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }
    IEnumerator SendUserDataRoutine(string id_user, string id_music, string score, string start)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID_User", id_user);
        form.AddField("ID_Music", id_music);
        form.AddField("Score", score);
        form.AddField("Stars", start);
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
    IEnumerator SendUpdateUserDataRoutine(string id_U_M, string score, string start)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID", id_U_M);
        form.AddField("Score", score);
        form.AddField("Stars", start);

        using (UnityWebRequest www = UnityWebRequest.Post(link_API_UpScore, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Lỗi: " + www.error);
            }
            else
            {
                Debug.Log("Dữ liệu đã cập nhật được gửi thành công!");
                Debug.Log("Phản hồi từ server: " + www.downloadHandler.text);
            }
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
                string filePath = Application.persistentDataPath + "/data_Music_User.json";
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
                string filePath = Application.persistentDataPath + "/data_Music_User.json";
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

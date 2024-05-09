using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RankPlayerStars : MonoBehaviour
{
    List<Users> listUser;
    List<Music_User> listUser_Music;
    [SerializeField] GameObject panelRank;
    [SerializeField] GameObject itemRank;
    [SerializeField] Transform curentItemRank;
    [SerializeField] Image imgGold;
    [SerializeField] Image imgSliver;
    [SerializeField] Image imgCu;
    string link_API_user = "https://musicgame0911.000webhostapp.com/user.php";
    string link_API_score = "https://musicgame0911.000webhostapp.com/score.php";
    private void Start()
    {
        listUser = new List<Users>();
        listUser_Music = new List<Music_User>();
        StartCoroutine(InitializeUserData());
    }
    IEnumerator InitializeUserData()
    {
        yield return StartCoroutine(LoadDataUser(link_API_user, listUser));
        yield return StartCoroutine(LoadDataMusic(link_API_score, listUser_Music));
        SortStarPlayer();
    }
    public void LoadRankPlayerStar()
    {
        ClearViewportItems();
        int id = 1;
        foreach (var user in listUser)
        {
            if (user.User_Email != null && user.User_Email != "")
            {
                GameObject itemUser = Instantiate(itemRank, curentItemRank);
                Image imgRank = itemUser.transform.Find("Image").GetComponent<Image>();
                TextMeshProUGUI txtID = itemUser.transform.Find("Text_Rank").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI txtUserName = itemUser.transform.Find("Text_Username").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI txtStar = itemUser.transform.Find("Text_Stars").GetComponent<TextMeshProUGUI>();
                if (id == 1)
                {
                    imgRank.sprite = imgGold.sprite;
                }
                if (id == 2)
                {
                    imgRank.sprite = imgSliver.sprite;
                }
                if (id == 3)
                {
                    imgRank.sprite = imgCu.sprite;
                }
                txtID.text = id.ToString();
                txtUserName.text = user.User_Name;
                txtStar.text = "" + CountTotalStar(user.User_ID);
                id++;
            }
        }
    }
    public void ClearViewportItems()
    {
        foreach (Transform child in curentItemRank)
        {
            Destroy(child.gameObject);
        }
    }
    public void SortStarPlayer()
    {
        listUser.Sort((x, y) => CountTotalStar(y.User_ID).CompareTo(CountTotalStar(x.User_ID)));
    }
    public int CountTotalStar(string id)
    {
        int countStar = listUser_Music
            .Where(x => x.ID_User == id)
            .Sum(x => Convert.ToInt32(x.Stars));
        return countStar;
    }
    public void clickOnShowPanelRank()
    {
        panelRank.SetActive(true);
        LoadRankPlayerStar();
    }

    public void clickCloseShowPanelRank()
    {
        panelRank.SetActive(false);
    }
    IEnumerator LoadDataUser<T>(string urlApi, List<T> list)
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
    IEnumerator LoadDataMusic<T>(string urlApi, List<T> list)
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

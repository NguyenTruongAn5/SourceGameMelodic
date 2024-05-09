using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadListMusic : MonoBehaviour
{
    [SerializeField] GameObject canvasHome;
    [SerializeField] GameObject canvasMusic;
    [SerializeField] GameObject ItemListMusic;
    [SerializeField] Image starImage;
    [SerializeField] TextMeshProUGUI MusicName;
    [SerializeField] Transform contentTransform;
    public static LoadListMusic Instance;
    public int index;
    public List<ListMusic> ListMusic;
    public ListMusic list;
    private string jsonUrl = "https://musicgame0911.000webhostapp.com/";
    public string midiClick;
    public string mp3Click;
    private bool isMP3Downloaded = false;
    public bool isStartGame = true;
    bool CheckDownload = true;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }
    private void Start()
    {
        ListMusic = new List<ListMusic>();
        StartCoroutine(InitializeUserData());
        list = new ListMusic();
    }
    IEnumerator InitializeUserData()
    {
        yield return StartCoroutine(LoadData(jsonUrl, ListMusic));
    }
    private void Update()
    {
        NextPlayMusic();
        if (ListMusic != null)
        {
            list = ListMusic[index];
            MusicName.text = list.MusicName;
            if (GameManager.Instance._isStatusGame == 1)
            {
                if (isStartGame)
                {
                    LoadMusicFirtFrame(list);
                    StartCoroutine(WaitForDownloadsAndLoadScene());
                }
            }
        }
        LoadDataLocal(ListMusic);
    }
    public void NextPlayMusic()
    {
        if (GameManager.Instance.CheckedNext)
        {
            index = GameManager.Instance.indexMusic;
        }
    }
    public void LoadMusicFirtFrame(ListMusic list)
    {
        if (ListMusic != null && ListMusic.Count > 0)
        {
            string midiUrl = list.LinkMidiFile;
            string urlMp3 = list.LinkMP3File;
            index = Convert.ToInt32(list.ID);
            string pathMidiLocal = Application.persistentDataPath + "/" + index + ".mid";
            string pathMP3Local = Application.persistentDataPath + "/" + index + ".mp3";
            midiClick = pathMidiLocal;
            mp3Click = pathMP3Local;
            StartCoroutine(DownLoadFileMidi(midiUrl, pathMidiLocal));
            StartCoroutine(DownLoadFileMP3(urlMp3, pathMP3Local));
            isStartGame = false;
        }
    }
    public void OnClickListMusic()
    {
        canvasHome.SetActive(false);
        canvasMusic.SetActive(true);
        LoadItemListMusic();
    }
    public void OnClickBackToHome()
    {
        canvasHome.SetActive(true);
        canvasMusic.SetActive(false);
        ListMusic.Clear();
    }
    public void LoadItemListMusic()
    {
        foreach (var item in ListMusic.OrderByDescending(x=>x.ID))
        {
            GameObject Item = Instantiate(ItemListMusic, contentTransform);
            TextMeshProUGUI nameMusic = Item.transform.Find("NameMusic").GetComponent<TextMeshProUGUI>();
            Transform imgParent = Item.transform.Find("Image_Avatar");
            Button img = imgParent.GetComponentInChildren<Button>();
            StartCoroutine(LoadImage(item.LinkImageFile, img.image));
            Button btnPlay = Item.transform.Find("Button_Play").GetComponent<Button>();
            btnPlay.onClick.AddListener(() => OnClickBtnPlay(item.ID, item.LinkMidiFile, item.LinkMP3File));
            nameMusic.text = item.MusicName;
            Image star1 = Item.transform.Find("Star1").GetComponent<Image>();
            Image star2 = Item.transform.Find("Star2").GetComponent<Image>();
            Image star3 = Item.transform.Find("Star3").GetComponent<Image>();
            CreateStarItemListMusic(item.ID,star1,star2,star3);
        }
    }
    private void CreateStarItemListMusic(string idMusic, Image s1, Image s2, Image s3)
    {
        List<Music_User> List_Music_User = Load_Music_User.Instance.List_Music_User;
        if (ManagerUsers.Instance._isLogin)
        {
            foreach(var item in List_Music_User)
            {
                if(item.ID_User == ManagerUsers.Instance.ID_User_Login && item.ID_Music == idMusic)
                {
                    int count = Convert.ToInt32(item.Stars);
                    if (count >= 1)
                    {
                        s1.sprite = starImage.sprite;
                    }
                    if (count >= 2)
                    {
                        s2.sprite = starImage.sprite;
                    }
                    if (count == 3)
                    {
                        s3.sprite = starImage.sprite;
                    }
                }
            }
        }
        else
        {
            foreach (var item in List_Music_User)
            {
                if (item.ID_User == ManagerUsers.Instance.ID_User && item.ID_Music == idMusic)
                {
                    int count = Convert.ToInt32(item.Stars);
                    if (count >= 1)
                    {
                        s1.sprite = starImage.sprite;
                    }
                    if (count >= 2)
                    {
                        s2.sprite = starImage.sprite;
                    }
                    if (count == 3)
                    {
                        s3.sprite = starImage.sprite;
                    }
                }
            }
        }
    }
    private void OnClickBtnPlay(string id, string midiUrl, string urlMp3)
    {
        index = Convert.ToInt32(id);
        string pathMidiLocal = Application.persistentDataPath + "/" + id + ".mid";
        string pathMP3Local = Application.persistentDataPath + "/" + id + ".mp3";
        midiClick = pathMidiLocal;
        mp3Click = pathMP3Local;
        StartCoroutine(DownLoadFileMidi(midiUrl, pathMidiLocal));
        StartCoroutine(DownLoadFileMP3(urlMp3, pathMP3Local));
        Debug.Log("Đã chọn: " + index);
        StartCoroutine(WaitForDownloadsAndLoadScene());
    }
    public IEnumerator DownLoadFileMidi(string midiUrl, string path)
    {
        UnityWebRequest www = UnityWebRequest.Get(midiUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            File.WriteAllBytes(path, www.downloadHandler.data);
        }
    }
    public IEnumerator DownLoadFileMP3(string midiUrl, string path)
    {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(midiUrl, AudioType.MPEG);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] data = www.downloadHandler.data;

            if (data != null && data.Length > 0)
            {
                File.WriteAllBytes(path, data);
                isMP3Downloaded = true;
            }
            else
            {
                isMP3Downloaded = false;
            }
        }
        else
        {
            isMP3Downloaded = false;
            Debug.LogError("Download failed: " + www.error);
        }
    }
    public IEnumerator WaitForDownloadsAndLoadScene()
    {
        while (!isMP3Downloaded)
        {
            yield return null;
        }
        SceneManager.LoadScene("Game");
    }
    IEnumerator LoadImage(string url, Image img)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture2D = DownloadHandlerTexture.GetContent(www);
            if (texture2D != null)
            {
                Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                    new Vector2(0.5f, 0.5f));

                img.sprite = sprite;
            }
        }
    }
    void LoadDataLocal<T>(List<T> list)
    {
        if (CheckDownload)
        {
            string filePath = Application.persistentDataPath + "/data.json";
            if (System.IO.File.Exists(filePath))
            {
                string jsonAPI = System.IO.File.ReadAllText(filePath);
                T[] listqs = JsonHelper.GetArray<T>(jsonAPI);
                if (listqs != null)
                {
                    list.Clear();
                    list.AddRange(listqs);
                    Debug.Log("Danh sách tải về bằng local trên!");
                }
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
                string filePath = Application.persistentDataPath + "/data.json";
                if (System.IO.File.Exists(filePath))
                {
                    string jsonAPI = System.IO.File.ReadAllText(filePath);
                    T[] listqs = JsonHelper.GetArray<T>(jsonAPI);
                    if (listqs != null)
                    {
                        list.Clear();
                        list.AddRange(listqs);
                        Debug.Log("Danh sách tải về bằng local!");
                    }
                }
            }

        }
        else
        {
            string jsonAPI = www.downloadHandler.text;
            if (jsonAPI != "Không có dữ liệu")
            {
                string filePath = Application.persistentDataPath + "/data.json";
                System.IO.File.WriteAllText(filePath, jsonAPI);
                CheckDownload = false;
                T[] listqs = JsonHelper.GetArray<T>(jsonAPI);
                if (listqs != null)
                {
                    list.Clear();
                    list.AddRange(listqs);
                    Debug.Log("Danh sách tải về bằng web!");
                }
            }
        }
    }
    public void SetIndex(int id)
    {
        index = id;
    }
}

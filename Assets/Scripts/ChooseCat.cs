using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChooseCat : MonoBehaviour
{
    [SerializeField] Button CatLeft;
    [SerializeField] Button CatRight;
    [SerializeField] GameObject ItemCat;
    [SerializeField] Transform curentTranform;
    [SerializeField] GameObject content;
    [SerializeField] GameObject arrowLeft;
    [SerializeField] GameObject arrowRight;
    [SerializeField] GameObject panelConfirm;
    [SerializeField] TextMeshProUGUI txtCoin;
    List<Cats> List_Cat;
    [SerializeField] List<Sprite> gameObjectCat;
    string link_Api = "https://musicgame0911.000webhostapp.com/cat.php";
    string link_Api_InsertCat = "https://musicgame0911.000webhostapp.com/insert_catunlock.php";
    private void Start()
    {
        List_Cat = new List<Cats>();
        StartCoroutine(InitializeUserData());
        LoadItemCatLeft();
        LoadImgBtnCat();
    }
    private void Update()
    {
        ShowCoin();
    }
    private void ShowCoin()
    {
        txtCoin.text = GameManager.Instance.total_Coin.ToString();
    }
    IEnumerator InitializeUserData()
    {
        yield return StartCoroutine(LoadData(link_Api, List_Cat));
        LoadItemCatLeft();
    }
    private void LoadImgBtnCat()
    {
        int chooseCatLeft = ManagerChooseCat.Instance.chooseCatLeft;
        int chooseCatRight = ManagerChooseCat.Instance.chooseCatRight;
        CatLeft.image.sprite = gameObjectCat[chooseCatLeft];
        CatRight.image.sprite = gameObjectCat[chooseCatRight];
    }
    public void btnClickCatLeft()
    {
        arrowLeft.SetActive(true);
        arrowRight.SetActive(false);
        LoadItemCatLeft();
    }
    public void btnClickCatRight()
    {
        arrowLeft.SetActive(false);
        arrowRight.SetActive(true);
        LoadItemCatRight();
    }
    public void LoadItemCatLeft()
    {
        ClearViewportItems();
        var list_Cat_User = Load_Cat_User.Instance.List_Cat_User;
        foreach (var item in List_Cat)
        {
            if (ManagerUsers.Instance._isLogin)
            {
                string idUser = ManagerUsers.Instance.ID_User_Login;
                var cat_user = list_Cat_User.FirstOrDefault(x => x.ID_Cat == item.ID && x.ID_User == idUser);
                if (cat_user != null)
                {
                    int id = Convert.ToInt32(item.ID);
                    GameObject catItem = Instantiate(ItemCat, curentTranform);
                    TextMeshProUGUI txtCoin = catItem.transform.Find("txtCoin").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI txtNameCat = catItem.transform.Find("txt_CatName").GetComponent<TextMeshProUGUI>();
                    Button cat = catItem.transform.Find("Button_Cat").GetComponent<Button>();
                    Sprite imgCat = gameObjectCat[id - 1];
                    cat.image.sprite = imgCat;
                    txtCoin.text = "Chọn";
                    txtNameCat.text = item.Name;
                    int price = Convert.ToInt32(item.Price);
                    cat.onClick.AddListener(() => ClickChooseCatLeft(item.ID, price));
                }
                else
                {
                    int id = Convert.ToInt32(item.ID);
                    GameObject catItem = Instantiate(ItemCat, curentTranform);
                    TextMeshProUGUI txtCoin = catItem.transform.Find("txtCoin").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI txtNameCat = catItem.transform.Find("txt_CatName").GetComponent<TextMeshProUGUI>();
                    Button cat = catItem.transform.Find("Button_Cat").GetComponent<Button>();
                    Sprite imgCat = gameObjectCat[id - 1];
                    cat.image.sprite = imgCat;
                    txtCoin.text = item.Price;
                    txtNameCat.text = item.Name;
                    int price = Convert.ToInt32(item.Price);
                    cat.onClick.AddListener(() => ClickChooseCatLeft(item.ID, price));
                }
            }
            else
            {
                string idUser = ManagerUsers.Instance.ID_User;
                var cat_user = list_Cat_User.FirstOrDefault(x => x.ID_Cat == item.ID && x.ID_User == idUser);
                if (cat_user != null)
                {
                    int id = Convert.ToInt32(item.ID);
                    GameObject catItem = Instantiate(ItemCat, curentTranform);
                    TextMeshProUGUI txtCoin = catItem.transform.Find("txtCoin").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI txtNameCat = catItem.transform.Find("txt_CatName").GetComponent<TextMeshProUGUI>();
                    Button cat = catItem.transform.Find("Button_Cat").GetComponent<Button>();
                    Sprite imgCat = gameObjectCat[id - 1];
                    cat.image.sprite = imgCat;
                    txtCoin.text = "Chọn";
                    txtNameCat.text = item.Name;
                    int price = Convert.ToInt32(item.Price);
                    cat.onClick.AddListener(() => ClickChooseCatLeft(item.ID, price));
                }
                else
                {
                    int id = Convert.ToInt32(item.ID);
                    GameObject catItem = Instantiate(ItemCat, curentTranform);
                    TextMeshProUGUI txtCoin = catItem.transform.Find("txtCoin").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI txtNameCat = catItem.transform.Find("txt_CatName").GetComponent<TextMeshProUGUI>();
                    Button cat = catItem.transform.Find("Button_Cat").GetComponent<Button>();
                    Sprite imgCat = gameObjectCat[id - 1];
                    cat.image.sprite = imgCat;
                    txtCoin.text = item.Price;
                    txtNameCat.text = item.Name;
                    int price = Convert.ToInt32(item.Price);
                    cat.onClick.AddListener(() => ClickChooseCatLeft(item.ID, price));
                }
            }
        }
    }
    public void ClickChooseCatLeft(string index, int priceCat)
    {
        var list_Cat_User = Load_Cat_User.Instance.List_Cat_User;
        if (ManagerUsers.Instance._isLogin)
        {
            string idUser = ManagerUsers.Instance.ID_User_Login;
            var cat_user = list_Cat_User.FirstOrDefault(x => x.ID_Cat == index && x.ID_User == idUser);
            if (cat_user != null)
            {
                int id = Convert.ToInt32(index);
                ManagerChooseCat.Instance.SetCatLeft(id - 1);
                int chooseCatLeft = ManagerChooseCat.Instance.chooseCatLeft;
                CatLeft.image.sprite = gameObjectCat[chooseCatLeft];
            }
            else
            {
                int check = GameManager.Instance.total_Coin - priceCat;
                if (check >= 0)
                {
                    string idUserNoLogin = ManagerUsers.Instance.ID_User;
                    int id = Convert.ToInt32(index);
                    ManagerChooseCat.Instance.SetCatLeft(id - 1);
                    int chooseCatLeft = ManagerChooseCat.Instance.chooseCatLeft;
                    Update_Coin_User.Instance.DescreaseCoin(priceCat);
                    StartCoroutine(SendUserDataRoutine(idUserNoLogin, index));
                    ShowCoin();
                }
                else
                {
                    panelConfirm.SetActive(true);
                    Debug.Log("Bạn k đủ tiền mua mèo");
                }
            }
        }
        else
        {
            string idUser = ManagerUsers.Instance.ID_User;
            var cat_user = list_Cat_User.FirstOrDefault(x => x.ID_Cat == index && x.ID_User == idUser);
            if (cat_user != null)
            {
                int id = Convert.ToInt32(index);
                ManagerChooseCat.Instance.SetCatLeft(id - 1);
                int chooseCatLeft = ManagerChooseCat.Instance.chooseCatLeft;
                CatLeft.image.sprite = gameObjectCat[chooseCatLeft];
              
            }
            else
            {
                int check = GameManager.Instance.total_Coin - priceCat;
                if (check >= 0)
                {
                    int id = Convert.ToInt32(index);
                    ManagerChooseCat.Instance.SetCatLeft(id - 1);
                    int chooseCatLeft = ManagerChooseCat.Instance.chooseCatLeft;
                    CatLeft.image.sprite = gameObjectCat[chooseCatLeft];
                    Update_Coin_User.Instance.DescreaseCoin(priceCat);
                    StartCoroutine(SendUserDataRoutine(idUser, index));
                    ShowCoin();
                }
                else
                {
                    panelConfirm.SetActive(true);
                    Debug.Log("Bạn k đủ tiền mua mèo");
                }
            }
        }
    }
    IEnumerator SendUserDataRoutine(string id_user, string id_cat)
    {
        WWWForm form = new WWWForm();
        form.AddField("ID_User", id_user);
        form.AddField("ID_Cat", id_cat);
        using (UnityWebRequest www = UnityWebRequest.Post(link_Api_InsertCat, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Lỗi: " + www.error);
            }
            else
            {
                Debug.Log("Dữ liệu đã được gửi thành công lên cat user!");
                Debug.Log("Phản hồi từ server: " + www.downloadHandler.text);
            }
        }
    }
    public void LoadItemCatRight()
    {
        ClearViewportItems();
        var list_Cat_User = Load_Cat_User.Instance.List_Cat_User;
        foreach (var item in List_Cat)
        {
            if (ManagerUsers.Instance._isLogin)
            {
                string idUser = ManagerUsers.Instance.ID_User_Login;
                var cat_user = list_Cat_User.FirstOrDefault(x => x.ID_Cat == item.ID && x.ID_User == idUser);
                if (cat_user != null)
                {
                    int id = Convert.ToInt32(item.ID);
                    GameObject catItem = Instantiate(ItemCat, curentTranform);
                    TextMeshProUGUI txtCoin = catItem.transform.Find("txtCoin").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI txtNameCat = catItem.transform.Find("txt_CatName").GetComponent<TextMeshProUGUI>();
                    Button cat = catItem.transform.Find("Button_Cat").GetComponent<Button>();
                    Sprite imgCat = gameObjectCat[id - 1];
                    cat.image.sprite = imgCat;
                    txtCoin.text = item.Price;
                    txtNameCat.text = item.Name;
                    int price = Convert.ToInt32(item.Price);
                    cat.onClick.AddListener(() => ClickChooseCatRight(item.ID, price));
                }
                else
                {
                    int id = Convert.ToInt32(item.ID);
                    GameObject catItem = Instantiate(ItemCat, curentTranform);
                    TextMeshProUGUI txtCoin = catItem.transform.Find("txtCoin").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI txtNameCat = catItem.transform.Find("txt_CatName").GetComponent<TextMeshProUGUI>();
                    Button cat = catItem.transform.Find("Button_Cat").GetComponent<Button>();
                    Sprite imgCat = gameObjectCat[id - 1];
                    cat.image.sprite = imgCat;
                    txtCoin.text = item.Price;
                    txtNameCat.text = item.Name;
                    int price = Convert.ToInt32(item.Price);
                    cat.onClick.AddListener(() => ClickChooseCatRight(item.ID, price));
                }
            }
            else
            {
                string idUser = ManagerUsers.Instance.ID_User;
                var cat_user = list_Cat_User.FirstOrDefault(x => x.ID_Cat == item.ID && x.ID_User == idUser);
                if (cat_user != null)
                {
                    int id = Convert.ToInt32(item.ID);
                    GameObject catItem = Instantiate(ItemCat, curentTranform);
                    TextMeshProUGUI txtCoin = catItem.transform.Find("txtCoin").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI txtNameCat = catItem.transform.Find("txt_CatName").GetComponent<TextMeshProUGUI>();
                    Button cat = catItem.transform.Find("Button_Cat").GetComponent<Button>();
                    Sprite imgCat = gameObjectCat[id - 1];
                    cat.image.sprite = imgCat;
                    txtCoin.text = "Chọn";
                    txtNameCat.text = item.Name;
                    int price = Convert.ToInt32(item.Price);
                    cat.onClick.AddListener(() => ClickChooseCatRight(item.ID, price));
                }
                else
                {
                    int id = Convert.ToInt32(item.ID);
                    GameObject catItem = Instantiate(ItemCat, curentTranform);
                    TextMeshProUGUI txtCoin = catItem.transform.Find("txtCoin").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI txtNameCat = catItem.transform.Find("txt_CatName").GetComponent<TextMeshProUGUI>();
                    Button cat = catItem.transform.Find("Button_Cat").GetComponent<Button>();
                    Sprite imgCat = gameObjectCat[id - 1];
                    cat.image.sprite = imgCat;
                    txtCoin.text = item.Price;
                    txtNameCat.text = item.Name;
                    int price = Convert.ToInt32(item.Price);
                    cat.onClick.AddListener(() => ClickChooseCatRight(item.ID, price));
                }
            }
        }
    }
    private void ClickChooseCatRight(string index, int priceCat)
    {
        var list_Cat_User = Load_Cat_User.Instance.List_Cat_User;
        if (ManagerUsers.Instance._isLogin)
        {
            string idUser = ManagerUsers.Instance.ID_User_Login;
            var cat_user = list_Cat_User.FirstOrDefault(x => x.ID_Cat == index && x.ID_User == idUser);
            if (cat_user != null)
            {
                int id = Convert.ToInt32(index);
                ManagerChooseCat.Instance.SetCatRight(id - 1);
                int chooseCatRight = ManagerChooseCat.Instance.chooseCatRight;
                CatRight.image.sprite = gameObjectCat[chooseCatRight];
            }
            else
            {
                int check = GameManager.Instance.total_Coin - priceCat;
                if (check >= 0)
                {
                    int id = Convert.ToInt32(index);
                    ManagerChooseCat.Instance.SetCatRight(id - 1);
                    int chooseCatRight = ManagerChooseCat.Instance.chooseCatRight;
                    CatRight.image.sprite = gameObjectCat[chooseCatRight];
                    Update_Coin_User.Instance.DescreaseCoin(priceCat);
                    StartCoroutine(SendUserDataRoutine(idUser, index));
                    ShowCoin();
                }
                else
                {
                    panelConfirm.SetActive(true);
                    Debug.Log("Bạn k đủ tiền mua mèo");
                }
            }
        }
        else
        {
            string idUser = ManagerUsers.Instance.ID_User;
            var cat_user = list_Cat_User.FirstOrDefault(x => x.ID_Cat == index && x.ID_User == idUser);
            if (cat_user != null)
            {
                int id = Convert.ToInt32(index);
                ManagerChooseCat.Instance.SetCatRight(id - 1);
                int chooseCatRight = ManagerChooseCat.Instance.chooseCatRight;
                CatRight.image.sprite = gameObjectCat[chooseCatRight];
            }
            else
            {
                int check = GameManager.Instance.total_Coin - priceCat;
                if (check >= 0)
                {
                    int id = Convert.ToInt32(index);
                    ManagerChooseCat.Instance.SetCatRight(id - 1);
                    int chooseCatRight = ManagerChooseCat.Instance.chooseCatRight;
                    CatRight.image.sprite = gameObjectCat[chooseCatRight];
                    Update_Coin_User.Instance.DescreaseCoin(priceCat);
                    StartCoroutine(SendUserDataRoutine(idUser, index));
                    ShowCoin();
                }
                else
                {
                    panelConfirm.SetActive(true);
                    Debug.Log("Bạn k đủ tiền mua mèo");
                }
            }
        }
    }
    public void btnClickCloseConfirm()
    {
        panelConfirm.SetActive(false);
    }
    public void ClearViewportItems()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public IEnumerator DownLoadFile(string Url, string path)
    {
        UnityWebRequest www = UnityWebRequest.Get(Url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            System.IO.File.WriteAllBytes(path, www.downloadHandler.data);
        }
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
                string filePath = Application.persistentDataPath + "/dataCat.json";
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
                string filePath = Application.persistentDataPath + "/dataCat.json";
                System.IO.File.WriteAllText(filePath, jsonAPI);
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
}

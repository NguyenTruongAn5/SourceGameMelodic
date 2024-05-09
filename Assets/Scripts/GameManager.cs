using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(GameManager).Name);
                    _instance = singletonObject.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    [SerializeField] TextMeshProUGUI txtScore;
    [SerializeField] GameObject panelWinGame;
    [SerializeField] GameObject panelWinBackGame;
    [SerializeField] GameObject panelLosseGame;
    [SerializeField] GameObject GroupStar;
    [SerializeField] GameObject ButtonPause;
    [SerializeField] TextMeshProUGUI txtNameMusic;
    public int score;
    public int coin;
    public int total_Coin = 0;
    public int _isStatusGame = 0;
    public bool _isAudioGame = true;
    public bool _isAudioBackground = true;
    public int diamondPlayer = 100;
    public bool _isBuyPlay = false;
    public bool CheckedNext = false;
    public int indexMusic;
    public int countTotalStar = 0;
    //0 , 1 beging game, 2, end game, 3 win game
    private void Start()
    {
        CheckSaveLoginPlayer();
        Update_Coin_User.Instance.SetTotalCoin();
    }
    private void Update()
    {
        UpdateTxtScore();
        StartCoroutine(SetWinGame());
        SetLosseGame();
    }
    public void UpdateTxtScore()
    {
        txtScore.text = $"{score}";
    }
    public void SetScore(int inScore)
    {
        score += inScore;
    }
    public void SetStatus(int isStatus)
    {
        _isStatusGame = isStatus;
    }
    public void SetPlayAudioGame(bool isStatus)
    {
        _isAudioGame = isStatus;
    }
    public void SetPlayAudioBackground(bool isStatus)
    {
        _isAudioBackground = isStatus;
    }
    public void SetBuyPlay(bool value)
    {
        _isBuyPlay = value;
    }
    public IEnumerator SetWinGame()
    {
        if (_isStatusGame == 3)
        {
            yield return new WaitForSeconds(1f);
            panelWinGame.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void SetLosseGame()
    {
        if (_isStatusGame == 2)
        {
            SetCoin(countTotalStar);
            panelLosseGame.SetActive(true);
            Time.timeScale = 0f;
            _isAudioGame = false;
        }
    }
    public void ContinueGameWithDiamond()
    {
        total_Coin -= 1000;
        if (total_Coin > 0)
        {
            score += 3;
            _isStatusGame = 0;
            panelLosseGame.SetActive(false);
            Time.timeScale = 1f;
            _isAudioGame = true;
            _isBuyPlay = true;
        }
        else
        {
            Debug.Log("Bạn đã hết vàng!");
        }
        Update_Coin_User.Instance.DescreaseCoin(1000);
    }
    public void BackHome()
    {
        total_Coin += coin;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Home");
    }
    public void WinGameBack()
    {
        panelWinGame.SetActive(false);
        panelWinBackGame.SetActive(true);
        GroupStar.SetActive(false);
        ButtonPause.SetActive(false);
        int id = indexMusic++;
        if (id >= LoadListMusic.Instance.ListMusic.Count)
        {
            id = 0;
        }
        txtNameMusic.text = LoadListMusic.Instance.ListMusic[id].MusicName;
    }
    public void NextMusic()
    {
        CheckedNext = true;
        indexMusic = LoadListMusic.Instance.index;
        indexMusic++;
        if (indexMusic >= LoadListMusic.Instance.ListMusic.Count)
        {
            indexMusic = 0;
        }
        StartCoroutine(LoadGameScene("NextMusic"));
    }
    private IEnumerator LoadGameScene(string name)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }
    public void SetCoin(int value)
    {
        if(value == 0)
        {
            coin = score;
        }
        else
        {
            coin = value * score;
        }
    }
    public void SetTotalStar(int value)
    {
        countTotalStar= value;
    }
    public void SetTotalCoin(int value)
    {
        total_Coin = value;
    }
    private void CheckSaveLoginPlayer()
    {
        string filePath = Application.persistentDataPath + "/CheckSaveLoginPlayer.json";
        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            ManagerUsers.Instance.SetID_UserLogin(jsonContent);
            ManagerUsers.Instance.SetStatusLogin(true);
        }
    }
}

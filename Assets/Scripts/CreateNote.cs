using Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CreateNote : MonoBehaviour
{
    [SerializeField] GameObject Item;
    [SerializeField] Transform landLeft1;
    [SerializeField] Transform landLeft2;
    [SerializeField] Transform landLeft3;
    [SerializeField] Transform landRight1;
    [SerializeField] Transform landRight2;
    [SerializeField] Transform landRight3;
    [SerializeField] TextMeshProUGUI txtNameMusic;
    private AudioSource _audioSource;
    string path;
    string pathMp3;
    List<TileBeat> _tileDatas;
    int _currentBeatIndex = 0;
    float _currentTime = 0;
    float _timeSpawn = 9f / 3.7f;
    float _tempoBPM;
    float _timeoffStrong;
    bool _isPlayAudio = false;
    bool _isSpawnAudio = false;
    bool _checkPauseAudio = false;
    float _audioPosition;
    float currentTimeAudio = 0;
    private bool isMP3Downloaded = false;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _tileDatas = new List<TileBeat>();
        pathMp3 = LoadListMusic.Instance.mp3Click;
        StartCoroutine(LoadMp3(pathMp3));
        path = LoadListMusic.Instance.midiClick;
        ReadNoteFile(path);
    }
    private void Update()
    {
        if (GameManager.Instance._isStatusGame == 1)
        {
            _isSpawnAudio = true;
            currentTimeAudio += Time.deltaTime;
        }
        if (_isSpawnAudio)
        {
            StarSpawn();
            CheckSpawnTime();
        }
        if (!GameManager.Instance._isAudioGame)
        {
            _audioPosition = _audioSource.time;
            _audioSource.Pause();
            _checkPauseAudio = true;
        }
        else
        {
            if (_checkPauseAudio)
            {
                PlayAudio();
            }
        }
        CheckBuyPlay();
        CheckWinGame();
    }
    private void CheckWinGame()
    {
        if (currentTimeAudio >= _audioSource.clip.length + 2)
        {
            GameManager.Instance.SetStatus(3);//Win game
        }
    }
    private void CheckBuyPlay()
    {
        if (GameManager.Instance._isBuyPlay)
        {
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            for (int i = 0; i < 3; i++)
            {
                Destroy(items[i]);
            }
            GameManager.Instance.SetBuyPlay(false);
        }
    }
    public void ReadNoteFile(string path)
    {
        MidiParser midiParser = new MidiParser(path);
        _tempoBPM = midiParser.TempoBPM;
        int totalBeat = 0;
        foreach (var track in midiParser.Tracks)
        {
            foreach (var beat in track.Beats)
            {
                var tileBeat = new TileBeat();
                tileBeat.currentTime = beat.CurrentTime / 1000;
                tileBeat.beatIndex = _tileDatas.Count;
                tileBeat.notes = new List<TileNote>();
                _timeoffStrong = (float)beat.Notes[0].TimeOff / _tempoBPM;
                foreach (var note in beat.Notes)
                {
                    var tileNote = new TileNote();
                    tileNote.number = note.Number;
                    tileNote.timeOff = note.TimeOff;
                    if (note.TimeOff > _timeoffStrong * _tempoBPM)
                    {
                        tileNote.type = TileType.Strong;
                    }
                    else
                    {
                        tileNote.type = TileType.Normal;
                    }
                    tileBeat.notes.Add(tileNote);
                }
                _tileDatas.Add(tileBeat);
                totalBeat++;
            }
        }
        _tileDatas = _tileDatas.OrderBy(x => x.currentTime).ToList();
        _currentBeatIndex = 0;
        _currentTime = 0;
        CreateStar.Instance.SetTotalBeat(totalBeat);
    }
    void CheckSpawnTime()
    {
        CreateStar.Instance.SetCurrentBeat(_currentBeatIndex);
        if (_currentBeatIndex < _tileDatas.Count)
        {
            foreach (var item in _tileDatas[_currentBeatIndex].notes)
            {
                if (_currentBeatIndex < _tileDatas.Count
                   && _currentTime >= _tileDatas[_currentBeatIndex].currentTime)
                {
                    int height = Mathf.CeilToInt((float)item.timeOff / _tempoBPM);
                    switch (item.number)
                    {
                        case 74:
                            if (item.type == TileType.Strong)
                            {
                                StartCoroutine(CreateTilesScale(landLeft1, height));
                                break;
                            }
                            else
                            {
                                CreateTiles(landLeft1);
                                break;
                            }
                        case 75:
                            if (item.type == TileType.Strong)
                            {
                                StartCoroutine(CreateTilesScale(landLeft2, height));
                                break;
                            }
                            else
                            {
                                CreateTiles(landLeft2);
                                break;
                            }
                        case 76:
                            if (item.type == TileType.Strong)
                            {
                                StartCoroutine(CreateTilesScale(landLeft3, height));
                                break;
                            }
                            else
                            {
                                CreateTiles(landLeft3);
                                break;
                            }
                        case 78:
                            if (item.type == TileType.Strong)
                            {
                                StartCoroutine(CreateTilesScale(landRight1, height));
                                break;
                            }
                            else
                            {
                                CreateTiles(landRight1);
                                break;
                            }
                        case 79:
                            if (item.type == TileType.Strong)
                            {
                                StartCoroutine(CreateTilesScale(landRight2, height));
                                break;
                            }
                            else
                            {
                                CreateTiles(landRight2);
                                break;
                            }
                        case 80:
                            if (item.type == TileType.Strong)
                            {
                                StartCoroutine(CreateTilesScale(landRight3, height));
                                break;
                            }
                            else
                            {
                                CreateTiles(landRight3);
                                break;
                            }
                    }
                    _currentBeatIndex++;
                }
            }
            _currentTime += Time.deltaTime;
        }
    }
    IEnumerator StarSpawn()
    {
        if (!_isPlayAudio)
        {
            yield return new WaitForSeconds(_timeSpawn);

            if (_audioSource != null)
            {
                _audioSource.Play();
                _isPlayAudio = true;
            }
        }
    }
    void PlayAudio()
    {
        if (_checkPauseAudio)
        {
            if (_audioSource != null)
            {
                _audioSource.time = _audioPosition;
                _audioSource.Play();
                _checkPauseAudio = false;
            }
        }
    }
    IEnumerator LoadMp3(string fileName)
    {
        string fullPath = "file://" + fileName;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(fullPath, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                _audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
            }
            else
            {
                Debug.LogError("Error loading MP3: " + www.error);
            }
        }
    }
    public void CreateTiles(Transform pos)
    {
        Instantiate(Item, pos.position, Quaternion.identity);
    }
    public IEnumerator CreateTilesScale(Transform pos, int timeoff)
    {
        if (timeoff > 1)
        {
            timeoff--;
        }
        for (int i = 0; i < timeoff; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject obj = Instantiate(Item, pos.position, Quaternion.identity);
        }
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
    public struct TileBeat
    {
        public int beatIndex;
        public double currentTime;
        public List<TileNote> notes;
    }
    public struct TileNote
    {
        public int number;
        public bool leftSide;
        public TileType type;
        public int chanel;
        public double timeOff;
    }
    public enum TileType
    {
        Normal,
        Star,
        Coin,
        Strong
    }
}

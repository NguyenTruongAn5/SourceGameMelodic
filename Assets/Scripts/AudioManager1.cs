using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager1 : MonoBehaviour
{
    [SerializeField] GameObject audioManager;
    private static AudioManager1 _instance;
    public static AudioManager1 Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager1>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("AudioManager1");
                    _instance = singletonObject.AddComponent<AudioManager1>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }
    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Game")
        {
            audioManager.SetActive(false);
        }
    }
}

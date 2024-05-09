using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScence : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    public void LoadNewScence(string nameScence)
    {
        SceneManager.LoadScene(nameScence);
    }
}

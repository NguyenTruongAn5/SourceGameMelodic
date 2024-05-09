using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] GameObject pauseMenu;

	public void Pause()
	{
		pauseMenu.SetActive(true);
		Time.timeScale = 0;
		GameManager.Instance.SetPlayAudioGame(false);
	}
	public void HomeBack()
	{
        Time.timeScale = 1f;
        SceneManager.LoadScene("Home");
    }
	public void ContinueGame()
	{
		Time.timeScale = 1f;
        GameManager.Instance.SetPlayAudioGame(true);
        pauseMenu.SetActive(false);
    }
}

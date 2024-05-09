using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
	private Text scoreText;

	private int score = 0;
	// Start is called before the first frame update
	void Start()
	{

	}
	private void Awake()
	{
		scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
		scoreText.text = "0";
	}
	void OnTriggerEnter2D(Collider2D target)
	{
		if (target.tag == "gai")
		{
			transform.position = new Vector2(0, 100);
			target.gameObject.SetActive(false);
			StartCoroutine(RestartGame());
		}
		if (target.tag == "Item")
		{
			target.gameObject.SetActive(false);
			score++;
			scoreText.text = score.ToString();

		}
		IEnumerator RestartGame()
		{
			yield return new WaitForSecondsRealtime(2f);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

	}

	// Update is called once per frame
	void Update()
	{

	}
}

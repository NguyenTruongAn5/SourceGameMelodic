using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioClip[] audioClips;

	private AudioSource audioSource;

	private string[] scenesNeedMusic = { "Home", "ListCat", "NextMusic" };

	private int currentSongIndex = -1;

	private bool musicEnabled = true;

	public Toggle[] sceneToggles; // Mảng các Toggle Button để chọn các scene không muốn phát nhạc

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject); // Đánh dấu GameObject chứa script AudioManager không bị hủy khi chuyển scene
		}
		else
		{
			Destroy(gameObject);
			return; // Ngăn script được khởi tạo nhiều lần
		}

		audioSource = GetComponent<AudioSource>();
		audioSource.loop = true;

		SceneManager.sceneLoaded += OnSceneLoaded; // Đăng ký sự kiện để tự động gọi khi một scene được load
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		// Khi scene được load, kiểm tra xem AudioManager cần phát nhạc hay không
		if (IsMusicScene(scene.name) && musicEnabled)
		{
			PlaySong(currentSongIndex);
		}
		else
		{
			StopSong();
		}
	}

	public void PlaySong(int songIndex)
	{
		if (songIndex >= 0 && songIndex < audioClips.Length)
		{
			audioSource.Stop();
			audioSource.clip = audioClips[songIndex];
			audioSource.Play();
			currentSongIndex = songIndex;
		}
		else
		{
			Debug.LogWarning("Invalid song index: " + songIndex);
		}
	}

	public void ToggleMusic()
	{
		musicEnabled = !musicEnabled;

		if (!musicEnabled)
		{
			StopSong();
		}
		else
		{
			// Khi âm nhạc được bật lại, kiểm tra xem AudioManager cần phát nhạc hay không
			if (IsMusicScene(SceneManager.GetActiveScene().name))
			{
				PlaySong(currentSongIndex);
			}
		}
	}

	public void StopSong()
	{
		audioSource.Stop();
		currentSongIndex = -1;
	}

	private bool IsMusicScene(string sceneName)
	{
		foreach (string name in scenesNeedMusic)
		{
			if (name == sceneName)
			{
				return true;
			}
		}
		return false;
	}
}

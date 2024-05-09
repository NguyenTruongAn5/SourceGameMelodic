using UnityEngine;
using UnityEngine.UI;

public class MusicSceneController : MonoBehaviour
{
    public AudioManager audioManager; // Gán AudioManager vào trên inspector

    public Toggle[] sceneToggles; // Mảng các Toggle Button

    void Start()
    {
        // Gắn hàm OnValueChanged cho mỗi Toggle Button
        foreach (Toggle toggle in sceneToggles)
        {
            toggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(toggle); });
        }
    }

    // Xử lý sự kiện khi giá trị của Toggle Button thay đổi
    void OnToggleValueChanged(Toggle toggle)
    {
        if (!toggle.isOn)
        {
            // Tắt nhạc nền nếu Toggle Button được chọn
            audioManager.StopSong();
        }
    }
}

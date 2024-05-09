using UnityEngine;

public class ManagerChooseCat : MonoBehaviour
{
    private static ManagerChooseCat _instance;
    public static ManagerChooseCat Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ManagerChooseCat>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("ManagerChooseCat");
                    _instance = singletonObject.AddComponent<ManagerChooseCat>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }
    public int chooseCatLeft = 1;
    public int chooseCatRight;
    public void SetCatLeft(int value)
    {
        chooseCatLeft = value;
    }
    public void SetCatRight(int value)
    {
        chooseCatRight = value;
    }
}

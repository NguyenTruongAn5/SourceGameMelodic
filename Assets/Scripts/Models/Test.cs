using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    private static Test _instance;
    public static Test Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Test>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(Test).Name);
                    _instance = singletonObject.AddComponent<Test>();
                }
            }
            return _instance;
        }
    }
    public bool checkClickLeft = false;
    private void Start()
    {
        EventTrigger evtrig = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry moveEvent = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerClick 
        };
        moveEvent.callback.AddListener((data) => { moveCharacter(); }); 
        evtrig.triggers.Add(moveEvent);
    }

    public void moveCharacter()
    {
        checkClickLeft = true;
    }
    public void SetClickLeft()
    {
        checkClickLeft = false;
    }
}

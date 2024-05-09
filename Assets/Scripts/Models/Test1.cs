using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test1 : MonoBehaviour
{
    private static Test1 _instance;
    public static Test1 Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Test1>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(Test1).Name);
                    _instance = singletonObject.AddComponent<Test1>();
                }
            }
            return _instance;
        }
    }
    public bool checkClickRight = false;
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
        checkClickRight = true;
    }
    public void SetClickLeft()
    {
        checkClickRight = false;
    }
}

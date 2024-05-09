using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    public static ControlPlayer Instance { get; private set; }
    public float speed = 1.4f;
    public GameObject leftObject;
    public GameObject rightObject;
    [SerializeField] GameObject HandLeft;
    [SerializeField] GameObject HandRight;
    [SerializeField] GameObject txtTutorial;
    private int _indexTouchLeft = 0;
    private int _indexTouchRight = 0;
    private bool _isControl = true;
    public Transform posLeft;
    public Transform posRight;
    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }
    private void Start()
    {
        posRight = rightObject.transform;
        posLeft = leftObject.transform;
    }
    void Update()
    {
        if (Input.touchCount == 1)
        {
            if(GameManager.Instance._isStatusGame != 1)
            {
                GameManager.Instance.SetStatus(1);
            }
            HandLeft.SetActive(false);
            HandRight.SetActive(false);
            txtTutorial.SetActive(false);
        }
        CheckPointTouch();
        if (_isControl)
        {
            ControlPlayerLeft();
        }
        else
        {
            ControlPlayerRight();
        }
        if (Input.touchCount == 2)
        {
            _indexTouchRight = 1;
            ControlPlayerLeft();
            ControlPlayerRight();
        }
        else
        {
            _indexTouchRight = 0;
        }
    }
    private void CheckPointTouch()
    {
        Touch touchLeft = Input.GetTouch(0);
        if (touchLeft.phase == TouchPhase.Began)
        {
            float posionTouchX;
            posionTouchX = touchLeft.position.x;
            if (posionTouchX > Screen.width / 2)
            {
                _isControl = false;
            }
            else
            {
                _isControl = true;
            }
        }
    }
    private bool canMoveLeft = true;
    public void ControlPlayerLeft()
    {
        if (Input.touchCount > 0)
        {
            Touch touchLeft = Input.GetTouch(_indexTouchLeft);
            Vector3 touchPostion = Camera.main.ScreenToWorldPoint(touchLeft.position);
            Vector3 positionLeft = leftObject.transform.position;
            float distance = Vector3.Distance(touchPostion, positionLeft);
            Debug.Log($"Khoảng cách chạm và mèo {distance}");
            if (touchLeft.phase == TouchPhase.Moved)
            {
                if (canMoveLeft)
                {
                    MoveObject(leftObject, touchPostion.x);
                }
                //ChangeFace(leftObject, touchPostion.x);
                posLeft = leftObject.transform;
                if (posLeft.position.x < -1.7)
                {
                    MoveObject(leftObject, -1.7f);
                    canMoveLeft = false;
                }
                else
                {
                    canMoveLeft = true;
                }
                if (posLeft.position.x > -0.6)
                {
                    MoveObject(leftObject, -0.6f);
                    canMoveLeft = false;
                }
                else
                {
                    canMoveLeft = true;
                }
            }
        }
    }
    public void ControlPlayerRight()
    {
        if (Input.touchCount > 0)
        {
            Touch touchRight = Input.GetTouch(_indexTouchRight);
            Vector3 touchPostion = Camera.main.ScreenToWorldPoint(touchRight.position);
            if (touchRight.phase == TouchPhase.Moved)
            {
                MoveObject(rightObject, touchPostion.x);
                //ChangeFace(rightObject, touchPostion.x);
                posRight = rightObject.transform;
                if (posRight.position.x < 0.6)
                {
                    MoveObject(rightObject, 0.6f);
                }
                if (posRight.position.x > 1.7)
                {
                    MoveObject(rightObject, 1.7f);
                }
            }
        }
    }
    void MoveObject(GameObject obj, float newX)
    {
        Vector3 newPosition = new
            Vector3(newX, obj.transform.position.y, 0f);
        obj.transform.position = newPosition;
    }
    private void ChangeFace(GameObject obj, float touchPossion)
    {
        float currentPosX = obj.transform.position.x;
        if (currentPosX - touchPossion <= 0)
        {
            obj.transform.localScale = new
                Vector3(-obj.transform.localScale.x, obj.transform.localScale.y);
        }
        else if (touchPossion > 0)
        {
            leftObject.transform.localScale = new
                Vector3(obj.transform.localScale.x, obj.transform.localScale.y);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Animator _animiEat;
    public bool _isCollision;
    private void Update()
    {
        if (GameManager.Instance._isStatusGame == 2)
        {
            SetAnimationCry(1);
        }
        else
        {
            SetAnimationCry(0);
        }
        if(_isCollision)
        {
            SetOnAnimationEat(1);
        }
        else
        {
            SetOnAnimationEat(0);
        }
    }
    private void Start()
    {
        _animiEat = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Item"))
        {
            GameManager.Instance.SetScore(1);
            _isCollision = true;
        }
        if (col.gameObject.CompareTag("Star"))
        {
            CreateStar.Instance.SetActiveStar(true);
            _isCollision = true;
            Destroy(col.gameObject);
        }
    }
    public void SetOnAnimationEat(float value)
    {
        _animiEat.SetFloat("Eating", value);
        _isCollision= false;
    }
    public void SetAnimationCry(float value)
    {
        _animiEat.SetFloat("Crying", value);
    }
}

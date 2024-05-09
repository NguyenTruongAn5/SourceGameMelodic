using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItemColliderBase : MonoBehaviour
{
    private Animator _animiDestroy;
    private void Start()
    {
        _animiDestroy = GetComponent<Animator>();
    }
    private void SetOnDestroy(float value)
    {
        _animiDestroy.SetFloat("Destroy", value);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("DeathZone"))
        {
            GameManager.Instance.SetStatus(2);
            SetOnDestroy(1);
            Destroy(gameObject);
        }
    }
}
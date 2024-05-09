using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunItem : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MoveItem();
    }
    public void MoveItem()
    {
        rb.velocity = new Vector2(0f,-Time.deltaTime*speed);
    }
}

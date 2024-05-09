using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	public float moveSpeed;
	public KeyCode left;
	public KeyCode right;
	private Rigidbody2D theRB;
	void Start()
	{
		theRB = GetComponent<Rigidbody2D>();
	}
	void Update()
	{
		if (Input.GetKey(left))
		{
			theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
		}
		else if (Input.GetKey(right))
		{
			theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
		}
	}
}

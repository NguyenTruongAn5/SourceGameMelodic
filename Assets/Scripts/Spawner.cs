using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[SerializeField]
	private GameObject[] item;

	private BoxCollider2D col;

	float x1, x2;
	void Awake()
	{
		col = GetComponent<BoxCollider2D>();
		x1 = transform.position.x - col.bounds.size.x / 2f;
		x2 = transform.position.x + col.bounds.size.x / 2f;
	}
	private void Start()
	{
		StartCoroutine(SpawnItem(Random.Range(1f, 2f)));

	}
	IEnumerator SpawnItem(float time)
	{
		yield return new WaitForSecondsRealtime(time);


		Vector3 temp = transform.position;
		temp.x = Random.Range(x1, x2);

		Instantiate(item[Random.Range(0, item.Length)], temp, Quaternion.identity);

		StartCoroutine(SpawnItem(Random.Range(1f, 2f)));

	}

}

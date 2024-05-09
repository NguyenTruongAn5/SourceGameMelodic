using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItem : MonoBehaviour
{
    private Animator _animition;
    private void Start()
    {
        _animition= GetComponent<Animator>();
    }
    private void Update()
    {
        CheckCollider();
    }
    public void CheckCollider()
    {
        Transform posCatLeft = ControlPlayer.Instance.posLeft;
        Transform posCatRight = ControlPlayer.Instance.posRight;
        float posXItem = gameObject.transform.position.x;
        float posYItem = gameObject.transform.position.y;

        float leftCatLeft_X = posCatLeft.position.x - .35f;
        float rightCatLeft_X = posCatLeft.position.x + .35f;
        float topCatLeft_Y = posCatLeft.position.y + 0.4f;
        float botCatLeft_Y = posCatLeft.position.y - 0.2f;

        float leftCatRight_X = posCatRight.position.x - .35f;
        float rightCatRight_X = posCatRight.position.x + .35f;
        float topCatRight_Y = posCatRight.position.y + 0.4f;
        float botCatRight_Y = posCatRight.position.y - 0.2f;

        if (posXItem > leftCatLeft_X && posXItem < rightCatLeft_X && posYItem < topCatLeft_Y && posYItem > botCatLeft_Y)
        {
            SetOnAnimiton(1);
        }
        if (posXItem > leftCatRight_X && posXItem < rightCatRight_X && posYItem < topCatRight_Y && posYItem > botCatRight_Y)
        {
            SetOnAnimiton(1);
        }
    }
    private void SetOnAnimiton(float value)
    {
        _animition.SetFloat("Destroy", value);
        StartCoroutine(WaitForAnimation());
    }
    IEnumerator WaitForAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}

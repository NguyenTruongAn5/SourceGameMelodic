using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeUI : MonoBehaviour
{
    // Start is called before the first frame update
    private Canvas canvas;
    void Start()
    {
        canvas = this.GetComponentInParent<Canvas>();
        float Rate_X;
        Transform transform = this.GetComponent<RectTransform>();

        Rate_X = canvas.pixelRect.width / 720;
        transform.localScale = new Vector3(transform.localScale.x * Rate_X, transform.localScale.y * Rate_X, 0.1f);

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ResizeGridLayout : MonoBehaviour
{
    // Start is called before the first frame update
    private GridLayoutGroup gridLayout;
    private Canvas canvas;
    void Start()
    {
        canvas = this.GetComponentInParent<Canvas>();
        float Rate_X;
        gridLayout = this.GetComponent<GridLayoutGroup>();

        Rate_X = canvas.pixelRect.width / 720;
        gridLayout.cellSize = new Vector2(Rate_X * gridLayout.cellSize.x, Rate_X * gridLayout.cellSize.y);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

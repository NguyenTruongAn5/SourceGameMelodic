using UnityEngine;

public class FitWork : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero) * 100;
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(mainCamera.rect.width, mainCamera.rect.height)) * 100;
        Vector3 screenSize = topRight - bottomLeft;

        float screenRatio = screenSize.x / screenSize.y;
        float desiredRatio = transform.localScale.x / transform.localScale.y;

        if (screenRatio == 0.5625)
        {
            float width = screenSize.x / 100;
            float height = screenSize.y / 100;
            float rate = width / transform.localScale.x;
            transform.localScale = new Vector3(transform.localScale.x * rate, transform.localScale.y * rate);
        }
        else
        {
            float width = screenSize.x / 100;
            float height = screenSize.y / 100;
            float rate_X = width / transform.localScale.x;
            float rate_Y = height / transform.localScale.y;



            if (width > transform.localScale.x)
            {
                transform.localScale = new Vector3(transform.localScale.x * rate_Y, transform.localScale.y * rate_Y);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x * rate_X, transform.localScale.y * rate_X);
            }
        }
    }
}

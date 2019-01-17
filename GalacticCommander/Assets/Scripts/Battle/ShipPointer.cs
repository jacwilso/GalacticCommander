using UnityEngine;

public class ShipPointer : MonoBehaviour
{
    Ship ship;

    Camera cam;
    RectTransform pointerRect;
    WorldToCanvas w2c;
    Vector2 canvasBorder;
    bool offset;

    void Start()
    {
        cam = Camera.main;
        pointerRect = GetComponent<RectTransform>();
        pointerRect.gameObject.SetActive(false);

        w2c = new WorldToCanvas(GetComponentInParent<Canvas>());
        canvasBorder = 0.05f * new Vector2(w2c.canvasRect.rect.width, w2c.canvasRect.rect.height);
    }

    void Update()
    {
        Vector2 screenPos2 = w2c.ConvertWorldToCanvas(ship.transform.position);
        //Vector3 screenPos = cam.WorldToScreenPoint(ship.transform.position);
        //if (screenPos.z > 0 &&
        //    screenPos.x > 0 && screenPos.x < Screen.width &&
        //    screenPos.y > 0 && screenPos.y < Screen.height)
        //{
        //    Vector2 viewportPos = Camera.main.WorldToViewportPoint(ship.transform.position);
        //    Vector2 screenPos2 = new Vector2(
        //        ((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
        //        ((viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
        //        );
        if (screenPos2.x > w2c.canvasRect.rect.xMin + canvasBorder.x && screenPos2.x < w2c.canvasRect.rect.xMax - canvasBorder.x &&
            screenPos2.y > w2c.canvasRect.rect.yMin + canvasBorder.y && screenPos2.y < w2c.canvasRect.rect.yMax - canvasBorder.y)
        {
            pointerRect.anchoredPosition = screenPos2 + 70f * Vector2.up;
            transform.rotation = Quaternion.identity;
            offset = true;
        }
        else // offscreen
        {
            Vector3 screenPos = cam.WorldToScreenPoint(ship.transform.position);
            if (screenPos.z < 0)
            {
                screenPos *= -1;
            }
            Vector3 screenCenter = 0.5f * new Vector3(Screen.width, Screen.height);
            screenPos -= screenCenter;

            float angle = Mathf.Atan2(screenPos.y, screenPos.x);
            angle += 90 * Mathf.Deg2Rad;
            Vector2 screenBounds = 0.5f * 0.9f * new Vector3(Screen.width, Screen.height);

            transform.localRotation = Quaternion.Euler(Vector3.forward * angle * Mathf.Rad2Deg);
            transform.localPosition += transform.up * 30f * (offset ? 1 : 0);
            offset = false;
        }
    }

    public void PointTo(Ship ship)
    {
        this.ship = ship;
        pointerRect.gameObject.SetActive(true);
    }
}

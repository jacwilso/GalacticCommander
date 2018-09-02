using UnityEngine;

public class ShipPointer : MonoBehaviour
{
    private Ship ship;

    private Camera cam;
    private RectTransform pointerRect, canvasRect;
    private Vector2 canvasBorder;
    private bool offset;

    private void Start()
    {
        cam = Camera.main;
        pointerRect = GetComponent<RectTransform>();
        pointerRect.gameObject.SetActive(false);

        canvasRect = GetComponentInParent<RectTransform>();
        canvasBorder = 0.05f * new Vector2(canvasRect.rect.width, canvasRect.rect.height);
    }

    private void Update()
    {
        Vector2 viewportPos = Camera.main.WorldToViewportPoint(ship.transform.position);
        Vector2 screenPos2 = new Vector2(
            ((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
            );
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
        if (screenPos2.x > canvasRect.rect.xMin + canvasBorder.x && screenPos2.x < canvasRect.rect.xMax - canvasBorder.x &&
            screenPos2.y > canvasRect.rect.yMin + canvasBorder.y && screenPos2.y < canvasRect.rect.yMax - canvasBorder.y)
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

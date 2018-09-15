using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToCanvas
{
    public RectTransform canvasRect;
    public Camera cam;

    public WorldToCanvas(Canvas canvas)
    {
        canvasRect = canvas.GetComponent<RectTransform>();
        cam = Camera.main;
    }

    public Vector2 ConvertWorldToCanvas(Vector3 worldPos)
    {
        if (canvasRect == null)
        {
            Debug.LogError("WorldToCanvas::Convert must call init in start");
        }
        Vector2 viewportPos = cam.WorldToViewportPoint(worldPos);
        Vector2 screenPos2 = new Vector2(
            ((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f))
            );
        return screenPos2;
    }
}

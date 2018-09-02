using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Segmenter : MonoBehaviour
{
    public int Segments
    {
        set
        {
            segments = value;
            // TODO PASS INFO TO SHADER
            SegmentIcons();
        }
    }

    [SerializeField]
    private float radius;
    [SerializeField]
    private SpriteRenderer icon;
    [SerializeField]
    private Transform iconParent;

    private int segments;

    private List<SpriteRenderer> icons;

    private void Start()
    {
        icons = new List<SpriteRenderer>();
    }

    public void SegmentIcons()
    {
        if (icons.Count < segments)
        {
            InstantiateIcons();
        }

        int num = segments + segments % 2;
        float angle = 360 / num;
        angle *= Mathf.Deg2Rad;
        for (int i = 0; i < segments; i++)
        {
            SpriteRenderer s = icons[i];
            s.transform.localPosition = new Vector2(
                radius * Mathf.Cos(angle * (i + 0.5f)),
                radius * Mathf.Sin(angle * (i + 0.5f)));
        }

        for (int i = segments; i < icons.Count; i++)
        {
            icons[i].gameObject.SetActive(false);
        }
    }

    private void InstantiateIcons()
    {
        for (int i = icons.Count; i < segments; i++)
            icons.Add(Instantiate(icon, iconParent));
    }
}

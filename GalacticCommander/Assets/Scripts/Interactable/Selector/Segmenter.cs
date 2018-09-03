using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIWheel
{

    [ExecuteInEditMode]
    public class Segmenter : MonoBehaviour
    {
        public int Segments
        {
            get { return segments; }
        }

        [SerializeField]
        private float radius;
        [SerializeField]
        private SpriteRenderer icon;
        [SerializeField]
        private Transform iconParent;

        private int segments;
        private MeshRenderer rend;
        private List<SpriteRenderer> iconPool;

        private void OnEnable()
        {
            rend = GetComponent<MeshRenderer>();
            iconPool = new List<SpriteRenderer>();
        }

        private void SegmentLines()
        {
            float angle = 2f * Mathf.PI / segments;
            float outerRadius = rend.material.GetFloat("_OuterRadius");
            Vector4[] _LineSegments = new Vector4[segments];
            for (int i = 0; i < segments; i++)
            {
                _LineSegments[i] = new Vector4(
                    outerRadius * Mathf.Cos(angle * i),
                    outerRadius * Mathf.Sin(angle * i),
                    0, 0
                    );
            }
            rend.sharedMaterial.SetFloat("_Segments", segments);
            rend.sharedMaterial.SetVectorArray("_LineSegments", _LineSegments);
        }

        private void SegmentIcons(List<Sprite> icons)
        {
            IconPool();
            float angle = 360 / segments;
            angle *= Mathf.Deg2Rad;
            for (int i = 0; i < segments; i++)
            {
                SpriteRenderer s = iconPool[i];
                s.transform.localPosition = new Vector2(
                    radius * Mathf.Cos(angle * (i + 0.5f)),
                    radius * Mathf.Sin(angle * (i + 0.5f)));
                s.sprite = icons[i];
            }
        }

        private void IconPool()
        {
            for (int i = iconPool.Count; i < segments; i++)
            {
                iconPool.Add(Instantiate(icon, iconParent));
            }
            for (int i = segments; i < iconPool.Count; i++)
            {
                iconPool[i].gameObject.SetActive(false);
            }
        }

        public void SetSegments(List<Sprite> icons)
        {
            segments = icons.Count;
            SegmentIcons(icons);
            SegmentLines();
        }
    }
}
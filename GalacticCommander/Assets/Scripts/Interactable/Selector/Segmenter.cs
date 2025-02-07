﻿using System.Collections.Generic;
using System.Linq;
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
        float radius = 3;
        [SerializeField]
        UnityEngine.UI.Image iconPrefab = null;
        [SerializeField]
        Transform iconParent = null;
        [SerializeField]
        Material segmentMaterial = null;
        [SerializeField]
        int segments;

        List<UnityEngine.UI.Image> iconPool;

        void OnEnable()
        {
            if (iconPool == null)
            {
                iconPool = new List<UnityEngine.UI.Image>();
            }
        }

        [ContextMenu("Segment Lines")]
        void SegmentLinesTest()
        {
            SegmentLines(Enumerable.Repeat(true, segments).ToList());
        }

        void SegmentLines(List<bool> availableActions)
        {
            if (segments <= 1)
            {
                segmentMaterial.SetFloat("_Segments", 0);
                return;
            }
            float angle = 2f * Mathf.PI / segments;
            float outerRadius = segmentMaterial.GetFloat("_OuterRadius");
            Vector4[] _LineSegments = new Vector4[segments];
            for (int i = 0; i < segments; i++)
            {
                _LineSegments[i] = new Vector4(
                    outerRadius * Mathf.Cos(angle * i),
                    outerRadius * Mathf.Sin(angle * i),
                    availableActions[(i + 2) % segments] ? 1 : 0, 0
                    );
            }
            segmentMaterial.SetFloat("_Angle", angle);
            segmentMaterial.SetFloat("_Segments", segments);
            segmentMaterial.SetVectorArray("_LineSegments", _LineSegments);
        }

        void SegmentIcons(List<Sprite> icons)
        {
            IconPool();

            float angle = 2f * Mathf.PI / segments;
            float offset = 0.5f * angle * (1 - segments % 2);
            for (int i = 0; i < segments; i++)
            {
                UnityEngine.UI.Image s = iconPool[i];
                s.transform.localPosition = new Vector2(
                    radius * Mathf.Cos(offset + angle * i),
                    radius * Mathf.Sin(offset + angle * i));
                s.sprite = icons[i];
            }
        }

        void IconPool()
        {
            for (int i = iconPool.Count; i < segments; i++)
            {
                iconPool.Add(Instantiate(iconPrefab, iconParent));
            }
            for (int i = segments; i < iconPool.Count; i++)
            {
                iconPool[i].gameObject.SetActive(false);
            }
        }

        public void SetSegments(List<Sprite> icons, List<bool> availableActions)
        {
            segments = icons.Count;
            SegmentIcons(icons);
            SegmentLines(availableActions);
        }
    }
}
using System;
using UnityEngine;

namespace UIWheel
{
    public struct SegmentIcon
    {
        public Type segment;
        public Sprite icon;

        public SegmentIcon(Type segment, Sprite icon)
        {
            if (segment == typeof(Segment))
            {
                Debug.LogError("SegmentIcon segment must be type of Segment.");
            }
            this.segment = segment;
            this.icon = icon;
        }
    }
}
using FirstModProject.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirstModProject.HitMarker
{
    [System.Serializable]
    public class HitMarkerData
    {
        public float Duration { get; private set; }
        public float ScaleStart { get; private set; }
        public float ScaleEnd { get; private set; } 
        public Vector2 Size { get; private set; }   
        public Color Color { get; private set; }

        public HitMarkerData()
        {
           
            Duration = ModConstants.HITMARKER_DURATION;
            ScaleStart = ModConstants.HITMARKER_SCALE_START;
            ScaleEnd = ModConstants.HITMARKER_SCALE_END;
            Size = new Vector2(ModConstants.HITMARKER_SIZE, ModConstants.HITMARKER_SIZE);
            Color = ModConstants.HITMARKER_COLOR;
        }
        public HitMarkerData(float duration, float scaleStart, float scaleEnd, Vector2 size, Color color)
        {
            Duration = duration;
            ScaleStart = scaleStart;
            ScaleEnd = scaleEnd;
            Size = size;
            Color = color;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatrixJam.Team11
{
    [System.Serializable]
    public struct SceneTransition
    {
        public float FadeOutTime;
        public float FadeInTime;
        public float FadedDuration;
        public Color FadeColor;

        public SceneTransition(
            float fadeOutTime,
            float fadeInTime,
            float fadedDuration,
            Color fadeColor
            )
        {
            this.FadeOutTime = fadeOutTime;
            this.FadeInTime = fadeInTime;
            this.FadedDuration = fadedDuration;
            this.FadeColor = fadeColor;
        }

        public static readonly SceneTransition Default = new SceneTransition()
        {
            FadeOutTime = 1f,
            FadeInTime = 1f,
            FadedDuration = 1f,
            FadeColor = Color.black
        };
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace DyeTonic
{
    public static class TweenSequenceAnimation
    {
        public static void PopSequence (Transform transform, float popScale,float inOutTime)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOScale(popScale, inOutTime))
                .Append(transform.DOScale( 1f, inOutTime));
        }
        
    }
}

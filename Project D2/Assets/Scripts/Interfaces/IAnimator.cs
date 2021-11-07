using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimator
{
    Coroutine PlayAnimation(Sprite[] animationTrackToPlay, float secondsBetween, SpriteRenderer rendererToAnimate);

    void StopAnimation(Coroutine animationToStop);

    //void PlayNextSpriteInAnimation();

}

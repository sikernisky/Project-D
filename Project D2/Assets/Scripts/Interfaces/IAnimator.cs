using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimator
{
    void PlayAnimation(Sprite[] animationTrackToPlay, float secondsBetween);

    void StopAnimation(Coroutine animationToStop);

    //void PlayNextSpriteInAnimation();

    void SetUpAnimationTracks();

}

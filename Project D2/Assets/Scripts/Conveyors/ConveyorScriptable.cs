using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Conveyor")]
public class ConveyorScriptable : MoveableScriptable
{
    public Animator conveyorAnimator;

    public Sprite[] conveyorMovementAnimationTrack;
}

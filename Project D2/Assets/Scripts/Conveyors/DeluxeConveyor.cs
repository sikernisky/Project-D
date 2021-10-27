using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeluxeConveyor : Conveyor
{
    public override string NAME { get; } = "DeluxeConveyor";

    public override float MovementAnimationSpeed { get; } = .1f;
}

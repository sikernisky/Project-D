using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerStructure : Structure
{
    /// <summary>This PowerStructure's animation when it is powering something.</summary>
    private Sprite[] poweringAnimation;

    /// <summary><strong>Returns:</strong> true if this PowerStructure is powering its connections. </summary>
    public bool IsPowering(Structure s)
    {
        return true; // for now
    }
}

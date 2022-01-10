using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConnectable
{
    /// <summary><strong>Returns:</strong> The range of this IMoveable's LineRenderer, or -1 if it does not have one.</summary>
    public int AttachRange();
}

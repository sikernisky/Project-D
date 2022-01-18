using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConnectable
{
    /// <summary><strong>Returns:</strong> The (x,y) range of this IConnectble's LineRenderer. </summary>
    public Vector2Int AttachRange();
}

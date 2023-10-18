using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points
{
    // Points where the verlet will be positioned
    public Vector2 position;
    public Vector2 prevPosition;
    public Vector2 eyeDistance;

    public Points(Vector2 eyePosition, Vector2 eyeDistance)
    {
        this.eyeDistance = eyeDistance;
        position = new Vector2(eyePosition.x + eyeDistance.x, eyePosition.y + eyeDistance.y);
    }
}

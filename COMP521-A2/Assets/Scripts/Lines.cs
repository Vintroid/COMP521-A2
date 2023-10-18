using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Lines 
{
    // Line between verlets to form the fish
    public Points v1;
    public Points v2;
    public float length;

    public Lines(Points v1, Points v2)
    {
        this.v1 = v1;
        this.v2 = v2;
        
        // Calculates pythagorean length between point coordinates
        length = Mathf.Sqrt(Mathf.Pow(v2.position.x - v1.position.x,2) +
            Mathf.Pow(v2.position.y - v1.position.y,2));
    }
}

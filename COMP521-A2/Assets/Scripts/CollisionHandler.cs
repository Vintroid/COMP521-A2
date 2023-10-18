 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] GameObject eye;
    [SerializeField] float eye_radius = 0.1f;

    // We will call EyeMovement functions
    [SerializeField] EyeMovement eyeMovement;
    
    // Information from the raycasting we will do
    RaycastHit2D hit;

    // Update is called once per frame
    void Update()
    {
        if (IsColliding())
        {
            eyeMovement.CollisionMovement(eye,hit);
        }

    }

    // Checking if collision is occuring using circle raycast
    private bool IsColliding()
    {
        eye.GetComponent<Collider2D>().enabled = false; // Turning off own collider for hit detection
                                                       // Otherwise, would always hit itself

        hit = Physics2D.CircleCast(new Vector2(eye.transform.position.x, eye.transform.position.y), eye_radius, Vector2.zero);
        eye.GetComponent <Collider2D>().enabled = true;
        
        if(hit)
        {
            return true;
        }

        return false;
    }
}

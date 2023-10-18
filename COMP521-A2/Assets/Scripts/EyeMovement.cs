using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    [SerializeField] GameObject eye;
    [SerializeField] Verlets verlets;

    // Movement variables
    public float time = 0f;
    public float eye_speed;
    public Vector2 eye_velocity;
    public float eye_acceleration = -9.81f;
    public float eye_angle;


    // Start is called before the first frame update
    void Start()
    {
        if (eye.gameObject.tag == "Blue")
        {
            // On fish creation, set movement
            SetBlueMovement(eye);
        }

        else
        {
            // On fish creation, set movement in the negative dir
            SetRedMovement(eye);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Checking if time elapsed
        CheckFishTime();
        // Moving fish physics
        MoveFishEye(eye);
        // Checking boundaries
        OutOfBounds(eye);

        time = Time.deltaTime;
        
    }

    private void CheckFishTime()
    {
        if(time > 5.0f)
        {
            verlets.CleanUpFish();
            Destroy(eye);
        }
    }

    // Moving the fish using projectile motion equations
    private void MoveFishEye(GameObject eye)
    {
        // Updating vertical velocity
        eye_velocity += new Vector2(0f, eye_acceleration * (Time.deltaTime));

        // Updating position
        float delta_x = eye_velocity.x * (Time.deltaTime);
        float delta_y = eye_velocity.y * (Time.deltaTime) + 0.5f * eye_acceleration * (Time.deltaTime);
        eye.transform.position += new Vector3(delta_x, delta_y , 0f);

        // Updating angle of the fish for realism
        float deg_angle = Vector2.Angle(Vector2.right, eye_velocity);
        eye_angle = deg_angle * Mathf.Deg2Rad;
    }

    // Function to check if the fish is out of scene bounds
    private void OutOfBounds(GameObject eye)
    {
        if(eye.transform.position.x > 13 || eye.transform.position.x < -13)
        {
            verlets.CleanUpFish();
            Destroy(eye);
        }

        if(eye.transform.position.y > 6 || eye.transform.position.y < -6)
        {
            verlets.CleanUpFish();
            Destroy(eye);
        }
    }

    private void SetBlueMovement(GameObject eye)
    {
        // Get a random angle
        float random_angle = UnityEngine.Random.Range(45f, 70f);
        eye_angle = random_angle * Mathf.Deg2Rad;
        
        eye_speed = UnityEngine.Random.Range(14f, 20f);
        float x_vel = (float)(eye_speed * Math.Cos(eye_angle));
        float y_vel = (float)(eye_speed * Mathf.Sin(eye_angle));
        eye_velocity = new Vector2(x_vel, y_vel);
    }

    private void SetRedMovement(GameObject eye)
    {
        // Get a random angle
        float random_angle = UnityEngine.Random.Range(110f, 135f);
        eye_angle = random_angle * Mathf.Deg2Rad;

        // Velocity in the negative direction
        eye_speed = UnityEngine.Random.Range(14f, 20f);
        float x_vel = (float)(eye_speed * Math.Cos(eye_angle));
        float y_vel = (float)(eye_speed * Mathf.Sin(eye_angle));
        eye_velocity = new Vector2(x_vel, y_vel);

    }

    public void CollisionMovement(GameObject eye, RaycastHit2D hit)
    {
        // Since colliders cannot be used for collision handling, tags will be used.

        // Hitting the peak
        if(hit.transform.tag == "Peak")
        {
            Vector2 old_velocity = eye_velocity;
            eye_velocity = new Vector2(-old_velocity.x,-old_velocity.y);
            eye.transform.position += new Vector3(0f, 0.2f, 0f); // boost to bounce better
        }

        // Hitting the ponds
        else if(hit.transform.tag == "Pond")
        {   
            verlets.CleanUpFish();
            Destroy(eye);
        }

        // Hitting ground or other fishes
        else if(hit.transform.tag == "Ground")
        {
            Vector2 old_velocity = eye_velocity;
              
            // reversing the y-velocity
            eye_velocity = new Vector2(old_velocity.x, -old_velocity.y);
            eye.transform.position += new Vector3(0f, 0.2f, 0f); // Boost to bounce better
        }

        else if(hit.transform.tag == "Blue" || hit.transform.tag == "Red")
        {
            // Fishes interacting
            Vector2 old_velocity = eye_velocity;

            // reversing the y-velocity
            eye_velocity = new Vector2(-old_velocity.x, -old_velocity.y);

            // For more realistic fish collisions
            if(eye.transform.position.y >= hit.transform.position.y)
            {
                eye.transform.position += new Vector3(0f, 0.2f, 0f); // Bouncing on other fish
            }
            else
            {
                eye.transform.position += new Vector3(0f, -0.2f, 0f); // Getting bounced on
            }
                
                    
        }
        
    }

}



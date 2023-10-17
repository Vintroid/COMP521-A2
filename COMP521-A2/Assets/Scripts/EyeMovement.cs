using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    [SerializeField] GameObject eye;

    // Movement variables
    private float time = 0f;
    private float eye_speed;
    private Vector2 eye_velocity;
    private float eye_acceleration = -9.81f;
    private float eye_angle;
    Quaternion eye_rotation = Quaternion.identity;


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

        // Destroys fish eye after 5 seconds
        Destroy(eye, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        // Moving fish physics
        MoveFishEye(eye);
        // Checking boundaries
        OutOfBounds(eye);

        time = Time.deltaTime;
        
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
    }

    // Function to check if the fish is out of scene bounds
    private void OutOfBounds(GameObject eye)
    {
        if(eye.transform.position.x > 13 || eye.transform.position.x < -13)
        {
            Destroy(eye);
        }

        if(eye.transform.position.y > 6 || eye.transform.position.y < -6)
        {
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
        eye_speed = UnityEngine.Random.Range(15f, 20f);
        float x_vel = (float)(eye_speed * Math.Cos(eye_angle));
        float y_vel = (float)(eye_speed * Mathf.Sin(eye_angle));
        eye_velocity = new Vector2(x_vel, y_vel);

    }

    public void CollisionMovement(GameObject eye, RaycastHit2D hit)
    {
        // Since colliders cannot be used for collision handling, static x,y coordinates
        // will be used.

        // Hitting the peak
        if(hit.transform.position.x == 0f && hit.transform.position.y == -3.5f)
        {
            eye_velocity *= hit.normal;
        }

        // Hitting the ponds
        else if(hit.transform.position.y == -4.7f)
        {
            Destroy(eye);
        }

        // Hitting ground or other fishes
        else
        {
            Vector2 old_velocity = eye_velocity;
            eye_velocity = new Vector2(old_velocity.x, -old_velocity.y); ;
            
        }
        
    }

}



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Verlets : MonoBehaviour
{
    // The eye that we will use to center our verlets
    [SerializeField] GameObject eye;
    [SerializeField] EyeMovement eyeMovement;
    [SerializeField] GameObject pointObject;
    [SerializeField] GameObject lineObject;

    public Points[] points = new Points[9]; // Array of points
    public Lines[] lines = new Lines[9]; // Array of lines
    public GameObject[] pointObjects = new GameObject[9]; // Array of gameobjects to represent points
    public GameObject[] lineObjects = new GameObject[9]; // Array of gameobjects to represent lines

    // Rotation Matrix that we will use to rotate fish following velocities
    public float prevAngle = 0f;
    public float[] rotationMatrix = new float[4];
    

    void Start()
    {
        InitializePoints();
        InitializeLines();
    }

    void Update()
    {
        UpdatePoints();
        UpdateLines();
    }

    private void InitializeLines()
    {
        for(int i=0; i<9; i++)
        {
            lines[i] = new Lines(points[i], points[(i+1)%9]);

            // Using lines to instantiate line gameobjects
            float line_x = (lines[i].v2.position.x - lines[i].v1.position.x) / 2.0f;
            float line_y = (lines[i].v2.position.y - lines[i].v1.position.y) / 2.0f;

            // Calculating angle depending on line from given package
            float angle = Vector2.Angle(lines[i].v2.position - lines[i].v1.position, new Vector2(1, 1));
            lineObjects[i] = GameObject.Instantiate(lineObject, new Vector2(line_x + eye.transform.position.x,
                line_y + eye.transform.position.y), Quaternion.Euler(0f, 0f, angle));

            // Set vertices of line to its points
            Vector3[] positions = { new Vector3 { x = lines[i].v1.position.x, y = lines[i].v1.position.y, z = 0f },
                                    new Vector3 { x = lines[i].v2.position.x, y = lines[i].v2.position.y, z = 0f } };
            lineObjects[i].GetComponent<LineRenderer>().SetPositions(positions);
        }

    }

    // Creating verlet for the fish. These will be positionned around fish eye.
    private void InitializePoints()
    {
        points[0] = new Points(new Vector2(eye.transform.position.x, eye.transform.position.y), new Vector2(0f,0.5f));
        points[1] = new Points(new Vector2(eye.transform.position.x, eye.transform.position.y), new Vector2(0.5f, 0.25f));
        points[2] = new Points(new Vector2(eye.transform.position.x, eye.transform.position.y), new Vector2(0.25f, 0f));
        points[3] = new Points(new Vector2(eye.transform.position.x, eye.transform.position.y), new Vector2(0.5f, -0.25f));
        points[4] = new Points(new Vector2(eye.transform.position.x, eye.transform.position.y), new Vector2(0f, -0.5f));
        points[5] = new Points(new Vector2(eye.transform.position.x, eye.transform.position.y), new Vector2(-0.5f, -0.25f));
        points[6] = new Points(new Vector2(eye.transform.position.x, eye.transform.position.y), new Vector2(-0.75f, -0.5f));
        points[7] = new Points(new Vector2(eye.transform.position.x, eye.transform.position.y), new Vector2(-0.75f, 0.5f));
        points[8] = new Points(new Vector2(eye.transform.position.x, eye.transform.position.y), new Vector2(-0.5f, 0.25f));

        // Updating rotation matrix with current angle
        rotationMatrix[0] = Mathf.Cos(eyeMovement.eye_angle - prevAngle);
        rotationMatrix[1] = -Mathf.Sin(eyeMovement.eye_angle - prevAngle);
        rotationMatrix[2] = Mathf.Sin(eyeMovement.eye_angle - prevAngle);
        rotationMatrix[3] = rotationMatrix[0];

        prevAngle = eyeMovement.eye_angle;

        foreach (Points p in points)
        {
            // Changing eyeDistance component of points depending rotation of fish
            Vector2 oldEyeDistance = p.eyeDistance;

            // Performing vector and matrix multiplication
            p.eyeDistance.x = oldEyeDistance.x * rotationMatrix[0] + oldEyeDistance.y * rotationMatrix[1];
            p.eyeDistance.y = oldEyeDistance.x * rotationMatrix[2] + oldEyeDistance.y * rotationMatrix[3];

        }

        // Creating visible gameobjects following these points
        for (int i=0;i<9;i++){
            pointObjects[i] = GameObject.Instantiate(pointObject, points[i].position, Quaternion.identity);
        }

    }

    // Comparing to eye position and changing points position
    public void UpdatePoints()
    {
        // Updating rotation matrix with current angle
        rotationMatrix[0] = Mathf.Cos(eyeMovement.eye_angle - prevAngle);
        rotationMatrix[1] = -Mathf.Sin(eyeMovement.eye_angle - prevAngle);
        rotationMatrix[2] = Mathf.Sin(eyeMovement.eye_angle - prevAngle);
        rotationMatrix[3] = rotationMatrix[0];

        prevAngle = eyeMovement.eye_angle;
        
        foreach(Points p in points)
        {
            // Changing eyeDistance component of points depending rotation of fish
            Vector2 oldEyeDistance = p.eyeDistance;

            // Performing vector and matrix multiplication
            p.eyeDistance.x = oldEyeDistance.x * rotationMatrix[0] + oldEyeDistance.y * rotationMatrix[1];
            p.eyeDistance.y = oldEyeDistance.x * rotationMatrix[2] + oldEyeDistance.y * rotationMatrix[3];

            float p_xoffset = p.position.x - eye.transform.position.x;
            float p_yoffset = p.position.y - eye.transform.position.y;
            if(new Vector2(p_xoffset,p_yoffset) != p.eyeDistance)
            {
                // Saving position as prevposition
                p.prevPosition = p.position;

                // Following the eye's change of position
                p.position.x = eye.transform.position.x + p.eyeDistance.x;
                p.position.y = eye.transform.position.y + p.eyeDistance.y;
            }
        }

        // Changing point gameobjects' position too
        for(int i=0;i<9;i++)
        {
            pointObjects[i].transform.position = new Vector3(points[i].position.x, points[i].position.y,0f);
        }
    }

    public void UpdateLines()
    {
        for(int i = 0; i < 9; i++)
        {
            Vector3[] positions = { new Vector3 { x = lines[i].v1.position.x, y = lines[i].v1.position.y, z = 0f },
                                    new Vector3 { x = lines[i].v2.position.x, y = lines[i].v2.position.y, z = 0f } };
            lineObjects[i].GetComponent<LineRenderer>().SetPositions(positions);
        }
    }

    // Cleaning up fish representation on game screen when eye disappears
    public void CleanUpFish()
    {
        foreach(GameObject obj in pointObjects)
        {
            Destroy(obj);
        }

        foreach(GameObject obj in lineObjects)
        {
            Destroy(obj);
        }
        
    }
}



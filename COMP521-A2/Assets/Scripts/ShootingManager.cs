using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{

    // Objects to instantiate
    [SerializeField] GameObject blueEye;
    [SerializeField] GameObject redEye;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            GameObject newBlueFish = Instantiate(blueEye);
        }

        if(Input.GetKeyDown("2"))
        {
            GameObject newRedFish = Instantiate(redEye);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public List<LaserData> laserToDisplay;
    private float currentBallDistance = 0;
    [HideInInspector]
    public PlayerController playerController;
    private RaycastHit2D hitPoint;
    private void Update()
    {
        //Temp, c'est une boule pas un laser

        if (laserToDisplay.Count > 0)
        {
            if (currentBallDistance < laserToDisplay[0].length)
            {
                transform.position = Vector2.Lerp(laserToDisplay[0].origin, laserToDisplay[0].hit.point, (currentBallDistance / laserToDisplay[0].length));
                currentBallDistance += laserToDisplay[0].length / (AudioEngine.timeBetweenTwoBeat * (1.0f / Time.deltaTime));
            }
            else
            {
                currentBallDistance = 0;
                hitPoint = laserToDisplay[0].hit;
                laserToDisplay.RemoveAt(0);
            }
        }
        else
        {
            DestroySequence();
        }
       


    }

    private void DestroySequence()
    {
        playerController.Teleport(hitPoint);
        Destroy(gameObject);
    }
}

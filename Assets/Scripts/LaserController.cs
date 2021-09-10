using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public List<LaserData> laserToDisplay;
    private float currentBallDistance = 0;
    [SerializeField, Range(0, 10)]
    private float laserSpeed = 1;

    private void Update()
    {
        //Temp, c'est une boule pas un laser

        if (laserToDisplay.Count > 0)
        {
            if (currentBallDistance < laserToDisplay[0].length)
            {
                transform.position = Vector2.Lerp(laserToDisplay[0].origin, laserToDisplay[0].hitPoint, (currentBallDistance / laserToDisplay[0].length));
                currentBallDistance += laserToDisplay[0].length / (laserSpeed * (1.0f / Time.deltaTime));
            }
            else
            {
                currentBallDistance = 0;
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
        Destroy(gameObject);
    }
}

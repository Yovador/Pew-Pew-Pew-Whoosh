using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaserData
{
    public Vector2 origin { get; set; }
    public Vector2 direction { get; set; }
    public Vector2 hitPoint { get; set; } 
    public LaserData(Vector2 origin, Vector2 direction, Vector2 hitPoint = new Vector2())
    {
        this.origin = origin;
        this.direction = direction;
        this.hitPoint = hitPoint;
    }

    public float length {
        get {
                if(this.hitPoint != null)
                {
                    return Mathf.Sqrt(Mathf.Pow(origin.x - hitPoint.x, 2) + Mathf.Pow(origin.y - hitPoint.y, 2));
                }

                else
                {
                    return 0;
                }
            } 
        }

    public bool CheckIfOriginInList(List<LaserData> list)
    {
        foreach (var item in list)
        {
            if(item.origin == this.origin)
            {
                return true;
            }
        }

        return false;
    }

}

public class LazerThrower : MonoBehaviour
{
    [SerializeField, Range(1, 5)]
    private int bounceLimit = 3;
    private List<LaserData> laserToDisplay = new List<LaserData>();
    private LaserData nextLaser;
    private GameObject previousObjectHit;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {


            laserToDisplay = new List<LaserData>();
            nextLaser = new LaserData(transform.position, transform.up);
            Shoot();
        }

        int i = 0;
        foreach (LaserData laser in laserToDisplay)
        {
            Debug.Log("Displaying : " + laser.origin + " / " + laser.direction + " / " + laser.length);
            Color color = Color.clear;
            switch (i)
            {
                case 0:
                    color = Color.blue;
                    break;
                case 1:
                    color = Color.green;
                    break;
                case 2:
                    color = Color.red;
                    break;
                case 3:
                    color = Color.cyan;
                    break;
                case 4:
                    color = Color.magenta;
                    break;
                default:
                    break;
            }
            Debug.DrawRay(laser.origin, laser.direction.normalized * laser.length, color);
            i++;
        }

    }

    Vector2 CalculateBounceVector(Vector2 d, Vector2 n)
    {
        return d -2 * (Vector2.Dot(d, n.normalized)) * n.normalized;
    }


    void Shoot()
    {
        for (int i = 0; i < bounceLimit; i++)
        {
            if (!nextLaser.Equals(new KeyValuePair<Vector2, Vector2>()))
            {
                Debug.Log("Shooting ray number : " + i + " / " + nextLaser.origin + " / " + nextLaser.direction);
                SendRay(nextLaser);
            }
        }
        previousObjectHit = null;
    }

     void SendRay(LaserData laser)
    {

        string txtDebug = "Sending Ray ! \n \n";




        RaycastHit2D[] hits = Physics2D.RaycastAll(laser.origin, laser.direction);

        foreach (var hit in hits)
        {
            txtDebug += "Enter in contact with : " + hit.collider.name + "\n\n";
            if (hit)
            {
                txtDebug += "hit = true \n";
                txtDebug += "Bounceable = " + hit.collider.CompareTag("Bounceable") + "\n";
                if (hit.collider.CompareTag("Bounceable"))
                {
                    if (previousObjectHit != hit.collider.gameObject)
                    {
                        if (!laser.CheckIfOriginInList(laserToDisplay))
                        {
                            txtDebug += "Adding to display \n\n";

                            laserToDisplay.Add(new LaserData(laser.origin, laser.direction, hit.point));
                        }

                        if (previousObjectHit != null)
                        {
                            txtDebug += "Previous object : " + previousObjectHit.name + "\n";
                        }
                        else
                        {
                            txtDebug += "No previous object" + "\n";
                        }

                        txtDebug += "bouncing at : " + hit.point + "\n";


                        Vector2 origin = hit.point;
                        Vector2 bounce = CalculateBounceVector(laser.direction, hit.normal);

                        txtDebug += "Calculating Bounce : \n\t origin of point : " + origin + " bounce vector " + bounce + "\n";


                        txtDebug += "Updating previous hit with : " + hit.collider.gameObject.name +"\n";
                        previousObjectHit = hit.collider.gameObject;
                        nextLaser = new LaserData(origin, bounce, hit.point);
                        

                        txtDebug += "Exiting Ray... \n\n";

                        Debug.Log(txtDebug);
                        break;
                    }
                    else
                    {
                        txtDebug += "\n" + hit.collider.name + " has already by bounced off\n\n";  

                    }
                }
                else
                { 
                    nextLaser = null;
                }
            }
        }
    }
}
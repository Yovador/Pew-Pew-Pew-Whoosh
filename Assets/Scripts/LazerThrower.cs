using System.Collections;
using System.Collections.Generic;
using UnityEngine;




//Class stockant les donn?es d'un laser
public class LaserData
{
    public Vector2 origin { get; set; }
    public Vector2 direction { get; set; }
    //Point de contact avec une surface rebndissable / teleportable
    public RaycastHit2D hit { get; set; } 
    
    public int beat { get; set; }

    //Constructor
    public LaserData(Vector2 origin, Vector2 direction, int beat,  RaycastHit2D hit = new RaycastHit2D())
    {
        this.origin = origin;
        this.direction = direction;
        this.hit = hit;
        this.beat = beat;
    }

    //Renvoie la distance du laser
    public float length {
        get {
                if(hit.collider)
                {
                    return Mathf.Sqrt(Mathf.Pow(origin.x - hit.point.x, 2) + Mathf.Pow(origin.y - hit.point.y, 2));
                }
                else
                {
                    return 0;
                }
            } 
        }

}

public class LazerThrower : MonoBehaviour
{
    [SerializeField]
    private GameObject laserProj;

    [SerializeField, Range(1, 5)]
    private int bounceLimit = 3;
    private List<LaserData> laserToDisplay = new List<LaserData>();
    private LaserData nextLaser;
    private RaycastHit2D previousHit;
    bool displayCircle = false;
    [HideInInspector]
    public Vector2 laserAngle;
    [HideInInspector]
    public PlayerController playerController;

    private void Start()
    {
        laserAngle = transform.up;
    }

    private void Update()
    {
        DebugDisplayTrajectory();

        if (displayCircle)
        {
            GameObject TempLaser = Instantiate(laserProj, transform.position, transform.rotation);
            LaserController laserController = TempLaser.GetComponent<LaserController>();
            laserController.playerController = playerController;
            laserController.laserToDisplay = laserToDisplay;
            displayCircle = false;
        }

    }

    void DebugDisplayTrajectory()
    {
        int i = 0;
        foreach (LaserData laser in laserToDisplay)
        {
            Color color = Color.clear;
            switch (i)
            {
                case 0:
                    color = Color.green;
                    break;
                case 1:
                    color = Color.cyan;
                    break;
                case 2:
                    color = Color.blue;
                    break;
                case 3:
                    color = Color.magenta;
                    break;
                case 4:
                    color = Color.red;
                    break;
                default:
                    break;
            }
            Debug.DrawRay(laser.origin, laser.direction.normalized * laser.length, color);
            Debug.DrawRay(laser.hit.point, Vector2.up * 0.5f, Color.grey);
            i++;
        }
    }

    Vector2 CalculateBounceVector(Vector2 d, Vector2 n)
    {
        return d -2 * (Vector2.Dot(d, n.normalized)) * n.normalized;
    }


    public void Shoot()
    {
        laserToDisplay = new List<LaserData>();
        nextLaser = new LaserData(transform.position, laserAngle, 0);
        //Calcule la trajectoire du projectile
        for (int i = 0; i < bounceLimit; i++)
        {
            if (!nextLaser.Equals(new KeyValuePair<Vector2, Vector2>()))
            {
                Debug.Log("Shooting ray number : " + i + " / " + nextLaser.origin + " / " + nextLaser.direction);
                SendRay(nextLaser, i);
            }
        }
        previousHit = new RaycastHit2D();

        foreach (var laser in laserToDisplay)
        {
            RaycastHit2D[] deathHits = Physics2D.CircleCastAll(laser.hit.point, 0.5f, Vector2.zero);
            foreach (var deathHit in deathHits)
            {
                if (deathHit)
                {
                    Debug.Log("Hitting : " + deathHit.collider.name + " from " + laser.hit.centroid + " at " + laser.hit.point);
                    if (deathHit.collider.CompareTag("Player"))
                    {
                        Debug.Log(transform.parent.gameObject + " is hitting player " + deathHit.collider.gameObject.name);
                        if (deathHit.collider.gameObject != transform.parent.gameObject)
                        {

                            deathHit.collider.gameObject.GetComponent<PlayerController>().Death(nextLaser);

                        }
                    }
                }
            }
        }

        //Active l'affichage du laser 
        displayCircle = true;


    }

    void SendRay(LaserData laser, int beat)
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
                    if (previousHit.point != hit.point)
                    {
                        
                        txtDebug += "Adding to display \n\n";

                        laserToDisplay.Add(new LaserData(laser.origin, laser.direction, beat ,hit));


                        if (previousHit.collider != null)
                        {
                            txtDebug += "Previous object : " + previousHit.collider.name + "\n";
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
                        previousHit = hit;
                        nextLaser = new LaserData(origin, bounce, beat, hit);
                        

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

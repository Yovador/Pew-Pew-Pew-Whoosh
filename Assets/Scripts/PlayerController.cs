using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public LazerThrower lazerThrower;
    public Vector2 aimVector;
    GameObject aim;
    [SerializeField]
    private float aimDistance = 1f;
    [SerializeField, Range(10, 180)]
    private float aimAngle = 160;

    // Start is called before the first frame update
    void Start()
    {
        lazerThrower = GetComponentInChildren<LazerThrower>();
        lazerThrower.playerController = this;
        aim = GameObject.Find("Aim");
        aimVector = transform.up;
        UpdateAim();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, aimVector * 2, Color.yellow);
    }
    public void OnMove(InputValue input)
    {
        Vector2 previousAimV = aimVector;
        Vector2 inputVec = input.Get<Vector2>();
        if (inputVec != Vector2.zero)
        {
            aimVector = inputVec;   
        }

        if(Mathf.Abs(Vector2.Angle(transform.up, aimVector)) <= aimAngle/ 2)
        {
            UpdateAim();
        }
        else
        {
            aimVector = previousAimV;
        }
        lazerThrower.laserAngle = aimVector;

        //Debug.Log("Moving + " + aimVector);
    }


    private void UpdateAim ()
    {
        //Par défaut, regarde vers le haut absolue
        Vector3 newPos = aimVector * aimDistance;
        aim.transform.position = newPos + transform.position;
    }

    public void Teleport(RaycastHit2D hit)
    {
        Debug.Log("Teleporting... " + hit.point);
        transform.position = hit.point;
        transform.up = hit.normal;
        aimVector = transform.up;
        UpdateAim();
        lazerThrower.laserAngle = aimVector;
    }
}

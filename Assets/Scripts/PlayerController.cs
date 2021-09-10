using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    LazerThrower lazerThrower;
    Vector2 aimVector;
    GameObject aim;
    [SerializeField]
    float aimDistance = 2f;

    // Start is called before the first frame update
    void Start()
    {
        lazerThrower = GetComponentInChildren<LazerThrower>();
        aim = GameObject.Find("Aim");
    }

    public void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();

        aimVector = inputVec;

        Debug.Log("Moving + " + aimVector);
    }

    public void OnShoot(InputValue input)
    {
        lazerThrower.Shoot();
    }
}

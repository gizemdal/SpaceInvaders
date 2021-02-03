using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    /*
     * Some public variables to alter ship's movement
     */
    public float moveAcceleration;
    public GameObject bullet; // the GameObject to spawn
    public GameObject globalOBJ; // global game object
    public float bulletBuffer = 1.5f; // bullet buffer time (3 seconds)
    public bool hasShot = false; // has the ship sent a bullet?
    // Start is called before the first frame update
    void Start()
    {
        moveAcceleration = 0.1f;
    }

    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            Vector3 updatedPosition = gameObject.transform.position;
            if (updatedPosition.x < globalOBJ.GetComponent<Global>().maxPos.x)
            {
                updatedPosition.x += moveAcceleration;
                gameObject.transform.position = updatedPosition;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            Vector3 updatedPosition = gameObject.transform.position;
            if (updatedPosition.x > -globalOBJ.GetComponent<Global>().maxPos.x)
            {
                updatedPosition.x -= moveAcceleration;
                gameObject.transform.position = updatedPosition;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasShot)
        {
            bulletBuffer -= Time.deltaTime;
            if (bulletBuffer <= 0)
            {
                // Ship is ready for sending bullets
                hasShot = false;
                bulletBuffer = 1.5f;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (!hasShot)
            {
                Vector3 spawnPos = gameObject.transform.position;
                // instantiate the Bullet
                GameObject obj = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
                Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), obj.GetComponent<Collider>());
                for (int i = 0; i < Global.shields.Count; ++i)
                {
                    if (Global.shields[i].GetComponent<Shield>().remainingHits == 0)
                    {
                        Physics.IgnoreCollision(Global.shields[i].GetComponent<Collider>(), obj.GetComponent<Collider>());
                    }
                }
                hasShot = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public static float resurrectTime = 5;
    public static float secondCount = 1;
    public static int remainingLives = 3;
    /*
     * Some public variables to alter ship's movement
     */
    public float moveAcceleration;
    public float bulletSpeed;
    public GameObject bullet; // the GameObject to spawn
    public GameObject globalOBJ; // global game object
    public float bulletBuffer; // bullet buffer time (1 second)
    public bool hasShot; // has the ship sent a bullet?
    public bool isResurrecting; // is the ship currently resurrecting?
    // Start is called before the first frame update
    void Start()
    {
        moveAcceleration = 0.1f;
        bulletSpeed = 400f;
        bulletBuffer = 1f;
        hasShot = false;
        isResurrecting = false;
        gameObject.GetComponent<Renderer>().enabled = true;
    }

    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            Vector3 updatedPosition = gameObject.transform.position;
            if (updatedPosition.x < (globalOBJ.GetComponent<Global>().maxPos.x - 1f))
            {
                updatedPosition.x += moveAcceleration;
                gameObject.transform.position = updatedPosition;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            Vector3 updatedPosition = gameObject.transform.position;
            if (updatedPosition.x > (-globalOBJ.GetComponent<Global>().maxPos.x + 1f))
            {
                updatedPosition.x -= moveAcceleration;
                gameObject.transform.position = updatedPosition;
            }
        }
    }

    public void Resurrect()
    {
        // Reset ship position
        gameObject.transform.position = new Vector3(0, 0, -7);
        // Enable ship render
        gameObject.GetComponent<Renderer>().enabled = true;
        // Start the time
        Time.timeScale = 1;
        // Resurrection is done - set isResurrecting to false

        isResurrecting = false;
    }

    // Call this method when the ship is shot
    public void Kill()
    {
        Ship.remainingLives--;
        // Stop the time
        Time.timeScale = 0;
        // Stop rendering the ship
        gameObject.GetComponent<Renderer>().enabled = false;
        if (Ship.remainingLives > 0)
        {
            isResurrecting = true;
        } else
        {
            // All the lives are used - display Game Over title
            Global.isGameOver = true;
            Time.timeScale = 1;
            Debug.Log("Game Over!!!");
        }
    }

    public void ResetState()
    {
        Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Global.isGameOver)
        {
            if (!isResurrecting)
            {
                if (hasShot)
                {
                    bulletBuffer -= Time.deltaTime;
                    if (bulletBuffer <= 0)
                    {
                        // Ship is ready for sending bullets
                        hasShot = false;
                        bulletBuffer = 1f;
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
            else
            {
                // Ship is currently resurrecting
                Ship.resurrectTime -= Time.fixedDeltaTime;
                Ship.secondCount -= Time.fixedDeltaTime;
                if (Ship.secondCount <= 0)
                {
                    gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
                    Ship.secondCount = 1;
                }
                if (resurrectTime <= 0)
                {
                    // Time to resurrect
                    resurrectTime = 5;
                    Resurrect();
                }
            }
        }
    }
}

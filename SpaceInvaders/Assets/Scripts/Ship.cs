using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public static float resurrectTime = 3;
    public static float secondCount = 1;
    public static int remainingLives = 3;
    /*
     * Some public variables to alter ship's movement
     */
    public float moveAcceleration;
    public float bulletSpeed;
    public GameObject bullet; // the GameObject to spawn
    public GameObject globalOBJ; // global game object
    public GameObject explosion; // explosion debris
    public GameObject currExplosion;
    public float bulletBuffer; // bullet buffer time (1 second)
    public bool hasShot; // has the ship sent a bullet?
    public bool isResurrecting; // is the ship currently resurrecting?

    public AudioClip ship_kill;

    // Start is called before the first frame update
    void Start()
    {
        moveAcceleration = 0.2f;
        bulletSpeed = 400f;
        bulletBuffer = 0.5f;
        hasShot = false;
        isResurrecting = false;
        gameObject.GetComponent<Renderer>().enabled = true;
    }

    void FixedUpdate()
    {
        if (!Global.isPause) {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                Vector3 updatedPosition = gameObject.transform.position;
                if (updatedPosition.x <= (7.5f))
                {
                    updatedPosition.x += moveAcceleration;
                    gameObject.transform.position = updatedPosition;
                }
            }
            else if (Input.GetAxisRaw("Horizontal") < 0)
            {
                Vector3 updatedPosition = gameObject.transform.position;
                if (updatedPosition.x >= (-7.5f))
                {
                    updatedPosition.x -= moveAcceleration;
                    gameObject.transform.position = updatedPosition;
                }
            }
        }
    }

    public void Resurrect()
    {
        // Reset ship position
        gameObject.transform.position = new Vector3(0, 0, -7);
        // Delete the particles
        Destroy(currExplosion);
        // Delete existing bullets, debris and rewards in the game
        GameObject[] allAlienBullets = GameObject.FindGameObjectsWithTag("AlienBullet");
        foreach (GameObject alienB in allAlienBullets) {
            Destroy(alienB);
        }
        GameObject[] allDebris = GameObject.FindGameObjectsWithTag("Debris");
        foreach (GameObject deb in allDebris) {
            Destroy(deb);
        }
        GameObject[] allRewards = GameObject.FindGameObjectsWithTag("Resource");
        foreach (GameObject rew in allRewards) {
            Destroy(rew);
        }
        // Enable ship render
        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.layer = 8;
        // Start the time
        Global.isPause = false;
        // Resurrection is done - set isResurrecting to false

        isResurrecting = false;
    }

    // Call this method when the ship is shot
    public void Kill()
    {
        AudioSource.PlayClipAtPoint(ship_kill, gameObject.transform.position);
        Ship.remainingLives--;
        // Stop the time
        Global.isPause = true;
        // Stop rendering the ship
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.layer = 0;
        Global.shipStreak = 0; // Kill ship streak
        bulletSpeed = 400f;
        bulletBuffer = 0.5f;
        Global.isRewardActive = false;
        Global.rewardDuration = 5;
        // Create explosion
        Vector3 spawnPos = gameObject.transform.position;
        currExplosion = Instantiate(explosion, new Vector3(spawnPos.x, spawnPos.y, spawnPos.z + 1), Quaternion.identity) as GameObject;
        GameObject[] allShipBullets = GameObject.FindGameObjectsWithTag("ShipBullet");
        foreach (GameObject shipB in allShipBullets) {
            Destroy(shipB);
        }
        if (Ship.remainingLives > 0)
        {
            isResurrecting = true;
        } else
        {
            // All the lives are used - display Game Over title
            Global.isGameOver = true;
            Global.isPause = false;
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
                        bulletBuffer = 0.5f;
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
                    Ship.secondCount = 1;
                }
                if (resurrectTime <= 0)
                {
                    // Time to resurrect
                    resurrectTime = 3;
                    Resurrect();
                }
            }
        }
    }
}

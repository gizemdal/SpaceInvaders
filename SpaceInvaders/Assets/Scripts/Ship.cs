using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public float bulletBuffer; // bullet buffer time (0.5 seconds)
    public bool hasShot; // has the ship sent a bullet?
    public bool isResurrecting; // is the ship currently resurrecting?

    public AudioClip ship_kill; // audio clip for ship explosion

    public float prevMouseX; // previously recorded mouse X position
    public bool recording; // are we recording mouse movement?
    public float pressTime; // how long did the player hold the button?
    public float holdTime; // how long was the button held?

    // Start is called before the first frame update
    void Start()
    {
        moveAcceleration = 0.06f;
        bulletSpeed = 400f;
        bulletBuffer = 0.5f;
        hasShot = false;
        isResurrecting = false;
        gameObject.GetComponent<Renderer>().enabled = true;
        recording = false;
        pressTime = 0f;
        holdTime = 0f;
    }

    void FixedUpdate()
    {
        if (!Global.isPause) {
            // Left/Right keys are recognized for the PC/Mac versions
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
                        if (Global.activeReward == 2) {
                            bulletBuffer = 0.05f;
                        } else {
                            bulletBuffer = 0.5f;
                        }
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
                    holdTime = 0;
                }

                // Check for primary mouse button hold/release
                if (Input.GetMouseButtonDown(0)) {
                    // Player is pressing - record the current position
                    recording = true;
                    prevMouseX = Input.mousePosition.x;
                    pressTime = Time.time;
                }
                if (Input.GetMouseButtonUp(0)) {
                    // Player released the mouse - no need to track movement
                    recording = false;
                    holdTime = Time.time - pressTime;
                    pressTime = 0f;
                }
                if (recording) {
                    // Capture the difference between each frame
                    float deltaX = Input.mousePosition.x - prevMouseX;
                    if (Mathf.Abs(deltaX) < 0.001f) return;
                    if (deltaX > 0) {
                        Vector3 updatedPosition = gameObject.transform.position;
                        if (updatedPosition.x <= (7.5f))
                        {
                            updatedPosition.x += (deltaX * moveAcceleration);
                            gameObject.transform.position = updatedPosition;
                        }
                    } else if (deltaX < 0) {
                        Vector3 updatedPosition = gameObject.transform.position;
                        if (updatedPosition.x >= (-7.5f))
                        {
                            updatedPosition.x += (deltaX * moveAcceleration);
                            gameObject.transform.position = updatedPosition;
                        }
                    }
                    prevMouseX = Input.mousePosition.x;
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

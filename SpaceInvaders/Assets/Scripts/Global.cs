using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    /*
     * Static variables
     */
    public static int numAliens = 55; // total number of aliens in the beginning
    public static int numShields = 4; // total number of
    public static int rowNum = 5; // number of rows for alien group
    public static float alienTimer = 1f; // timer for alien attack
    public static float UFOTimer = 10; // timer for UFO spawn
    public static int remainingLives = 3; // total lives
    public static int playerScore = 0; // Keep track of player score
    public static bool isGameOver = false; // has the player lost all their lives?
    public static List<GameObject> aliens = new List<GameObject>();
    public static List<GameObject> shields = new List<GameObject>();

    /*
     * Instance variables
     */
    public Vector2 alienSpeed; // initial speed
    public GameObject jelly; // alien to spawn
    public GameObject skull; // alien to spawn
    public GameObject crab; // alien to spawn
    public GameObject bullet; // alien bullet to spawn
    public GameObject shield; // shield to spawn
    public GameObject UFO; // UFO to spawn
    public GameObject ship; // ship game object
    public Vector2 maxPos; // maximum position an alien or the ship can take
    public bool maxZReached; // determine if the aliens need to go lower

    // Start is called before the first frame update
    void Start()
    {
        alienSpeed = new Vector2(0.01f, 0.075f); // initial speed
        // Create the aliens
        float z_coor = 1.2f;
        float x_coor = -7f;
        Vector3 spawnPos = new Vector3(x_coor, 0, z_coor);
        maxPos = new Vector2 (Mathf.Abs(spawnPos.x) + 7f, -3f);
        maxZReached = false;
        int idx = 0;
        for (int row = 0; row < rowNum; ++row)
        {
            for (int col = 0; col < numAliens / rowNum; ++col)
            {
                Quaternion rot = Quaternion.identity;
                rot *= Quaternion.Euler(Vector3.up * -90);
                if (row <= 1)
                {
                    Global.aliens.Add(Instantiate(skull, spawnPos, rot));
                } else if (row <= 3)
                {
                    Global.aliens.Add(Instantiate(crab, spawnPos, rot));
                } else if (row == 4)
                {
                    Global.aliens.Add(Instantiate(jelly, spawnPos, rot));
                }
                spawnPos.x += 1.55f;
                idx++;
            }
            spawnPos.z += 1.25f;
            spawnPos.x = -7f;
        }
        // Make aliens ignore collision between each other
        for (int i = 0; i < numAliens; ++i)
        {
            for (int j = 0; j < numAliens; ++j)
            {
                if (i == j) continue;
                Physics.IgnoreCollision(Global.aliens[i].GetComponent<Collider>(), Global.aliens[j].GetComponent<Collider>());
            }
        }
        // Create the shields
        Global.shields.Add(Instantiate(shield, new Vector3(-6, 0, -4f), Quaternion.identity));
        Global.shields.Add(Instantiate(shield, new Vector3(-2, 0, -4f), Quaternion.identity));
        Global.shields.Add(Instantiate(shield, new Vector3(2, 0, -4f), Quaternion.identity));
        Global.shields.Add(Instantiate(shield, new Vector3(6, 0, -4f), Quaternion.identity));
    }

    public void RemoveShield(int id)
    {
        for (int i = 0; i < numShields; ++i)
        {
            if (Global.shields[i].GetComponent<Shield>().id == id)
            {
                // Remove the shield
                Global.shields.RemoveAt(i);
                break;
            }
        }
        Global.numShields--;
    }

    public void RemoveAlien(int id)
    {
        for (int i = 0; i < numAliens; ++i)
        {
            if (Global.aliens[i].GetComponent<Alien>().id == id)
            {
                // Add this alien's score value to player score
                playerScore += Global.aliens[i].GetComponent<Alien>().score;
                Global.aliens.RemoveAt(i);
                break;
            }
        }
        // Update remaining number of aliens
        Global.numAliens--;
        if (numAliens == 0)
        {
            // Player killed all the aliens - restore everything!
            Global.numAliens = 55;
            Alien.timer = 1;
            // Reset the shields
            for (int i = 0; i < numShields; ++i)
            {
                GameObject toRemove = shields[0];
                shields.RemoveAt(0);
                Destroy(toRemove);
            }
            Global.numShields = 4;
            Start();
            return;
        }
        // Update alien speed if needed
        if (Global.numAliens % 8 == 0)
        {
            alienSpeed.x += 0.01f;
            alienSpeed.y += 0.05f;
            Alien.timer /= 2f;
        }
    }



    // Update is called once per frame
    void Update()
    {
        float deltaT = Time.deltaTime;
        alienTimer -= deltaT;
        // Make the closest alien shoot every 1 second
        if (alienTimer <= 0 && Global.numAliens > 0)
        {
            float minDist = 9999f; // set to a high enough value
            int alienIdx = 0;
            for (int i = 0; i < numAliens; ++i)
            {
                // Compute the Euclidian distance
                float dist = Mathf.Sqrt((Global.aliens[i].transform.position.x - ship.transform.position.x) * (Global.aliens[i].transform.position.x - ship.transform.position.x) +
                             (Global.aliens[i].transform.position.z - ship.transform.position.z) * (Global.aliens[i].transform.position.z - ship.transform.position.z));
                if (dist < minDist)
                {
                    minDist = dist;
                    alienIdx = i;
                }
            }
            GameObject toShoot = Global.aliens[alienIdx];
            // remove the selected id from the list
            GameObject obj = toShoot.GetComponent<Alien>().Shoot();
            for (int i = 0; i < numAliens; ++i)
            {
                Physics.IgnoreCollision(Global.aliens[i].GetComponent<Collider>(), obj.GetComponent<Collider>());
            }
            alienTimer = 1f;
        }
        // Check if any alien has reached the max distance
        bool zReached = false;
        for (int i = 0; i < numAliens; ++i)
        {
            if (Global.aliens[i].transform.position.x >= maxPos.x || Global.aliens[i].transform.position.x <= -maxPos.x)
            {
                for (int j = 0; j < numAliens; ++j)
                {
                    // Update movement direction
                    Global.aliens[j].GetComponent<Alien>().moveLeft = !Global.aliens[j].GetComponent<Alien>().moveLeft;
                    // Bring aliens closer (unless they're at maximum closeness)
                    Vector3 updatedPosition = Global.aliens[j].GetComponent<Alien>().transform.position;
                    if (!maxZReached)
                    {
                        updatedPosition.z -= alienSpeed.y;
                        if (updatedPosition.z <= maxPos.y)
                        {
                            zReached = true;
                        }
                        Global.aliens[j].GetComponent<Alien>().transform.position = updatedPosition;
                    }
                }
                break;
            }
        }
        if (zReached)
        {
            maxZReached = true;
        }
        // Decide if an UFO should be spawned
        UFOTimer -= deltaT;
        if (UFOTimer <= 0)
        {
            // Spawn an UFO
            Vector3 spawnPos = new Vector3(maxPos.x, 0, 7);
            Quaternion rot = Quaternion.identity;
            rot *= Quaternion.Euler(Vector3.up * -90);
            Instantiate(UFO, spawnPos, rot);
            // Set the next UFO spawn to happen at a random time
            UFOTimer = Random.Range(10, 20);
        }
    }
}

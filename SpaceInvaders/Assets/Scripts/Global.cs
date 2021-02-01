using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static int numAliens = 30; // total number of aliens in the beginning
    public static int rowNum = 3; // number of rows for alien group
    public Vector2 alienSpeed; // initial speed
    public GameObject alien; // alien to spawn
    public GameObject bullet; // alien bullet to spawn
    public GameObject shield; // shield to spawn
    public GameObject UFO; // UFO to spawn
    public List<GameObject> aliens = new List<GameObject>();
    public GameObject[] shields = new GameObject[4]; // shields
    public Vector2 maxPos;
    public bool maxZReached;
    public static float alienTimer = 3; // timer for alien attack
    public static float UFOTimer = 10; // timer for UFO spawn
    public static int remainingLives = 3; // total lives
    // Start is called before the first frame update
    void Start()
    {
        alienSpeed = new Vector2(0.01f, 0.05f); // initial speed
        // Create the aliens
        float z_coor = 3;
        float x_coor = -5f;
        Vector3 spawnPos = new Vector3(x_coor, 0, z_coor);
        maxPos = new Vector2 (Mathf.Abs(spawnPos.x) + 5f, -1f);
        maxZReached = false;
        int idx = 0;
        for (int row = 0; row < rowNum; ++row)
        {
            for (int col = 0; col < numAliens / rowNum; ++col)
            {
                aliens[idx] = Instantiate(alien, spawnPos, Quaternion.identity) as GameObject;
                spawnPos.x += 1.35f;
                idx++;
            }
            spawnPos.z += 1.35f;
            spawnPos.x = -5f;
        }
        // Make aliens ignore collision between each other
        for (int i = 0; i < numAliens; ++i)
        {
            for (int j = 0; j < numAliens; ++j)
            {
                if (i == j) continue;
                Physics.IgnoreCollision(aliens[i].GetComponent<Collider>(), aliens[j].GetComponent<Collider>());
            }
        }
        // Create the shields
        shields[0] = Instantiate(shield, new Vector3(-6, 0, -2.5f), Quaternion.identity) as GameObject;
        shields[1] = Instantiate(shield, new Vector3(-2, 0, -2.5f), Quaternion.identity) as GameObject;
        shields[2] = Instantiate(shield, new Vector3(2, 0, -2.5f), Quaternion.identity) as GameObject;
        shields[3] = Instantiate(shield, new Vector3(6, 0, -2.5f), Quaternion.identity) as GameObject;
    }

    public void RemoveAlien(int id)
    {
        for (int i = 0; i < numAliens; ++i)
        {
            if (aliens[i].GetComponent<Alien>().id == id)
            {
                aliens.RemoveAt(i);
                break;
            }
        }
        // Update remaining number of aliens
        Global.numAliens--;
        // Update alien speed if needed
        if (Global.numAliens % 6 == 0)
        {
            alienSpeed.x += 0.01f;
            alienSpeed.y += 0.05f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float deltaT = Time.deltaTime;
        alienTimer -= deltaT;
        // Make 3 random aliens shoot every 3 seconds
        if (alienTimer <= 0)
        {
            for (int count = 0; count < 3; ++count)
            {
                GameObject randAlien = aliens[Random.Range(0, numAliens)];
                Vector3 spawnPos = randAlien.transform.position;
                spawnPos.z -= (randAlien.transform.lossyScale.z / 2 + 0.1f);
                // instantiate the AlienBullet
                GameObject obj = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
                for (int i = 0; i < numAliens; ++i)
                {
                    Physics.IgnoreCollision(aliens[i].GetComponent<Collider>(), obj.GetComponent<Collider>());
                }
                for (int i = 0; i < shields.Length; ++i)
                {
                    if (shields[i].GetComponent<Shield>().remainingHits == 0)
                    {
                        Physics.IgnoreCollision(shields[i].GetComponent<Collider>(), obj.GetComponent<Collider>());
                    }
                }
            }
            alienTimer = 3;
        }
        // Check if any alien has reached the max distance
        bool zReached = false;
        for (int i = 0; i < numAliens; ++i)
        {
            if (aliens[i].transform.position.x >= maxPos.x || aliens[i].transform.position.x <= -maxPos.x)
            {
                for (int j = 0; j < numAliens; ++j)
                {
                    // Update movement direction
                    aliens[j].GetComponent<Alien>().moveLeft = !aliens[j].GetComponent<Alien>().moveLeft;
                    // Bring aliens closer (unless they're at maximum closeness)
                    Vector3 updatedPosition = aliens[j].GetComponent<Alien>().transform.position;
                    if (!maxZReached)
                    {
                        updatedPosition.z -= alienSpeed.y;
                        if (updatedPosition.z <= maxPos.y)
                        {
                            zReached = true;
                        }
                        aliens[j].GetComponent<Alien>().transform.position = updatedPosition;
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
            Instantiate(UFO, spawnPos, Quaternion.identity);
            // Set the next UFO spawn to happen at a random time
            UFOTimer = Random.Range(10, 20);
        }
    }
}

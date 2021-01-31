using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static int numAliens = 30;
    public GameObject alien; // alien to spawn
    public GameObject bullet; // alien bullet to spawn
    public GameObject shield; // shield to spawn
    public List<GameObject> aliens = new List<GameObject>();
    public GameObject[] shields = new GameObject[4]; // shields
    public float maxPos;
    public static float timer = 3; // timer for alien attack
    public static int remainingLives = 3; // total lives
    // Start is called before the first frame update
    void Start()
    {
        // Create the aliens
        float z_coor = 2;
        float x_coor = -5f;
        Vector3 spawnPos = new Vector3(x_coor, 0, z_coor);
        maxPos = Mathf.Abs(spawnPos.x) + 5f;
        int idx = 0;
        for (int row = 0; row < 3; ++row)
        {
            for (int col = 0; col < numAliens / 3; ++col)
            {
                aliens[idx] = Instantiate(alien, spawnPos, Quaternion.identity) as GameObject;
                spawnPos.x += 1.35f;
                idx++;
            }
            spawnPos.z += 1.35f;
            spawnPos.x = -5f;
        }
        // Create the shields
        shields[0] = Instantiate(shield, new Vector3(-6, 0, -1), Quaternion.identity) as GameObject;
        shields[1] = Instantiate(shield, new Vector3(-2, 0, -1), Quaternion.identity) as GameObject;
        shields[2] = Instantiate(shield, new Vector3(2, 0, -1), Quaternion.identity) as GameObject;
        shields[3] = Instantiate(shield, new Vector3(6, 0, -1), Quaternion.identity) as GameObject;
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
        Global.numAliens--;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        // Make 3 random aliens shoot every 3 seconds
        if (timer <= 0)
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
            timer = 3;
        }
        // Check if any alien has reached the max distance
        for (int i = 0; i < numAliens; ++i)
        {
            if (aliens[i].transform.position.x >= maxPos || aliens[i].transform.position.x <= -maxPos)
            {
                // Update movement direction
                for (int j = 0; j < numAliens; ++j)
                {
                    aliens[j].GetComponent<Alien>().moveLeft = !aliens[j].GetComponent<Alien>().moveLeft;
                }
                break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static int numAliens = 30;
    public GameObject alien; // alien to spawn
    public GameObject bullet; // alien bullet to spawn
    public List<GameObject> aliens = new List<GameObject>();
    public float maxPos;
    public static float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        // Create the aliens
        float z_coor = 0;
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
    }

    public void RemoveAlien(int id)
    {
        Debug.Log("yo!");
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
        timer += Time.deltaTime;
        int seconds = (int)(timer % 60);
        // Make a random alien shoot
        Alien randAlien = aliens[Random.Range(0, numAliens)].GetComponent<Alien>();

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public static int idCount = 0;
    public bool moveLeft;
    public int id;
    public GameObject bullet; // the GameObject to spawn
    // Start is called before the first frame update
    void Start()
    {
        moveLeft = true;
        id = idCount;
        idCount++;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 updatedPosition = gameObject.transform.position;
        if (moveLeft)
        {
            updatedPosition.x -= 0.01f;
        } else
        {
            updatedPosition.x += 0.01f;
        }
        gameObject.transform.position = updatedPosition;
    }

    public void Die()
    {
        // Destroy removes the gameObject from the scene and
        // marks it for garbage collection
        GameObject obj = GameObject.FindWithTag("Global");
        obj.GetComponent<Global>().RemoveAlien(this.id);
        Destroy(gameObject);
    }

    public void Shoot()
    {
        Vector3 spawnPos = gameObject.transform.position;
        spawnPos.z -= (gameObject.transform.lossyScale.z / 2 - 0.1f);
        // instantiate the Bullet
        GameObject obj = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
        // get the Bullet Script Component of the new Bullet instance
        AlienBullet b = obj.GetComponent<AlienBullet>();
    }
}

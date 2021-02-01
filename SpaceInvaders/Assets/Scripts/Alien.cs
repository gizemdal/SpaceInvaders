using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public static int idCount = 0; // each alien has a unique ID
    public bool moveLeft;
    public int id;
    public GameObject bullet; // spawned for alien attack
    public GameObject globalOBJ; // global game object
    // Start is called before the first frame update
    void Start()
    {
        moveLeft = true;
        id = idCount;
        idCount++;
        globalOBJ = GameObject.FindWithTag("Global");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 updatedPosition = gameObject.transform.position;
        if (moveLeft)
        {
            updatedPosition.x -= globalOBJ.GetComponent<Global>().alienSpeed.x;
        } else
        {
            updatedPosition.x += globalOBJ.GetComponent<Global>().alienSpeed.x;
        }
        gameObject.transform.position = updatedPosition;
    }

    public void Die()
    {
        globalOBJ.GetComponent<Global>().RemoveAlien(this.id); // remove the alien with given id
        Destroy(gameObject); // Kill the alien
    }

    public void Shoot()
    {
        // position for the alien attack to be spawned
        Vector3 spawnPos = gameObject.transform.position;
        // add offset
        spawnPos.z -= (gameObject.transform.lossyScale.z / 2 - 0.1f);
        // instantiate the alien attack
        Instantiate(bullet, spawnPos, Quaternion.identity);
    }
}

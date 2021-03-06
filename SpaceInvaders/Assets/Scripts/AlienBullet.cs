﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBullet : MonoBehaviour
{
    public Vector3 thrust; // direction of movement
    public Quaternion heading;
    public GameObject globalOBJ; // global game object
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        globalOBJ = GameObject.FindWithTag("Global");
        // travel straight in the Z-axis
        thrust.z = -400.0f;
        heading.z = 1;

        // do not passively decelerate
        GetComponent<Rigidbody>().drag = 0;

        // set the direction it will travel in
        GetComponent<Rigidbody>().MoveRotation(heading);

        // apply thrust once, no need to apply it again
        GetComponent<Rigidbody>().AddRelativeForce(thrust);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the bullet is outside the screen
        Vector3 currentPos = gameObject.transform.position;
        if (currentPos.z < -20f)
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.CompareTag("Ship"))
        {
            // Kill the ship
            collider.gameObject.GetComponent<Ship>().Kill();
            //Debug.Log("Remaining lives: " + collider.gameObject.GetComponent<Ship>().remainingLives);
            Destroy(gameObject);
        } else if (collider.CompareTag("Shield"))
        {
            // Generate explosion and debris
            Vector3 spawnPos = gameObject.transform.position;
            Instantiate(explosion, spawnPos, Quaternion.identity);
            collider.gameObject.GetComponent<Shield>().HitUpdate();
            Destroy(gameObject);
        } else if (collider.CompareTag("AlienBullet"))
        {
            // Destroy the bullet
            Destroy(gameObject);
        } else if (collider.CompareTag("ShipBullet"))
        {
            // Destroy the bullet
            Destroy(gameObject);
        } else if (collider.CompareTag("Platform")) {
            // Generate explosion and debris
            Vector3 spawnPos = gameObject.transform.position;
            Instantiate(explosion, spawnPos, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

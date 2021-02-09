using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBullet : MonoBehaviour
{
    public Vector3 thrust; // direction of movement
    public Quaternion heading;
    public GameObject shipOBJ; // the ship game object
    public GameObject globalOBJ; // global game object
    public GameObject explosion;

    public AudioClip laser;
    public AudioClip alien_kill;

    // Start is called before the first frame update
    void Start()
    { 
        shipOBJ = GameObject.FindWithTag("Ship");
        globalOBJ = GameObject.FindWithTag("Global");
        // travel straight in the Z-axis
        thrust.z = shipOBJ.GetComponent<Ship>().bulletSpeed;
        heading.z = 1;

        // do not passively decelerate
        GetComponent<Rigidbody>().drag = 0;

        // set the direction it will travel in
        GetComponent<Rigidbody>().MoveRotation(heading);

        // apply thrust once, no need to apply it again
        GetComponent<Rigidbody>().AddRelativeForce(thrust);

        AudioSource.PlayClipAtPoint(laser, gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the bullet is outside the screen
        Vector3 currentPos = gameObject.transform.position;
        if (currentPos.z > 40f)
        {
            // The ship missed the targets
            Global.shipStreak = 0;
            Destroy(gameObject);
            return;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.CompareTag("Alien"))
        {
            // Kill the alien
            Vector3 spawnPos = collider.gameObject.GetComponent<Alien>().transform.position;
            AudioSource.PlayClipAtPoint(alien_kill, gameObject.transform.position);
            collider.gameObject.GetComponent<Alien>().Die();
            Instantiate(explosion, spawnPos, Quaternion.identity);
            // Hit the target successfully - increase streak count
            Global.shipStreak++;
            if (Global.shipStreak % 10 == 0 && !Global.isRewardActive) {
                // generate a reward
                globalOBJ.GetComponent<Global>().generateReward(spawnPos);
            }
            Destroy(gameObject);
        }
        else if (collider.CompareTag("Shield"))
        {
            Vector3 spawnPos = gameObject.transform.position;
            Instantiate(explosion, spawnPos, Quaternion.identity);
            collider.gameObject.GetComponent<Shield>().HitUpdate();
            Global.shipStreak = 0;
            Destroy(gameObject);
        } 
        else if (collider.CompareTag("AlienBullet"))
        {
            // Destroy the bullet
            Destroy(gameObject);
        } 
        else if (collider.CompareTag("UFO"))
        {
            Vector3 spawnPos = collider.gameObject.GetComponent<UFO>().transform.position;
            AudioSource.PlayClipAtPoint(alien_kill, gameObject.transform.position);
            collider.gameObject.GetComponent<UFO>().Die();
            Instantiate(explosion, spawnPos, Quaternion.identity);
            // Hit the target successfully - increase streak count
            Global.shipStreak++;
            if (Global.shipStreak % 10 == 0 && !Global.isRewardActive) {
                // generate a reward
                globalOBJ.GetComponent<Global>().generateReward(spawnPos);
            }
            Destroy(gameObject);
        }
        else if (collider.CompareTag("Debris"))
        {
            Destroy(gameObject);
        }
    }
}

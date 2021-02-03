using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBullet : MonoBehaviour
{
    public Vector3 thrust; // direction of movement
    public Quaternion heading;
    public GameObject shipOBJ; // the ship game object

    // Start is called before the first frame update
    void Start()
    {
        shipOBJ = GameObject.FindWithTag("Ship");
        // travel straight in the Z-axis
        thrust.z = shipOBJ.GetComponent<Ship>().bulletSpeed;
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
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.CompareTag("Alien"))
        {
            // Kill the alien
            collider.gameObject.GetComponent<Alien>().Die();
            Destroy(gameObject);
        }
        else if (collider.CompareTag("Shield"))
        {
            collider.gameObject.GetComponent<Shield>().HitUpdate();
            Destroy(gameObject);
        } 
        else if (collider.CompareTag("AlienBullet"))
        {
            // Destroy the bullet
            Destroy(gameObject);
        } 
        else if (collider.CompareTag("UFO"))
        {
            // TODO: Give player points
            // Kill the UFO
            collider.gameObject.GetComponent<UFO>().Die();
        }
    }
}

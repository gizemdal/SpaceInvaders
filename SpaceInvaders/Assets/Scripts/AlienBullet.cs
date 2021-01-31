using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBullet : MonoBehaviour
{
    public Vector3 thrust; // direction of movement
    public Quaternion heading;
    // Start is called before the first frame update
    void Start()
    {
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
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        if (collider.CompareTag("Ship"))
        {
            // Subtract from remaining lives
            Global.remainingLives--;
            Debug.Log("Remaining lives: " + Global.remainingLives);
            Destroy(gameObject);
        } else if (collider.CompareTag("Shield"))
        {
            collider.gameObject.GetComponent<Shield>().HitUpdate();
            Destroy(gameObject);
        }
    }
}

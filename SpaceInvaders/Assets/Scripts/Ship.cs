using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    // some public variables to alter ship's movement
    public float moveAcceleration;
    public GameObject bullet; // the GameObject to spawn

    // Start is called before the first frame update
    void Start()
    {
        moveAcceleration = 0.1f;
    }

    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            Vector3 updatedPosition = gameObject.transform.position;
            updatedPosition.x += moveAcceleration;
            gameObject.transform.position = updatedPosition;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            Vector3 updatedPosition = gameObject.transform.position;
            updatedPosition.x -= moveAcceleration;
            gameObject.transform.position = updatedPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire! ");
            /* we don’t want to spawn a Bullet inside our ship, so some
            Simple trigonometry is done here to spawn the bullet
            at the tip of where the ship is pointed.
            */
            Vector3 spawnPos = gameObject.transform.position;
            spawnPos.z += (gameObject.transform.lossyScale.z / 2 + 0.1f);
            // instantiate the Bullet
            GameObject obj = Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
            // get the Bullet Script Component of the new Bullet instance
            ShipBullet b = obj.GetComponent<ShipBullet>();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardScript : MonoBehaviour
{
        public Vector3 thrust; // direction of movement
        public Quaternion heading;
        public int rewardIdx;
        public GameObject sparkle; // reward sparkle
        public GameObject shipOBJ; // ship object
        public GameObject globalOBJ; // ship object
    // Start is called before the first frame update
    void Start()
    {
        shipOBJ = GameObject.FindWithTag("Ship");
        globalOBJ = GameObject.FindWithTag("Global");
        thrust.z = -400.0f;
        heading.z = 1;
        Physics.IgnoreLayerCollision(12, 9); // ignore certain layer collisions
        Physics.IgnoreLayerCollision(12, 10); // ignore certain layer collisions
        Physics.IgnoreLayerCollision(12, 11); // ignore certain layer collisions
        Physics.IgnoreLayerCollision(12, 13); // ignore certain layer collisions

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
        if (gameObject.transform.position.z < -10)
        {
            // reward fell down - just destroy it
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        Vector3 spawnPos = gameObject.GetComponent<RewardScript>().transform.position;
        if (collider.CompareTag("Ship")) 
        {
            Global.activeReward = rewardIdx;
            // Earn the reward
            switch(rewardIdx)
            {
                case 0:
                    // Generate extra life
                    Ship.remainingLives++;
                    break;
                case 1:
                    // Generate bullet speed
                    shipOBJ.GetComponent<Ship>().bulletSpeed *= 1.5f;
                    globalOBJ.GetComponent<Global>().fasterBullet.gameObject.SetActive(true);
                    globalOBJ.GetComponent<Global>().fasterBullet.enabled = true;
                    Global.isRewardActive = true;
                    break;
                case 2:
                    // Generate faster bullet reload
                    shipOBJ.GetComponent<Ship>().bulletBuffer *= 0.5f;
                    globalOBJ.GetComponent<Global>().fasterBuffer.gameObject.SetActive(true);
                    globalOBJ.GetComponent<Global>().fasterBuffer.enabled = true;
                    Global.isRewardActive = true;
                    break;
                default:
                    break;
            }
            Instantiate(sparkle, spawnPos, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}

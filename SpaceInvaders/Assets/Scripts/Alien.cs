using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public static int idCount = 0; // each alien has a unique ID
    public static float timer = 1;
    public bool moveLeft;
    public int id;
    public GameObject bullet; // spawned for alien attack
    public GameObject globalOBJ; // global game object
    public Mesh firstMesh; // initial mesh
    public Mesh secondMesh; // other mesh
    public float meshTimer; // timer for alien move change
    public bool initMesh = true;
    public int score; // score value
    // Start is called before the first frame update
    protected virtual void Start()
    {
        moveLeft = true;
        id = idCount;
        idCount++;
        globalOBJ = GameObject.FindWithTag("Global");
        meshTimer = timer;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!Global.isGameOver && Time.timeScale > 0)
        {
            Vector3 updatedPosition = gameObject.transform.position;
            if (moveLeft)
            {
                updatedPosition.x -= globalOBJ.GetComponent<Global>().alienSpeed.x;
            }
            else
            {
                updatedPosition.x += globalOBJ.GetComponent<Global>().alienSpeed.x;
            }
            gameObject.transform.position = updatedPosition;
            // Update the mesh
            float deltaT = Time.deltaTime;
            meshTimer -= deltaT;
            if (meshTimer <= 0)
            {
                MeshFilter currentMesh = gameObject.GetComponent<MeshFilter>();
                // Switch jelly mesh
                if (initMesh)
                {
                    currentMesh.mesh = firstMesh;
                    initMesh = false;
                }
                else
                {
                    currentMesh.sharedMesh = secondMesh;
                    initMesh = true;
                }
                meshTimer = timer;
            }
        }
    }

    public void Die()
    {
        globalOBJ.GetComponent<Global>().RemoveAlien(this.id); // remove the alien with given id
        Destroy(gameObject); // Kill the alien
    }

    public GameObject Shoot()
    {
        // position for the alien attack to be spawned
        Vector3 spawnPos = gameObject.transform.position;
        // instantiate the alien attack
        return Instantiate(bullet, spawnPos, Quaternion.identity) as GameObject;
    }
}

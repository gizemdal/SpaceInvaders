using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public float xSpeed; // speed of the UFO in x-direction
    public GameObject globalOBJ; // global game object
    // Start is called before the first frame update
    void Start()
    {
        xSpeed = 0.015f;
        globalOBJ = GameObject.FindWithTag("Global");
    }

    // Update is called once per frame
    void Update()
    {
        // Check if UFO is outside the screen
        Vector3 currentPos = gameObject.transform.position;
        if (currentPos.x < -globalOBJ.GetComponent<Global>().maxPos.x)
        {
            Die();
        }
        currentPos.x -= xSpeed;
        gameObject.transform.position = currentPos;
    }

    public void Die()
    {
        // Destroy the UFO
        Destroy(gameObject);
    }
}

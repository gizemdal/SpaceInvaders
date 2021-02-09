using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public float xSpeed; // speed of the UFO in x-direction
    public GameObject globalOBJ; // global game object
    public GameObject explosion; // explosion debris
    public int score; // score value

    // Start is called before the first frame update
    void Start()
    {
        score = Random.Range(80, 101); // Randomize score
        xSpeed = 0.045f;
        globalOBJ = GameObject.FindWithTag("Global");
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Global.isGameOver && !Global.isPause)
        {
            // Check if UFO is outside the screen
            Vector3 currentPos = gameObject.transform.position;
            if (currentPos.x < -globalOBJ.GetComponent<Global>().maxPos.x)
            {
                GetComponent<AudioSource>().Stop();
                Destroy(gameObject);
                return;
            }
            currentPos.x -= xSpeed;
            gameObject.transform.position = currentPos;
        }
    }

    public void Die()
    {
        // Destroy the UFO
        Global.playerScore += score;
        // Create debris
        Vector3 spawnPos = gameObject.transform.position;
        Instantiate(explosion, spawnPos, Quaternion.identity);
        GetComponent<AudioSource>().Stop();
        Destroy(gameObject);
    }
}

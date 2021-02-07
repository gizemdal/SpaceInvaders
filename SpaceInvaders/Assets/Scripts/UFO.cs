using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public float xSpeed; // speed of the UFO in x-direction
    public GameObject globalOBJ; // global game object
    public int score; // score value
    // Start is called before the first frame update
    void Start()
    {
        score = Random.Range(40, 51); // Randomize score
        xSpeed = 0.025f;
        globalOBJ = GameObject.FindWithTag("Global");
    }

    // Update is called once per frame
    void Update()
    {
        if (!Global.isGameOver && Time.timeScale > 0)
        {
            // Check if UFO is outside the screen
            Vector3 currentPos = gameObject.transform.position;
            if (currentPos.x < -globalOBJ.GetComponent<Global>().maxPos.x)
            {
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
        Destroy(gameObject);
    }
}

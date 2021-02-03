using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update
    public static int idCount = 0; // each shield has a unique ID
    public GameObject globalOBJ; // global game object
    public int id;
    public int remainingHits;
    void Start()
    {
        globalOBJ = GameObject.FindWithTag("Global");
        id = idCount;
        remainingHits = 5;
        idCount++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Break()
    {
        globalOBJ.GetComponent<Global>().RemoveShield(this.id); // remove the shield with given id
        Destroy(gameObject); // Break the shield
    }

    public void HitUpdate()
    {
        remainingHits--;
        // Each hit makes the shield shrink
        if (GetComponent<Renderer>().enabled)
        {
            Vector3 scaleChange = new Vector3(-0.4f, 0, 0);
            gameObject.transform.localScale += scaleChange;
        }
        if (remainingHits == 0)
        {
            Break();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update
    public int remainingHits = 5;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitUpdate()
    {
        remainingHits--;
        if (remainingHits == 0)
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
}

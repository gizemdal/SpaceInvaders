﻿using System.Collections;
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
        // Each hit makes the shield shrink
        if (GetComponent<Renderer>().enabled)
        {
            Vector3 scaleChange = new Vector3(-0.4f, 0, 0);
            gameObject.transform.localScale += scaleChange;
        }
        if (remainingHits == 0)
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : Alien
{
    // Start is called before the first frame update
    
    protected override void Start()
    {
        score = 30;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}

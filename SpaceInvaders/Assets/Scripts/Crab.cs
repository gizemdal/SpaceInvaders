using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : Alien
{
    // Start is called before the first frame update
    protected override void Start()
    {
        score = 20;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}

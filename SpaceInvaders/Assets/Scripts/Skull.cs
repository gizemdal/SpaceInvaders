using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Alien
{
    // Start is called before the first frame update
    protected override void Start()
    {
        score = 10;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}

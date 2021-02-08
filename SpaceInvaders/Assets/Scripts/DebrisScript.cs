using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisScript : MonoBehaviour
{
    ParticleSystem.Particle[] m_Particles;
    ParticleSystem m_System;
    int numParticles;
    // Start is called before the first frame update
    void Start()
    {

    }

    void LateUpdate()
    {
        if (m_System == null) {
            m_System = GetComponent<ParticleSystem>();
            if (m_Particles == null) {
                m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
            }
        }
        numParticles = m_System.GetParticles(m_Particles);
        bool canDestroy = true;
        for (int i = 0; i < numParticles; ++i) {
            if (m_Particles[i].position.z > -10) {
                canDestroy = false;
            }
        }
        if (canDestroy) {
            // Give points to player for removing all the debris
            Global.playerScore += 1;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}

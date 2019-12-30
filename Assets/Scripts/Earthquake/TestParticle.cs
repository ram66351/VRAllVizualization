using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticle : MonoBehaviour
{
    public ParticleSystem m_System;
    public ParticleSystem.Particle[] m_Particles;
    public int numParticlesAlive;
    public Vector3[] pos;

    public bool receivedPos;

    public static TestParticle Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        //m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
    }

    // Update is called once per frame
    void Update()
    {
        if(receivedPos)
        {

            //numParticlesAlive = m_System.GetParticles(m_Particles);
            //if (numParticlesAlive < pos.Length)
            //{
            //    //pos = new Vector3[numParticlesAlive];
            //    for (int i = 0; i < numParticlesAlive; i++)
            //    {
            //        m_Particles[i].position = pos[i];
            //    }

            //    m_System.SetParticles(m_Particles, numParticlesAlive);
            //}   

            m_Particles = new ParticleSystem.Particle[pos.Length];
            for(int i=0; i<pos.Length; i++)
            {
                m_Particles[i].position = pos[i] * 10;
                m_Particles[i].size = 0.01f;
            }

            m_System.SetParticles(m_Particles, m_Particles.Length);
        }    
    }

    public void SetParticlePos(Vector3[] pos)
    {
        this.pos = pos;
        receivedPos = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCloud : MonoBehaviour
{
    ParticleSystem.Particle[] cloud;
    bool bPointsUpdated = false;
    public ParticleSystem ps;

    public static PointCloud Instance;

    public ParticleSystem m_System;
    public ParticleSystem.Particle[] m_Particles;
    public int numParticlesAlive;

    public bool GotPos = false;
    void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {
        m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
    }

    void Update()
    {
        if (bPointsUpdated)
        {
            ps.SetParticles(cloud, cloud.Length);
            ps.Emit(cloud.Length);
            Debug.Log("Applide particles : "+ cloud.Length);
            bPointsUpdated = false;
        }
    }

    private void LateUpdate()
    {
        
        // GetParticles is allocation free because we reuse the m_Particles buffer between updates
        int particlesAlive = m_System.GetParticles(m_Particles);
       
        // Change only the particles that are alive
        for (int i = 0; i < particlesAlive; i++)
        {
            m_Particles[i].position = new Vector3(1,0,0);
        }

        // Apply the particle changes to the Particle System
        m_System.SetParticles(m_Particles, numParticlesAlive);
    }

    public void SetPoints(Vector3[] positions)
    {
        Debug.Log("SetPoints called : " + positions.Length);

        GotPos = true;
        ////cloud = new ParticleSystem.Particle[positions.Length];
        //for (int ii = 0; ii < positions.Length; ++ii)
        //{
        //    cloud[ii].position = positions[ii];
        //    //cloud[ii].color = colors[ii];
        //    //cloud[ii].size = 0.001f;
        //}

        ////bPointsUpdated = true;
    }
}

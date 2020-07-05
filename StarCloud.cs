using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class StarCloud : MonoBehaviour
{
    public LineRenderer lineRendererTempplate;
    List<LineRenderer> lineRenderers = new List<LineRenderer>();

    public int maxLineRenderers = 10;


    new ParticleSystem m_particleSystem;


    ParticleSystem.Particle[] particles;

    Vector3[] particlePosition;
    Color[] particleColours;
    float[] particleSize;


    float timer = 0;

    [Range(0.0f, 1.0f)]
    public float delay = 0.0f;


    Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        m_particleSystem = GetComponent<ParticleSystem>();

        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        int maxParticles = m_particleSystem.main.maxParticles;

        if(particles==null||particles.Length<maxParticles)
        {
            particles = new ParticleSystem.Particle[maxParticles];//只是初始化了内存空间但是并未得到粒子系统产生的粒子

            particlePosition = new Vector3[maxParticles];
            particleColours = new Color[maxParticles];

            particleSize = new float[maxParticles];

        }
      


        timer += Time.deltaTime;
        if(timer>delay)
        {

            int lrIndex = 0;

            timer = 0;
           
            m_particleSystem.GetParticles(particles);

            //得到所有粒子属性
            for (int i = 0; i <m_particleSystem.particleCount ;i++)
            {
                particlePosition[i] = particles[i].position;

                particleColours[i] = particles[i].GetCurrentColor(m_particleSystem);
                particleSize[i] = particles[i].GetCurrentSize(m_particleSystem);

            }


            for (int i=0;i<m_particleSystem.particleCount;i++)
            {
                

                for(int j=i+1;j<m_particleSystem.particleCount;j++)
                {
                    //初始化线渲染器
                    LineRenderer lr;

                    if (lineRenderers.Count<maxLineRenderers)
                    {
                        lr = Instantiate(lineRendererTempplate, _transform);
                        lineRenderers.Add(lr);
                    }

                    //怎么渲染出来?
                    lr = lineRenderers[lrIndex];
                    lr.enabled = true;


                    //设置线的属性等
                    lr.SetPosition(0,particlePosition[i]);
                    lr.SetPosition(1,particlePosition[j]);

                    lr.startColor = particleColours[i];
                    lr.endColor = particleColours[j];


                    lr.startWidth = particleSize[i];
                    lr.endWidth = particleSize[j];


                    lrIndex++;

                   
                }
                for(int j=i+1;j<m_particleSystem.particleCount-1;j++)
                {
                    //初始化线渲染器
                    LineRenderer lr;

                    if (lineRenderers.Count < maxLineRenderers)
                    {
                        lr = Instantiate(lineRendererTempplate, _transform);
                        lineRenderers.Add(lr);
                    }

                    //怎么渲染出来?
                    lr = lineRenderers[lrIndex];
                    lr.enabled = true;


                    //设置线的属性等
                    lr.SetPosition(0, particlePosition[j]);
                    lr.SetPosition(1, particlePosition[j+1]);

                    lr.startColor = particleColours[j];
                    lr.endColor = particleColours[j+1];


                    lr.startWidth = particleSize[j];
                    lr.endWidth = particleSize[j+1];


                    lrIndex++;
                }

                break;
            }

        }



    }
}

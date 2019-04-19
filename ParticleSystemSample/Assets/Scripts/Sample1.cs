using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Sample for control multiple particle system rewind and play again after rewind. 
 */
public class Sample1 : MonoBehaviour
{

    [SerializeField]
    ParticleSystem ps;

    ParticleSystem []rewinds = new ParticleSystem[2];
    // record each particle system simulationTime to implement rewind effect.
    float[]simulationTimes = new float[2];
    public float startTime = 2.0f;
    public float simulationSpeedScale = 1.0f;
    // partical system sample position shift
    float shift = 1.0f;
    bool isDnoe = false;
    void Start()
    {
        // only play rewind 
        var rewind = Instantiate(ps) as ParticleSystem;
        var shapeModule = rewind.shape;
        shapeModule.position = new Vector3(0f, 0, 0f);
        rewinds[0] = rewind;
        // play normal after play rewind 
        var rewindAndPlay = Instantiate(ps) as ParticleSystem;
        shapeModule = rewindAndPlay.shape;
        shapeModule.position = new Vector3(0f, -shift, 0f);
        rewinds[1] = rewindAndPlay;
        // noraml play sample
        ps.Play();
    }

    void Update()
    {
        if(!isDnoe)
        {
            for (int i =0; i < rewinds.Length; i++)
            {
                rewinds[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                bool useAutoRandomSeed = ps.useAutoRandomSeed;
                rewinds[i].useAutoRandomSeed = false;
                rewinds[i].Play(false);
                //  caculate simulation time for rewind.
                float deltaTime = rewinds[i].main.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                simulationTimes[i] += (-deltaTime * rewinds[i].main.simulationSpeed) * simulationSpeedScale;
                float currentSimulationTime = startTime + simulationTimes[i];

                // rewind.
                rewinds[i].Simulate(currentSimulationTime, false, false, true);
                rewinds[i].useAutoRandomSeed = useAutoRandomSeed;
                // After rewind play again.
                if (i == 1 && currentSimulationTime <= 0)
                {  
                    isDnoe = true;
                    rewinds[1].Simulate(0.0f, true, true);
                    rewinds[1].Play();
                }
            }
        }
    }
}

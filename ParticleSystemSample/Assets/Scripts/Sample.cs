using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 *  Sample for basic particle system. 
 */
public class Sample : MonoBehaviour
{
    [SerializeField]
    ParticleSystem ps;

    void Start()
    {
        ps.Play();
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Sample for use particle system to display text through camera convert text 
 * to texture2D, then put in particle system.  
 */
public class Sample2 : MonoBehaviour
{
    [SerializeField]
    ParticleSystem ps;
    [SerializeField]
    Camera textCamera;
    [SerializeField]
    Text word;
    [SerializeField]
    RenderTexture renderTexture;

    private string[] wordList = {
    "Hello, World!",
    "Sampe lien 1.",
    "Sampe lien 2.",
    "Sampe lien 3.",
    "Magical Particle System."
    };

    private bool launch = false;
    private bool done = false;
    private int index = 0;
    private Texture2D tmp = null;
    private float startTime = 2.0f;
    private float simulationSpeedScale = 1.0f;
    private float simulationTimes = 0f;

    void Start()
    {
    }

    void Update()
    {
        if (!done)
        {
            if(!launch)
            { 
                word.text = wordList[index];
                tmp = CaptureScreen();
                var shapeModule = ps.shape;
                shapeModule.texture = tmp;
                ps.Simulate(0.0f, true, true);
                launch = true;
                ps.Play();
            }
            else
            {
                // caculate simulate time for check is done or not.
                float deltaTime = ps.main.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                simulationTimes += (-deltaTime * ps.main.simulationSpeed) * simulationSpeedScale;
                float currentSimulationTime = startTime + simulationTimes;
                // play next word or stop play when play done
                if (currentSimulationTime <= 0)
                {
                    simulationTimes = 0.0f;
                    launch = false;
                    index++;
                    done |= index == wordList.Length;
                }
            }
        }
    }
    /*
     *  Convert Text to Texture2D by camera.  
     */   
    private Texture2D CaptureScreen()
    {
        textCamera.Render();
        var currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        var tex = new Texture2D(textCamera.pixelWidth, textCamera.pixelHeight, TextureFormat.RGBA32, false);
        tex.ReadPixels(textCamera.pixelRect, 0, 0);
        tex.Apply();
        RenderTexture.active = currentRT;
        return tex;
    }
}

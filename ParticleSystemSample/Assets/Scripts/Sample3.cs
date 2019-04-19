using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Sample for rewind particle system and pause in the screen.
 */
public class Sample3 : MonoBehaviour
{
    [SerializeField]
    ParticleSystem ps;
    [SerializeField]
    ParticleSystem textps;
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
    private int index = -1;
    private Texture2D tmp = null;
    private float startTime = 2.0f;
    private float simulationSpeedScale = 1.0f;
    private float simulationTimes = 0f;
    private float shift = 0.0f;
    ParticleSystem target;

    void Start()
    {
        shift = 7f / (wordList.Length + 1);
    }

    void Update()
    {
        if (!done)
        {
            if (!launch)
            {
                if (index < 0)
                {
                    target = Instantiate(ps) as ParticleSystem;
                }
                else
                {
                    target = Instantiate(textps) as ParticleSystem;
                }
                target.Simulate(startTime, true, false, true);
                var shapeModule = target.shape;
                shapeModule.position = new Vector3(0f, 3.5f - (index + 1) * shift, 0f);
                if (index >= 0)
                {
                    word.text = wordList[index];
                    tmp = CaptureScreen();
                    shapeModule.texture = tmp;
                }
                launch = true;
            }

            if (launch)
            {
                target.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                bool useAutoRandomSeed = target.useAutoRandomSeed;
                target.useAutoRandomSeed = false;
                target.Play(false);
                // caculate simulate time for rewind.
                float deltaTime = target.main.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                simulationTimes += (-deltaTime * target.main.simulationSpeed) * simulationSpeedScale;

                float currentSimulationTime = startTime + simulationTimes;
                // rewind
                target.Simulate(currentSimulationTime, false, false, true);

                target.useAutoRandomSeed = useAutoRandomSeed;
                // keep picture in the screen and change next word or done.
                if (currentSimulationTime < 0.3f)
                {
                    target.Pause();
                    index++;
                    launch = false;
                    done |= index == wordList.Length;
                    startTime = 2.0f;
                    simulationTimes = 0.0f;
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

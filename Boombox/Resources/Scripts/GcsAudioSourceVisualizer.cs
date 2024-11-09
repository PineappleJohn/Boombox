using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceVisualizer : MonoBehaviour
{
    //[Header("Made by Glitched Cat Studios!\nPlease give credits if you use it!")]
    //[Space] Yes i know i commented it (i'm not a skid, i can code)
    public AudioSource audioSource;
    public GameObject[] visualizerBars;
    public float heightMultiplier = 10.0f;
    public BarAlignment barAlignment = BarAlignment.Middle;
    public bool rgbBars = true;
    public float rgbSpeed = 1.0f;

    private float[] samples = new float[64];
    private float[] spectrum = new float[64];
    private float[] barHeights;

    float a = 0f;
    private void Start()
    {
        barHeights = new float[visualizerBars.Length];

        for (int i = 0; i < visualizerBars.Length; i++)
        {
            barHeights[i] = visualizerBars[i].transform.localScale.y;
        }

        a = barHeights[0];
    }

    float hue = 0f;
    private void Update()
    {
        if (rgbBars)
        {
            foreach (var bar in visualizerBars)
            {
                Renderer renderer = bar.GetComponent<Renderer>();
                hue = (hue + Time.deltaTime * rgbSpeed) % 1.0f;
                Color prog = Color.HSVToRGB(hue, 1, 1);
                renderer.material.color = prog;
            }
        }

        if (audioSource.isPlaying)
        {
            audioSource.GetOutputData(samples, 0);
            audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

            int samplesPerBar = spectrum.Length / visualizerBars.Length;

            for (int i = 0; i < visualizerBars.Length; i++)
            {
                float sum = 0.0f;

                for (int j = 0; j < samplesPerBar; j++)
                {
                    int index = i * samplesPerBar + j;
                    if (index < spectrum.Length)
                    {
                        sum += spectrum[index];
                    }
                }

                float averageHeight = (sum / samplesPerBar) * heightMultiplier;
                barHeights[i] = Mathf.Lerp(barHeights[i], averageHeight, Time.deltaTime * 50);
            }

            for (int i = 0; i < visualizerBars.Length; i++)
            {
                float barHeight = barHeights[i];
                Vector3 scale = visualizerBars[i].transform.localScale;
                Vector3 position = visualizerBars[i].transform.localPosition;

                if (barHeight > a) barHeight = a;

                switch (barAlignment)
                {
                    case BarAlignment.Middle:
                        visualizerBars[i].transform.localScale = new Vector3(scale.x, barHeight, scale.z);
                        break;
                    case BarAlignment.Bottom:
                        visualizerBars[i].transform.localScale = new Vector3(scale.x, barHeight, scale.z);
                        visualizerBars[i].transform.localPosition = new Vector3(position.x, barHeight / 2, position.z);
                        break;
                    case BarAlignment.Top:
                        visualizerBars[i].transform.localScale = new Vector3(scale.x, barHeight, scale.z);
                        visualizerBars[i].transform.localPosition = new Vector3(position.x, -barHeight / 2, position.z);
                        break;
                }
            }
        }
    }
}

public enum BarAlignment
{
    Middle,
    Bottom,
    Top
}

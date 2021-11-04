using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent (typeof(AudioListener))]
public class AudioSpectrumAnalyzer : MonoBehaviour
{
    public const int SampleCount = 512;
    public const int BandCount = 64;

    public FFTWindow FFTWindow = FFTWindow.BlackmanHarris;
    [Range(0.25f, 2.00f)]
    public float Decay = 1.00f;

    private AudioListener Listener;

    private float[] Samples = new float[SampleCount];
    private float[] Bands = new float[BandCount];

    private float MovingMaximum = 0.0f;
    private float[] SmoothBands = new float[BandCount];

    void Start()
    {
        Listener = GetComponent<AudioListener>();
    }

    void Update()
    {
        AnalyzeSpectrum();
        GenerateBands();
        NormalizeBands();
        CalculateDecay();
    }

    void AnalyzeSpectrum()
    {
        AudioListener.GetSpectrumData(Samples, 0, FFTWindow);
    }

    void GenerateBands()
    {
        int currentSample = 0;

        float[] normalizedSamples = new float[SampleCount];
        for (int i = 0; i < SampleCount; i++)
        {
            normalizedSamples[i] = Samples[i] * (i + 1);
        }

        // Bands 0 to 15
        int samplesPerBand = 1;
        int start = 0;
        int end   = 15;
        for (int i = start; i <= end; i++)
        {
            Bands[i] = normalizedSamples.Skip(currentSample).Take(samplesPerBand).Average();
            currentSample += samplesPerBand;
        }

        // Bands 16 to 31
        samplesPerBand = 2;
        start = 16;
        end   = 31;
        for (int i = start; i <= end; i++)
        {
            Bands[i] = normalizedSamples.Skip(currentSample).Take(samplesPerBand).Average();
            currentSample += samplesPerBand;
        }

        // Bands 32 to 39
        samplesPerBand = 4;
        start = 32;
        end = 39;
        for (int i = start; i <= end; i++)
        {
            Bands[i] = normalizedSamples.Skip(currentSample).Take(samplesPerBand).Average();
            currentSample += samplesPerBand;
        }

        // Bands 40 to 47
        samplesPerBand = 6;
        start = 40;
        end = 47;
        for (int i = start; i <= end; i++)
        {
            Bands[i] = normalizedSamples.Skip(currentSample).Take(samplesPerBand).Average();
            currentSample += samplesPerBand;
        }
        
        // Bands 48 to 55
        samplesPerBand = 16;
        start = 48;
        end = 55;
        for (int i = start; i <= end; i++)
        {
            Bands[i] = normalizedSamples.Skip(currentSample).Take(samplesPerBand).Average();
            currentSample += samplesPerBand;
        }

        // Bands 56 to 63
        samplesPerBand = 32;
        start = 56;
        end = 63;
        for (int i = start; i <= end; i++)
        {
            Bands[i] = normalizedSamples.Skip(currentSample).Take(samplesPerBand).Average();
            currentSample += samplesPerBand;
        }
    }

    void NormalizeBands()
    {
        float currentMax = Bands.Max();
        if (currentMax > MovingMaximum)
        {
            MovingMaximum = currentMax;
        }

        for (int i = 0; i < Bands.Length; i++)
        {
            Bands[i] = Bands[i] / MovingMaximum;
        }
    }

    void CalculateDecay()
    {
        float time = Time.deltaTime;
        float amplitudeDecay = Decay * time;

        MovingMaximum = MovingMaximum - amplitudeDecay;
        MovingMaximum = Mathf.Max(MovingMaximum, 0f);

        for (int i = 0; i < Bands.Length; i++)
        {
            if (Bands[i] > SmoothBands[i])
            {
                SmoothBands[i] = Bands[i];
            }
            else
            {
                SmoothBands[i] = SmoothBands[i] - amplitudeDecay;
                SmoothBands[i] = Mathf.Max(SmoothBands[i], 0f);
            }
        }
    }

    public IReadOnlyCollection<float> GetSamples()
    {
        return Samples as IReadOnlyCollection<float>;
    }

    public IReadOnlyCollection<float> GetBands()
    {
        return Bands as IReadOnlyCollection<float>;
    }

    public IReadOnlyCollection<float> GetBandsSmooth()
    {
        return SmoothBands as IReadOnlyCollection<float>;
    }
}

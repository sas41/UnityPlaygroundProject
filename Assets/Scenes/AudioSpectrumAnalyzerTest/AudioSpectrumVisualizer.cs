using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioSpectrumVisualizer : MonoBehaviour
{
    public GameObject SpectrumAnalyzer;
    public float Scale = 5;
    private GameObject[] Cubes;
    private AudioSpectrumAnalyzer Analyzer;

    void Start()
    {
        if (SpectrumAnalyzer != null)
        {
            Analyzer = SpectrumAnalyzer.GetComponent<AudioSpectrumAnalyzer>();
        }

        Cubes = new GameObject[AudioSpectrumAnalyzer.BandCount];

        float r = 0;
        float g = 0;
        float b = 0;
        int count = Cubes.Length;
        for (int i = 0; i < count; i++)
        {
            Cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Cubes[i].transform.parent = this.transform;
            Cubes[i].transform.localPosition += new Vector3(1 * i, 0, 0);

            int start = 0;
            int mid1 = (count / 4) -1;
            int mid2 = (count / 2) - 1;
            int mid3 = ((count / 4) * 3) -1;
            int end = count-1;

            float div = mid2 - mid1;

            if (i >= start && i <= mid1)
            {
                r += (1 / div);
            }
            else if(i > mid1 && i <= mid2)
            {
                r -= (1 / div);
            }

            if (i >= mid1 && i <= mid2)
            {
                g += (1 / div);
            }
            else if (i > mid2 && i <= mid3)
            {
                g -= (1 / div);
            }

            if (i >= mid2 && i <= mid3)
            {
                b += (1 / div);
            }
            else if (i > mid3 && i <= end)
            {
                b -= (1 / div);
            }

            Color col = new Color(r, g, b);
            Cubes[i].GetComponent<MeshRenderer>().material.color = col;

        }
    }

    void Update()
    {
        var bands = Analyzer.GetBandsSmooth().ToArray();
        for (int i = 0; i < Cubes.Length; i++)
        {
            Vector3 scale = Cubes[i].transform.localScale;
            scale.y = 1f + (bands[i] * Scale);
            Cubes[i].transform.localScale = scale;
        }
    }
}

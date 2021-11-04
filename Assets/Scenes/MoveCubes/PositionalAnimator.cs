using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalAnimator : MonoBehaviour
{
    public enum AnimationType
    {
        SineWave,
        CenterWave,
        Circle,
        GoldenRatio,
        Galaxy
    }

    public enum DetectionType
    {
        FromArray,
        FromChildren
    }

    public DetectionType DetectionMethod;
    public AnimationType AnimationTypeSelection;
    public float MovementMagnitude = 3.0f;
    public float GalaxyTurnRate = 0.07f;
    public GameObject[] Objects;
    private Transform[] ObjectTransforms;
    private Vector3[] ObjectOrigins;
    private float GoldenRatio;


    // Start is called before the first frame update
    void Start()
    {
        GoldenRatio = (1 + Mathf.Sqrt(5)) / 2;

        if (DetectionMethod == DetectionType.FromChildren)
        {
            var childrenCount = this.transform.childCount;
            Objects = new GameObject[childrenCount];
            for (int i = 0; i < childrenCount; i++)
            {
                Objects[i] = this.transform.GetChild(i).gameObject;
            }
        }

        ObjectTransforms = new Transform[Objects.Length];
        ObjectOrigins = new Vector3[Objects.Length];
        int pos = 0;
        foreach(var obj in Objects)
        {
            ObjectTransforms[pos] = obj.GetComponent<Transform>();
            ObjectOrigins[pos] = obj.GetComponent<Transform>().position;
            pos++;
        }    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (AnimationTypeSelection == AnimationType.SineWave)
        {
            float time = UnityEngine.Time.time;
            for (int i = 0; i < ObjectTransforms.Length; i++)
            {
                var transform = ObjectTransforms[i];
                var pos = ObjectOrigins[i];
                pos.y = pos.y + (Mathf.Sin(time + i) * MovementMagnitude);
                transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime / 2);
            }
        }
        else if (AnimationTypeSelection == AnimationType.CenterWave)
        {
            float time = UnityEngine.Time.time;
            int full = ObjectTransforms.Length - 1;
            int half = ObjectTransforms.Length / 2 - 1;

            for (int i = half; i >= 0; i--)
            {
                float offset = (Mathf.Sin(time + i) * MovementMagnitude);
                Transform transform;
                Vector3 pos;

                transform = ObjectTransforms[i];
                pos = ObjectOrigins[i];
                pos.y += offset;
                transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime / 2);

                transform = ObjectTransforms[full-i];
                pos = ObjectOrigins[full - i];
                pos.y += offset;
                transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime / 2);
            }

        }
        else if (AnimationTypeSelection == AnimationType.Circle)
        {
            float time = UnityEngine.Time.time;
            for (int i = 0; i < ObjectTransforms.Length; i++)
            {
                var transform = ObjectTransforms[i];
                var pos = ObjectOrigins[i];
                var theta = (Mathf.PI * 2) / ObjectTransforms.Length;
                var offset = theta * i;
                pos.y = pos.y + (Mathf.Sin(time + offset) * MovementMagnitude);
                pos.x = pos.x + (Mathf.Cos(time + offset) * MovementMagnitude);
                transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime / 2);
            }
        }
        else if (AnimationTypeSelection == AnimationType.GoldenRatio)
        {
            float time = UnityEngine.Time.time;
            for (int i = 0; i < ObjectTransforms.Length; i++)
            {
                var transform = ObjectTransforms[i];
                var pos = ObjectOrigins[i];

                var distance = i / (ObjectTransforms.Length - 1f);
                var angle = 2 * Mathf.PI * GoldenRatio * i;
                pos.y = pos.y + (Mathf.Sin(time + angle) * distance * MovementMagnitude);
                pos.x = pos.x + (Mathf.Cos(time + angle) * distance * MovementMagnitude);
                transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime / 2);
            }
        }
        else if (AnimationTypeSelection == AnimationType.Galaxy)
        {
            float time = UnityEngine.Time.time;
            for (int i = 0; i < ObjectTransforms.Length; i++)
            {
                var transform = ObjectTransforms[i];
                var pos = ObjectOrigins[i];

                var distance = i / (ObjectTransforms.Length - 1f);
                var angle = 2 * Mathf.PI * GalaxyTurnRate * i;
                pos.y = pos.y + (Mathf.Sin(time + angle) * distance * MovementMagnitude);
                pos.x = pos.x + (Mathf.Cos(time + angle) * distance * MovementMagnitude);
                transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime / 2);
            }
        }
    }
}

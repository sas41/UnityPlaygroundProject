using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChildrenSmoothly : MonoBehaviour
{
    public enum MovementMethod
    {
        Lerp,
        MoveTo,
        MoveToSynced
    }

    public MovementMethod Movement;
    public float TimeToTarget = 2f;

    public bool GoBack;

    public GameObject TargetPosition;
    private Vector3 TargetOrigin;

    private GameObject[] Objects;
    private Transform[] ObjectTransforms;
    private Vector3[] ObjectOrigins;
    private float MaxDistance;

    void Start()
    {
        TargetOrigin = TargetPosition.GetComponent<Transform>().position;

        var childrenCount = this.transform.childCount;
        Objects = new GameObject[childrenCount];
        for (int i = 0; i < childrenCount; i++)
        {
            Objects[i] = this.transform.GetChild(i).gameObject;
        }

        ObjectTransforms = new Transform[Objects.Length];
        ObjectOrigins = new Vector3[Objects.Length];
        int pos = 0;
        foreach (var obj in Objects)
        {
            ObjectTransforms[pos] = obj.GetComponent<Transform>();
            ObjectOrigins[pos] = obj.GetComponent<Transform>().position;
            pos++;
        }

        MaxDistance = ObjectOrigins.Select(origin => Vector3.Distance(origin, TargetOrigin)).Max();
    }

    void FixedUpdate()
    {

        if (Movement == MovementMethod.Lerp)
        {
            for (int i = 0; i < ObjectTransforms.Length; i++)
            {
                Transform transform = ObjectTransforms[i];
                float delta = Time.fixedDeltaTime / TimeToTarget;
                if (GoBack)
                {
                    transform.position = LerpOverTime(TargetOrigin, ObjectOrigins[i], transform.position, delta);
                }
                else
                {
                    transform.position = LerpOverTime(ObjectOrigins[i], TargetOrigin, transform.position, delta);
                }
            }
        }
        else if (Movement == MovementMethod.MoveTo)
        {
            for (int i = 0; i < ObjectTransforms.Length; i++)
            {
                Transform transform = ObjectTransforms[i];
                float delta = Time.fixedDeltaTime / TimeToTarget;
                float distanceDelta = MaxDistance * delta;
                if (GoBack)
                {
                    transform.position = Vector3.MoveTowards(transform.position, ObjectOrigins[i], distanceDelta);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, TargetOrigin, distanceDelta);
                }
            }
        }
        else if (Movement == MovementMethod.MoveToSynced)
        {
            for (int i = 0; i < ObjectTransforms.Length; i++)
            {
                Transform transform = ObjectTransforms[i];
                float delta = Time.fixedDeltaTime / TimeToTarget;
                if (GoBack)
                {
                    transform.position = MoveTowardsOverTime(TargetOrigin, ObjectOrigins[i], transform.position, delta);
                }
                else
                {
                    transform.position = MoveTowardsOverTime(ObjectOrigins[i], TargetOrigin, transform.position, delta);
                }
            }
        }
    }

    Vector3 LerpOverTime(Vector3 original, Vector3 target, Vector3 current, float delta)
    {
        float distance = Vector3.Distance(original, target);
        float currentDistance = Vector3.Distance(current, target);
        float percentTraveled = (distance - currentDistance) / distance;
        float step = percentTraveled + delta;
        return Vector3.Lerp(original, target, step);
    }

    Vector3 MoveTowardsOverTime(Vector3 original, Vector3 target, Vector3 current, float delta)
    {
        float distance = Vector3.Distance(original, target);
        float step = distance * delta;
        return Vector3.MoveTowards(current, target, step);
    }
}

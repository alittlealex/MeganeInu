using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class RotateAroundAxis : MonoBehaviour
{
    public float speed = 10;
    public Vector3 axis;
    public bool isLocal = false;
    public bool randomStartRotation = false;
    [BoxGroup("Random")]
    [ShowIf("randomStartRotation")]
    public bool RandomX = true;
    [BoxGroup("Random")]
    [ShowIf("randomStartRotation")]
    public bool RandomY = true;
    [BoxGroup("Random")]
    [ShowIf("randomStartRotation")]
    public bool RandomZ = true;
    Quaternion startRotation;
    // Update is called once per frame
    private void Start()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        if (randomStartRotation)
        {
            if (RandomX)
            {
                rot.x = Random.Range(0f, 360f);
            }
            if (RandomY)
            {
                rot.y = Random.Range(0f, 360f);
            }
            if (RandomZ)
            {
                rot.z = Random.Range(0f, 360f);
            }
            transform.rotation = Quaternion.Euler(rot);
        }
        startRotation = transform.rotation;
    }
    void Update()
    {
        if (!isLocal)
            transform.Rotate(axis, speed * Time.deltaTime);
        else
            transform.Rotate(axis, speed * Time.deltaTime, Space.Self);
    }
}

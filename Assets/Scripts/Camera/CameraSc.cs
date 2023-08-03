using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSc : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private float lerpSmooth;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetOffset;
    [SerializeField] private Vector3 ratotion;
    [HideInInspector] public bool isFollow;

    private void Start()
    {
        transform.eulerAngles = ratotion;
    }
    private void LateUpdate()
    {
        if (isFollow && target!=null)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target.localPosition + targetOffset, lerpSmooth);
        }
    }
}

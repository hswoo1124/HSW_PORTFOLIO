using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTracker : MonoBehaviour
{
    [SerializeField]
    Transform target;

    void OnEnable()
    {
    }

    void FixedUpdate()
    {
        this.transform.LookAt(target);
    }
}

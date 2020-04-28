using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform PlayerPosition;
    public Vector3 Offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerPosition.position + Offset;
    }
}

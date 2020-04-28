using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    public Transform PlayerTransform;
    public int DistanceOffsetPercentualLimit = 10;
    private float _distanceFromPlayerZ;

    // Start is called before the first frame update
    void Start()
    {
        _distanceFromPlayerZ = transform.position.z - PlayerTransform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //if (PlayerTransform.position.z - transform.position.z > transform.localScale.z * ((float)DistanceOffsetPercentualLimit / 100) - transform.localScale.z / 2)
        if (PlayerTransform.position.z - transform.position.z > 10 - transform.localScale.z / 2)
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y, 
                PlayerTransform.position.z + _distanceFromPlayerZ);
    }
}

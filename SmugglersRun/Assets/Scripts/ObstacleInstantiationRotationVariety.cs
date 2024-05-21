using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInstantiationRotation : MonoBehaviour
{
    public int xRotationMin = -180;
    public int xRotationMax = 180;
    public int yRotationMin = -180;
    public int yRotationMax = 180;
    public int zRotationMin = -180;
    public int zRotationMax = 180;
    // Start is called before the first frame update
    void Start()
    {
        int x = Random.Range(xRotationMin, xRotationMax);

        int y = Random.Range(yRotationMin, yRotationMax);

        int z = Random.Range(zRotationMin, zRotationMax);
        gameObject.transform.rotation = Quaternion.Euler(x, y, z);
    }
}

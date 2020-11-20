using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTenon : MonoBehaviour
{
    //public string ID;
    public Vector3 positionOffset;
    public Vector3 angleOffset;

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Vector3 center = positionOffset;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(center, Quaternion.Euler(angleOffset) * Vector3.right / 10 + center);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(center, Quaternion.Euler(angleOffset) * Vector3.up / 10 + center);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(center, Quaternion.Euler(angleOffset) * Vector3.forward / 10 + center);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHintRing : MonoBehaviour
{
    public Vector3 startDir;
    public Vector3 currentDir;
    public Vector3 rotateAxis;
    private Material hintRingMat;
    private Vector3 lastFrameDir;
    private bool isClockWiseRotate;
    private bool isRingOnClockWise;
    private bool isOnRight;
    private float lastFrameDeltaAngle;
    // Start is called before the first frame update
    void Start()
    {
        hintRingMat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Angle(lastFrameDir, currentDir) > 0.5)
        {
            isClockWiseRotate = Vector3.Dot(Vector3.Cross(lastFrameDir, currentDir), rotateAxis) > 0;
        }
        isOnRight = Vector3.Dot(Vector3.Cross(startDir, currentDir), rotateAxis) > 0;
        float deltaAngle = Vector3.Angle(startDir, currentDir);
        if (currentDir != startDir)
        { hintRingMat.SetFloat("_Angle", isRingOnClockWise == isOnRight ? deltaAngle : 360 - deltaAngle); }
        else
        { hintRingMat.SetFloat("_Angle", 0); }

        if (deltaAngle <= 45 && lastFrameDeltaAngle < deltaAngle)
        {
            isRingOnClockWise = isOnRight;
            hintRingMat.SetFloat("_isClockWise", isRingOnClockWise ? 1 : 0);
        }
        lastFrameDir = currentDir;
        lastFrameDeltaAngle = deltaAngle;
    }

}

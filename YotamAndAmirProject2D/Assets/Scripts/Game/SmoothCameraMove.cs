using UnityEngine;

public class SmoothCameraMove : MonoBehaviour
{
    [Header("Properties")]
    public Transform target;

    public float smoothSpeed;
    public Vector3 offset;

    public float xMax;
    public float xMin;

    public float yMax;
    public float yMin;


    void FixedUpdate ()
    {
        if (target)
        {
            Vector3 desiredPos = target.position + offset;
            Vector3 calmpedPos = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax), -12);//limiting the camera movement
            Vector3 smoothedPos = Vector3.Lerp(calmpedPos, desiredPos, smoothSpeed);//making the camera move smoothly
            transform.position = smoothedPos;
        }
    }
}

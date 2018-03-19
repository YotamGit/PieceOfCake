using UnityEngine;

public class SmoothCameraMove : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed;
    public Vector3 offset;

    public float xMax;
    public float xMin;

    public float yMax;
    public float yMin;

    // Update is called once per frame
    void FixedUpdate (){
        if (target)
        {
            Vector3 desiredPos = target.position + offset;
            Vector3 calmpedPos = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax), transform.position.z);
            Vector3 smoothedPos = Vector3.Lerp(calmpedPos, desiredPos, smoothSpeed);
            transform.position = smoothedPos;
        }
        
        //target.LookAt(target); // ONLY FOR 3D!
    }
}

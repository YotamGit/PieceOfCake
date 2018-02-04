using UnityEngine;

public class SmoothCameraMove : MonoBehaviour
{
    public string targetTag;
    private Transform target;

    public float smoothSpeed;
    public Vector3 offset;

    // Use this for initialization
    private void Start()
    {
        target = GameObject.FindGameObjectsWithTag(targetTag)[0].transform; // Recieving the transform of the desired player
    }

    // Update is called once per frame
    void FixedUpdate (){
        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothedPos;

        //target.LookAt(target); // ONLY FOR 3D!
    }
}

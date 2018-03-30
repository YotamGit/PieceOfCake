using UnityEngine;

public class MovingText : MonoBehaviour
{
    [Range(-1, 1)]
    public float HalfOfMaxRotate; // this value moves between 1 and -1

    [Range(-1, 1)]
    public float RotateSpeed;
    
    private float RotateParam; // this value moves between 1 and 0

    private RectTransform objTransform;
    
    private void Start()
    {
        objTransform = GetComponent<RectTransform>();
        RotateParam = 0.5f;
    }

    private void FixedUpdate()
    {
        RotateParam += RotateSpeed;

        objTransform.rotation = new Quaternion(objTransform.rotation.x, objTransform.rotation.y, RotateFunc(RotateParam), objTransform.rotation.w);

        if (RotateParam > 1.5 || RotateParam < 0.5)
        {
            RotateSpeed = -RotateSpeed;
        }
    }

    // returns a smooth value
    private float RotateFunc(float x) // uses Sin() instead of the other func
    {
        return Mathf.Sin(Mathf.PI*x)* HalfOfMaxRotate;
    }
}

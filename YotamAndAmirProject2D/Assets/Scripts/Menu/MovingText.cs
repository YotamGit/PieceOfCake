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

        objTransform.rotation = new Quaternion(objTransform.rotation.x, objTransform.rotation.y, RotateFuncBetter(RotateParam), objTransform.rotation.w);

        if (RotateParam > 1.5 || RotateParam < 0.5)
        {
            RotateSpeed = -RotateSpeed;
        }
    }

    // returns a smooth value between -0.5 MaxRotate and 0.5 MaxRotate
    /*private float RotateFunc(float x)
    {
        return x * x * x * HalfOfMaxRotate * (x * (6 * x - 15) + 10)  - 0.5f * HalfOfMaxRotate;
    }*/

    private float RotateFuncBetter(float x) // uses Sin() instead of the other func
    {
        return Mathf.Sin(Mathf.PI*x)* HalfOfMaxRotate;
    }
}

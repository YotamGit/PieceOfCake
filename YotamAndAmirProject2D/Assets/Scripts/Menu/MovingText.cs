using UnityEngine;

public class MovingText : MonoBehaviour
{
    [Range(-1, 1)]
    public float MaxRotate; // this value moves between 1 and -1

    [Range(-1, 1)]
    public float RotateSpeed;
    
    private float RotateParam; // this value moves between 1 and 0

    private RectTransform objTransform;
    
    private void Start()
    {
        objTransform = GetComponent<RectTransform>();
        RotateParam = 0;
    }

    private void FixedUpdate()
    {
        RotateParam += RotateSpeed;

        objTransform.rotation = new Quaternion(objTransform.rotation.x, objTransform.rotation.y, RotateFunc(RotateParam), objTransform.rotation.w);

        if (RotateParam > 1 || RotateParam < 0)
        {
            RotateSpeed = -RotateSpeed;
        }
    }

    // returns a smooth value between -0.5 MaxRotate and 0.5 MaxRotate
    private float RotateFunc(float n)
    {
        return n * n * n * MaxRotate * (n * (6 * n - 15) + 10)  - 0.5f * MaxRotate;
    }
}

using UnityEngine;

public class MovingText : MonoBehaviour
{
    [Range(-1, 1)]
    public float MaxRotate; // this value moves between 1 and -1

    [Range(-1, 1)]
    public float RotateSpeed, add;
    
    private float currentRot; // this value moves between 1 and 0

    private RectTransform objTransform;
    
    private void Start()
    {
        objTransform = GetComponent<RectTransform>();
        currentRot = 0;
    }

    private void FixedUpdate()
    {
        currentRot += RotateSpeed;

        objTransform.rotation = new Quaternion(objTransform.rotation.x, objTransform.rotation.y, Smoother(currentRot), objTransform.rotation.w);

        if (currentRot + add > 1 || currentRot - add < 0)
        {
            RotateSpeed = -RotateSpeed;
        }
    }

    // returns a smooth value between -0.5 MaxRotate and 0.5 MaxRotate
    private float Smoother(float n)
    {
        return n * n * n * (n * (6 * n * MaxRotate - 15 * MaxRotate) + 10 * MaxRotate) - 0.5f * MaxRotate;
    }
}

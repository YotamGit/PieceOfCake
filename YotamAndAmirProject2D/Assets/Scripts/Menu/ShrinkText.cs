using UnityEngine;

public class ShrinkText : MonoBehaviour {

    [Range(-1, 2)]
    public float MaxShrink; // this value moves between 1 and -1

    [Range(-1, 1)]
    public float ShrinkSpeed, add;

    [Range(0, 100)]
    public float minShrink;

    private float currentShr; // this value moves between 1 and 0

    private RectTransform objTransform;

    private void Start()
    {
        objTransform = GetComponent<RectTransform>();
        currentShr = 0;
    }

    private void FixedUpdate()
    {
        currentShr += ShrinkSpeed;

        float smooth = Smoother(currentShr);
        objTransform.localScale = new Vector3(smooth, smooth, objTransform.localScale.z);

        if (currentShr + add > 1 || currentShr - add < 0)
        {
            ShrinkSpeed = -ShrinkSpeed;
        }
    }

    private float Smoother(float n)
    {
        return n * n * n * (n * (6 * n * MaxShrink - 15 * MaxShrink) + 10 * MaxShrink) + minShrink;
    }
}

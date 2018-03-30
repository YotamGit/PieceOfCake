using UnityEngine;

public class ShrinkText : MonoBehaviour
{
    // This value + the min scale = max scale the obj will get
    // so if we set it to 1 our object will get to a max scale of: 1 + the min scale
    [Range(0, 2)]
    public float HalfOfAddScale; // this value moves between 0 and infinate

    // we're adding the scaling speed on each FixedUpdate to the current scale variable
    [Range(-1, 1)]
    public float ScalingSpeed; 

    // the smallest the object will become
    [Range(0, 100)]
    public float minScale;

    private float ScaleParam; // this value moves between 1 and 0 at the speed of scaling speed

    private RectTransform objTransform;

    private void Start()
    {
        objTransform = GetComponent<RectTransform>();
        ScaleParam = 0.5f;
    }

    private void FixedUpdate()
    {
        ScaleParam += ScalingSpeed;

        float value = ScaleFunc(ScaleParam);
        objTransform.localScale = new Vector3(value, value, objTransform.localScale.z);

        if (ScaleParam  > 1.5 || ScaleParam  < 0.5)
        {
            ScalingSpeed = -ScalingSpeed;
        }
    }

    private float ScaleFunc(float x)
    {
        return HalfOfAddScale * (Mathf.Sin(Mathf.PI * x) + 1) + minScale;
    }
}

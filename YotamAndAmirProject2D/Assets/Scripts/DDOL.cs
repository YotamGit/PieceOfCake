using UnityEngine;

public class DDOL : MonoBehaviour
{
    private void Awake()
    {
        GameObject Ddol = GameObject.FindGameObjectWithTag("DDOL");
        if (Ddol != null)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }
}

using UnityEngine;

public class DDOL : MonoBehaviour
{
    [SerializeField]
    private GameObject disableOnReturn, enableOnReturn;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("DDOL").Length > 1) // this happens when you return to the same scene and it duplicate DDOLs
        {
            disableOnReturn.SetActive(false);
            enableOnReturn.SetActive(true);
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }
}

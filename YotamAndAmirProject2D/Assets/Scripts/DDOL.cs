using UnityEngine;

public class DDOL : MonoBehaviour
{
    [SerializeField]
    private GameObject connectingScreen, MainMenu;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("DDOL").Length > 1) // this happens when you return to the same scene and it duplicate DDOLs
        {
            if (PhotonNetwork.connected)
            {
                connectingScreen.SetActive(false);
                MainMenu.SetActive(true);
            }
            Destroy(gameObject);
        }
        transform.GetChild(0).GetComponent<PlayerNetwork>().InstantiateSelf(); //telling the PlayerNetwork that he is not a duplicate
        DontDestroyOnLoad(this);
    }
}

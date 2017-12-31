using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour
{
    [SerializeField] private GameObject playerCameraBlue;
    [SerializeField] private GameObject playerCameraRed;
    [SerializeField] private MonoBehaviour[] playerControlScripts;

    private PhotonView photonView;

    // Use this for initialization
    private void Start ()
    {
        photonView = GetComponent<PhotonView>();

        Initialize();
	}

    private void Initialize()
    {
        if(photonView.isMine)
        {

        }
        else
        {
            playerCameraBlue.SetActive(false);
            playerCameraRed.SetActive(false);

            foreach (MonoBehaviour m in playerControlScripts)
            {
                m.enabled = false;
            }
        }
    }
}

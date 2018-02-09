using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableScriptsOnLoad : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] playerControlScripts;

    private PhotonView photonView;

    // Use this for initialization
    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        Initialize();
    }

    private void Initialize()
    {
        if (!photonView.isMine)
        {
            foreach (MonoBehaviour m in playerControlScripts)
            {
                m.enabled = false;
            }
        }
    }
}

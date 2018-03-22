using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork instance;

    [HideInInspector]
    public string PlayerName;// { get; private set; }
    
    public bool restartedSceneAlready;

    public bool duplicate = true;

    public void InstantiateSelf()
    {
        instance = this;
        PlayerName = "Guest#" + Random.Range(1000, 9999); // Guest#3490

        duplicate = false; // this will stop if we know that we're not the second player network

        restartedSceneAlready = false;
        if (!PhotonNetwork.connected) /*DDOL will initiate when you enter the menu scene, thus you shouldnt return to in. so just return to a diff simulare menu*/
        {
            Debug.Log("Connecting to server...");
            PhotonNetwork.ConnectUsingSettings("game");
            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.autoJoinLobby = false;
        }
    }

    private void FixedUpdate()
    {
        if (!duplicate)
        {
            if (!PhotonNetwork.connected && !PhotonNetwork.connecting)
            {
                if (!restartedSceneAlready || SceneManager.GetActiveScene().buildIndex != 0)
                {
                    if(Time.timeScale == 0)
                    {
                        Time.timeScale = 1;
                    }
                    Debug.Log("Loading connection scene...");
                    restartedSceneAlready = true;
                    SceneManager.LoadScene(0);
                }
                PhotonNetwork.ConnectUsingSettings("game");
                //PhotonNetwork.ReconnectAndRejoin();
            }
        }
    }

    private void OnConnectedToMaster()
    {
        restartedSceneAlready = false;
    }
    /*void Plans()
     {
         if (InGameScene && notConnected)//this will trigger the DDOL in a way
         {
             returnToMainMenu();
         }
         else if(inMainMenu && notConnected)
         {
             reconnect();
             if (connected)
             {
                 RejoinGame();
             }
         }
     }*/

    /*[SerializeField] private GameObject playerCameraBlue;
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
    }*/
}

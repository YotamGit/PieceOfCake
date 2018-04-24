using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork instance;

    [HideInInspector]
    public string PlayerName;
    
    [HideInInspector]
    public bool restartedSceneAlready;

    [SerializeField]
    private GameObject connectingScreen, ReturnConnectedScreen;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("DDOL").Length > 1) // this happens when you return to the same scene and it duplicate PlayerNetworks
        {
            if (PhotonNetwork.connected)
            {
                connectingScreen.SetActive(false);
                ReturnConnectedScreen.SetActive(true);
            }
            Destroy(gameObject);
        }
        instance = this;

        restartedSceneAlready = false;

        Debug.Log("Connecting to server...");
        PhotonNetwork.ConnectUsingSettings("game");
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.autoJoinLobby = false;

        DontDestroyOnLoad(this);
    }

    /*reconnecting the player if disconnected and the playernetwork is not a duplicate*/
    private void FixedUpdate()
    {
        if (!PhotonNetwork.connected && !PhotonNetwork.connecting)
        {
            if (!restartedSceneAlready)// || SceneManager.GetActiveScene().buildIndex != 0)
            {
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                }
                Debug.Log("Loading connection scene...");
                restartedSceneAlready = true;
                SceneManager.LoadScene(0);
            }
            PhotonNetwork.ConnectUsingSettings("game");
        }
    }

    private void OnConnectedToMaster()
    {
        restartedSceneAlready = false;
    }
}

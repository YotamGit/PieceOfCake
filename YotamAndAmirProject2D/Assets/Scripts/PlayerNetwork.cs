using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour
{
    public static PlayerNetwork instance;

    [HideInInspector]
    public string PlayerName;
    
    public bool restartedSceneAlready;

    private bool duplicate = true;

    public void InstantiateSelf()//connecting to the server
    {
        instance = this;

        duplicate = false; // this will stop if we know that we're not the second playernetwork

        restartedSceneAlready = false;

        Debug.Log("Connecting to server...");
        PhotonNetwork.ConnectUsingSettings("game");
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.autoJoinLobby = false;
    }

    /*reconnecting the player if disconnected and the playernetwork is not a duplicate*/
    private void FixedUpdate()
    {
        if (!duplicate)
        {
            if (!PhotonNetwork.connected && !PhotonNetwork.connecting)
            {
                if (!restartedSceneAlready)// || SceneManager.GetActiveScene().buildIndex != 0)
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
            }
        }
    }

    private void OnConnectedToMaster()
    {
        restartedSceneAlready = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonNetworkManager : MonoBehaviour
{
    /*private void Start()
    {
        Debug.Log("Starting Game");
    }*/
    // Obj to spawn:
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject mainCamera1;
    [SerializeField] private GameObject mainCamera2;

    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;

    [SerializeField] private GameObject lobbyCamera;

    [SerializeField] private GameObject Instructions;

    public KeyCode InstructionsKey;

    /*// Scripts to disable:
    [SerializeField] private MonoBehaviour[] playerControlScripts;

    private PhotonView photonView;*/

    //private bool gameStarted; // freeze before other player joins

    //[SerializeField] private GameObject[] powerUps;


    // Use this for initialization
    //private void Update()
    //{
    //    Debug.Log(PhotonNetwork.connectionState.ToString());///ONLY FOR DEBUGGING
    //}
    /*private void Start()
    {
        Debug.Log("start");
        PhotonNetwork.ConnectUsingSettings("gameAmir1");
        Debug.Log("connected");
        //PhotonNetwork.automaticallySyncScene = true;

        gameStarted = false;
    }*/

    private void Start()
    {
        Debug.Log("Creating player...");

        lobbyCamera.SetActive(false);
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Instantiate(player1.name, spawnPoint1.position, spawnPoint1.rotation, 0);
            Instantiate(mainCamera1); // Creating a camera
        }
        else
        {
            PhotonNetwork.Instantiate(player2.name, spawnPoint2.position, spawnPoint2.rotation, 0);
            Instantiate(mainCamera2); // Creating a camera
        }

        Debug.Log("Player created");
    }

    private void Update()
    {
        // setting the instructions to the opposite of its current enable state when clicking on the M button
        if (Input.GetKeyDown(InstructionsKey) && Time.timeScale != 0)
        {
            Instructions.SetActive(!Instructions.activeInHierarchy);
        }
        // if the player clicks on one of those buttons (or the timescale is frozen) while the instructions is open, it will close
        if(Instructions.activeInHierarchy)
        {
            if((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3)
            || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space)) || Time.timeScale == 0)
            {
                Instructions.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // for testing only!
        {
            Application.Quit();
        }
    }

    /*private void Update()
    {
        if (!gameStarted)
        {
            if (PhotonNetwork.playerList.Length == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                gameStarted = true;
                Time.timeScale = 1;
                //StartCoroutine(WaitTwoSeconds());
            }
        }
    }*/

    /*private void Initialize()
    {
        if (photonView.isMine)
        {

        }
        else
        {
            foreach (MonoBehaviour m in playerControlScripts)
            {
                m.enabled = false;
            }
        }
    }*/

    /*public virtual void OnJoinedLobby()
    {
        Debug.Log("joining lobby");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("1Room", roomOptions, null);
        Debug.Log("joined");
    }*/

    /*public virtual void OnJoinedRoom()
    {
        Debug.Log("Creating player");
        //PhotonNetwork.Instantiate(player1.name, spawnPoint1.position, spawnPoint1.rotation, 0);
        //PhotonNetwork.Instantiate(player1.name, spawnPoint1.position, spawnPoint1.rotation, 0);

        if (PhotonNetwork.playerList.Length == 1)
        {
            PhotonNetwork.Instantiate(player1.name, spawnPoint1.position, spawnPoint1.rotation, 0);
            Instantiate(mainCamera1); // Creating a camera
        }
        else
        {
            PhotonNetwork.Instantiate(player2.name, spawnPoint2.position, spawnPoint2.rotation, 0);
            Instantiate(mainCamera2); // Creating a camera
        }

        //foreach(GameObject a in powerUps)
        //{
        //    PhotonNetwork.Instantiate(a.name,a.transform.position , a.transform.rotation, 0);
        //}
        Debug.Log("player created");
        lobbyCamera.SetActive(false);
        Debug.Log("deactivated lobby camera");
    }

    public virtual void OnPhotonSerializeView()
    {

    }*/

}

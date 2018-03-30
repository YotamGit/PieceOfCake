using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PhotonNetworkManager : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;

    [SerializeField] private GameObject lobbyCamera;

    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject[] ObjectsInMenu;

    [HideInInspector]
    public bool wonGame = false;

    //private bool playerLeftRoom;

    public KeyCode MenuKey;
    private GameObject waitingScreen;
   
    private void Start()
    {
        waitingScreen = GameObject.FindGameObjectWithTag("WaitingText");
        waitingScreen.SetActive(true);

        Transform tempPlayer;

        lobbyCamera.SetActive(false);
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("Creating player 1...");
            tempPlayer = PhotonNetwork.Instantiate(player1.name, spawnPoint1.position, spawnPoint1.rotation, 0).transform;
        }
        else
        {
            Debug.Log("Creating player 2...");
            tempPlayer = PhotonNetwork.Instantiate(player2.name, spawnPoint2.position, spawnPoint2.rotation, 0).transform;
        }
        GameObject mainCameraObj = Instantiate(mainCamera); // Creating a camera
        mainCameraObj.GetComponent<SmoothCameraMove>().target = tempPlayer.transform; // Assighning the player to the target

        Debug.Log("Player created");
    }

    private void Update()
    {
        // setting the instructions to the opposite of its current enable state when clicking on the M button
        if (Input.GetKeyDown(MenuKey) && Time.timeScale != 0 && !wonGame)
        {
            Menu.SetActive(!Menu.activeInHierarchy);
            
            foreach (GameObject obj in ObjectsInMenu)
            {
                obj.SetActive(false);
            }
            ObjectsInMenu[0].SetActive(true);
        }
        // if the player clicks on one of those buttons (or the timescale is frozen) while the instructions is open, it will close
        if(Menu.activeInHierarchy)
        {
            if((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3)
            || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space)) || Time.timeScale == 0)
            {
                Menu.SetActive(false);
                foreach (GameObject obj in ObjectsInMenu)
                {
                    obj.SetActive(false);
                }
                ObjectsInMenu[0].SetActive(true);
            }
        }
    }

    // called by photon when a player leaves the room
    void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        if (!wonGame)
        {
            Time.timeScale = 0;
            waitingScreen.SetActive(true);
        }
    }

    void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        TextMeshProUGUI text = waitingScreen.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "Player One Dissconnected";
        PhotonNetwork.LeaveRoom();
    }
}

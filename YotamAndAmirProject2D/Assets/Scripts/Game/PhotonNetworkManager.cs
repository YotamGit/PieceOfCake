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
    public bool wonGame = false, madeChoice = false;

    public KeyCode MenuKey;

    [SerializeField]
    private GameObject WinningAloneScreen, WinningTogetherScreen, WinningNoneScreen, WinningWaitingScreen, WinningChoiceScreen; //will hold all the winning screens

    private GameObject waitingScreen;

    private GameObject mainPlayer; // the player you control

    private void Start()
    {
        waitingScreen = GameObject.FindGameObjectWithTag("WaitingText");
        waitingScreen.SetActive(true);
        

        lobbyCamera.SetActive(false);
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("Creating player 1...");
            mainPlayer = PhotonNetwork.Instantiate(player1.name, spawnPoint1.position, spawnPoint1.rotation, 0);
        }
        else
        {
            Debug.Log("Creating player 2...");
            mainPlayer = PhotonNetwork.Instantiate(player2.name, spawnPoint2.position, spawnPoint2.rotation, 0);
        }
        GameObject mainCameraObj = Instantiate(mainCamera); // Creating a camera
        mainCameraObj.GetComponent<SmoothCameraMove>().target = mainPlayer.transform; // Assighning the player to the target
        mainCameraObj.transform.position = new Vector3(mainPlayer.transform.position.x, mainPlayer.transform.position.y, -12);
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            mainCameraObj.GetComponent<SmoothCameraMove>().yMax = 122;
        }

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

    public void DisplayWinningWaiting()
    {
        WinningWaitingScreen.SetActive(true);
        Destroy(mainPlayer);
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("StartWaitChoice", PhotonTargets.All);
    }

    public void DisplayWinningChoice() // this happens for the player that got hte cake
    {
        WinningChoiceScreen.SetActive(true);
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("StartWaitChoice", PhotonTargets.All);
    }
    
    [PunRPC]
    private void StartWaitChoice()
    {
        wonGame = true;
        StartCoroutine(ChoiceAutoShare());
    }

    private IEnumerator ChoiceAutoShare()
    {
        yield return new WaitForSeconds(30f);
        if(!madeChoice)
            WinningChoice(true);
    }

    public void WinningChoice(bool toShare)
    {
        madeChoice = true;
        PhotonView photonView = PhotonView.Get(this);
        if (toShare)
        {
            photonView.RPC("ClickedShare", PhotonTargets.All);
        }
        else
        {
            photonView.RPC("ClickedDontShare", PhotonTargets.Others);
            WinningAloneScreen.SetActive(true);

            StartCoroutine(GameObject.FindGameObjectWithTag("DataBaseManager").GetComponent<DBCManager>().AddVictoryCount(0, 2));
            // add score to self 2
        }
        WinningChoiceScreen.SetActive(false);
    }

    [PunRPC]
    void ClickedShare()
    {
        WinningWaitingScreen.SetActive(false);

        WinningTogetherScreen.SetActive(true);
        if (PhotonNetwork.inRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        Time.timeScale = 1;
        StartCoroutine(GameObject.FindGameObjectWithTag("DataBaseManager").GetComponent<DBCManager>().AddVictoryCount(1, 1));
        // add score to self 1
    }

    [PunRPC]
    void ClickedDontShare()
    {
        WinningWaitingScreen.SetActive(false);

        WinningNoneScreen.SetActive(true);
        if (PhotonNetwork.inRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        Time.timeScale = 1;
    }
}

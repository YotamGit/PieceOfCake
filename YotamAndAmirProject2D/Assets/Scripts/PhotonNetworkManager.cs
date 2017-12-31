using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonNetworkManager : MonoBehaviour
{

    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject lobbyCamera;
    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;
    //[SerializeField] private GameObject[] powerUps;


    // Use this for initialization
    private void Start ()
    {
        Debug.Log("start");
        PhotonNetwork.ConnectUsingSettings("game");
        Debug.Log("connected");
    }

    public virtual void OnJoinedLobby()
    {
        Debug.Log("joining lobby");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("1Room", null, null);
        Debug.Log("joined");
    }

    public virtual void OnJoinedRoom()
    {
        Debug.Log("creating player");
        //PhotonNetwork.Instantiate(player1.name, spawnPoint1.position, spawnPoint1.rotation, 0);
        PhotonNetwork.Instantiate(player1.name, spawnPoint1.position, spawnPoint1.rotation, 0);

        //if (PhotonNetwork.playerList.Length == 1)
        //{
        //    PhotonNetwork.Instantiate(player1.name, spawnPoint1.position, spawnPoint1.rotation, 0);
        //}
        //else
        //{
        //    PhotonNetwork.Instantiate(player2.name, spawnPoint2.position, spawnPoint2.rotation, 0);
        //}

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

    }
 
}

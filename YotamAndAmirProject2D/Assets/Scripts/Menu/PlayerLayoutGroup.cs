using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLayoutGroup : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerListingPrefab;
    //private GameObject PlayerListingPrefab
    //{
    //    get { return _playerListingPrefab; }
    //}

    private List<PlayerListing> PlayerListings = new List<PlayerListing>();
    //private List<PlayerListing> PlayerListings
    //{
    //    get {return _playerListings; }
    //}

    [SerializeField]
    private GameObject toDisableWaitingScreen, toEnableRoomMenu;//toEnableLobby;

    [SerializeField]
    private TextMeshProUGUI cancelButtonText;
    [SerializeField]
    private Button cancelButton;

    private void Awake()
    {
        ChangeTextAlpha(true);
    }

    // called by photon when a master client is switched
    private void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        foreach (PlayerListing tempPL in PlayerListings)
        {
            Destroy(tempPL.gameObject);
        }
        PlayerListings.Clear();
        PhotonNetwork.LeaveRoom();
    }

    public void ChangeTextAlpha(bool change) // true: pale, false: normal
    {
        if (change)
        {
            cancelButtonText.faceColor = new Color(1, 1, 1, 0.4F);
        }
        else
        {
            cancelButtonText.faceColor = new Color(1, 1, 1, 1F);
        }
    }

    // called by photon when you join a room
    private void OnJoinedRoom()
    {
        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
        for (int i = 0; i < photonPlayers.Length; i++)
        {
            PlayerJoinedRoom(photonPlayers[i]);
        }

        //enabling the cancle button
        ChangeTextAlpha(false);
        cancelButton.interactable = true;
    }

    [SerializeField]
    private GameObject PlayButton;

    // called by photon when a player joins the room
    private void OnPhotonPlayerConnected(PhotonPlayer photonPlayer)
    {
        if (PhotonNetwork.isMasterClient)
        {
            PlayButton.SetActive(true);
        }
        PlayerJoinedRoom(photonPlayer);
    }

    // called by photon when a player leaves the room
    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
    {
        if (PhotonNetwork.isMasterClient)
        {
            PlayButton.SetActive(false);
        }
        PlayerLeftRoom(photonPlayer);
    }
    
    public void PlayerJoinedRoom(PhotonPlayer photonPlayer)
    {
        if(photonPlayer == null)
        {
            return;
        }
        PlayerLeftRoom(photonPlayer);

        GameObject playerListingObj = Instantiate(PlayerListingPrefab);
        playerListingObj.transform.SetParent(transform, false);

        PlayerListing playerlisting = playerListingObj.GetComponent<PlayerListing>();
        playerlisting.ApplyPhotonPlayer(photonPlayer);

        PlayerListings.Add(playerlisting);
    }

    // finding the player in the playerListing List and removing him from the visable UI list
    private void PlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        int index = PlayerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
        
        if (index != -1) // if player found
        {
            Debug.Log("Deleting The Other Player");
            Destroy(PlayerListings[index].gameObject);
            PlayerListings.RemoveAt(index);
        }
    }

    //public void OnClickRoomState()
    //{
    //    if(PhotonNetwork.isMasterClient)
    //    {
    //        return;
    //    }
    //    PhotonNetwork.room.IsOpen = !PhotonNetwork.room.IsOpen;
    //    PhotonNetwork.room.IsVisible = PhotonNetwork.room.IsOpen;
    //}

    public void OnClickLeaveRoom()
    {
        foreach (PlayerListing tempPL in PlayerListings)
        {
            Destroy(tempPL.gameObject);
        }
        PlayerListings.Clear();
        PhotonNetwork.LeaveRoom();
        //disableing the cancle button
        ChangeTextAlpha(true);
        cancelButton.interactable = false;
    }

    private void OnLeftRoom()
    {
        if (toDisableWaitingScreen.activeInHierarchy)
        {
            toEnableRoomMenu.SetActive(true);
            toDisableWaitingScreen.SetActive(false);
        }
    }
}

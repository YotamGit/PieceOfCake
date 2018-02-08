using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayoutGroup : MonoBehaviour {

    [SerializeField]
    private GameObject _playerListingPrefab;
    private GameObject PlayerListingPrefab
    {
        get { return _playerListingPrefab; }
    }

    private List<PlayerListing> _playerListings = new List<PlayerListing>();
    private List<PlayerListing> PlayerListings
    {
        get {return _playerListings; }
    }

    [SerializeField]
    private GameObject toDisable, toEnable;

    // called by photon when a master client is switched
    private void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        toEnable.SetActive(true);
        toDisable.SetActive(false);
        foreach (PlayerListing tempPL in PlayerListings)
        {
            Destroy(tempPL.gameObject);
        }
        PlayerListings.Clear();
        PhotonNetwork.LeaveRoom();
    }

    // called by photon when you join a room
    private void OnJoinedRoom()
    {
        //MainMenu.Instance.WaitingRoomCanvas.transform.SetAsLastSibling();

        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
        for (int i = 0; i < photonPlayers.Length; i++)
        {
            PlayerJoinedRoom(photonPlayers[i]);
        }
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
        
        if (index != -1)
        {
            Debug.Log("Deleting The Other Player");
            Destroy(PlayerListings[index].gameObject);
            PlayerListings.RemoveAt(index);
        }
    }

    public void OnClickRoomState()
    {
        if(PhotonNetwork.isMasterClient)
        {
            return;
        }
        PhotonNetwork.room.IsOpen = !PhotonNetwork.room.IsOpen;
        PhotonNetwork.room.IsVisible = PhotonNetwork.room.IsOpen;
    }

    public void OnClickLeaveRoom()
    {
        foreach(PlayerListing tempPL in PlayerListings)
        {
            Destroy(tempPL.gameObject);
        }
        PlayerListings.Clear();
        PhotonNetwork.LeaveRoom();
    }
}

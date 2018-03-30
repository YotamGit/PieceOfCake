using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGroup : MonoBehaviour {

    [SerializeField]
    private GameObject RoomListingPrefab;

    private List<RoomListing> RoomListingButtons = new List<RoomListing>();

    //called by photon when the room list updates
    public void OnReceivedRoomListUpdate()
    {
        if(!PhotonNetwork.insideLobby) // joining lobby if we're not in one
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }

        RoomInfo[] rooms = PhotonNetwork.GetRoomList(); // getting all the current rooms


        foreach (RoomInfo room in rooms)
        {
            RoomReceived(room);
        }

        RemoveOldRooms();
    }

    // checking if the room already exists
    private void RoomReceived(RoomInfo room)
    {
        int index = RoomListingButtons.FindIndex(x => x.RoomName == room.Name); // finding room in list//find what is this find method!!

        if(index == -1) // adding room to list
        {
            if(room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(RoomListingPrefab);//creating the button
                roomListingObj.transform.SetParent(transform, false);

                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                RoomListingButtons.Add(roomListing);//adding the script to the list

                index = (RoomListingButtons.Count - 1);//getting the index of the last script added
            }
        }

        if (index != -1)//updating the data of the last room added or the found room
        {
            RoomListing roomListing = RoomListingButtons[index];
            roomListing.SetRoomNameText(room.Name);
            roomListing.Updated = true;
        }
    }

    public void RemoveOldRooms()
    {
        List<RoomListing> removeRooms = new List<RoomListing>();

        foreach(RoomListing roomListing in RoomListingButtons)
        {
            if(!roomListing.Updated)//if the room is not updated we delete it
            {
                removeRooms.Add(roomListing);
            }
            else
            {
                roomListing.Updated = false;
            }
        }

        foreach(RoomListing roomListing in removeRooms)
        {
            GameObject roomListingObj = roomListing.gameObject;
            RoomListingButtons.Remove(roomListing);//removing the script
            Destroy(roomListingObj);//destroying the object
        }
    }
}

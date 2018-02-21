using System.Collections.Generic;
using UnityEngine;

public class RoomLayoutGroup : MonoBehaviour {

    [SerializeField]
    private GameObject RoomListingPrefab;
    //private GameObject RoomListingPrefab
    //{
    //    get { return _roomListingPrefab; }
    //}

    private List<RoomListing> RoomListingButtons = new List<RoomListing>();
    //private List<RoomListing> RoomListingButtons
    //{
    //    get { return _roomListingButtons; }
    //}

    //called by photon when the room list updates
    public void OnReceivedRoomListUpdate()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList(); // getting all the current rooms
        Debug.Log("Num of rooms received: " + rooms.Length);
        //RemoveOldRooms();

        foreach (RoomInfo room in rooms)
        {
            Debug.Log("Received Room: " + room.Name);
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
            if(room.IsVisible && room.PlayerCount < room.MaxPlayers) //room.IsVisible &&
            {
                GameObject roomListingObj = Instantiate(RoomListingPrefab);
                roomListingObj.transform.SetParent(transform, false);

                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                RoomListingButtons.Add(roomListing);

                index = (RoomListingButtons.Count - 1);
            }
        }

        if (index != -1)
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
            if(!roomListing.Updated)
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
            RoomListingButtons.Remove(roomListing);
            Destroy(roomListingObj);
        }
    }

    /*public void RemoveAllRooms()
    {
        List<RoomListing> removeRooms = new List<RoomListing>();

        foreach (RoomListing roomListing in RoomListingButtons)
        {
            removeRooms.Add(roomListing);
        }

        foreach (RoomListing roomListing in removeRooms)
        {
            GameObject roomListingObj = roomListing.gameObject;
            RoomListingButtons.Remove(roomListing);
            Destroy(roomListingObj);
        }
    }*/
}

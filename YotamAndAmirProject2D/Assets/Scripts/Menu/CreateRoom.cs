using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateRoom : MonoBehaviour {

    [SerializeField]//write comments
    private GameObject toEnable, toDisable, backButton;

    [SerializeField]
    private TextMeshProUGUI RoomName, changeAlpha, cancelText;
    //private TextMeshProUGUI RoomName
    //{
    //    get { return _roomName; }
    //}

    [SerializeField]
    private MainMenu mainMenuScript;

    public void OnClick_CreateRoom()
    {
        DisableSelf();

        RoomOptions roomOptions = new RoomOptions { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        if (PhotonNetwork.CreateRoom(RoomName.text.ToUpper(), roomOptions, TypedLobby.Default))
        {
            Debug.Log("Create Room Successfully Sent");
        }
    }

    // enableing the create room (and back) button, and telling the player that this room name is taken (there will be a normal error too - dont mind it)
    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        if (codeAndMessage[1].ToString() == "A game with the specified id already exist.")
        {
            Button button = GetComponent<Button>();
            button.enabled = true;

            backButton.SetActive(true);

            changeAlpha.faceColor = new Color(1, 1, 1, 1F);
            StartCoroutine(mainMenuScript.DisplayError("Room Already Exists"));
        }
        else
        {
            Debug.Log("Create Room Failed: " + codeAndMessage[1]);
        }
    }
    
    private void DisableSelf()
    {
        changeAlpha.faceColor = new Color(1, 1, 1, 0.4F);
        Button button = GetComponent<Button>();
        button.enabled = false;
        backButton.SetActive(false);
    }

    private void OnCreatedRoom()
    {
        Button button = GetComponent<Button>();
        button.enabled = true;

        changeAlpha.faceColor = new Color(1, 1, 1, 1F);
        cancelText.faceColor = new Color(1, 1, 1, 1F);

        Debug.Log("Room Created successfully");
        toDisable.SetActive(false);
        toEnable.SetActive(true);
    }
}

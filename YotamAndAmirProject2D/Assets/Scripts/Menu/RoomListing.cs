using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomListing : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI RoomNameText;
    //private TextMeshProUGUI RoomNameText
    //{
    //    get { return _roomNameText; }
    //}

    public string RoomName { get; private set; }
    public bool Updated { get; set; }

    void Start()
    {
        GameObject lobbyCanvasObj = MainMenu.Instance.LobbyCanvas.gameObject;

        if (lobbyCanvasObj == null)
        {
            Debug.Log("Not Entering");
            return;
        }

        LobbyCanvas lobbyCanvas = lobbyCanvasObj.GetComponent<LobbyCanvas>();

        //Debug.Log("Room Name: " + RoomNameText.text);
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => lobbyCanvas.OnClickJoinRoom(RoomNameText.text));
    }

    private void OnDestroy()
    {
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }

    public void SetRoomNameText(string text)
    {
        RoomName = text;
        RoomNameText.text = RoomName;
    }
}

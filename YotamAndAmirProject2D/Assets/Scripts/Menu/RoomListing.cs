using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomListing : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI RoomNameText;

    public string RoomName;
    public bool Updated;

    void Start()
    {
        GameObject lobbyCanvasObj = MainMenu.Instance.LobbyCanvas.gameObject;

        if (lobbyCanvasObj == null)
        {
            Debug.Log("Not Entering");
            return;
        }

        LobbyCanvas lobbyCanvas = lobbyCanvasObj.GetComponent<LobbyCanvas>();

        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => lobbyCanvas.OnClickJoinRoom(RoomNameText.text, gameObject));//adding event to the button
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

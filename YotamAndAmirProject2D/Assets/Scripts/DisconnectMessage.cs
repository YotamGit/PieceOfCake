using UnityEngine;

public class DisconnectMessage : MonoBehaviour {

    public void OnClickExitToDesktop()
    {
        Debug.Log("Exiting Application!");
        Application.Quit();
    }

    public void OnClickReconnect()
    {
        if (!PhotonNetwork.connecting)
        {
            Debug.Log("Reconnecting to server...");
            PhotonNetwork.ConnectUsingSettings("game");
        }
    }
}

using UnityEngine;
using TMPro;

public class PlayerListing : MonoBehaviour {

    public PhotonPlayer PhotonPlayer { get; private set; }

    [SerializeField]
    private TextMeshProUGUI _playerName;
    private TextMeshProUGUI PlayerName
    {
        get { return _playerName; }
    }

    public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
    {
        PhotonPlayer = photonPlayer;
        PlayerName.text = photonPlayer.NickName;
    }
}

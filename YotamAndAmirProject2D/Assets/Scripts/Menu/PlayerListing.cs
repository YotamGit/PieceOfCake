using UnityEngine;
using TMPro;

public class PlayerListing : MonoBehaviour
{
    public PhotonPlayer PhotonPlayer;

    [SerializeField]
    private TextMeshProUGUI PlayerName;

    public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
    {
        PhotonPlayer = photonPlayer;
        PlayerName.text = photonPlayer.NickName;
    }
}

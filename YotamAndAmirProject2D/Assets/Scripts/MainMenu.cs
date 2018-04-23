using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public AudioMixer auidoMixer;

    public TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private Toggle fullScreen;


    public TMP_Dropdown qualityDropdown;

    Resolution[] resolutions;
    
    public static MainMenu Instance;

    public LobbyCanvas LobbyCanvas;

    public RoomLayoutGroup RoomLayoutGroup;

    [SerializeField]
    private Button roomCancelButton;
    [SerializeField]
    private GameObject LoadingGameText, errorMessage;
    [SerializeField]
    private TextMeshProUGUI errorText;

    [SerializeField]
    private TMP_InputField passwordText;

    [SerializeField]
    private GameObject mainMenu, optionsScreen, chooseingScreen;

    private void Awake()
    {
        Time.timeScale = 1;

        Instance = this;

        fullScreen.isOn = Screen.fullScreen;

        // setting the graphics settings according to the graphics that the player enters:

        qualityDropdown.value = QualitySettings.GetQualityLevel();


        // getting resolutions and placing them in the resolution dropdown:

        Resolution[] tempResolutions = Screen.resolutions;
        resolutions = new Resolution[tempResolutions.Length];

        resolutionDropdown.ClearOptions(); // clearing current res options in dropdown

        List<string> options = new List<string>(); // list that holds all the option resolutions

        string tempRes = "", LastTempRes = "";
        int currentNumInDropDown = 0;
        
        int currentResIndex = 0;
        for (int i = 0; i < tempResolutions.Length; i++) // placing res in the list
        {
            tempRes = tempResolutions[i].width + " x " + tempResolutions[i].height;

            if(LastTempRes != tempRes) // preventing duplicets of the same resolution
            {
                options.Add(tempRes);

                //entering the resolution to the array so we could switch resolutions later
                resolutions[currentNumInDropDown] = tempResolutions[i];

                if (tempResolutions[i].width == Screen.width && tempResolutions[i].height == Screen.height)//(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResIndex = currentNumInDropDown;
                }
                currentNumInDropDown++;
            }

            LastTempRes = tempResolutions[i].width + " x " + tempResolutions[i].height;
        }

        resolutionDropdown.AddOptions(options); // adding the resolution options to the list
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }
    
    public IEnumerator DisplayError(string error)
    {
        errorMessage.SetActive(true);
        errorText.text = error;
        yield return new WaitForSeconds(2f);
        errorMessage.SetActive(false);
    }
    
    // This func disables the cancel button, and after the master clients sees that the other canceld too it loads the scene
    public void PlayGame(int sceneIndex)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("DisableCancelButton", PhotonTargets.All, sceneIndex);
    }

    [PunRPC]
    private void DisableCancelButton(int sceneIndex)
    {
        if (PhotonNetwork.room.PlayerCount == 2)
        {
            roomCancelButton.interactable = false;
            LoadingGameText.SetActive(true);

            if (!PhotonNetwork.isMasterClient) // telling the master client that he can load the scene
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("LoadScene", PhotonTargets.MasterClient, sceneIndex);
            }
        }
    }

    [PunRPC]
    private void LoadScene(int sceneIndex)
    {
        if (PhotonNetwork.room.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel(sceneIndex); // loading the Game scene
        }
        else
        {
            roomCancelButton.interactable = true;
            roomCancelButton.GetComponent<GameObject>().SetActive(true);
            LoadingGameText.SetActive(false);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        auidoMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}
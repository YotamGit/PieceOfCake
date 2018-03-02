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
    //public LobbyCanvas LobbyCanvas
    //{
    //    get { return _lobbyCanvas; }
    //}

    [SerializeField]
    private WaitingRoomCanvas WaitingRoomCanvas;
    //public WaitingRoomCanvas WaitingRoomCanvas
    //{
    //    get { return _waitingRoomCanvas; }
    //}

    public RoomLayoutGroup RoomLayoutGroup;

    [SerializeField]
    private Button roomCancleButton;
    [SerializeField]
    private GameObject LoadingGameText;

    private void Awake()
    {
        Instance = this;

        fullScreen.isOn = Screen.fullScreen;

        // setting the graphics settings according to the graphics that the player enters:

        qualityDropdown.value = QualitySettings.GetQualityLevel();


        // getting resolutions and placing them in the resolution dropdown:

        Resolution[] tempResolutions = Screen.resolutions;
        resolutions = new Resolution[tempResolutions.Length];
        //resolutions = Screen.resolutions;

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
    
    /*private void Start() // probably doesnt matter if in start or awake im this case...
    {
        ; // updating the fullscreen button if the player started the game with fullscreen or not
        //SetFullscreen(Screen.fullScreen);
    }*/
    
    public void PlayGame()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("DisableCancleButton", PhotonTargets.All);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    [PunRPC]
    private void DisableCancleButton()
    {
        roomCancleButton.interactable = false;
        LoadingGameText.SetActive(true);
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
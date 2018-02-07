using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer auidoMixer;

    //public Dropdown resolutionDropdown; // will not work with a TextMeshPro Dropdown...

    Resolution[] resolutions;
    
    public static MainMenu Instance;

    [SerializeField]
    private LobbyCanvas _lobbyCanvas;
    public LobbyCanvas LobbyCanvas
    {
        get { return LobbyCanvas; }
    }

    private void Awake()
    {
        Instance = this;
    }

    /*private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions(); // clearing current res options

        List<string> options = new List<string>(); // list that holds all the option resolutions
        int currentResIndex = 0;
        for(int i = 0; i < resolutions.Length; i++) // placing res in the list
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options); // adding the resolution options to the list
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }*/
    
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
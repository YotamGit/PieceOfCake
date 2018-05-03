using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class FPSDisplay : Photon.MonoBehaviour
{
    private float deltaTime = 0.0f;
    private bool Toggle;


    [SerializeField] private KeyCode toggleButton;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private GameObject TextObj;

    public Image micIndicator;

    private void Start()
    {
        Toggle = false;
        TextObj.SetActive(false);
    }

    private void Update()
    {

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        if (Input.GetKeyDown(toggleButton))
        {
            Toggle = !Toggle;
        }

        if (Toggle && !TextObj.activeInHierarchy)
        {
            TextObj.SetActive(true);

            StartCoroutine(DisplayStats()); // this will start when the toggled and stop when not toggled
        }
        else if(!Toggle && TextObj.activeInHierarchy)
        {
            TextObj.SetActive(false);
        }

        if (Input.GetKey(KeyCode.V))
        {
            micIndicator.color = new Color(1, 0, 0, 0.5f);
        }
        else
        {
            micIndicator.color = new Color(1, 0.5f, 0.5f, 0.5f);
        }
    }

    private IEnumerator DisplayStats()
    {
        while (Toggle)
        {
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;

            Text.text = "FPS(" + ((int)fps).ToString() + ") - Ping(" + PhotonNetwork.GetPing() + ")";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
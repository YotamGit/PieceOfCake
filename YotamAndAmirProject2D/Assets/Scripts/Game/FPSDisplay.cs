using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FPSDisplay : Photon.MonoBehaviour
{
    private float deltaTime = 0.0f;
    private bool Toggle;

    [SerializeField] private KeyCode toggleButton;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private GameObject TextObj;

    private void Start()
    {
        Toggle = false;

        TextObj.SetActive(false);

        StartCoroutine(DisplayStats());
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
        }
        else if(!Toggle && TextObj.activeInHierarchy)
        {
            TextObj.SetActive(false);
        }
    }

    private IEnumerator DisplayStats()
    {
        while (true)
        {
            if (Toggle)
            {
                float msec = deltaTime * 1000.0f;
                float fps = 1.0f / deltaTime;

                Text.text = "FPS(" + ((int)fps).ToString() + ") - Ping(" + PhotonNetwork.GetPing() + ")";
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    //void OnGUI()
    //{
    /*if (Toggle)
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        //float msec = deltaTime * 1000.0f;
        //float fps = 1.0f / deltaTime;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;

        string text = "FPS(" + ((int)fps).ToString() + ") - Ping(" + PhotonNetwork.GetPing() + ")";
        GUI.Label(rect, text, style);
    }*/
    //string text = string.Format("{1:0.} ping", PhotonNetwork.GetPing() * 1.0f);//PhotonNetwork.networkingPeer.RoundTripTime); // GetPing works as well
    //Debug.Log("Ping: " + PhotonNetwork.GetPing());
    /*if (PhotonNetwork.GetPing() > 0)
    {
        //string text = string.Format("{1:0.} ping", PhotonNetwork.GetPing());//PhotonNetwork.networkingPeer.RoundTripTime); // GetPing works as well
        //GUI.Label(rect, text, style);
    }*/
    //}
}
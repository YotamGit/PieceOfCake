using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FPSDisplay : Photon.MonoBehaviour
{
    float deltaTime = 0.0f;
    private bool togglePing;

    [SerializeField] private KeyCode pingButton;

    private void Start()
    {
        togglePing = false;
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        if (Input.GetKeyDown(pingButton))
        {
            togglePing = !togglePing;
        }
    }

    void OnGUI()
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

        if (togglePing)
        {
            string text = "FPS(" + ((int)fps).ToString() + ") - Ping(" + PhotonNetwork.GetPing() + ")";
            GUI.Label(rect, text, style);
        }
        //string text = string.Format("{1:0.} ping", PhotonNetwork.GetPing() * 1.0f);//PhotonNetwork.networkingPeer.RoundTripTime); // GetPing works as well
        //Debug.Log("Ping: " + PhotonNetwork.GetPing());
        /*if (PhotonNetwork.GetPing() > 0)
        {
            //string text = string.Format("{1:0.} ping", PhotonNetwork.GetPing());//PhotonNetwork.networkingPeer.RoundTripTime); // GetPing works as well
            //GUI.Label(rect, text, style);
        }*/
    }
}
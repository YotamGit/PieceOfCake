using System.Collections.Generic;

//using Client.Photon.LoadBalancing;
using UnityEngine;
using UnityEngine.UI;

public class PushToTalkScript : MonoBehaviour
{
    [SerializeField]
    private PhotonVoiceRecorder rec;

    //public bool push = false;

    private void Start()
    {
        rec.gameObject.SetActive(false);
        rec.gameObject.SetActive(true);

        if (rec.IsTransmitting)
        { 
            rec.Transmit = false;
        }
    }
    private void Update()
    {
        Debug.Log(rec.IsTransmitting);
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            rec.Transmit = true;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            rec.Transmit = false;
        }
    }

}


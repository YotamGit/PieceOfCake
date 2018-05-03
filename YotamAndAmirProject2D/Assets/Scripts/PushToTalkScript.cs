using System.Collections.Generic;

//using Client.Photon.LoadBalancing;
using UnityEngine;
using UnityEngine.UI;

public class PushToTalkScript : MonoBehaviour
{
    [SerializeField]
    private PhotonVoiceRecorder rec;

    //[SerializeField]
    //private Image indicator;


    private void Update()
    {
        //Debug.Log(rec.IsTransmitting);
        rec.Transmit = Input.GetKey(KeyCode.V);
       
        /*if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            rec.Transmit = true;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            rec.Transmit = false;
        }*/
    }

}


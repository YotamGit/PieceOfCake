using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverHandler : Photon.MonoBehaviour ,IPunObservable
{

    [SerializeField]
    private Sprite turnedOff, turnedOn;

    [SerializeField]
    private bool isActivated = false;

    public AudioClip LeverSound;

    [SerializeField]
    private GameObject door;

    private SpriteRenderer leverSpriteRend;

    private Material doorMaterial;

    private Collider2D doorCol;

    // Use this for initialization
    void Start ()
    {
        leverSpriteRend = gameObject.GetComponent<SpriteRenderer>();
        doorMaterial = door.GetComponent<Renderer>().material;
        doorCol = door.GetComponent<Collider2D>();
    }
	
    //implement as RPC
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (photonView.isMine)
        {
            if (stream.isWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(isActivated);
            }
        }
        else
        {
            // Network player, receive data
            isActivated = (bool)stream.ReceiveNext();

            if(isActivated)
            {
                if(leverSpriteRend.sprite != turnedOn)
                {
                    leverSpriteRend.sprite = turnedOn;
                }
                if(doorMaterial.color.a != 0.4f)
                {
                    doorMaterial.color = new Color(1, 1, 1, 0.4F);
                }
                doorCol.enabled = false;
            }
            else
            {
                if (leverSpriteRend.sprite != turnedOff)
                {
                    leverSpriteRend.sprite = turnedOff;
                }
                if (doorMaterial.color.a != 1f)
                {
                    doorMaterial.color = new Color(1, 1, 1, 1F);
                }
                doorCol.enabled = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(photonView.isMine && (col.gameObject.tag == "Player1" || col.gameObject.tag == "Player2"))
        {
            if (leverSpriteRend.sprite == turnedOff && !isActivated)
            {
                if (col.gameObject.tag == "Player1") // sound effects
                {
                    SoundManager.instance.efxSource1.volume = 0.3f;
                    SoundManager.instance.PlayEffect1(LeverSound);
                }
                else
                {
                    SoundManager.instance.efxSource2.volume = 0.3f;
                    SoundManager.instance.PlayEffect2(LeverSound);
                }
                isActivated = true;
                leverSpriteRend.sprite = turnedOn;
                doorMaterial.color = new Color(1, 1, 1, 0.4F);
                doorCol.enabled = false;
            }
            else
            {
                if (col.gameObject.tag == "Player1")// sound effects
                {
                    SoundManager.instance.efxSource1.volume = 0.3f;
                    SoundManager.instance.PlayEffect1(LeverSound);
                }
                else
                {
                    SoundManager.instance.efxSource2.volume = 0.3f;
                    SoundManager.instance.PlayEffect2(LeverSound);
                }
                isActivated = false;
                leverSpriteRend.sprite = turnedOff;
                doorMaterial.color = new Color(1, 1, 1, 1F);
                doorCol.enabled = true;
            }
        }
    }
}

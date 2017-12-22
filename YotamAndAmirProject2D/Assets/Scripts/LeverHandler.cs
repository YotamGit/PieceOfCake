using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverHandler : MonoBehaviour {

    [SerializeField]
    private Sprite turnedOff, turnedOn;

    public AudioClip LeverSound;

    [SerializeField]
    private GameObject door;

    private SpriteRenderer leverSpriteRend;

    // Use this for initialization
    void Start ()
    {
        leverSpriteRend = gameObject.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update (){
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player1" || col.gameObject.tag == "Player2")
        {
            Collider2D doorCol = door.GetComponent<Collider2D>();
            Material doorMaterial = door.GetComponent<Renderer>().material;

            if (leverSpriteRend.sprite == turnedOff)
            {
                if (col.gameObject.tag == "Player1")
                {
                    SoundManager.instance.efxSource1.volume = 0.3f;
                    SoundManager.instance.PlayEffect1(LeverSound);
                }
                else
                {
                    SoundManager.instance.efxSource2.volume = 0.3f;
                    SoundManager.instance.PlayEffect2(LeverSound);
                }
                leverSpriteRend.sprite = turnedOn;
                doorMaterial.color = new Color(1, 1, 1, 0.4F);
            }
            else
            {
                if (col.gameObject.tag == "Player1")
                {
                    SoundManager.instance.efxSource1.volume = 0.3f;
                    SoundManager.instance.PlayEffect1(LeverSound);
                }
                else
                {
                    SoundManager.instance.efxSource2.volume = 0.3f;
                    SoundManager.instance.PlayEffect2(LeverSound);
                }
                leverSpriteRend.sprite = turnedOff;
                doorMaterial.color = new Color(1, 1, 1, 1F);
            }
            doorCol.enabled = !doorCol.enabled;
        }
    }
}

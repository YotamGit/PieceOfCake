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
        if(col.gameObject.tag == "Player")
        {

            Collider2D doorCol = door.GetComponent<Collider2D>();
            Material doorMaterial = door.GetComponent<Renderer>().material;

            if (leverSpriteRend.sprite == turnedOff)
            {
                SoundManager.instance.PlaySingle2(LeverSound);
                leverSpriteRend.sprite = turnedOn;
                doorMaterial.color = new Color(1, 1, 1, 0.4F);
            }
            else
            {
                SoundManager.instance.PlaySingle2(LeverSound);
                leverSpriteRend.sprite = turnedOff;
                doorMaterial.color = new Color(1, 1, 1, 1F);
            }
            doorCol.enabled = !doorCol.enabled;
        }
    }
}

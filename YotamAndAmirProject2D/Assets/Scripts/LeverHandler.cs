using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverHandler : MonoBehaviour {

    [SerializeField]
    private Sprite turnedOff;

    [SerializeField]
    private Sprite turnedOn;

    [SerializeField]
    private string doorTag;
    //public int numOfDoor;

    private GameObject door;

    // Use this for initialization
    void Start () {
        gameObject.GetComponent<SpriteRenderer>().sprite = turnedOff;
        door = GameObject.FindGameObjectsWithTag(doorTag)[0];
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

            if (gameObject.GetComponent<SpriteRenderer>().sprite == turnedOff)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = turnedOn;
                doorMaterial.color = new Color(1, 1, 1, 0.4F);
                doorCol.isTrigger = !doorCol.isTrigger;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = turnedOff;
                doorMaterial.color = new Color(1, 1, 1, 1F);
                doorCol.isTrigger = !doorCol.isTrigger;
            }
        }
    }
}

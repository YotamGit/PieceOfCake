using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : MonoBehaviour {

    [SerializeField]
    private bool grabbed;

    [SerializeField]
    private float throwforce;

    [SerializeField]
    private Transform holdpoint;

    private Collision2D heldObject;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (grabbed)
        {
            heldObject.transform.position = holdpoint.position;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (grabbed)
            {
                grabbed = false;
                heldObject.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * throwforce, 1 * throwforce);
                heldObject.collider.isTrigger = !heldObject.collider.isTrigger;
                heldObject = null;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Substring(0, 3) == "Key")
        {
            if (!grabbed)
            {
                grabbed = true;
                col.transform.position = holdpoint.position;
                heldObject = col;
                heldObject.collider.isTrigger = !heldObject.collider.isTrigger;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag.Substring(0, 4) == "Door" && heldObject != null) // need to find a better way of mapping a door to a key!
        {
            if (heldObject.gameObject.tag[3] == col.gameObject.tag[4]) // blue key + blue door collition
            {
                CancelObject(col);
            }
        }
    }


    private void CancelObject(Collision2D col)
    {
        heldObject.gameObject.SetActive(false);
        col.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.4F);
        col.collider.isTrigger = !col.collider.isTrigger;
        grabbed = false;
        heldObject = null;
    }
}

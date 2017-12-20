using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : MonoBehaviour
{

    [SerializeField]
    private bool grabbedKey, grabbedCube;

    [SerializeField]
    private float throwForce;

    [SerializeField]
    private Transform keyHoldPoint;
    private Transform cubeHoldPoint;


    private Collision2D heldKey, heldCube;

    public AudioClip pickUpSound;
    public AudioClip doorSound;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (grabbedKey)
        {
            heldKey.transform.position = keyHoldPoint.position;
        }
        if (grabbedCube)
        {
            heldCube.transform.position = cubeHoldPoint.position;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (grabbedKey)
            {
                grabbedKey = false;
                heldKey.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * throwForce, 1 * throwForce);
                heldKey.collider.enabled = !heldKey.collider.enabled;
                heldKey = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (grabbedCube)
            {
                grabbedCube = false;
                heldCube.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                heldCube.collider.enabled = !heldCube.collider.enabled;
                heldCube = null;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Substring(0, 3) == "Key")
        {
            if (!grabbedKey)
            {
                SoundManager.instance.PlaySingle1(pickUpSound);
                grabbedKey = true;
                col.transform.position = keyHoldPoint.position;
                heldKey = col;
                heldKey.collider.enabled = !heldKey.collider.enabled;
            }
        }
    }


    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag.Substring(0, 4) == "Door" && heldKey != null) // need to find a better way of mapping a door to a key!
        {
            if (heldKey.gameObject.tag[3] == col.gameObject.tag[4]) // blue key + blue door collition
            {
                SoundManager.instance.PlaySingle2(doorSound);
                CancelObject(col);
            }
        }
        if (col.gameObject.tag == "Cube" && Input.GetKey(KeyCode.Space))
        {
            if (!grabbedCube)
            {
                grabbedCube = true;
                col.transform.position = cubeHoldPoint.position;
                heldCube = col;
                heldCube.collider.enabled = !heldCube.collider.enabled;
            }
        }

    }


    private void CancelObject(Collision2D col)
    {
        heldKey.gameObject.SetActive(false);
        col.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.4F);
        col.collider.enabled = !col.collider.enabled;
        grabbedKey = false;
        heldKey = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : MonoBehaviour
{

    [SerializeField]
    private bool grabbedKey, grabbedCube;

    [SerializeField]
    private float throwForce;

    public Transform keyHoldPoint;
    public Transform cubeHoldPoint;

    private Collision2D heldKey;
    private Collision2D heldCube;

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
        else if (grabbedCube)
        {
            heldCube.transform.position = cubeHoldPoint.position;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (grabbedKey && !grabbedCube)
            {
                grabbedKey = false;
                heldKey.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * throwForce, 1 * throwForce);
                heldKey.collider.enabled = !heldKey.collider.enabled;
                heldKey = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (grabbedCube && !grabbedKey)
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
            if (!grabbedKey && !grabbedCube)
            {
                SoundManager.instance.efxSource.volume = 0.3f;
                SoundManager.instance.PlayEffect(pickUpSound);
                grabbedKey = true;
                col.transform.position = keyHoldPoint.position;
                heldKey = col;
                heldKey.collider.enabled = !heldKey.collider.enabled;
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag.Substring(0, 4) == "Door" && heldKey != null) // need to find a better way of mapping a door to a key!
        {
            if (heldKey.gameObject.tag[3] == col.gameObject.tag[4]) // blue key + blue door collition
            {
                SoundManager.instance.efxSource.volume = 0.2f;
                SoundManager.instance.PlayEffect(doorSound);
                CancelObject(col);
            }
        }
        else if (col.gameObject.tag == "Cube" && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.F)))
        {
            if (!grabbedCube && !grabbedKey)
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

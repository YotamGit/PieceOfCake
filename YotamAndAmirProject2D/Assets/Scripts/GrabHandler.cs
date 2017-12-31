using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : MonoBehaviour
{
    public GameObject VictoryScreen;
    [Space]
    [Header("Key Bindings")]
    public KeyCode pickUpKey;
    public KeyCode dropKey;

    [SerializeField]
    private bool grabbedKey, grabbedCube;

    [SerializeField]
    private float throwForce;

    [Space]

    [Header("Hold Points")]
    public Transform keyHoldPoint;
    public Transform cubeHoldPoint;
    public Transform otherPlayerKeyPoint;
    public Transform otherPlayerCubePoint;

    private Collision2D heldKey;
    private Collision2D heldCube;

    [Space]

    [Header("Audio")]
    public AudioClip pickUpSound;
    public AudioClip doorSound;
    public AudioClip VictoryTheme;

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
            heldKey.rigidbody.velocity = new Vector2(0, 0);
        }
        else if (grabbedCube)
        {
            heldCube.transform.position = cubeHoldPoint.position;
            heldCube.rigidbody.velocity = new Vector2(0, 0);
        }

        if (Input.GetKeyDown(dropKey))
        {
            if (grabbedKey && !grabbedCube)
            {
                grabbedKey = false;
                heldKey.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * throwForce, 1 * throwForce);
                heldKey.collider.enabled = !heldKey.collider.enabled;
                heldKey = null;
            }
            else if (grabbedCube && !grabbedKey)
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
        if(col.gameObject.tag == "Victory")
        {
            VictoryScreen.SetActive(true);
            Time.timeScale = 0.0f;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            SoundManager.instance.moveEfxSource1.Stop();
            SoundManager.instance.efxSource1.Stop();
            SoundManager.instance.moveEfxSource2.Stop();
            SoundManager.instance.efxSource2.Stop();
            SoundManager.instance.musicSource.Stop();

            SoundManager.instance.musicSource.loop = false;
            SoundManager.instance.musicSource.clip = VictoryTheme;
            SoundManager.instance.musicSource.Play();
        }
        else if (col.gameObject.tag.Substring(0, 3) == "Key")
        {
            if (!grabbedKey && !grabbedCube)
            {
                if(col.transform.position != otherPlayerKeyPoint.position)
                {
                    if (gameObject.tag == "Player1")
                    {
                        SoundManager.instance.efxSource1.pitch = 1.8f;
                        SoundManager.instance.efxSource1.volume = 0.3f;
                        SoundManager.instance.PlayEffect1(pickUpSound);
                    }
                    else
                    {
                        SoundManager.instance.efxSource2.pitch = 1.8f;
                        SoundManager.instance.efxSource2.volume = 0.3f;
                        SoundManager.instance.PlayEffect2(pickUpSound);
                    }
                    grabbedKey = true;
                    col.transform.position = keyHoldPoint.position;
                    heldKey = col;
                    heldKey.collider.enabled = !heldKey.collider.enabled;
                }
                
            }
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag.Substring(0, 4) == "Door" && heldKey != null) // need to find a better way of mapping a door to a key!
        {
            if (heldKey.gameObject.tag[3] == col.gameObject.tag[4]) // blue key + blue door collition
            {
                if (gameObject.tag == "Player1")
                {
                    SoundManager.instance.efxSource1.volume = 0.2f;
                    SoundManager.instance.PlayEffect1(doorSound);
                }
                else
                {
                    SoundManager.instance.efxSource2.volume = 0.2f;
                    SoundManager.instance.PlayEffect2(doorSound);
                }
                CancelObject(col);
            }
        }
        else if (col.gameObject.tag == "Cube" && (Input.GetKey(pickUpKey)))//KeyCode.Space) || Input.GetKey(KeyCode.F)))
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

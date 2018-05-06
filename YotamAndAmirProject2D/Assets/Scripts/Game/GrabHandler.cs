using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : Photon.MonoBehaviour, IPunObservable
{
    public LayerMask notGrabMask; // so the player will not drop the key/cube inside a collider
    [Space]
    [Header("Key Bindings")]
    public KeyCode pickUpKey;
    public KeyCode dropKey;

    public bool grabbedKey, grabbedCube;//used to check if the players are holding an object
    private Rigidbody2D heldObjRigidbody;


    [SerializeField]
    private float throwForce;

    [Space]

    [Header("Hold Points")]
    public Transform keyHoldPoint;
    public Transform cubeHoldPoint;

    private GrabHandler otherPlayerGrabHandler;//used to check if the other player is holding the same object as the current player

    public BoxCollider2D heldKey;
    public BoxCollider2D heldCube;

    [Space]

    [Header("Audio")]
    public AudioClip pickUpSound;
    public AudioClip doorSound;
    public AudioClip VictoryTheme;
    public AudioSource efxSource;
    public AudioSource moveSource;

    private PhotonNetworkManager gl;

    private PhotonView photonView;

    void Start()
    {
        photonView = PhotonView.Get(this);
        if (PhotonNetwork.isMasterClient && tag == "Player1" || !PhotonNetwork.isMasterClient && tag == "Player2") // if one of either main players
        {
            gl = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<PhotonNetworkManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {//implement as RPC
        //getting the other player's grabHandler script
        if (!otherPlayerGrabHandler)
        {
            if (gameObject.tag == "Player1") // if is player 1
            {
                GameObject otherPlayerObj = GameObject.FindGameObjectWithTag("Player2");
                if (otherPlayerObj)
                {
                    otherPlayerGrabHandler = otherPlayerObj.GetComponent<GrabHandler>();
                }
            }
            else
            {
                GameObject otherPlayerObj = GameObject.FindGameObjectWithTag("Player1");
                if (otherPlayerObj)
                {
                    otherPlayerGrabHandler = otherPlayerObj.GetComponent<GrabHandler>();
                }
            }
        }

        // placing the held object in it's position if needed
        if (grabbedKey)
        {
            if (heldKey)
            {
                if (otherPlayerGrabHandler.grabbedKey && gameObject.tag == "Player2")
                {
                    if (otherPlayerGrabHandler.heldKey.tag == heldKey.tag)
                    {
                        heldKey = null;
                        grabbedKey = false;
                        return;
                    }
                }
                heldKey.transform.position = keyHoldPoint.position;
                heldObjRigidbody.velocity = new Vector2(0, 0);
            }
            else
            {
                grabbedKey = false;
                Debug.Log("ERROR: Grabbing a null key!");
            }
        }
        else if (grabbedCube)
        {
            if (heldCube)
            {
                heldCube.transform.position = cubeHoldPoint.position;
                heldObjRigidbody.velocity = new Vector2(0, 0);
            }
            else
            {
                grabbedCube = false;
                Debug.Log("ERROR: Grabbing a null cube!");
            }
        }

        // throwing the object
        if (Input.GetKeyDown(dropKey) && photonView.isMine)
        {
            if (grabbedKey && !Physics2D.OverlapPoint(keyHoldPoint.position, notGrabMask))
            {
                photonView.RPC("DropObj", PhotonTargets.All, heldKey.gameObject.tag, gameObject.tag, false);
            }
            else if (grabbedCube && !Physics2D.OverlapPoint(cubeHoldPoint.position, notGrabMask))
            {
                photonView.RPC("DropObj", PhotonTargets.All, heldCube.gameObject.tag, gameObject.tag, true);
            }
        }
    }

    [PunRPC]
    private void GotObj(string objTag, string playerTag, bool objTybe) //objType is false => Key - objType is true => Cube
    {
        if (gameObject.tag == playerTag)
        {
            BoxCollider2D tempCol = GameObject.FindGameObjectWithTag(objTag).GetComponent<BoxCollider2D>();
            if (objTybe) // Recieving the cube
            {
                grabbedCube = true;
                heldCube = tempCol;
                heldObjRigidbody = heldCube.GetComponent<Rigidbody2D>();
                heldCube.enabled = false;
            }
            else // Recieving the key
            {
                grabbedKey = true;
                heldKey = tempCol;
                heldObjRigidbody = heldKey.GetComponent<Rigidbody2D>();
                heldKey.enabled = false;
                SoundManager.instance.PlayEffect(efxSource, pickUpSound);
            }
        }
    }

    [PunRPC]
    private void DropObj(string objTag, string playerTag, bool objTybe) //objType is false => Key - objType is true => Cube
    {
        if (gameObject.tag == playerTag)
        {
            if (objTybe)//throwing cube
            {
                grabbedCube = false;
                if (!heldCube)
                {
                    heldCube = GameObject.FindGameObjectWithTag(objTag).GetComponent<BoxCollider2D>();
                }
                heldCube.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                heldCube.enabled = true;
                heldCube = null;
            }
            else//throwing key
            {
                grabbedKey = false;
                if (!heldKey)
                {
                    heldKey = GameObject.FindGameObjectWithTag(objTag).GetComponent<BoxCollider2D>();
                }
                heldKey.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * throwForce, 1 * throwForce);
                heldKey.enabled = true;
                heldKey = null;
            }
            heldObjRigidbody = null;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }//the function is needed to synchronized the objects

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Victory" && ((PhotonNetwork.isMasterClient && tag == "Player1") || (!PhotonNetwork.isMasterClient && tag == "Player2")))
        {
            photonView.RPC("WaitVictory", PhotonTargets.Others);//telling the other player to wait 30s - exit if that happens
            gl.DisplayWinningChoice();
            Victory();
            //photonView.RPC("Victory", PhotonTargets.All);//activating the victory function
        }
        else if (col.gameObject.tag.Substring(0, 3) == "Key")
        {
            if (!grabbedKey && !grabbedCube)
            {

                //efxSource.pitch = 1.8f;
                //efxSource.volume = 0.3f;
                //SoundManager.instance.PlayEffect(efxSource, pickUpSound);
                photonView.RPC("GotObj", PhotonTargets.All, col.gameObject.tag, gameObject.tag, false);
            }
        }
    }

    [PunRPC]
    void WaitVictory()
    {
        if (gl)
        {
            gl.DisplayWinningWaiting();
        }
        else
        {
            efxSource.Stop();
            moveSource.Stop();
            SoundManager.instance.musicSource.Stop();

            SoundManager.instance.musicSource.loop = false;
            SoundManager.instance.musicSource.clip = VictoryTheme;
            SoundManager.instance.musicSource.Play();

            Time.timeScale = 1;

            GameObject.FindGameObjectWithTag("GameLogic").GetComponent<PhotonNetworkManager>().DisplayWinningWaiting();
        }
        Destroy(gameObject);
    }

    void Victory()
    {
        gl.wonGame = true;

        //gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        efxSource.Stop();
        moveSource.Stop();
        SoundManager.instance.musicSource.Stop();

        SoundManager.instance.musicSource.loop = false;
        SoundManager.instance.musicSource.clip = VictoryTheme;
        SoundManager.instance.musicSource.Play();

        Time.timeScale = 1;
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag.Substring(0, 4) == "Door" && heldKey != null)//opening a door using a key
        {
            if (heldKey.gameObject.tag[3] == col.gameObject.tag[4]) // blue key + blue door collision
            {
                efxSource.volume = 0.2f;
                SoundManager.instance.PlayEffect(efxSource, doorSound);

                CancelObject(col);//canceling the door
            }
        }
        else if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }
        else if ((col.gameObject.tag.Substring(0, 4) == "Cube" && (Input.GetKey(pickUpKey))))//picking up a cube
        {
            if (!grabbedCube && !grabbedKey)
            {
                photonView.RPC("GotObj", PhotonTargets.All, col.gameObject.tag, gameObject.tag, true);
            }
        }
    }

    private void CancelObject(Collision2D col)
    {
        Destroy(heldKey.gameObject);//destroying thr key
        col.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.4F);
        col.collider.enabled = !col.collider.enabled;
        grabbedKey = false;
        heldKey = null;
    }
}

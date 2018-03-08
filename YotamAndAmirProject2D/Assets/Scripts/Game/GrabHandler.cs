using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : Photon.MonoBehaviour , IPunObservable
{
    private GameObject VictoryScreen;
    public LayerMask notGrabMask; // so the player will not drop the key/cube inside a collider
    [Space]
    [Header("Key Bindings")]
    public KeyCode pickUpKey;
    public KeyCode dropKey;

    //[SerializeField]
    public bool grabbedKey, grabbedCube;
    private Rigidbody2D heldObjRigifbody;
    //private string heldKeyTag;
    //private string heldCubeTag;

    [SerializeField]
    private float throwForce;

    [Space]

    [Header("Hold Points")]
    public Transform keyHoldPoint;
    public Transform cubeHoldPoint;
    //public Transform otherPlayerKeyPoint;
    //public Transform otherPlayerCubePoint;

    private GrabHandler otherPlayerGrabHandler;

    public BoxCollider2D heldKey;
    public BoxCollider2D heldCube;

    [Space]

    [Header("Audio")]
    public AudioClip pickUpSound;
    public AudioClip doorSound;
    public AudioClip VictoryTheme;


    /*[Space]

    [Header("Keys In Scene")]
    public GameObject[] keys;*/


    // Use this for initialization
    void Start()
    {
        VictoryScreen = GameObject.FindGameObjectWithTag("WinningText");
        if (VictoryScreen != null) // preventing sccidents that occure when both the grabHandlers try to get the screen
        {
            VictoryScreen.SetActive(false);
            Debug.Log("victory at start - " + gameObject.tag);
        }
        //otherPlayerKeyPoint = null;
        //otherPlayerCubePoint = null;
        //heldKeyTag = "";
        //heldCubeTag = "";
    }

    // Update is called once per frame
    void Update()
    {
        //getting the other players grabHandler script
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
        // getting the hold points of the other player
        /*if (!otherPlayerKeyPoint)
        {
            if(gameObject.tag == "Player1")
            {
                GameObject tempOtherPlayer = GameObject.FindGameObjectWithTag("Player2");
                if (tempOtherPlayer)
                {
                    otherPlayerKeyPoint = tempOtherPlayer.GetComponent<GrabHandler>().keyHoldPoint;
                }
            }
            else
            {
                GameObject tempOtherPlayer = GameObject.FindGameObjectWithTag("Player1");
                if (tempOtherPlayer)
                {
                    otherPlayerKeyPoint = tempOtherPlayer.GetComponent<GrabHandler>().keyHoldPoint;
                }
            }
        }
        if (!otherPlayerCubePoint)
        {
            if (gameObject.tag == "Player1")
            {
                GameObject tempOtherPlayer = GameObject.FindGameObjectWithTag("Player2");
                if (tempOtherPlayer)
                {
                    otherPlayerCubePoint = tempOtherPlayer.GetComponent<GrabHandler>().cubeHoldPoint;
                }
            }
            else
            {
                GameObject tempOtherPlayer = GameObject.FindGameObjectWithTag("Player1");
                if (tempOtherPlayer)
                {
                    otherPlayerCubePoint = tempOtherPlayer.GetComponent<GrabHandler>().cubeHoldPoint;
                }
            }
        }*/

        // placing the held object in its position if needed
        if (grabbedKey)
        {
            if(heldKey)
            {
                if (otherPlayerGrabHandler.grabbedKey && gameObject.tag == "Player2")
                {
                    if(otherPlayerGrabHandler.heldKey.tag == heldKey.tag)
                    {
                        heldKey = null;
                        grabbedKey = false;
                        return;
                    }
                }

                heldKey.transform.position = keyHoldPoint.position;
                heldObjRigifbody.velocity = new Vector2(0, 0);
            }
            else
            {
                grabbedKey = false;
                Debug.Log("Grabbing a null key!");
            }
        }
        else if (grabbedCube)
        {
            if (heldCube)
            {
                heldCube.transform.position = cubeHoldPoint.position;
                heldObjRigifbody.velocity = new Vector2(0, 0);
            }
            else
            {
                grabbedCube = false;
                Debug.Log("Grabbing a null cube!");
            }
        }

        // throwing the object
        if (Input.GetKeyDown(dropKey) && photonView.isMine)
        {
            if (grabbedKey && !Physics2D.OverlapPoint(keyHoldPoint.position, notGrabMask))
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("DropObj", PhotonTargets.All, heldKey.gameObject.tag, gameObject.tag, false);
            }
            else if (grabbedCube && !Physics2D.OverlapPoint(cubeHoldPoint.position, notGrabMask))
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("DropObj", PhotonTargets.All, heldCube.gameObject.tag, gameObject.tag, true);
            }
        }

        
        /*if (photonView.isMine)
        {
            Debug.Log("MY - " + gameObject.tag + "key: " + heldKeyTag);
        }
        else
        {
            Debug.Log("NOT - " + gameObject.tag + "key: " + heldKeyTag);
        }*/
        /*if (photonView.isMine)
        {
            Debug.Log("MY key: " + heldKeyTag);
        }
        else
        {
            Debug.Log("NOT MY key: " + heldKeyTag);
        }*/
        /*Debug.Log("reckey: " + heldKeyTag);
        if (heldKey != null)
        {
            Debug.Log("heldtag: " + heldKey.tag);
        }*/
        /*if(grabbedKey)
        {
            
            if (heldKey != null)
            {
                Debug.Log("True Grabbed: key GOOD");
            }
            else
            {
                heldKey = GameObject.FindGameObjectsWithTag(heldKeyTag)[0].GetComponent<Collision2D>();
                Debug.Log("True Grabbed: " + heldKey.gameObject.tag);
                //Debug.Log("True Grabbed: Key False");
            }
        }
        if(heldKey != null && grabbedKey)
        {
            Debug.Log("FALSE Grabbed: Key True");
        }*/

        /*if(grabbedKey && heldKey == null)
        {
            foreach (GameObject key in keys)
            {
                if (key.tag == heldKeyTag)
                {
                    heldKey = key.GetComponent<BoxCollider2D>(); // SANE PROBLEM!
                    Debug.Log("NEW KEY IS(upd): " + heldKey.gameObject.tag);
                    break;
                }
            }
        }
        
        if (grabbedKey && heldKey != null)
        {
            if (heldKey.enabled)
            {
                Debug.Log("AT UPDT");
                heldKey.enabled = false;
            }
            heldKey.transform.position = keyHoldPoint.position;
            heldKey.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }*/

        //works:
        /*if (heldKeyTag != "") // if key
        {
            if (heldKey == null) // getting key
            {
                heldKey = GameObject.FindGameObjectsWithTag(heldKeyTag)[0].GetComponent<BoxCollider2D>();
                //heldKey = key.GetComponent<BoxCollider2D>();
                //Debug.Log("NEW KEY(upd): " + heldKey.gameObject.tag);
            }

            if (heldKey.enabled) // fixing the keys collider (from enabled to disabled)
            {
                heldKey.enabled = false;
            }

            if (!grabbedKey) // checking if grabbedkey has a value of true
            {
                grabbedKey = true;
            }
            heldKey.transform.position = keyHoldPoint.position;
            heldKey.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        else
        {
            //Debug.Log("empty key tag");
            grabbedKey = false;
            //heldKeyTag = "";

            if (heldKey != null)
            {
                //heldKey.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * throwForce, 1 * throwForce);
                heldKey.enabled = true;
            }
            heldKey = null;
        }

        if (heldCubeTag != "") // if cube
        {
            if (heldCube == null) // getting Cube
            {
                heldCube = GameObject.FindGameObjectsWithTag(heldCubeTag)[0].GetComponent<BoxCollider2D>();
                //heldCube = Cube.GetComponent<BoxCollider2D>();
                Debug.Log("NEW Cube(upd): " + heldCube.gameObject.tag);
            }

            if (heldCube.enabled) // fixing the cubes collider (from enabled to disabled)
            {
                heldCube.enabled = false;
            }

            if (!grabbedCube) // checking if grabbedCube has a value of true
            {
                grabbedCube = true;
            }
            heldCube.transform.position = cubeHoldPoint.position;
            heldCube.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        else
        {
            //Debug.Log("empty cube tag");
            grabbedCube = false;

            if (heldCube != null)
            {
                heldCube.enabled = true;
            }
            heldCube = null;
        }

        /*if (grabbedCube && heldCube != null)
        {
            heldCube.transform.position = cubeHoldPoint.position;
            heldCube.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        else if(!grabbedCube && heldCube != null)
        {
            grabbedCube = false;
            heldCube.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            heldCube.enabled = true;
            heldCube = null;
        }*/
        //works:
        /*if (!photonView.isMine && PhotonNetwork.connected == true) // stopping the script so the other player won't throw the key when you click on drop
        {
            return;
        }

        if (Input.GetKeyDown(dropKey))
        {
            if (grabbedKey && !grabbedCube && !Physics2D.OverlapPoint(keyHoldPoint.position, notGrabMask))
            {
                //Debug.Log("Throwing");
                grabbedKey = false;
                heldKeyTag = "";
                heldKey.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * throwForce, 1 * throwForce);
                //heldKey.collider.enabled = !heldKey.collider.enabled;
                heldKey.enabled = !heldKey.enabled;
                heldKey = null;
            }
            else if (grabbedCube && !grabbedKey && !Physics2D.OverlapPoint(cubeHoldPoint.position, notGrabMask))
            {
                grabbedCube = false;
                heldCubeTag = "";
                heldCube.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                heldCube.enabled = !heldCube.enabled;
                heldCube = null;
            }
        }*/
    }

    [PunRPC]
    void GotObj(string objTag, string playerTag, bool objTybe) //objType is false => Key - objType is true => Cube
    {
        if(gameObject.tag == playerTag)
        {
            BoxCollider2D tempCol = GameObject.FindGameObjectWithTag(objTag).GetComponent<BoxCollider2D>();
            if (objTybe) // Recieving the cube
            {
                grabbedCube = true;
                heldCube = tempCol;
                heldObjRigifbody = heldCube.GetComponent<Rigidbody2D>();
                heldCube.enabled = false;
            }
            else // Recieving the key
            {
                grabbedKey = true;
                heldKey = tempCol;
                heldObjRigifbody = heldKey.GetComponent<Rigidbody2D>();
                heldKey.enabled = false;
            }
        }
    }

    [PunRPC]
    void DropObj(string objTag, string playerTag, bool objTybe) //objType is false => Key - objType is true => Cube
    {
        if (gameObject.tag == playerTag)
        {
            if (objTybe) // Recieving the cube
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
            else // Recieving the key
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
            heldObjRigifbody = null;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (photonView.isMine)
        {
            if (stream.isWriting)
            {
                // We own this player: send the others our data
                /* //First Way:
                stream.SendNext(grabbedKey);
                if (heldKey != null && grabbedKey)
                {
                    //stream.SendNext(heldKey.gameObject.tag);
                    stream.SendNext(heldKey.collider.enabled);
                    //stream.SendNext(heldKey.transform.position);
                }*/

                //Second Way (BEST):
                //stream.SendNext(grabbedKey);
                //stream.SendNext(grabbedCube);

                //WORKS:
                /*if (heldKey != null ) // Sending Key info
                {
                    //Debug.Log("sending: " + heldKey.gameObject.tag);
                    stream.SendNext(heldKey.gameObject.tag);
                    //Debug.Log("Sending: " + heldKey.gameObject.tag);
                }
                else
                {
                    //Debug.Log("sending: ");
                    stream.SendNext("");
                }

                if (heldCube != null) // Sending cube info
                {
                    //Debug.Log("sending: " + heldKey.gameObject.tag);
                    stream.SendNext(heldCube.gameObject.tag);
                    //Debug.Log("Sending: " + heldCube.gameObject.tag);
                }
                else
                {
                    //Debug.Log("sending: ");
                    stream.SendNext("");
                }*/

                //stream.SendNext(heldKey.gameObject.tag);

                //stream.SendNext(heldKeyTag);
                /*stream.SendNext(grabbedCube);
                if(heldCube != null)
                {
                    stream.SendNext(heldCube.enabled);
                    //stream.SendNext(heldCube.transform.position);
                }*/
            }
        }
        else
        {
            // Network player, receive data
            /* //First Way:
            grabbedKey = (bool)stream.ReceiveNext();
            if (heldKey != null && grabbedKey)
            {
                //heldKeyTag = (string)stream.ReceiveNext();
                //if(!heldKey.gameObject.tag.Equals(heldKeyTag))
                //{
                //    heldKey = GameObject.FindGameObjectsWithTag(heldKeyTag)[0].GetComponent<Collision2D>();
                //}
                //else
                //{
                //    grabbedKey = false;
                //    heldKey = null;
                //}
                heldKey.collider.enabled = (bool)stream.ReceiveNext();
                //heldKey.transform.position = (Vector3)stream.ReceiveNext();
            }*/


            //Second Way (BEST):
            //grabbedKey = (bool)stream.ReceiveNext();
            //grabbedCube = (bool)stream.ReceiveNext();//??
            //heldKeyTag = (string)stream.ReceiveNext(); //WORKS:

            //heldCubeTag = (string)stream.ReceiveNext(); //WORKS:
            //Debug.Log("Recieved Tag: " + heldKeyTag);


            //WORKS:
            /*if (heldKeyTag != "" && heldKey == null) // && heldKey == null
            {
                heldKey = GameObject.FindGameObjectsWithTag(heldKeyTag)[0].GetComponent<BoxCollider2D>();
                //Debug.Log("New key(at recieved): " + heldKey.gameObject.tag);
                if (heldKey.enabled)
                {
                    heldKey.enabled = false;
                }
            }
            if (heldCubeTag != "" && heldCube == null) // && heldCube == null
            {
                heldCube = GameObject.FindGameObjectsWithTag(heldCubeTag)[0].GetComponent<BoxCollider2D>();
                //Debug.Log("New Cube(at recieved): " + heldCube.gameObject.tag);
                if (heldCube.enabled)
                {
                    heldCube.enabled = false;
                }
            }*/

            /*foreach (GameObject key in keys)
            {
                if (key.tag == heldKeyTag)
                {
                    heldKey = GameObject.FindGameObjectsWithTag(heldKeyTag)[0].GetComponent<BoxCollider2D>();
                    //heldKey = key.GetComponent<BoxCollider2D>();
                    Debug.Log("New key(at recieved): " + heldKey.gameObject.tag);
                    if (heldKey.enabled)
                    {
                        heldKey.enabled = false;
                    }
                    break;
                }
            }*/
            /* // SLOWER, but less memory
            if (heldKeyTag != "")
            {
                GameObject tempKey = GameObject.FindGameObjectsWithTag(heldKeyTag)[0];
                Debug.Log("gameobject: " + GameObject.FindGameObjectsWithTag(heldKeyTag)[0].tag);
                heldKey = tempKey.GetComponent<Collision2D>(); // NOT WORKING! (need to recieve the key's gameobject/collision2D in a different way...)
            }
            else
            {
                Debug.Log("Recieved a Null Key!");
            }*/




            /*heldKeyTag = (string)stream.ReceiveNext();
            if(heldKeyTag != "" && heldKeyTag == null)
            {
                heldKey = GameObject.FindGameObjectsWithTag(heldKeyTag)[0].GetComponent<Collision2D>();
            }*/
            /*grabbedCube = (bool)stream.ReceiveNext();
            if (heldCube != null)
            {
                heldCube.collider.enabled = (bool)stream.ReceiveNext();
                //heldCube.transform.position = (Vector3)stream.ReceiveNext();
            }*/

        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Victory")
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("Victory", PhotonTargets.All);
        }
        else if (col.gameObject.tag.Substring(0, 3) == "Key")
        {
            if (!grabbedKey && !grabbedCube)
            {
                //if (col.gameObject.tag != otherPlayerGrabHandler.heldKey.tag)
                //{
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
                //heldKeyTag = col.gameObject.tag;
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("GotObj", PhotonTargets.All, col.gameObject.tag, gameObject.tag, false);
                /*grabbedKey = true;
                col.transform.position = keyHoldPoint.position;
                heldKey = col.gameObject.GetComponent<BoxCollider2D>();
                //heldKey.collider.enabled = !heldKey.collider.enabled;
                heldKey.enabled = !heldKey.enabled;*/
                //heldKeyTag = heldKey.tag;
                //}
            }
        }
    }

    [PunRPC]
    void Victory()
    {
        if (VictoryScreen != null) // telling the other player to enable the victoryScreen if we dont have it
        {
            VictoryScreen.SetActive(true);
        }
        else
        {
            otherPlayerGrabHandler.VictoryScreen.SetActive(true);
        }
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
        else if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }
        else if ((col.gameObject.tag.Substring(0,4) == "Cube" && (Input.GetKey(pickUpKey))))//KeyCode.Space) || Input.GetKey(KeyCode.F)))
        {
            if (!grabbedCube && !grabbedKey)
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("GotObj", PhotonTargets.All, col.gameObject.tag, gameObject.tag, true);
                /*grabbedCube = true;
                col.transform.position = cubeHoldPoint.position;
                heldCube = col.gameObject.GetComponent<BoxCollider2D>();
                heldCube.enabled = !heldCube.enabled;*/
                //heldCubeTag = heldCube.tag;
            }
        }
    }
    
    private void CancelObject(Collision2D col)
    {
        Destroy(heldKey.gameObject);
        //heldKey.gameObject.SetActive(false);
        col.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.4F);
        col.collider.enabled = !col.collider.enabled;
        grabbedKey = false;
        heldKey = null;
    }
}

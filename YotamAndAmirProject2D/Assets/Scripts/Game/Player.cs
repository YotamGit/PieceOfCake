using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : Photon.PunBehaviour, IPunObservable
{
    public string PlayerMovement;
    public GameObject[] FlameLights;

    [Space]

    [Header("Key Bindings")]
    public KeyCode jumpKey;
    public KeyCode boostDownKey;

    [Header("Settings")]

    [SerializeField]
    private float movementSpeed;

    // Variables for Jump (and force fall):
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround; // indication for what is considered ground

    [SerializeField]
    private Transform spawnPoint1, spawnPoint2;

    [Space]

    [Header("Audio")]
    // Audio
    public AudioClip[] BackGroundMusic;
    public AudioClip[] DeathMusic;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip bounceSound;
    public AudioSource efxSource;
    public AudioSource moveSource;
    public AudioListener listener;

    private Rigidbody2D rigidBody;
    private Animator myAnimator;

    private bool facingRight;

    [SerializeField]
    private bool isGroundedVar;

    private bool clickingDown;

    private DBCManager dataBaseScript;

    private PhotonView photonView;

    private PhotonNetworkManager PNManagerScript;


    void Start()
    {
        photonView = PhotonView.Get(this);
        Time.timeScale = 0f;
        //SoundManager.instance.musicSource.Stop();
        //SoundManager.instance.RandomizeSfx(BackGroundMusic);

        rigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        facingRight = true;

        gameObject.SetActive(true);
        clickingDown = false;

        if (gameObject.tag == "Player1")
        {
            if (GameObject.FindGameObjectWithTag("Player2") != null)
            {
                GameObject.FindGameObjectWithTag("Player2").GetComponent<AudioListener>().enabled = false;
                Time.timeScale = 1f;
                photonView.RPC("DisableWaitingScreen", PhotonTargets.All);
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Player1") != null)
            {
                GameObject.FindGameObjectWithTag("Player1").GetComponent<AudioListener>().enabled = false;
                Time.timeScale = 1f;
                photonView.RPC("DisableWaitingScreen", PhotonTargets.All);
            }
        }
        dataBaseScript = GameObject.FindGameObjectWithTag("DataBaseManager").GetComponent<DBCManager>();

        PNManagerScript = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<PhotonNetworkManager>();

        GameObject[] tempOtherPlayers = GameObject.FindGameObjectsWithTag(gameObject.tag); // deleting all the other spare players
        foreach (GameObject tempotherPlayer in tempOtherPlayers)
        {
            if(tempotherPlayer != gameObject)
                Destroy(tempotherPlayer);
        }
    }

    [PunRPC]
    void DisableWaitingScreen()
    {
        GameObject waitingScreen = GameObject.FindGameObjectWithTag("WaitingText");

        if (waitingScreen)
        {
            waitingScreen.SetActive(false);
        }
    }

    private void Update()
    {
        // checking if the object is mid air, and moving him according to the key he pressed
        isGroundedVar = IsGrounded();

        if (Time.timeScale == 1) // will not be called if time != normal time
        {
            float horizontal = Input.GetAxis(PlayerMovement);
            HandleMovement(horizontal);
            //send location and velocity to playert
            Flip(horizontal);
        }
    }

    private void FixedUpdate()
    {
        if (clickingDown)
        {
            rigidBody.AddForce(new Vector2(0, -jumpForce / 10));
        }
    }

    // This function is called when this object touches a different object
    void OnTriggerEnter2D(Collider2D col)
    {
        //SoundManager.instance.efxSource.Stop();
        // Checking if the player got damaged. reseting his position if he did
        if (col.gameObject.tag == "Dangerous" && ((PhotonNetwork.isMasterClient && tag == "Player1") || (!PhotonNetwork.isMasterClient && tag == "Player2")))//&& gameObject.GetComponent<AbilityManager>().Immune == false)
        {
            AbilityManager abilityManager = gameObject.GetComponent<AbilityManager>();

            if (!abilityManager.Immune) // don't have dmg immune
            {
                moveSource.Stop();
                efxSource.Stop();
                SoundManager.instance.musicSource.Stop();

                //telling the other player to run the ReturnToCheckPoint function
                if ((PhotonNetwork.isMasterClient && gameObject.tag == "Player1") || (!PhotonNetwork.isMasterClient && gameObject.tag == "Player2")) 
                {
                    dataBaseScript.AddDeathAndRestart();
                }
                photonView.RPC("ReturnToCheckPoint", PhotonTargets.All);
                //ReturnToCheckPoint();
            }
            else // has dmg immune
            {
                abilityManager.PowerUps[3].SetActive(false);
                abilityManager.Immune = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bounce")
        {
            SoundManager.instance.PlayEffect(efxSource, bounceSound);
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
    
    [PunRPC]
    private void ReturnToCheckPoint()
    {
        Debug.Log("loading scene again...");
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    private void HandleMovement(float Horizontal)
    {
        if (Horizontal != 0 && !moveSource.isPlaying)
        {
            SoundManager.instance.PlayMove(moveSource, moveSound1);
        }
        else if (rigidBody.velocity.y > 0 && rigidBody.velocity.y < 10){}//making sure to not cancel the jump sound
        else if (Horizontal == 0 && moveSource.isPlaying)
        {
            moveSource.Stop();
        }
        

        rigidBody.velocity = new Vector2(Horizontal * movementSpeed, rigidBody.velocity.y); // adds speed to the right/left according to the player's input
        if (rigidBody.velocity.x > 0.01 || rigidBody.velocity.x < -0.01) 
        {
            FlameLights[0].SetActive(true);
        }
        else
        {
            FlameLights[0].SetActive(false);
        }

        myAnimator.SetFloat("speed", Mathf.Abs(Horizontal));

        myAnimator.SetBool("boostDown", false);//canceling the boost down animation
        FlameLights[1].SetActive(false);
        // checking if the grounded variable is true. if it is, the player is allowed to jump
        if (isGroundedVar)
        {
            isGroundedVar = false;
            if (Input.GetKeyDown(jumpKey))
            {
                SoundManager.instance.PlayMove(moveSource, moveSound1);

                rigidBody.AddForce(new Vector2(0, jumpForce));
                myAnimator.SetBool("jump", true);
                FlameLights[2].SetActive(true);
            }
        }

        else if (!isGroundedVar)
        {
            if (rigidBody.velocity.y < 1.5 && rigidBody.velocity.y > 0 && rigidBody.velocity.x != 0)
            {
                myAnimator.SetBool("jump", false);
                FlameLights[2].SetActive(false);
            }
            else if (rigidBody.velocity.y < 1.5 && rigidBody.velocity.y > 0)
            {
                moveSource.Stop();

                myAnimator.SetBool("jump", false);
                FlameLights[2].SetActive(false);
            }

            myAnimator.SetFloat("speedy", rigidBody.velocity.y);
            myAnimator.SetBool("boostDown", false);
            clickingDown = false;
            FlameLights[1].SetActive(false);

            if (Input.GetKey(boostDownKey))
            {
                if (!moveSource.isPlaying)
                {
                    SoundManager.instance.PlayMove(moveSource, moveSound1);
                }
 
                clickingDown = true;
                myAnimator.SetBool("boostDown", true);
                FlameLights[1].SetActive(true);
            }
        }
    }

    /*
    Running over the groundPoints and checking if one of them is touching a collider
    according to the radius of the points, which is defined as groundRadius (float)
    */
    private bool IsGrounded()
    {
        if (rigidBody.velocity.y < 0.05 && rigidBody.velocity.y > -0.05) // checking that velocity in y == 0 (the player is not moving up/down)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] collider = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < collider.Length; i++)
                {
                    if (collider[i].gameObject != gameObject && (collider[i].gameObject. tag != "Wall" || rigidBody.velocity.y == 0))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){}
}

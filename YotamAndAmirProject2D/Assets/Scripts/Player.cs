using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : Photon.PunBehaviour , IPunObservable
{
    //public Transform mask;

    [SerializeField]
    private GameObject Tutorial;
    [SerializeField]
    private string loadScene;
    public string PlayerMovement;
    public GameObject[] FlameLights;

    [Space]

    [Header("Key Bindings")]
    public KeyCode ShowTutorial;
    public KeyCode restartKey;
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


    private Rigidbody2D rigidBody;
    private Animator myAnimator;

    private bool facingRight;
    [SerializeField]
    private bool isGroundedVar;

    [Header("Stats")]
    public bool damaged;

    private bool TutorialIsShown = false;
    private bool currentPlayer = false;

    // Use this for initialization
    void Start()
    {
        SoundManager.instance.musicSource.Stop();
        SoundManager.instance.RandomizeSfx(BackGroundMusic);
        Tutorial.SetActive(false);
        rigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        facingRight = true;
        if(rigidBody.tag == "Player2")
        {
            currentPlayer = true;
        }
        gameObject.SetActive(true);
        damaged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }
        if (Input.GetKeyDown(ShowTutorial))//KeyCode.R)) // returns to check point
        {
            if(TutorialIsShown)
            {
                //Time.timeScale = 1.0f;
                Tutorial.SetActive(false);
                TutorialIsShown = false;

            }
            else
            {
                // stopping sounds while showiong the instructions
                SoundManager.instance.moveEfxSource1.Stop();
                SoundManager.instance.efxSource1.Stop();
                SoundManager.instance.moveEfxSource2.Stop();
                SoundManager.instance.efxSource2.Stop();

                //Time.timeScale = 0;
                Tutorial.SetActive(true);
                TutorialIsShown = true;
            }
        }
        //else if (Input.GetKeyDown(restartKey))//KeyCode.R)) // returns to check point
        //{
        //    Time.timeScale = 1;
        //    gameObject.GetComponent<GrabHandler>().VictoryScreen.SetActive(false);
        //    ReturnToCheckPoint();
        //}
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (damaged)
        {
            if (!SoundManager.instance.deathEfxSource.isPlaying)//deathEfxSource.isPlaying)
            {
                Time.timeScale = 1;
                ReturnToCheckPoint();
            }
        }
        else
        {
            // checking if the object is mid air, and moving him according to the key he pressed
            isGroundedVar = IsGrounded();

            float horizontal = Input.GetAxis(PlayerMovement);
            
            if (Time.timeScale == 1) // will not be called if time != normal time
            {
                HandleMovement(horizontal);
                ///send location and velocity to playert
                Flip(horizontal);
            }
        }
    }


    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.isWriting)
    //    {
    //        Vector3 pos = transform.localPosition;
    //        stream.Serialize(ref pos);
    //    }
    //    else
    //    {
    //        Vector3 pos = Vector3.zero;
    //        stream.Serialize(ref pos);  // pos gets filled-in. must be used somewhere
    //    }
    //}

    // This function is called when this object touches a different object
    void OnTriggerEnter2D(Collider2D col)
    {
        //SoundManager.instance.efxSource.Stop();
        // Checking if the player got damaged. reseting his position if he did
        if (col.gameObject.tag == "Dangerous" && gameObject.GetComponent<AbilityManager>().Immune == false)
        {
            Time.timeScale = 0f;
            damaged = true;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            SoundManager.instance.moveEfxSource1.Stop();
            SoundManager.instance.efxSource1.Stop();

            SoundManager.instance.moveEfxSource2.Stop();
            SoundManager.instance.efxSource2.Stop();

            SoundManager.instance.musicSource.Stop();

            //SoundManager.instance.musicSource.clip = deathSound;
            //SoundManager.instance.musicSource.volume = 0.5f;
            //SoundManager.instance.efxSource.pitch = 1f;
            SoundManager.instance.PlayDeathEffect(DeathMusic[0]);
            //SoundManager.instance.musicSource.loop = false;
            //SoundManager.instance.RandomizeSfx(DeathMusic);
        }
        else if(col.gameObject.tag == "Dangerous" && gameObject.GetComponent<AbilityManager>().Immune == true)
        {
            gameObject.GetComponent<AbilityManager>().PowerUps[3].SetActive(false); //PowerUps[1].SetActive(false);
            gameObject.GetComponent<AbilityManager>().Immune = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bounce")
        {
            if(!currentPlayer)
            {
                SoundManager.instance.PlayEffect1(bounceSound);
            }
            else
            {
                SoundManager.instance.PlayEffect2(bounceSound);
            }
        }
    }

    /*void OnCollisionStay2D(Collision2D collision) // Doesn't work on the wall...
    {
        if(collision.gameObject.tag == "Friction")
        {
            isGroundedVar = true;
        }
    }*/

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

    private void ReturnToCheckPoint()
    {
        //SceneManager.LoadScene(loadScene);
        PhotonNetwork.LoadLevel(loadScene); // doesn't yet work

        //SceneManager.LoadScene(loadScene, LoadSceneMode.Single); // loading a scene
    }

    private void HandleMovement(float Horizontal)
    {
        if (!currentPlayer)
        {
            if (Horizontal != 0 && !SoundManager.instance.moveEfxSource1.isPlaying)
            {
                SoundManager.instance.PlayMove1(moveSound1);
            }
            else if (rigidBody.velocity.y > 0 && rigidBody.velocity.y < 10)
            {
                //making sure to not cancel the jump sound
            }
            else if (Horizontal == 0 && SoundManager.instance.moveEfxSource1.isPlaying)
            {
                SoundManager.instance.moveEfxSource1.Stop();
            }
        }
        else
        {
            if (Horizontal != 0 && !SoundManager.instance.moveEfxSource2.isPlaying)
            {
                SoundManager.instance.PlayMove2(moveSound1);
            }
            else if (rigidBody.velocity.y > 0 && rigidBody.velocity.y < 10)
            {
                //making sure to not cancel the jump sound
            }
            else if (Horizontal == 0 && SoundManager.instance.moveEfxSource2.isPlaying)
            {
                SoundManager.instance.moveEfxSource2.Stop();
            }
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
            if (Input.GetKeyDown(jumpKey))//KeyCode.W))
            {
                if (!currentPlayer)
                {
                    SoundManager.instance.PlayMove1(moveSound1);
                }
                else
                {
                    SoundManager.instance.PlayMove2(moveSound1);
                }
                rigidBody.AddForce(new Vector2(0, jumpForce));
                myAnimator.SetBool("jump", true);
                FlameLights[2].SetActive(true);

            }
        }
        // force fall (amazing name for the ability - By Amir Weinfeld)
        else if (!isGroundedVar)
        {
            if (rigidBody.velocity.y < 1.5 && rigidBody.velocity.y > 0 && rigidBody.velocity.x != 0)
            {

                myAnimator.SetBool("jump", false);
                FlameLights[2].SetActive(false);
            }
            else if (rigidBody.velocity.y < 1.5 && rigidBody.velocity.y > 0)
            {
                if (!currentPlayer)
                {
                    SoundManager.instance.moveEfxSource1.Stop();
                }
                else
                {
                    SoundManager.instance.moveEfxSource2.Stop();
                }
                myAnimator.SetBool("jump", false);
                FlameLights[2].SetActive(false);
            }

            myAnimator.SetFloat("speedy", rigidBody.velocity.y);
            myAnimator.SetBool("boostDown", false);
            FlameLights[1].SetActive(false);
            if (Input.GetKey(boostDownKey))//KeyCode.S))
            {
                if (!currentPlayer)
                {
                    if (!SoundManager.instance.moveEfxSource1.isPlaying)
                    {
                        SoundManager.instance.PlayMove1(moveSound1);
                    }
                }
                else
                {
                    if (!SoundManager.instance.moveEfxSource2.isPlaying)
                    {
                        SoundManager.instance.PlayMove2(moveSound1);
                    }
                }
                rigidBody.AddForce(new Vector2(0, -jumpForce / 10));
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
        if (rigidBody.velocity.y == 0) // checking that velocity in y == 0 (the player isnt moving)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] collider = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < collider.Length; i++)
                {
                    if (collider[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (photonView.isMine)
        //{
        //    if (stream.isWriting)
        //    {
        //        // We own this player: send the others our data
        //        stream.SendNext(isGroundedVar);
        //        stream.SendNext(damaged);
        //    }
        //}
        //else
        //{
        //    // Network player, receive data
        //    isGroundedVar = (bool)stream.ReceiveNext();
        //    damaged = (bool)stream.ReceiveNext();

        //}
    }
}

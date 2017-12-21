using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private string loadScene;

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
    private LayerMask whatIsGroundMusic; // indication for what is considered ground

    // Audio
    public AudioClip[] BackGroundMusic;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip deathSound;
    public AudioClip bounceSound;

    private Rigidbody2D rigidBody;
    private Animator myAnimator;

    private bool facingRight;
    private bool isGroundedVar;

    private bool damaged;

    // Use this for initialization
    void Start ()
    {
        SoundManager.instance.RandomizeSfx(BackGroundMusic);
        facingRight = true;
        rigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        gameObject.SetActive (true);
        damaged = false;
    }
   
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R)) // returns to check point
        {
            ReturnToCheckPoint();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (damaged)
        {
            if (!SoundManager.instance.deathEfxSource.isPlaying)
            {
                Time.timeScale = 1;
                ReturnToCheckPoint();
            }
        }
        else
        {
            // checking if the object is mid air, and moving him according to the key he pressed
            isGroundedVar = IsGrounded();

            float horizontal = Input.GetAxis("Horizontal");
            HandleMovement(horizontal);
            Flip(horizontal);

        }
    }

    // This function is called when this object touches a different object
    void OnTriggerEnter2D(Collider2D col)
    {
        //SoundManager.instance.efxSource.Stop();
        // Checking if the player got damaged. reseting his position if he did
        if (col.gameObject.tag == "Dangerous")
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            SoundManager.instance.moveEfxSource.Stop();
            SoundManager.instance.efxSource.Stop();
            SoundManager.instance.musicSource.Stop();

            //SoundManager.instance.musicSource.clip = deathSound;
            //SoundManager.instance.musicSource.volume = 0.5f;
            //SoundManager.instance.efxSource.pitch = 1f;
            SoundManager.instance.PlayDeathEffect(deathSound);
            
            Time.timeScale = 0f;
            damaged = true;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bounce")
        {
            SoundManager.instance.PlayEffect(bounceSound);
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

    private void ReturnToCheckPoint()
    {
        SceneManager.LoadScene(loadScene, LoadSceneMode.Single); // loading a scene
    }

    private void HandleMovement(float Horizontal)
    {

        if (Horizontal != 0 && !SoundManager.instance.moveEfxSource.isPlaying)
        {
            SoundManager.instance.PlayMove(moveSound1);
        }
        else if (rigidBody.velocity.y > 0 && rigidBody.velocity.y < 10) 
        {
            //making sure to not cancel the jump sound
        }
        else if(Horizontal == 0 && SoundManager.instance.moveEfxSource.isPlaying)
        {
            SoundManager.instance.moveEfxSource.Stop();
        }

        rigidBody.velocity = new Vector2(Horizontal * movementSpeed, rigidBody.velocity.y); // adds speed to the right/left according to the player's input
        myAnimator.SetFloat("speed", Mathf.Abs(Horizontal));

        myAnimator.SetBool("boostDown", false);//canceling the boost down animation
        // checking if the grounded variable is true. if it is, the player is allowed to jump
        if (isGroundedVar)
        {
            isGroundedVar = false;
            if (Input.GetKeyDown(KeyCode.W))
            {
                SoundManager.instance.PlayMove(moveSound1);

                rigidBody.AddForce(new Vector2(0, jumpForce));
                myAnimator.SetBool("jump", true);
              
            }
        }
        // force fall (amazing name for the ability - By Amir Weinfeld)
        else if (!isGroundedVar)
        {
            if (rigidBody.velocity.y < 1.5 && rigidBody.velocity.y > 0 && rigidBody.velocity.x !=0)
            {
                myAnimator.SetBool("jump", false);
            }
            else if (rigidBody.velocity.y < 1.5 && rigidBody.velocity.y > 0)
            {
                SoundManager.instance.moveEfxSource.Stop();
                myAnimator.SetBool("jump", false);
            }

            myAnimator.SetFloat("speedy", rigidBody.velocity.y);
            myAnimator.SetBool("boostDown", false);
            if (Input.GetKey(KeyCode.S))
            {
                if(!SoundManager.instance.moveEfxSource.isPlaying)
                {
                    SoundManager.instance.PlayMove(moveSound1);
                }
                
                rigidBody.AddForce(new Vector2(0, -jumpForce / 10));
                myAnimator.SetBool("boostDown", true);
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
            foreach(Transform point in groundPoints)
            {
                Collider2D[] collider = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGroundMusic);
                for(int i =0; i<collider.Length; i++)
                {
                    if(collider[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}

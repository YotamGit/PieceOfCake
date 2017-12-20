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
    private LayerMask whatIsGround; // indication for what is considered ground

    public AudioClip BackGround1;
    public AudioClip BackGround2;
    public AudioClip BackGround3;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip deathSound;

    public float deathDelayTime;

    private Rigidbody2D rigidBody;
    private Animator myAnimator;

    private bool facingRight;
    private bool isGroundedVar;

    // Variables for location reset after taking damage
    private bool damaged;
    //private bool diedOnce = false;
    //private Transform pickedObjectLastPlace;
    /*[SerializeField]
    private Vector3 checkPoint;*/


    // Use this for initialization
    void Start ()
    {
        SoundManager.instance.RandomizeSfx(BackGround1,BackGround2,BackGround3);// PlaySingle(deathSound);
        facingRight = true;
        rigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        gameObject.SetActive (true);
	}

   
// Update is called once per frame
    void Update ()
    {
        //  checking if the player got damaged. reseting his position if he did
        if (damaged)
        {
            SoundManager.instance.efxSource1.Stop();
            //SoundManager.instance.efxSource2.Stop();
            SoundManager.instance.efxSource3.Stop();
            SoundManager.instance.musicSource.Stop();
            Time.timeScale = 0f;
            if(!SoundManager.instance.efxSource2.isPlaying)
            {
                Time.timeScale = 1;
                damaged = false;
                ReturnToCheckPoint();

            }
        }
        // checking if the object is mid air, and moving him according to the key he pressed
        isGroundedVar = IsGrounded();

        float horizontal = Input.GetAxis("Horizontal");
        HandleMovement(horizontal);
        Flip(horizontal);
    }

    // This function is called when this object touches a different object
    void OnCollisionEnter2D(Collision2D col)
    {
        //SoundManager.instance.efxSource.Stop();
        if (col.gameObject.tag == "Dangerous")
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            SoundManager.instance.efxSource2.volume = 0.5f;
            SoundManager.instance.PlaySingle2(deathSound);
            damaged = true;
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
        //gameObject.transform.position = checkPoint;
    }

    private void HandleMovement(float Horizontal)
    {

        if (Horizontal != 0 && !SoundManager.instance.efxSource3.isPlaying)
        {
            SoundManager.instance.PlaySingle3(moveSound1);
        }
        else if (rigidBody.velocity.y > 0 && rigidBody.velocity.y < 10) 
        {
            //making sure to not cancel the jump sound
        }
        else if(Horizontal == 0 && SoundManager.instance.efxSource3.isPlaying)
        {
            SoundManager.instance.efxSource3.Stop();
        }

        rigidBody.velocity = new Vector2(Horizontal * movementSpeed, rigidBody.velocity.y); // adds speed to the right/left according to the player's input
        myAnimator.SetFloat("speed", Mathf.Abs(Horizontal));
            
        if (Input.GetKeyDown(KeyCode.R)) // returns to check point
        {
            ReturnToCheckPoint();
        }
        myAnimator.SetBool("boostDown", false);//canceling the boost down animation
        // checking if the grounded variable is true. if it is, the player is allowed to jump
        if (isGroundedVar)
        {
            isGroundedVar = false;
            if (Input.GetKeyDown(KeyCode.W))
            {
                SoundManager.instance.PlaySingle3(moveSound1);

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
                SoundManager.instance.efxSource3.Stop();
                myAnimator.SetBool("jump", false);
            }

            myAnimator.SetFloat("speedy", rigidBody.velocity.y);
            myAnimator.SetBool("boostDown", false);
            if (Input.GetKey(KeyCode.S))
            {
                if(!SoundManager.instance.efxSource3.isPlaying)
                {
                    SoundManager.instance.PlaySingle3(moveSound1);
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
                Collider2D[] collider = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
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

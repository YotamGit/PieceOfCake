using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour {

    [Header("Stats")]
    public bool Resize = false;
    public bool Immune = false;
    public bool Teleport = false;
    public bool SpawnCube = false;
  
    [Header("PowerUps Assets")]
    public GameObject[] PowerUps;

    public Sprite CubeSprite;
    public PhysicsMaterial2D CubeMaterial;
    private GameObject CubeToSpawn;

    [Space]
    [Header("Key Bindings")]
    public KeyCode Ability1Key;
    public KeyCode Ability2Key;
    public KeyCode Ability3Key;
    public KeyCode Ability4Key;

    private GameObject otherPlayer;

    // Use this for initialization
    void Start ()
    {
        
        for (int i = 0;i<PowerUps.Length;i++)
        {
            PowerUps[i].SetActive(false);
        }
    }

    IEnumerator ResizeTimer()
    {
        yield return new WaitForSeconds(2.1f);
        gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5F);
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1F);
        yield return new WaitForSeconds(0.25f);
        gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5F);


        yield return new WaitForSeconds(0.20f);
        gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1F);
        yield return new WaitForSeconds(0.15f);
        gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5F);

        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1F);
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5F);
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1F);
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5F);
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1F);
        yield return new WaitForSeconds(0.05f);
        gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5F);

        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * 2, gameObject.transform.localScale.y * 2, gameObject.transform.localScale.z);

        gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1F);

        //gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x / 2, gameObject.transform.localScale.y / 2, gameObject.transform.localScale.z);
    }

    // Update is called once per frame
    void Update ()
    {
        if (Resize)
        {
            PowerUps[0].SetActive(true);
            Debug.Log("ActivatePowerUp");
        }
        if (Immune)
        {
            PowerUps[1].SetActive(true);
            Debug.Log("ActivatePowerUp");

        }
        if (Teleport)
        {
            PowerUps[2].SetActive(true);
            Debug.Log("ActivatePowerUp");

        }
        if (SpawnCube)
        {
            PowerUps[3].SetActive(true);
            Debug.Log("ActivatePowerUp");

        }
        if (Input.GetKey(Ability1Key))
        {
            if (Resize)
            {
                Resize = false;
                ResizePowerUp(); 

            }
        }
        else if (Input.GetKey(Ability3Key))//grants a 1 time dagame immunity
        {
            if (Teleport)
            {
                Teleport = false;
                TeleportPowerUp();
            }
        }
        else if(Input.GetKey(Ability4Key))
        {
            if (SpawnCube)
            {
                SpawnCube = false;
                SpawnCubePowerUp();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Resize")//resizes player for 5 seconds
        {
            Resize = true;
            col.gameObject.SetActive(false);
        }
        else if (col.gameObject.tag == "DamageImmune")//grants a 1 time dagame immunity
        {
            Immune = true;
            col.gameObject.SetActive(false);
        }
        else if (col.gameObject.tag == "Teleport")//grants a 1 time dagame immunity
        {
            Teleport = true;
            col.gameObject.SetActive(false);
        }
        else if (col.gameObject.tag == "SpawnCube")
        {
            SpawnCube = true;
            col.gameObject.SetActive(false);
        }
    }


    void ResizePowerUp()
    {
        PowerUps[0].SetActive(false);
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x / 2, gameObject.transform.localScale.y / 2, gameObject.transform.localScale.z);
        StartCoroutine(ResizeTimer());
    }

    void TeleportPowerUp()
    {
        PowerUps[2].SetActive(false);

        if (otherPlayer == null)
        {
            if (this.tag == "Player1")
            {
                otherPlayer = GameObject.Find("Player2(Clone)");
            }
            else
            {
                otherPlayer = GameObject.Find("Player1(Clone)");
            }
        }
        gameObject.transform.position = otherPlayer.transform.position;

        /*if (gameObject.tag == "Player1(Clone)")
        {
            gameObject.transform.position = GameObject.Find("Player2(Clone)").transform.position;
        }
        else
        {
            gameObject.transform.position = GameObject.Find("Player1(Clone)").transform.position;
        }*/
    }

    void SpawnCubePowerUp()
    {
        PowerUps[3].SetActive(false);
        //CubeToSpawn = new GameObject("Cube")
        //{
        //    //Add Components
        //    tag = "Cube"
        //};
        //CubeToSpawn.transform.position = gameObject.transform.position;
        //CubeToSpawn.AddComponent<Rigidbody2D>();
        //CubeToSpawn.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        //CubeToSpawn.AddComponent<SpriteRenderer>();
        //CubeToSpawn.GetComponent<SpriteRenderer>().sprite = CubeSprite;
        //CubeToSpawn.AddComponent<BoxCollider2D>();
        //CubeToSpawn.GetComponent<BoxCollider2D>().sharedMaterial = CubeMaterial;
        PhotonNetwork.Instantiate("Cube", gameObject.transform.position, Quaternion.identity, 0);
    }

}


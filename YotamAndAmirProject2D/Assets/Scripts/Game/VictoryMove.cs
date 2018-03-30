using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryMove : MonoBehaviour {
        
    private int curPosIndex = 0; // this holds the current index of our place in the position list (0 == starting point)
    [SerializeField]
    private Vector3[] positions;
    
    private bool goToNextPos = false;
    private Vector3 nextPos;

    [SerializeField]
    private float smoothness, distance; // distance is to check if the victory is close enough to the next pos

    PhotonView photonView;

    private void Start()
    {
        photonView = PhotonView.Get(this);

        if (!PhotonNetwork.isMasterClient)
        {
            photonView.RPC("GetCurrVictoryPos", PhotonTargets.MasterClient);
        }
    }

    private void Update ()
    {
        if (goToNextPos)
        {
            transform.position = Vector3.Lerp(transform.position, positions[curPosIndex], Time.deltaTime * smoothness); // lerping to the next pos
            
            if(Vector3.Distance(transform.position, positions[curPosIndex]) < distance) // if we reached the next pos we're stoping the lerp
            {
                goToNextPos = false;
            }
        }
    }

    // if a player got close to the object, and we're not at the last pos the func will tell the Update to lerp to next pos
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!goToNextPos) // if not on the move
        {
            if (collision.gameObject.tag.Substring(0, 6) == "Player" && curPosIndex < positions.Length - 1)
            {
                curPosIndex++;
                goToNextPos = true; // telling the obj to lerp to the other pos
            }
        }
    }

    // telling the other player what is the position of the cake currently
    [PunRPC]
    private void GetCurrVictoryPos()
    {
        photonView.RPC("GetCurrVictoryPos", PhotonTargets.Others, curPosIndex);
    }

    // setting the current victory index and pos with the index we got (using the list)
    [PunRPC]
    private void GetCurrVictoryPos(int recPosIndex) 
    {
        if (!PhotonNetwork.isMasterClient)
        {
            curPosIndex = recPosIndex;
            transform.position = positions[curPosIndex];
        }
    }
}

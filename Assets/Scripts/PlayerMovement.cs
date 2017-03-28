using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PlayerMovement : Photon.PunBehaviour {

    public float speed;

    public bool canMove = true;

    private GameObject Scripts;

    public LayerMask WallLayer;

    public float RayCastLength = 0.1f;

    private GameManager Gm;

    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        Scripts = GameObject.FindGameObjectWithTag("GameController");
        Gm = Scripts.GetComponent<GameManager>();
    }

    void Start()
    {
        if ( PhotonNetwork.isMasterClient)
        {
            
        }else
        {
            speed = speed * 0.75f;
        }
    }
    
    // Update is called once per frame
    void Update () {

        if (Gm.delayedControl)
        {
            return;
        }

        Vector2 addTrans = new Vector2(0, 0);
        Vector2 CurPos = GetComponent<Transform>().position;


        if(Input.GetKey(KeyCode.D))
        {
            addTrans.x += speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A))
        {
            addTrans.x -= speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.W))
        {
            addTrans.y += speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S))
        {
            addTrans.y -= speed * Time.deltaTime;
        }

        bool nextBlockIsWall = Physics2D.Raycast(CurPos, addTrans.normalized, RayCastLength, WallLayer);

        if ( !nextBlockIsWall && (PhotonNetwork.playerList.Length == 2))
        {
            //if ( (Gm.CanServerControl && PhotonNetwork.isMasterClient) || (Gm.CanClientControl && !PhotonNetwork.isMasterClient ))
            if (!Gm.delayedControl)
                transform.position = new Vector3(CurPos.x + addTrans.x, CurPos.y + addTrans.y, 0);
            
        }else if ( nextBlockIsWall)
        {
            //GetComponent<AudioSource>().Play();
        }

        //Debug.Log(Gm.AmIClient);
        //Debug.Log(Gm.AmIServer);
        //Debug.Log(Gm.CanClientControl);
        //Debug.Log(Gm.CanServerControl);
        
        //if ( ( Gm.AmIServer && Gm.CanServerControl ) || ( Gm.AmIClient && Gm.CanClientControl))
        //{
            //transform.Translate(addTrans);
        //}
            

    }
}

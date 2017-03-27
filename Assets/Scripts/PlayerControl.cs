using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PlayerControl : PunBehaviour {

    public int control;

    public static PlayerControl instance = null;

    public float timer;

    public bool changeOwner = false;

    public GameObject player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject.transform);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start() {
        control = 1;
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;
        if ( timer > 10.0f && isServerRunning())
        {
            clientCanRun();
        }else if ( timer > 10.0f && isClientRunning())
        {
            serverCanRun();
        }
    }

    bool isServerRunning()
    {
        if ( player.GetComponent<PhotonView>().ownerId == 1)
        {
            return true;
        }else
        {
            return false;
        }
    }

    bool isClientRunning()
    {
        if (player.GetComponent<PhotonView>().ownerId == 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void clientCanRun()
    {
        control = 2;
        //player.GetComponent<PhotonView>()
        timer = 0.0f;
    }

    void serverCanRun()
    {
        control = 1;
        timer = 0.0f;
    }
}

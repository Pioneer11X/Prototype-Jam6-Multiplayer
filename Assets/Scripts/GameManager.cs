using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class GameManager : PunBehaviour {

    public static GameManager gm;

    public GameObject Player;

    public Camera MainCamera;

    public bool CanServerControl = false;
    public bool CanClientControl = false;

    public bool AmIServer = PhotonNetwork.isMasterClient;
    public bool AmIClient = !PhotonNetwork.isMasterClient;

    public GameObject BlackScreen;

    public Transform BlackScreenInvisPos;
    public Transform BlackScreenVisPos;

    public float RoundTime = 60.0f;

    public string LevelObjTag;

    public int MaxLevels;

    private PhotonView pv;

    private float timer = 0.0f;
    private float completeTimer = 0.0f;

    private GameObject curLevelObj;

    public int curLevel;
    private GameObject level;

    private bool waitingForSecondPlayer = true;

    private float delayControls;
    private float delayCounter = 0;
    public bool delayedControl = false;

    //private void CacheLevels()
    //{
    //    //GameObject[] Puzzles = new GameObject[MaxLevels];
    //    Puzzles = new GameObject[MaxLevels];
    //    string temp;
    //    for ( int i = 1; i <= MaxLevels; i++)
    //    {
    //        temp = "Puzzle_" + i.ToString();
    //        Puzzles[i-1] = GameObject.Find(temp);
    //    }
    //}

    private void Awake()
    {
        if ( gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gm.transform);
        }else if ( gm != this)
        {
            Destroy(gm);
        }

        GameObject[] temp = GameObject.FindGameObjectsWithTag(LevelObjTag);
        for ( int i = 0; i < temp.Length; i++)
        {
            if (temp[i].GetActive())
            {
                curLevelObj = temp[i];
            }
        }
        //CacheLevels();
        delayControls = 0.25f;
    }

    void InitLevel()
    {
        if ( CheckIf2Players() && waitingForSecondPlayer)
        {
            waitingForSecondPlayer = false;
            if (PhotonNetwork.isMasterClient)
            {
                level = PhotonNetwork.Instantiate("Puzzle_1", transform.position, transform.rotation, 0);
                curLevel = 1;
                Player.GetComponent<Transform>().position = level.GetComponent<Transform>().FindChild("PlayerSpawnPoint").GetComponent<Transform>().position;
                level.GetComponent<Transform>().FindChild("LevelEndObj").GetComponent<LevelEnd>().enabled = true;
            }
        }
    }

    private void Start()
    {
        completeTimer = RoundTime;
        BlackScreen.GetComponent<Transform>().position = BlackScreenInvisPos.position;
        pv = Player.GetComponent<PhotonView>();
        CanServerControl = true;
        UpdateOwner();
    }

    private void Update()
    {

        if ( !CheckIf2Players())
        {
            // If you are in the room without the other person then you are the server.
            completeTimer = RoundTime;
            return;
        }

        if ( waitingForSecondPlayer )
            InitLevel();

        if ( delayedControl )
        {
            delayCounter += Time.deltaTime;
        }
        if ( delayCounter >= delayControls)
        {
            delayCounter = 0;
            delayedControl = false;
        }

        completeTimer -= 0.0f;

        if ( completeTimer <= 0.0f)
        {
            completeTimer = RoundTime;
            LevelTimedOut();
        }

        // If we do have two players in.
        timer += Time.deltaTime;
        if ( timer > 15.0f)
        {
            timer = 0.0f;
            CanClientControl = !CanClientControl;
            CanServerControl = !CanServerControl;
            UpdateOwner();
        }

        if ( pv.isMine)
        {
            BlackScreen.GetComponent<Transform>().position = BlackScreenVisPos.position;
        }else
        {
            BlackScreen.GetComponent<Transform>().position = BlackScreenInvisPos.position;
        }
        
    }

    void UpdateOwner()
    {
        if ( !pv.isMine)
        {
            int whose = pv.ownerId;

            if (whose == 2 && CanServerControl)
            {
                pv.RequestOwnership();
            }
            else if (whose == 1 && CanServerControl)
            {
                return;
            }
            else if (whose == 2 && CanClientControl)
            {
                return;
            }
            else if (whose == 1 && CanClientControl)
            {
                pv.RequestOwnership();
            }
            
        }
        else
        {
            
        }
    }

    bool CheckIf2Players()
    {
        int count = PhotonNetwork.playerList.Length;
        if ( count == 2)
        {
            return true;
        }else
        {
            return false;
        }
    }

    public void LevelEnded(int _curLevel)
    {
        if ( PhotonNetwork.isMasterClient)
        {
            // Server tries to get out.
            Debug.Log("You Won");
        }else if ( AmIClient)
        {
            Debug.Log("You Lose");
        }else
        {
            Debug.Log("WutFace");
        }
        //Debug.Log(_curLevel);
        //Debug.Log(Puzzles);

        if (PhotonNetwork.isMasterClient)
        {
            //if (!Puzzles[_curLevel - 1].GetComponent<PhotonView>().isMine)
            //{
            //    Puzzles[_curLevel - 1].GetComponent<PhotonView>().RequestOwnership();
            //}
            //Puzzles[_curLevel - 1].SetActive(false);
            //if (_curLevel < Puzzles.Length)
            //{
            //    Puzzles[_curLevel].SetActive(true);
            //    Player.GetComponent<Transform>().position = Puzzles[_curLevel].GetComponent<Transform>().FindChild("PlayerSpawnPoint").GetComponent<Transform>().position;
            //}
            //Debug.Log(level);
            //Destroy(level);
            PhotonNetwork.Destroy(level);
            curLevel += 1;
            if ( curLevel > MaxLevels)
            {
                // Do something.
            }
            level = PhotonNetwork.Instantiate("Puzzle_" + curLevel.ToString(), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), 0);
            
            Player.GetComponent<Transform>().position = level.GetComponent<Transform>().FindChild("PlayerSpawnPoint").GetComponent<Transform>().position;
            //Debug.Log(level.GetComponent<Transform>().FindChild("PlayerSpawnPoint").GetComponent<Transform>().position);
        }
    }

    public void LevelTimedOut()
    {
        int currentLevel = curLevelObj.GetComponent<LevelEnd>().GetCurrentLevel();
        if ( AmIClient)
        {
            Debug.Log("You wom");
        }else if ( AmIServer)
        {
            Debug.Log("You Lost");
        }else
        {
            Debug.Log("Jebaited");
        }
        LevelEnded(currentLevel);
    }

    private void OnGUI()
    {
        GUILayout.Label("Time Remaining: " + completeTimer.ToString());
    }

}

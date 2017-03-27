using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonNetManager : Photon.PunBehaviour {


    public static PhotonNetManager instance = null;

    public InputField RoomName;

    public GameObject RoomPrefab;

    public bool CanServerControl;
    public bool CanClientControl;

    private List<GameObject> RoomPrefabs = new List<GameObject>();

    void Awake()
    {
        if ( instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject.transform);
        }else if ( instance != this)
        {
            Destroy(gameObject);
        }
    }



    public void ButtonEvents(string Event)
    {
        switch (Event)
        {
            case "CreateRoom":
                if (PhotonNetwork.JoinLobby()) {
                    RoomOptions RO = new RoomOptions();
                    RO.MaxPlayers = byte.Parse("2");
                    PhotonNetwork.CreateRoom(RoomName.text, RO, TypedLobby.Default);
                }
                break;
            case "Refresh":
                if (PhotonNetwork.JoinLobby())
                {
                    RefreshRoomsList();
                }
                break;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    // Use this for initialization
    void Start () {
        PhotonNetwork.ConnectUsingSettings("0.1f");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnJoinedLobby()
    {
        Invoke("RefreshRoomsList", 0.1f);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SceneManager.LoadScene("Game");

    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        base.OnPhotonJoinRoomFailed(codeAndMsg);
    }

    void RefreshRoomsList()
    {
        if (RoomPrefabs.Count > 0)
        {
            for (int i = 0; i < RoomPrefabs.Count; i++)
            {
                Destroy(RoomPrefabs[i]);
            }
            RoomPrefabs.Clear();
        }

        for (int i = 0; i < PhotonNetwork.GetRoomList().Length; )
        {
            //Debug.Log(PhotonNetwork.GetRoomList()[i].name);
            GameObject temp_room = Instantiate(RoomPrefab);
            temp_room.transform.SetParent(RoomPrefab.transform.parent);
            temp_room.GetComponent<RectTransform>().localScale = RoomPrefab.GetComponent<RectTransform>().localScale;
            Vector3 tempos = RoomPrefab.GetComponent<RectTransform>().position;
            temp_room.GetComponent<RectTransform>().position = new Vector3(tempos.x, tempos.y - 15 * i, tempos.z);
            temp_room.transform.FindChild("Room_Name_Text").GetComponent<Text>().text = PhotonNetwork.GetRoomList()[i].name;
            temp_room.transform.FindChild("Player_Count").GetComponent<Text>().text = PhotonNetwork.GetRoomList()[i].playerCount.ToString() + "/2";
            temp_room.SetActive(true);
            Debug.Log(PhotonNetwork.GetRoomList()[i].name.ToString());
            temp_room.transform.FindChild("Join").GetComponent<Button>().onClick.AddListener(
                () => {
                    JoinRoom(
                        temp_room.transform.FindChild("Room_Name_Text").GetComponent<Text>().text
                        );
                }
                );
            RoomPrefabs.Add(temp_room);

            i++;
        }
    }

    void JoinRoom(string _RoomName)
    {
        PhotonNetwork.JoinRoom(_RoomName);
    }

    void OnPhotonJoinRoomFailed()
    {
        // Connecting failed.
        Debug.Log("It should have never reached this");
    }
}



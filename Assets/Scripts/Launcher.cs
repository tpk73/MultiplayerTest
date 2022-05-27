using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomnameIF;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomText;
    [SerializeField] Transform roomlistContent;
    [SerializeField] Transform playerlistContent;
    [SerializeField] GameObject roomlistitemPrefab;
    [SerializeField] GameObject playerlistitemPrefab;
    [SerializeField] GameObject startGameButton;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        //base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomnameIF.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomnameIF.text);
        MenuManager.Instance.OpenMenu("loading");
        Debug.Log("Room Created");
    }
    public override void OnJoinedRoom()
    {
        roomText.text = PhotonNetwork.CurrentRoom.Name;
        MenuManager.Instance.OpenMenu("room");
        Debug.Log("Joined Room");

        Player[] players = PhotonNetwork.PlayerList;

        foreach(Transform child in playerlistContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerlistitemPrefab, playerlistContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message; 
        MenuManager.Instance.OpenMenu("error");
        Debug.Log("Room Failed to Create");
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public void LeaveRoom()
    {
        Debug.Log("Left Room");
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }
    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomlist)
    {
        foreach (Transform trans in roomlistContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomlist.Count; i++)
        {
            if(roomlist[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomlistitemPrefab, roomlistContent).GetComponent<RoomListItem>().SetUp(roomlist[i]);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerlistitemPrefab, playerlistContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}

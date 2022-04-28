using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomnameIF;

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
    }
    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined Lobby");
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

        Debug.Log("Joined Room");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Room Failed to Create");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

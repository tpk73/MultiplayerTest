using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomnameIF;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomText;

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
        roomText.text = PhotonNetwork.CurrentRoom.Name;
        MenuManager.Instance.OpenMenu("room");
        Debug.Log("Joined Room");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed: " + message; 
        MenuManager.Instance.OpenMenu("error");
        Debug.Log("Room Failed to Create");
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
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

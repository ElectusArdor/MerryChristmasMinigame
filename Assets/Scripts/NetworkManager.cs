using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using StarterAssets;
using Cinemachine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject playerSample;

    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject playerContainer;
    [SerializeField] private GameManager GM;
    [SerializeField] private ViewManager VM;
    [SerializeField] private CinemachineVirtualCamera CVC;

    private RoomOptions options = new RoomOptions {MaxPlayers = 5};

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom(roomName: "ChristmasTree", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        int id = PhotonNetwork.LocalPlayer.ActorNumber;

        if (id > spawnPoints.Count)
        {
            Debug.Log("Room is full");
        }
        else
        {
            GameObject player;
            player = PhotonNetwork.Instantiate(playerSample.name, spawnPoints[id - 1].position, Quaternion.identity);
            player.transform.SetParent(playerContainer.transform, false);
            player.transform.position = spawnPoints[id - 1].position;
            VM.player = player;
            GM.player = player;
            GM.input = player.GetComponent<StarterAssetsInputs>();
            CVC.Follow = player.transform.GetChild(0).transform;
        }
    }

    public void UpdateRoomVariables(bool started, bool treeOn, bool restart)
    {
        this.photonView.RPC(methodName: "UpdateRoomState", RpcTarget.All, started, treeOn, restart);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(this);
    }

    [PunRPC]
    private void UpdateRoomState(bool started, bool treeOn, bool restart)
    {
        GM.started = started;
        GM.treeOn = treeOn;
        GM.restart = restart;
    }
}

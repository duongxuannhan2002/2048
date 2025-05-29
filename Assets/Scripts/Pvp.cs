using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class Pvp : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] GameObject MenuPanel;
    [SerializeField] GameObject JoinRoomPanel;
    [SerializeField] GameObject InRoomPanel;
    [SerializeField] GameObject StartButton;

    [SerializeField] TMP_InputField roomIdInput;
    int idRoom;
    TextMeshProUGUI player1Text;
    TextMeshProUGUI player2Text;
    private bool isReady = false;
    void Start()
    {
        PhotonNetwork.NickName = "Player_" + Random.Range(1000, 9999);
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
    }

    // Update is called once per frame
    public void OnCreateRoomButtonClicked()
    {
        if (!isReady)
        {
            NotificationManager.Instance.Show("Đang kết nối đến server, vui lòng thử lại", 3f);
            return;
        }
        idRoom = Random.Range(1000, 9999);

        RoomOptions options = new RoomOptions();
        options.PlayerTtl = 0;
        options.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(idRoom.ToString(), options);
    }

    public void OnJoinRoomButtonClicked()
    {
        if (!isReady)
        {
            NotificationManager.Instance.Show("Đang kết nối đến server, vui lòng thử lại", 3f);
            return;
        }
        string roomId = roomIdInput.text;

        if (!string.IsNullOrEmpty(roomId))
        {
            PhotonNetwork.JoinRoom(roomId);
        }
        else
        {
            NotificationManager.Instance.Show("Vui lòng nhập ID phòng.", 3f);
        }
    }

    public override void OnCreatedRoom()
    {
        MenuPanel.SetActive(false);
        InRoomPanel.SetActive(true);
        TextMeshProUGUI nameRoom = InRoomPanel.transform.Find("IdRoom").GetComponent<TextMeshProUGUI>();
        nameRoom.SetText(idRoom.ToString());
        CachePlayerTextUI();
        UpdatePlayerNames();
    }

    public override void OnJoinedRoom()
    {
        JoinRoomPanel.SetActive(false);
        InRoomPanel.SetActive(true);

        TextMeshProUGUI nameRoom = InRoomPanel.transform.Find("IdRoom").GetComponent<TextMeshProUGUI>();
        nameRoom.SetText(PhotonNetwork.CurrentRoom.Name);

        CachePlayerTextUI();
        UpdatePlayerNames();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerNames();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerNames();
    }
    void CachePlayerTextUI()
    {
        if (player1Text == null || player2Text == null)
        {
            player1Text = InRoomPanel?.transform.Find("Player1").GetComponent<TextMeshProUGUI>();
            player2Text = InRoomPanel?.transform.Find("Player2").GetComponent<TextMeshProUGUI>();
        }
    }
    void UpdatePlayerNames()
    {
        CachePlayerTextUI();

        Player[] players = PhotonNetwork.PlayerList;

        if (players.Length > 0)
            player1Text.SetText(players[0].NickName);

        if (players.Length > 1)
            player2Text.SetText(players[1].NickName);

        if (players.Length < 2)
            player2Text.SetText("...");
        if (PhotonNetwork.IsMasterClient && players.Length == 2)
        {
            StartButton.SetActive(true);
        }
        else
        {
            StartButton.SetActive(false);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        NotificationManager.Instance.Show("Tạo phòng thất bại", 3f);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        NotificationManager.Instance.Show("Vào phòng thất bại: ", 3f);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Đã vào lobby.");
        isReady = true;
    }
    public void ClickJoinRoom()
    {
        MenuPanel.SetActive(false);
        JoinRoomPanel.SetActive(true);
    }
    public override void OnLeftRoom()
    {
        Debug.Log("✅ Đã rời khỏi phòng - chuyển scene PvP");
        SceneManager.LoadScene(4); // Load scene PvP sau khi rời phòng
    }
    public void OnStartGameClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false; // đóng phòng
            PhotonNetwork.CurrentRoom.IsVisible = false; // ẩn khỏi danh sách phòng
            PhotonNetwork.LoadLevel(2); // chuyển scene cho tất cả
        }
    }
}

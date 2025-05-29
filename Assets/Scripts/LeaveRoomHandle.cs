using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LeaveRoomHandle : MonoBehaviourPunCallbacks
{
    public override void OnLeftRoom()
    {
        Debug.Log("✅ Đã rời khỏi phòng - chuyển scene PvP");
        SceneManager.LoadScene(4); // Load scene PvP sau khi rời phòng
    }
}
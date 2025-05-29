using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class HandleDisconnect : MonoBehaviourPunCallbacks
{
    private bool hasHandled = false;

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("🔌 OnDisconnected được gọi: " + cause);
        NotificationManager.Instance.Show("Mất kết nối mạng", 3f);
        StartCoroutine(HandleDisconnection());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Chỉ xử lý khi đang ở màn hình game
        if (SceneManager.GetActiveScene().buildIndex == 2) // Thay "GameScene" bằng tên scene game của bạn
        {
            NotificationManager.Instance.Show("Đối phương rời đi. Trở về menu chính", 3f);
            StartCoroutine(HandleDisconnection());
        }
    }
    private void Update()
    {
        // Phát hiện sớm nếu Photon mất kết nối hoặc mạng không khả dụng
        if (PlayerPrefs.GetString("mode") == "pvp")
        {
            if ((!PhotonNetwork.IsConnected || Application.internetReachability == NetworkReachability.NotReachable) && !hasHandled)
            {
                NotificationManager.Instance.Show("Mất kết nối mạng ở đây", 3f);
                StartCoroutine(HandleDisconnection());
            }
        }
    }

    IEnumerator HandleDisconnection()
    {
        yield return new WaitForSeconds(2f);
        AfterWait();
    }

    private void AfterWait()
    {
        if (hasHandled) return;
        hasHandled = true;

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0); // đổi tên scene tùy bạn
    }
}
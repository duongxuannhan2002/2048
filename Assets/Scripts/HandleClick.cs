using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HandleClick : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void clickBoom()
    {
        if (!GameController.isBoom)
        {
            int count = 0;
            foreach (Cell2048 cell in GameController.instance.allCells)
            {
                if (cell == null) continue;
                if (cell.fill != null) count++;
                if (count > 1) break;
            }
            if (count < 2)
            {
                NotificationManager.Instance.Show("Không thể thực hiện", 3f);
                return;
            }
            else
            {
                GameController.isBoom = !GameController.isBoom;
                return;
            }
        }
        GameController.isBoom = !GameController.isBoom;
    }
    public void ClickPause()
    {
        if (!SetUIGamePanel.Instance.pausePanel.activeSelf)
        {
            GameController.instance.isPlaying = false;
            SetUIGamePanel.Instance.pausePanel.SetActive(true);
            if (PlayerPrefs.GetString("mode") == "pvp")
            {
                if (!PhotonNetwork.IsMasterClient)
                {
                    Transform panel = SetUIGamePanel.Instance.pausePanel.transform.Find("Panel");
                    Transform restartButton = panel?.Find("ButtonRestart");
                    restartButton.gameObject.SetActive(false);
                }
            }
            SetColorMusic();
            SetColorSound();
            SetUIGamePanel.Instance.pausePanel.transform.localScale = Vector3.zero;
            LeanTween.scale(SetUIGamePanel.Instance.pausePanel, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack);
        }
        else
        {
            GameController.instance.isPlaying = true;
            LeanTween.scale(SetUIGamePanel.Instance.pausePanel, Vector3.zero, 0.3f)
        .setEase(LeanTweenType.easeInBack)
        .setOnComplete(() => SetUIGamePanel.Instance.pausePanel.SetActive(false));
        }
    }
    public void ClickUndo()
    {
        if (GameController.instance.isPlaying)
        {
            if (!GameController.instance.canUndo)
            {
                NotificationManager.Instance.Show("Không thể thực hiện", 3f);
                return;
            }
            foreach (Cell2048 cell in GameController.instance.allCells)
            {
                if (cell == null) continue;
                cell.undo();
            }
            Database.instance.updateUndo(-1);
        }
    }

    public void ClickMap()
    {
        if (PlayerPrefs.GetString("mode") == "challent") SceneManager.LoadScene(1);
        if (PlayerPrefs.GetString("mode") == "classic") SceneManager.LoadScene(3);
        if (PlayerPrefs.GetString("mode") == "pvp")
        {
            PhotonNetwork.LeaveRoom();
            // SceneManager.LoadScene(4);
        }
    }


    public void ClickNextMap()
    {
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        SceneManager.LoadScene(2);
    }

    public void ClickRestart()
    {
        if (PlayerPrefs.GetString("mode") == "pvp")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("OnRestart", RpcTarget.Others);
                // PhotonNetwork.LoadLevel(2);
            }
        }
        SceneManager.LoadScene(2);
    }
    public void OnMusicButtonClicked()
    {
        SoundManager.ToggleMusic();

        SetColorMusic();
    }

    public void SetColorMusic()
    {
        Transform btn = SetUIGamePanel.Instance.pausePanel.transform.Find("ButtonMusic");
        if (btn != null)
        {
            Image img = btn.GetComponent<Image>();
            if (img != null)
            {
                if (SoundManager.isMusicOn) img.color = Color.white;
                else img.color = Color.red;
            }
        }
    }
    public void SetColorSound()
    {
        Transform btn = SetUIGamePanel.Instance.pausePanel.transform.Find("ButtonSound");
        if (btn != null)
        {
            Image img = btn.GetComponent<Image>();
            if (img != null)
            {
                if (SoundManager.isSfxOn) img.color = Color.white;
                else img.color = Color.red;
            }
        }
    }

    public void OnSfxButtonClicked()
    {
        SoundManager.ToggleSFX();
        SetColorSound();
    }
    public void ClickShop()
    {
        Vector3 tranLocal = SetUIGamePanel.Instance.shopPanel.transform.localScale;
        if (!SetUIGamePanel.Instance.shopPanel.activeSelf)
        {

            SetUIGamePanel.Instance.shopPanel.SetActive(true);
            SetUIGamePanel.Instance.shopPanel.transform.localScale = Vector3.zero;
            LeanTween.scale(SetUIGamePanel.Instance.shopPanel, tranLocal, 0.3f).setEase(LeanTweenType.easeOutBack);
        }
        else
        {
            LeanTween.scale(SetUIGamePanel.Instance.shopPanel, Vector3.zero, 0.3f)
        .setEase(LeanTweenType.easeInBack)
        .setOnComplete(() =>
        {
            SetUIGamePanel.Instance.shopPanel.SetActive(false);
            SetUIGamePanel.Instance.shopPanel.transform.localScale = tranLocal;
        }
        );
        }
    }

    public void ClickBuy1Boom()
    {
        if (Database.instance.getMoney() >= 2)
        {
            Database.instance.updateMoney(-2);
            Database.instance.updateBoom(1);
            NotificationManager.Instance.Show("Mua thành công", 3f);
        }
        else
        {
            NotificationManager.Instance.Show("Không đủ tiền", 3f);
        }
    }

    public void ClickBuy3Boom()
    {
        Debug.Log("hello");
        if (Database.instance.getMoney() >= 5)
        {
            Database.instance.updateMoney(-5);
            Database.instance.updateBoom(3);
            NotificationManager.Instance.Show("Mua thành công", 3f);
        }
        else
        {
            NotificationManager.Instance.Show("Không đủ tiền", 3f);
        }
    }

    public void ClickBuy1Undo()
    {
        Debug.Log("hello");
        if (Database.instance.getMoney() >= 2)
        {
            Database.instance.updateMoney(-2);
            Database.instance.updateUndo(1);
            NotificationManager.Instance.Show("Mua thành công", 3f);
        }
        else
        {
            NotificationManager.Instance.Show("Không đủ tiền", 3f);
        }
    }

    public void ClickBuy3Undo()
    {
        Debug.Log("hello");
        if (Database.instance.getMoney() >= 5)
        {
            Database.instance.updateMoney(-5);
            Database.instance.updateUndo(3);
            NotificationManager.Instance.Show("Mua thành công", 3f);
        }
        else
        {
            NotificationManager.Instance.Show("Không đủ tiền", 3f);
        }
    }
}

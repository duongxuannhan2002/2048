using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class Fill2048 : MonoBehaviour
{
    public int value;
    [SerializeField] TextMeshProUGUI valueDisplay;
    [SerializeField] float speed;
    bool hasCombine;
    Image myImage;

    // Hàm này gọi khi 2 ô ghép lại
    
    public void FillValueUpdate(int valueIn)
    {
        value = valueIn;
        valueDisplay.text = value.ToString();
        int colorIndex = GetColorIndex(value);
        // Debug.Log(colorIndex);
        myImage = GetComponent<Image>();
        Sprite newColor = GameController.instance.fillColors[colorIndex];
        // newColor.a = 1f;
        myImage.sprite = newColor;
        // myImage.color = GameController.instance.fillColors[colorIndex];
    }

    int GetColorIndex(int valueIn)
    {
        int index = 0;
        while (valueIn != 1)
        {
            index++;
            valueIn /= 2;
        }
        index--;
        return index;
    }
    private void Update()
    {

        if (transform.localPosition != Vector3.zero)
        {
            hasCombine = false;

            transform.localPosition =
            Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
        }
        else if (hasCombine == false)
        {
            if (transform.parent.GetChild(0) != this.transform)
            {
                Destroy(transform.parent.GetChild(0).gameObject);
            }
            hasCombine = true;
        }
    }
    public void Double()
    {
        value *= 2;
        ShowImportEffect();
        SoundManager.Instance.PlayMergeSound();
        FillValueUpdate(value);
        if (PlayerPrefs.GetString("mode") == "classic")
        {
            GameController.instance.GetScore(value);
        }
        // valueDisplay.text = value.ToString();
        if (PlayerPrefs.GetString("mode") == "challent")
        {
            GameController.instance.checkWin(value);
        }
        if (PlayerPrefs.GetString("mode") == "pvp")
        {
            if (value == 8)
            {
                GameController.instance.photonView.RPC("OnReached", RpcTarget.Others);
            }
            if (value == 16)
            {
                GameController.instance.photonView.RPC("OnReachedWin", RpcTarget.Others);
                SetUIGamePanel.Instance.DisplayWinPannel();
            }
        }
    }
    public void ShowSpawnEffect()
    {
        // Đặt kích thước ban đầu nhỏ và xoay
        transform.localScale = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 0, 180); // Xoay 180 độ

        // Dùng LeanTween để xoay về 0 độ và phóng to từ nhỏ đến 1
        LeanTween.scale(gameObject, Vector3.one, 0.5f).setEaseOutBack();
        LeanTween.rotateZ(gameObject, 1080, 0.5f).setEaseOutBack();
    }

    public void ShowImportEffect()
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        LeanTween.scale(gameObject, Vector3.one, 0.5f).setEaseOutBack();
    }

    public void ClickWhenBoom()
    {
        if (GameController.isBoom&&GameController.instance.isPlaying)
        {
            GameController.instance.saveStatus();
            // Tạo hiệu ứng nổ
            GameObject explosion = Instantiate(GameController.instance.explosionEffectPrefab, transform.position, Quaternion.identity);
            explosion.transform.SetParent(transform.parent, false); // Giữ trong cùng một UI Canvas
            explosion.GetComponent<ExplosionAnimation>().PlayExplosion(transform.position);

            // Hủy ô sau khi hiệu ứng chạy xong
            Destroy(gameObject);

            GameController.isBoom = false;
            Database.instance.updateBoom(-1);
        }
    }

}

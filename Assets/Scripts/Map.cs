using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public GameObject imageObject; // Kéo Image vào đây
    float moveSpeed = 500f;

    private RectTransform rectTransform;
    float minY = -878f; // Giới hạn dưới
    float maxY = 878f;  // Giới hạn trên
    public GameObject[] levelPositions; // Danh sách vị trí các level trên bản đồ
    public GameObject locationIcon; // Hình ảnh location

    int highestLevel;


    void Start()
    {
        if (imageObject != null)
        {
            rectTransform = imageObject.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("Chưa gán GameObject Image!");
        }

        highestLevel = Database.instance.getHightestLever();
        UpdateLocationIcon();
        UpdateColorLevel();
        foreach (GameObject item in levelPositions)
        {
            Button button = item.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => ClickLevel(item));
            }
        }
    }

    void Update()
    {
        if (rectTransform == null) return;

        Vector2 newPosition = rectTransform.anchoredPosition;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition.y += moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            newPosition.y -= moveSpeed * Time.deltaTime;
        }

        // Giới hạn vị trí trong khoảng minY và maxY
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Cập nhật vị trí
        rectTransform.anchoredPosition = newPosition;
    }
    void UpdateLocationIcon()
    {
        if (highestLevel - 1 < levelPositions.Length)
        {
            // Lấy vị trí của level trong không gian của màn hình (world position)
            Vector3 levelWorldPos = levelPositions[highestLevel - 1].transform.position;
            levelWorldPos.y += 100.0f; // Dịch lên trên một chút

            // Đặt locationIcon theo tọa độ thế giới
            locationIcon.transform.position = levelWorldPos;


            StartBouncing();
        }
    }
    void UpdateColorLevel()
    {
        foreach (GameObject item in levelPositions)
        {
            if (Convert.ToInt32(item.gameObject.name) <= highestLevel)
            {
                Button myButton = item.GetComponent<Button>();
                myButton.image.color = Color.green;
            }
        }
    }
    void StartBouncing()
    {
        LeanTween.scale(locationIcon, Vector3.one * 1.2f, 0.5f).setLoopPingPong();
    }
    public void ClickLevel(GameObject clickedButton)
    {
        Debug.Log("Bạn đã click map " + clickedButton.name);
        if (Convert.ToInt32(clickedButton.name) <= highestLevel)
        {
            PlayerPrefs.SetInt("level", Convert.ToInt32(clickedButton.name));
            SceneManager.LoadScene(2);
        }
    }
    public void ClickHome()
    {
        SceneManager.LoadScene(0);
    }
}

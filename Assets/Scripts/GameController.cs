using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

using Photon.Pun;
using UnityEngine.UI;



public class GameController : MonoBehaviourPun
{
    public static GameController instance;
    public static int ticker;
    public static bool isMove = false;
    public static bool isBoom = false;

    public bool canUndo = false;
    public static string[] skipCell;
    [SerializeField] public GameObject fillPrefab;
    [SerializeField] Cell2048[] allCells4x4;
    [SerializeField] Cell2048[] allCells3x3;
    [SerializeField] Cell2048[] allCells5x5;
    public Cell2048[] allCells;

    [SerializeField] public Button displayChallent;
    [SerializeField] public Button displayChallentItem;

    [SerializeField] public GameObject explosionEffectPrefab;
    public int MaxN;
    public int MaxD;
    Vector2 startTouchPosition;
    Vector2 endTouchPosition;
    public Sprite[] fillColors;
    public static Action<string> slide;
    public int length;
    public int winScore;

    public int score;

    public int highestScore;
    public bool isPlaying = false;
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        winScore = Database.instance.GetScore(PlayerPrefs.GetInt("level", 1));
        if (PlayerPrefs.GetString("mode") == "classic")
        {
            if (PlayerPrefs.GetString("nxn") == "3x3") allCells = allCells3x3;
            if (PlayerPrefs.GetString("nxn") == "4x4") allCells = allCells4x4;
            if (PlayerPrefs.GetString("nxn") == "5x5") allCells = allCells5x5;
            SetUIGamePanel.Instance.setGame();
        }
        else if (PlayerPrefs.GetString("mode") == "challent")
        {
            allCells = allCells4x4;
            SetUIGamePanel.Instance.setChallentPanel();
        }
        else if (PlayerPrefs.GetString("mode") == "pvp")
        {
            allCells = allCells4x4;
            SetUIGamePanel.Instance.setGame();
        }

        // winScore = 32;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            saveStatus();
            ticker = 0;
            slide("w");
            canUndo = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            saveStatus();
            ticker = 0;
            slide("d");
            canUndo = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            saveStatus();
            ticker = 0;
            slide("s");
            canUndo = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            saveStatus();
            ticker = 0;
            slide("a");
            canUndo = true;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                Vector2 swipe = endTouchPosition - startTouchPosition;

                if (swipe.magnitude > 50f) // threshold to ignore tiny movements
                {
                    if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                    {
                        if (swipe.x > 0)
                        {
                            // Swipe Right → D
                            saveStatus();
                            ticker = 0;
                            slide("d");
                            canUndo = true;
                        }
                        else
                        {
                            // Swipe Left → A
                            saveStatus();
                            ticker = 0;
                            slide("a");
                            canUndo = true;
                        }
                    }
                    else
                    {
                        if (swipe.y > 0)
                        {
                            // Swipe Up → W
                            saveStatus();
                            ticker = 0;
                            slide("w");
                            canUndo = true;
                        }
                        else
                        {
                            // Swipe Down → S
                            saveStatus();
                            ticker = 0;
                            slide("s");
                            canUndo = true;
                        }
                    }
                }
            }
        }
        if (isPlaying)
        {
            if (checkFull() && !CanMove())
            {
                SetUIGamePanel.Instance.setGameOver();
                SoundManager.Instance.PlayLoseSound();
                isPlaying = false;
            }
            if (isBoom)
            {
                SetUIGamePanel.Instance.noTice.gameObject.SetActive(true);
            }
            else
            {
                SetUIGamePanel.Instance.noTice.gameObject.SetActive(false);
            }
        }
    }
    public void SpawnFill()
    {
        bool isFull = true;
        for (int i = 0; i < length; i++)
        {
            if (allCells[i] == null) continue;
            if (allCells[i].fill == null)
            {
                isFull = false;
            }
        }
        if (isFull == true)
        {
            return;
        }
        int whichSpawn;
        do
        {
            whichSpawn = UnityEngine.Random.Range(0, length);
        }
        while (allCells[whichSpawn] == null || allCells[whichSpawn].fill != null);
        float chance = UnityEngine.Random.Range(0f, 1f);
        if (chance < .8f)
        {
            GameObject tempFill = Instantiate(fillPrefab, allCells[whichSpawn].transform);
            Canvas canvas = tempFill.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = tempFill.AddComponent<Canvas>();
            }

            canvas.overrideSorting = true;
            canvas.sortingOrder = 1000;
            Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
            allCells[whichSpawn].GetComponent<Cell2048>().fill = tempFillComp;
            tempFillComp.ShowSpawnEffect();
            tempFillComp.FillValueUpdate(2);
        }
        else
        {

            GameObject tempFill = Instantiate(fillPrefab, allCells[whichSpawn].transform);
            Canvas canvas = tempFill.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = tempFill.AddComponent<Canvas>();

            }
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1000;

            Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
            allCells[whichSpawn].GetComponent<Cell2048>().fill = tempFillComp;
            tempFillComp.ShowSpawnEffect();
            tempFillComp.FillValueUpdate(4);
        }
    }
    public void StartSpawnFill()
    {
        int whichSpawn;
        do
        {
            whichSpawn = UnityEngine.Random.Range(0, length);
        } while (whichSpawn == 0 || allCells[whichSpawn] == null || allCells[whichSpawn].transform.childCount != 0);
        GameObject tempFill = Instantiate(fillPrefab, allCells[whichSpawn].transform);
        Canvas canvas = tempFill.GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = tempFill.AddComponent<Canvas>();

        }

        canvas.overrideSorting = true;
        canvas.sortingOrder = 1000;

        Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
        allCells[whichSpawn].GetComponent<Cell2048>().fill = tempFillComp;
        tempFillComp.ShowSpawnEffect();
        tempFillComp.FillValueUpdate(2);
    }

    public bool checkFull()
    {
        bool isFull = true;
        foreach (Cell2048 cell in allCells)
        {

            if (cell == null) continue;
            if (cell.fill == null)
            {
                isFull = false;
            }
        }
        return isFull;
    }
    public bool CanMove()
    {
        foreach (Cell2048 cell in allCells)
        {
            if (cell == null) continue;
            if (cell.CheckCell()) return true;
        }
        return false;
    }
    public void checkWin(int value)
    {
        if (value == winScore)
        {
            SetUIGamePanel.Instance.DisplayWinPannel();
            SoundManager.Instance.PlayWinSound();
            isPlaying = false;
        }
    }
    public void GetScore(int value)
    {
        score += value;
        SetUIGamePanel.Instance.TextScore.SetText("Score: " + score.ToString());
        if (PlayerPrefs.GetString("nxn") == "3x3")
        {
            if (score > PlayerPrefs.GetInt("highest3x3", 0))
            {
                PlayerPrefs.SetInt("highest3x3", score);
                SetUIGamePanel.Instance.TextHighestScore.SetText("Hightest Score:" + score);
            }
        }
        if (PlayerPrefs.GetString("nxn") == "4x4")
        {
            if (score > PlayerPrefs.GetInt("highest4x4", 0))
            {
                PlayerPrefs.SetInt("highest4x4", score);
                SetUIGamePanel.Instance.TextHighestScore.SetText("Hightest Score:" + score);
            }
        }
        if (PlayerPrefs.GetString("nxn") == "5x5")
        {
            if (score > PlayerPrefs.GetInt("highest5x5", 0))
            {
                PlayerPrefs.SetInt("highest5x5", score);
                SetUIGamePanel.Instance.TextHighestScore.SetText("Hightest Score:" + score);
            }
        }
    }

    public void getSkipCell()
    {
        int level = PlayerPrefs.GetInt("level", 0);
        string getSkipCell = Database.instance.GetSkipCell(level);
        skipCell = getSkipCell.Split(";");
        foreach (string cell in skipCell)
        {
            try
            {
                int index = Convert.ToInt16(cell) - 1;
                Destroy(allCells[index].gameObject);
                allCells[index] = null;
            }
            catch { }
        }
    }
    public void saveStatus()
    {
        foreach (Cell2048 cell in allCells)
        {
            if (cell == null) continue;
            cell.saveStatus();
        }
    }

    [PunRPC]
    public void OnReached()
    {
        Debug.Log("[RPC] Bên kia vừa đạt ô 64!");
        SpawnFill();
        if (checkFull() == true)
        {
            bool isDivide = false;
            foreach (Cell2048 cell in allCells)
            {
                if (cell == null) continue;
                if (cell.fill.value > 2)
                {
                    isDivide = true;
                    break;
                }
            }
            if (isDivide)
            {
                int whichDivide;
                do
                {
                    whichDivide = UnityEngine.Random.Range(0, length);
                }
                while (allCells[whichDivide].fill.value <= 2);
                allCells[whichDivide].fill.FillValueUpdate(allCells[whichDivide].fill.value / 2);
            }
        }
    }
    [PunRPC]
    public void OnReachedWin()
    {
        SetUIGamePanel.Instance.gameOverPanel.SetActive(true);
        isPlaying = false;
        if (!PhotonNetwork.IsMasterClient)
        {
            Transform panel = SetUIGamePanel.Instance.gameOverPanel.transform.Find("Panel");
            Transform restartButton = panel?.Find("ButtonRestart");
            restartButton.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void OnRestart()
    {
        SceneManager.LoadScene(2);
    }
    
}

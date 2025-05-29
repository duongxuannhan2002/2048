using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class SetUIGamePanel : MonoBehaviour
{
    // Start is called before the first frame update
    public static SetUIGamePanel Instance;
    [SerializeField] GameObject challentPanel;
    [SerializeField] GameObject itemPanel;

    [SerializeField] public GameObject gameOverPanel;
    [SerializeField] GameObject gameOverPanelClassic;

    [SerializeField] GameObject gameWinPanel;
    [SerializeField] public GameObject pausePanel;
    [SerializeField] public GameObject shopPanel;
    [SerializeField] TextMeshProUGUI Level;
    [SerializeField] TextMeshProUGUI TextMoney;
    [SerializeField] TextMeshProUGUI TextBoom;
    [SerializeField] TextMeshProUGUI TextUndo;
    [SerializeField] public TextMeshProUGUI TextScore;
    [SerializeField] public TextMeshProUGUI TextHighestScore;
    [SerializeField] public TextMeshProUGUI noTice;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TextMoney.SetText(Database.instance.getMoney().ToString());
        TextBoom.SetText(Database.instance.getBoom().ToString());
        TextUndo.SetText(Database.instance.getUndo().ToString());
    }
    public void setGame()
    {
        LeanTween.scale(challentPanel, Vector3.zero, 0.15f).setEase(LeanTweenType.easeOutBack)
        .setOnComplete(() =>
        {
            if (PlayerPrefs.GetString("mode") == "classic")
            {
                TextScore.SetText("Score: 0");
                if (PlayerPrefs.GetString("nxn") == "3x3") TextHighestScore.SetText("Highest Score: " + PlayerPrefs.GetInt("highest3x3", 0));
                if (PlayerPrefs.GetString("nxn") == "4x4") TextHighestScore.SetText("Highest Score: " + PlayerPrefs.GetInt("highest4x4", 0));
                if (PlayerPrefs.GetString("nxn") == "5x5") TextHighestScore.SetText("Highest Score: " + PlayerPrefs.GetInt("highest5x5", 0));
            }

        });
        GameController.instance.length = GameController.instance.allCells.Length;
        if (PlayerPrefs.GetString("mode") == "challent") GameController.instance.getSkipCell();
        AnimateStart();
        StartCoroutine(Delay());
        GameController.instance.displayChallentItem.image.sprite = GameController.instance.displayChallent.image.sprite;
        TextMeshProUGUI buttonText = GameController.instance.displayChallentItem.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.SetText(GameController.instance.winScore.ToString());
        GameController.instance.isPlaying = true;
    }
    void AnimateStart()
    {
        itemPanel.SetActive(true);
        Vector3 startPosPaelItem = itemPanel.transform.position;
        itemPanel.transform.position = new Vector3(startPosPaelItem.x, startPosPaelItem.y + 500f, startPosPaelItem.z); // Đặt thấp hơn vị trí ban đầu
        LeanTween.moveY(itemPanel.gameObject, startPosPaelItem.y, 1f).setEase(LeanTweenType.easeOutBack);
        foreach (Cell2048 cell in GameController.instance.allCells)
        {
            if (cell != null)
            {
                cell.gameObject.SetActive(true);
                Vector3 startPos = cell.transform.position;
                cell.transform.position = new Vector3(startPos.x, startPos.y - 1500f, startPos.z); // Đặt thấp hơn vị trí ban đầu
                LeanTween.moveY(cell.gameObject, startPos.y, 1f).setEase(LeanTweenType.easeOutBack); // Di chuyển lên
            }
        }
    }
    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        challentPanel.SetActive(false);
        GameController.instance.StartSpawnFill();
        GameController.instance.StartSpawnFill();
        GameController.instance.saveStatus();
        GameController.instance.MaxN = Database.instance.GetMaxN(PlayerPrefs.GetInt("level", 0));
        GameController.instance.MaxD = Database.instance.GetMaxD(PlayerPrefs.GetInt("level", 0));
    }

    public void setChallentPanel()
    {
        challentPanel.SetActive(true);
        Vector3 originScale = challentPanel.transform.localScale;
        challentPanel.transform.localScale = Vector3.zero;
        LeanTween.scale(challentPanel, originScale, 0.3f).setEase(LeanTweenType.easeOutBack);
        int index = 0;
        int valueIn = GameController.instance.winScore;
        while (valueIn != 1)
        {
            index++;
            valueIn /= 2;
        }
        index--;
        GameController.instance.displayChallent.image.sprite = GameController.instance.fillColors[index];
        TextMeshProUGUI buttonText = GameController.instance.displayChallent.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.SetText(GameController.instance.winScore.ToString());
        Level.SetText(PlayerPrefs.GetInt("level", 0).ToString());
    }
    public void DisplayWinPannel()
    {
        gameWinPanel.SetActive(true);

        if (PlayerPrefs.GetString("mode") == "pvp")
        {
            Transform panel = gameWinPanel.transform.Find("Panel");
            Transform nextButton = panel?.Find("ButtonNextGame");
            nextButton.gameObject.SetActive(false);
            if (!PhotonNetwork.IsMasterClient)
            {
                Transform restartButton = panel?.Find("ButtonRestart");
                restartButton.gameObject.SetActive(false);
            }
        }
        Vector3 panelScale = gameWinPanel.transform.localScale;
        gameWinPanel.transform.localScale = Vector3.zero;
        LeanTween.scale(gameWinPanel, panelScale, 0.3f).setEase(LeanTweenType.easeOutBack);
        Database.instance.updateMoney(1);
        Database.instance.updateLevel(PlayerPrefs.GetInt("level") + 1);
    }
    public void setGameOver()
    {
        if (PlayerPrefs.GetString("mode") == "challent")
        {
            if (!gameOverPanel.activeSelf)
            {
                gameOverPanel.SetActive(true);
                Vector3 localScale = gameOverPanel.transform.localScale;
                gameOverPanel.transform.localScale = Vector3.zero;
                LeanTween.scale(gameOverPanel, localScale, 0.3f).setEase(LeanTweenType.easeOutBack);
            }
        }
        if (PlayerPrefs.GetString("mode") == "classic")
        {
            if (!gameOverPanelClassic.activeSelf)
            {
                gameOverPanelClassic.SetActive(true);
                TextMeshProUGUI textMeshProUGUI = gameOverPanelClassic.transform.Find("Score").GetComponent<TextMeshProUGUI>();
                textMeshProUGUI.SetText("SCORE:" + GameController.instance.score);
                textMeshProUGUI = gameOverPanelClassic.transform.Find("MaxScore").GetComponent<TextMeshProUGUI>();
                if (PlayerPrefs.GetString("nxn") == "3x3") textMeshProUGUI.SetText("HIGHEST SCORE:" + PlayerPrefs.GetInt("highest3x3", 0));
                if (PlayerPrefs.GetString("nxn") == "4x4") textMeshProUGUI.SetText("HIGHEST SCORE:" + PlayerPrefs.GetInt("highest4x4", 0));
                if (PlayerPrefs.GetString("nxn") == "5x5") textMeshProUGUI.SetText("HIGHEST SCORE:" + PlayerPrefs.GetInt("highest5x5", 0));
                Vector3 localScale = gameOverPanelClassic.transform.localScale;
                gameOverPanelClassic.transform.localScale = Vector3.zero;
                LeanTween.scale(gameOverPanelClassic, localScale, 0.3f).setEase(LeanTweenType.easeOutBack);
            }
        }
    }
}

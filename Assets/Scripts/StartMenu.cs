using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ClickChallentButton()
    {
        PlayerPrefs.SetString("mode", "challent");
        SceneManager.LoadScene(1);
    }

    public void ClickClassicButton()
    {
        PlayerPrefs.SetString("mode", "classic");
        SceneManager.LoadScene(3);
    }

    public void ClickPvpButton()
    {
        PlayerPrefs.SetString("mode", "pvp");
        SceneManager.LoadScene(4);
    }


    public void Click3x3Button()
    {
        PlayerPrefs.SetString("nxn", "3x3");
        SceneManager.LoadScene(2);
    }

    public void Click4x4Button()
    {
        PlayerPrefs.SetString("nxn", "4x4");
        SceneManager.LoadScene(2);
    }

    public void Click5x5Button()
    {
        PlayerPrefs.SetString("nxn", "5x5");
        SceneManager.LoadScene(2);
    }
    public void ClickHome()
    {
        if (PlayerPrefs.GetString("mode") == "pvp") PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}

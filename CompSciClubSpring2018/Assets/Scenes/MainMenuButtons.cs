using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour {
        public  void playGame()
    {
        SceneManager.LoadScene("HunterGoodin-FirstScene");
    }
    public void quitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}

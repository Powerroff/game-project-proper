using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager = null;

    public BoardManager boardManager;
    public BulkLoader bulkLoader;
    public UIManager uiManager;


    //public int stamina;

	// Use this for initialization
	void Awake () {
        Random.InitState(System.DateTime.Now.Day + System.DateTime.Now.Millisecond);

        if (gameManager == null) {
            gameManager = this;
        } else if (gameManager != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
	}

    private void Start() {
        boardManager.BoardSetup();
    }

    public void GameOver(bool win) {
        uiManager.gameEndPanel.SetActive(true);
        if (win)
            uiManager.gameEndText.GetComponent<Text>().text = "Victory!";
        else
            uiManager.gameEndText.GetComponent<Text>().text = "Defeat";
        Time.timeScale = 0;
    }
}

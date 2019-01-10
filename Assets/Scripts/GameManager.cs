using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;


    public BoardManager boardScript;

    //public int stamina;

	// Use this for initialization
	void Awake () {
        Random.InitState(System.DateTime.Now.Day + System.DateTime.Now.Millisecond);

        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<BoardManager>();
        boardScript.boardSetup();
	}
	
    public void GameOver(bool win) {
        GameObject.Find("UICanvas").transform.Find("Game End Panel").gameObject.SetActive(true);
        Text t = GameObject.Find("Game End Panel").GetComponentInChildren<Text>();
        if (win)
            t.text = "Victory!";
        else
            t.text = "Defeat";
        Time.timeScale = 0;
        enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    //public LoseScreen lose;
    //public WinScreen win;
    public WebController game;

	// Use this for initialization
	void Start () {
        game.gameObject.SetActive(false);

        StartGame();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartGame()
    {
        game.gameObject.SetActive(true);
    }
}

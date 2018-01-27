using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    //public LoseScreen lose;
    //public WinScreen win;
    public WebController game;
	public GameObject startScreen;
	public GameObject endScreen;
	public GameObject hud;

	// Use this for initialization
	void Start () {
		game.gameObject.SetActive(false);
		startScreen.gameObject.SetActive (true);
		endScreen.gameObject.SetActive (false);
		hud.gameObject.SetActive (false);
        //StartGame();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EndGame()
	{
		hud.gameObject.SetActive (false);
		endScreen.SetActive (true);
		game.gameObject.SetActive(false);
	}

    public void StartGame()
    {
		startScreen.SetActive (false);
		endScreen.SetActive (false);
		hud.gameObject.SetActive (true);
		game.gameObject.SetActive(true);

    }
}

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
	public GameObject credits;

    WebController currentGame;

	// Use this for initialization
	void Start () {
        //StartGame();
		ShowMenu();
	}

	public void ShowMenu()
	{
		game.gameObject.SetActive(false);
		startScreen.gameObject.SetActive (true);
		endScreen.gameObject.SetActive (false);
		hud.gameObject.SetActive (false);
		credits.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void EndGame()
    {
        Score score = new Score(0, 0);
        EndGame(score);
    }

	public void EndGame(Score score)
	{

		hud.gameObject.SetActive (false);
		credits.gameObject.SetActive (false);
		endScreen.SetActive (true);
		currentGame.gameObject.SetActive(false);
        GameObject.Destroy(currentGame);

        FlyBar[] flyBars = GameObject.FindObjectsOfType<FlyBar>();
        for (int i = 0; i < flyBars.Length; i++)
        {
            GameObject.Destroy(flyBars[i].gameObject);
        }
    }

	public void ShowCredits()
	{
		credits.gameObject.SetActive (true);
		startScreen.SetActive (false);
		endScreen.SetActive (false);
		hud.gameObject.SetActive (false);
	}

    public void StartGame()
    {
        currentGame = GameObject.Instantiate<WebController>(game);
		startScreen.SetActive (false);
		endScreen.SetActive (false);
		credits.gameObject.SetActive (false);
		hud.gameObject.SetActive (true);
        currentGame.gameObject.SetActive(true);
        currentGame.stateManager = this;
    }
}

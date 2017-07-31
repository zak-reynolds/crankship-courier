using Assets.StateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour {
    
    public enum GameStage { Intro, LearnMovement, LearnGun, LearnEngine, BirdsA }

    public GameStage currentStage = GameStage.Intro;

    private IState currentState;

    public GameObject[] gameUI;

	void Start () {
        GobPool.Clear();
        currentState = new IntroState();
        currentState.OnEnter();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
        if (Input.GetKeyDown(KeyCode.Backspace)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Game state machining
        var nextState = currentState.Update();
        if (currentState != nextState)
        {
            currentState.OnExit();
            nextState.OnEnter();
        }
        currentState = nextState;
    }

    public void StartGame()
    {
        currentState.OnExit();
        currentState = new LearnMovementState();
        currentState.OnEnter();
        foreach (var gob in gameUI)
        {
            gob.SetActive(true);
        }
    }
}

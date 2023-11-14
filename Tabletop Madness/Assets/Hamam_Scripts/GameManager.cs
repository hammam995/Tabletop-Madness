using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Game Manager will controll the game from it is enum states

    public int turnsTillSpecialMove = 3;
    public int specialDiceMultiplier = 2; // it will be double steps from the normal one
    public TextMeshProUGUI diceScoreText, turnText, specialCooldownText;
    public Canvas gameUi;
    public MenuScreens menuScreens;
    public ConeController player, enemy;
    public CameraController cameraController;
    public Dice normalDice, specialDice;
    public Button normalDiceButton, specialDiceButton;
    public InputAction escAction; // new input system to make this variable esc as action button whe pressing it it will activade the event we want

    private int currentDiceMultiplier;
    private int playerCooldown, enemyCooldown;
    private AudioController audioController;

    private enum GameState
    {
        MainMenu,
        PlayerTurn,
        PlayerMoving,
        EnemyTurn,
        EnemyMoving,
        Victory,
        Defeat
    }
    private GameState gameState;

    private void Awake()
    {
        audioController = gameObject.GetComponent<AudioController>();
    }

    private void OnEnable()
    {
        escAction.Enable();
        escAction.performed += EscKeyPressed;
    }

    private void OnDisable()
    {
        escAction.Disable();
    }
    private void Start()
    {
        normalDice.RemoveFromView();
        specialDice.RemoveFromView();
        player.ResetCone();
        enemy.ResetCone();
        MainMenu();
    }
    // reset the system
    public void StartTheGame()
    {
        Time.timeScale = 1;
        gameUi.enabled = true;
        normalDiceButton.interactable = false;
        specialDiceButton.interactable = false;
        menuScreens.ResetPanels();

        normalDice.RemoveFromView();
        specialDice.RemoveFromView();
        player.ResetCone();
        enemy.ResetCone();

        playerCooldown = turnsTillSpecialMove;
        enemyCooldown = turnsTillSpecialMove;
        cameraController.SwitchToFollow();
        audioController.PlayGameplayMusic();

        turnText.text = string.Empty;
        diceScoreText.text = "Dice score:\n0";


        //if we finish the game or not , if we cam from winning or lose sitiuation
        if (gameState == GameState.Victory)
            StartCoroutine(StartEnemyTurn(3));
        else if (gameState == GameState.Defeat)
            StartCoroutine(StartPlayerTurn(3));
        else
        {
            if (Random.Range(0, 2) == 0)
                StartCoroutine(StartPlayerTurn(3));
            else
                StartCoroutine(StartEnemyTurn(3));
        }
            
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        gameState = GameState.MainMenu;
        gameUi.enabled = false;

        menuScreens.ResetPanels();
        menuScreens.SetMainMenuPanel();
        cameraController.SwitchToRotate();

        audioController.PlayMenuMusic();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        gameUi.enabled = false;

        menuScreens.SetPauseMenuPanel();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        gameUi.enabled = true;

        menuScreens.ResetPanels();
    }

    public void Restart()
    {
        StartTheGame();
        cameraController.LookAtTarget(CameraController.LookTarget.Player);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void NormalDiceRolled()
    {
        currentDiceMultiplier = 1;
        cameraController.LookAtTarget(CameraController.LookTarget.NormalDice);

        if (gameState == GameState.PlayerTurn)
        {
            normalDiceButton.interactable = false;
            playerCooldown = Mathf.Clamp(playerCooldown - 1, 0, turnsTillSpecialMove); // to figure out how many times untile we can use the special dice
        }
        else if (gameState == GameState.EnemyTurn)
        {
            enemyCooldown = Mathf.Clamp(enemyCooldown - 1, 0, turnsTillSpecialMove);
        }
            
    }

    public void SpecialDiceRolled()
    {
        currentDiceMultiplier = specialDiceMultiplier;
        cameraController.LookAtTarget(CameraController.LookTarget.SpecialDice);

        if (gameState == GameState.PlayerTurn)
        {
            normalDiceButton.interactable = false;
            specialDiceButton.interactable = false;

            playerCooldown = turnsTillSpecialMove;
        }
        else if (gameState == GameState.EnemyTurn)
        {
            enemyCooldown = turnsTillSpecialMove;
        }
    }

    public IEnumerator StartPlayerTurn(float timeToWait)
    {
        gameState = GameState.PlayerTurn;
        yield return new WaitForSeconds(timeToWait);

        UpdateDiceText(0);
        turnText.text = "Your turn";
        normalDiceButton.interactable = true;
        cameraController.LookAtTarget(CameraController.LookTarget.Player);

        if (playerCooldown == 0)
        {
            specialDiceButton.interactable = true;
            audioController.PlaySpecialCharged();
        }
        
        UpdateCooldownText();
    }

    public IEnumerator StartEnemyTurn(float timeToWait)
    {  
        gameState = GameState.EnemyTurn;
        yield return new WaitForSeconds(timeToWait);

        UpdateCooldownText();
        UpdateDiceText(0);
        turnText.text = "Opponent's turn";
        cameraController.LookAtTarget(CameraController.LookTarget.Enemy);

        if(enemyCooldown == 0)
        {
            specialDice.RollDice();
            SpecialDiceRolled();
        }
        else
        {
            normalDice.RollDice();
            NormalDiceRolled();
        }       
    }

    public IEnumerator GetDiceScore(int score, float timeToWait)
    {
        if (gameState == GameState.PlayerTurn)
        {
            gameState = GameState.PlayerMoving;
            int scoreMultiplied = score * currentDiceMultiplier;
            UpdateDiceText(scoreMultiplied);

            yield return new WaitForSeconds(timeToWait);
            cameraController.LookAtTarget(CameraController.LookTarget.Player);
            player.MoveXTiles(scoreMultiplied);

            normalDice.RemoveFromView();
            specialDice.RemoveFromView();
        }
        else if (gameState == GameState.EnemyTurn)
        {
            gameState = GameState.EnemyMoving;
            int scoreMultiplied = score * currentDiceMultiplier;
            UpdateDiceText(scoreMultiplied);
            
            yield return new WaitForSeconds(timeToWait);
            cameraController.LookAtTarget(CameraController.LookTarget.Enemy);
            enemy.MoveXTiles(scoreMultiplied);

            normalDice.RemoveFromView();
            specialDice.RemoveFromView();
        }        
    }
    // to check if one of the player or the enemy won or not , to continue the game or not
    public void ReachedDestination(bool isPlayer, bool isFinished)
    {
        if (isPlayer)
        {
            if (isFinished)
                Victory();
            else
                StartCoroutine(StartEnemyTurn(2));
        }

        else
        {
            if (isFinished)
                Defeat();
            else
                StartCoroutine(StartPlayerTurn(2));
        }
    }

    public void Victory()
    {
        gameState = GameState.Victory;
        gameUi.enabled = false;

        menuScreens.SetWinPanel();
        cameraController.SwitchToRotate();
        audioController.PlayWon();
    }

    public void Defeat()
    {
        gameState = GameState.Defeat;
        gameUi.enabled = false;

        menuScreens.SetLosePanel();
        cameraController.SwitchToRotate();
        audioController.PlayLost();
    }

    public void UpdateDiceText(int score)
    {
        diceScoreText.text = "Dice score:\n" + score.ToString();
    }

    public void UpdateCooldownText()
    {
        if (playerCooldown != 0)
            specialCooldownText.text = "Special dice ready in:\n" + playerCooldown.ToString();
        else
            specialCooldownText.text = "SPECIAL DICE READY!!!";
    }
    // depending on the new input system actions, events
    private void EscKeyPressed(InputAction.CallbackContext context)
    {
        switch(gameState)
        {
            case GameState.MainMenu:
                Application.Quit();
                break;
            case GameState.PlayerTurn:
                Pause();
                break;
            case GameState.EnemyTurn:
                Pause();
                break;
            case GameState.PlayerMoving:
                Pause();
                break;
            case GameState.EnemyMoving:
                Pause();
                break;
            case GameState.Victory:
                MainMenu();
                break;
            case GameState.Defeat:
                MainMenu();
                break;
        }
    }    
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] TextMeshProUGUI livesText;

    void Awake()
    {
        int gameSessionCount = FindObjectsOfType<GameManager>().Length;
        if (gameSessionCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString() + "x";
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGame();
        }
    }

    void TakeLife()
    {
        playerLives--;
        UpdateLivesText();
    }

    void UpdateLivesText()
    {
        livesText.text = playerLives.ToString() + "x";
    }

    void ResetGame()
    {
        SceneManager.LoadScene( (int)Enums.Levels.Level_1 ); //might change later depending on what do I wanna load upon Game over(Main menu?)
        Destroy(gameObject);
    }

}

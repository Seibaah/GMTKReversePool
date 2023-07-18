using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Singleton instance
    public TextMeshProUGUI pointsText;
    public Canvas originalCanvas;
    public static bool IsGameOver;

    public void Setup(int score)
    {
        originalCanvas.gameObject.SetActive(false);
        gameObject.SetActive(true);
        pointsText.text = "Score: " + score;
        IsGameOver = true;
    }

    public void RestartButton()
    {
        IsGameOver = false;
        SceneManager.LoadScene("Mobile Gameplay Test");
    }
    
    public void ExitButton()
    {
        IsGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }
}
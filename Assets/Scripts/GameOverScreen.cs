using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Singleton instance
    public TextMeshProUGUI pointsText;
    public Canvas originalCanvas;

    public void Setup(int score)
    {
        originalCanvas.gameObject.SetActive(false);
        gameObject.SetActive(true);
        pointsText.text = "Score: " + score;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Gameplay Test");
    }
    
    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
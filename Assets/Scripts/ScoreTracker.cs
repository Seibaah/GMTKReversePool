using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

public class ScoreTracker : MonoBehaviour
{
    // UI
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;
    
    // Timer
    public Slider timerSlider;
    public float timeScale = 0.1f;
    
    // Lives
    public int numLives;
    public int ballsJustHitIn;

    // Testing purposes
    private int _counter;
    private Random rnd;
    
    // Score, multiplier
    public static int Score;
    public static int Multiplier;
    private Coroutine _multiplierCoroutine;
    
    // Game Over Screen
    public GameOverScreen gameOverScreen;
    public bool isGameOver;

    private readonly Dictionary<int, Color> _colorThresholds = new()
    {
        { 1, Color.white },                            // White to start
        { 2, new Color(1f, 1f, 0.5f) },         // Light yellow
        { 3, new Color(1f, 0.75f, 0.25f) },     // Light orange
        { 4, new Color(1f, 0.5f, 0.5f) },       // Light red
        { 5, new Color(1f, 0.75f, 0.75f) },     // Light pink
        { 6, new Color(0.9f, 0.75f, 1f) },      // Light purple
        { 7, new Color(0.75f, 0.75f, 1f) },     // Light blue
    };
    
    // Singleton instance
    public static ScoreTracker Instance { get; private set; }
    
    private void Awake()
    {
        // Set the instance to this object
        ScoreTracker save = null;
        if (Instance != null && Instance != this)
        {
            save = Instance;
        }
        Instance = this;

        Destroy(save);
    }

    private void Start()
    {
        scoreText.text = "0";
        multiplierText.text = "";
        numLives = 3;
    }
    
    private void FixedUpdate()
    {
        var timeToDecrement = timeScale * Time.deltaTime;
        timerSlider.value -= timeToDecrement;
        
        // TODO Change slider
        if (timerSlider.value <= 0f)
        {
            LoseALife();
            timerSlider.value = 1f;
        }
    }

    public void PlayShot()
    {
        ballsJustHitIn = rnd.Next(0, 3);
        Debug.Log("Balls hit in: " + ballsJustHitIn);
    }

    private void LoseALife()
    {
        numLives -= 1;
        // Animation
        if (numLives == 0)
        {
            GameOver();
        }
    }

    private void CheckPlayTickingSound()
    {
        while (timerSlider.value <= 0.2f)
        {
            // Play the looping ticking sound
        }
    }
    
    private IEnumerator MultiplierActivated()
    {
        var elapsedTime = 0f;
        var setActive = false;
        foreach (var threshold in _colorThresholds.Where(threshold => Multiplier == threshold.Key))
        {
            scoreText.color = threshold.Value;
            multiplierText.color = threshold.Value;
            break;
        }
        while (elapsedTime < 0.2f)
        {
            scoreText.gameObject.SetActive(setActive);
            multiplierText.gameObject.SetActive(setActive);

            elapsedTime += Time.deltaTime;
            setActive = !setActive;
            yield return null;
        }
        scoreText.gameObject.SetActive(true);
        multiplierText.gameObject.SetActive(true);
        scoreText.color = Color.white;
        multiplierText.color = Color.white;
    }
    
    private void GameOver()
    {
        isGameOver = true;
        // Set level text to white
        scoreText.color = Color.white;
        multiplierText.gameObject.SetActive(false);
        
        gameOverScreen.Setup(Score);
    }
}

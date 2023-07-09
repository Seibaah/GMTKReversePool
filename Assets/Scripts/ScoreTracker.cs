using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ScoreTracker : MonoBehaviour
{
    // UI
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI plusOneText;

    // Timer
    public Slider timerSlider;
    public float timeScale = 0.1f;
    public bool isPolling;
    
    // Lives
    public int numLives;
    public int ballsJustHitIn;
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    public Sprite[] spriteArray;

    // Score, multiplier
    public static int Score;
    public static int Multiplier;
    private Coroutine _multiplierCoroutine;

    // Game Over Screen
    public GameOverScreen gameOverScreen;
    public bool isGameOver;

    // Colors for Multiplier
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
        isPolling = true;
        Multiplier = 1;
        Score = 0;
    }

    private void FixedUpdate()
    {
        if (PauseMenu.IsPaused || TutorialMenu.IsTutorial || GameOverScreen.IsGameOver) return;

        var timeToDecrement = isPolling ? timeScale * Time.deltaTime : 0;
        timerSlider.value -= timeToDecrement;
        if (timerSlider.value <= 0.0f)
        {
            Multiplier = 1;
            if (numLives >= 1)
            {
                timerSlider.value = 1f;
                ballsJustHitIn = 0;
            }
            StartCoroutine(LoseMultiplier());
            StartCoroutine(LoseALife());
        }
    }

    public void CueBallSunk()
    {
        Multiplier = 1;
        if (numLives >= 1)
        {
            timerSlider.value = 1f;
            ballsJustHitIn = 0;
        }
        StartCoroutine(LoseMultiplier());
        StartCoroutine(LoseALife());
    }

    public void PlayShot()
    { 
        // ballsJustHitIn = Random.Range(0, 3);
        // print("Amount of balls hit in: " + ballsJustHitIn);
        // if (ballsJustHitIn == 0)
        // {
        //     Multiplier = 1;
        //     StartCoroutine(LoseMultiplier());
        //     StartCoroutine(LoseALife());
        // }
        // else
        // {
        //     var totalScore = timerSlider.value * 100;
        //     Score += (int) totalScore * Multiplier;
        //     scoreText.text = Score.ToString();
        //     Multiplier += ballsJustHitIn;
        //     multiplierText.text = "x" + Multiplier;
        //     StartCoroutine(MultiplierActivated(ballsJustHitIn));
        // }
        // timerSlider.value = 1f;
        
        var totalScore = timerSlider.value * 100;
        Score += (int) totalScore * Multiplier;
        scoreText.text = Score.ToString();
        Multiplier += ballsJustHitIn;
        multiplierText.text = "x" + Multiplier;
        StartCoroutine(MultiplierActivated(ballsJustHitIn));
        timerSlider.value = 1f;
        ballsJustHitIn = 0;
    }

    private int _mIndexSprite;
    private Coroutine _mCoroutineAnimation1;

    private IEnumerator LoseALife()
    {
        numLives -= 1;
        // Check if the previous animation coroutine is still running
        switch (numLives)
        {
            case 2:
                _mCoroutineAnimation1 = StartCoroutine(PlayAnimation(life3.GetComponent<Image>()));
                break;
            case 1:
                _mCoroutineAnimation1 = StartCoroutine(PlayAnimation(life2.GetComponent<Image>()));                break;
            case 0:
                _mCoroutineAnimation1 = StartCoroutine(PlayAnimation(life1.GetComponent<Image>()));
                yield return new WaitForSeconds(1f);
                GameOver();
                break;
        }
        yield return null;
    }

    private IEnumerator PlayAnimation(Image heart)
    {
        yield return new WaitForSeconds(0.05f);
        if (_mIndexSprite >= spriteArray.Length)
        {
            _mIndexSprite = 0;
        }

        heart.sprite = spriteArray[_mIndexSprite];
        _mIndexSprite += 1;
        if (_mIndexSprite != 6)
        {
            _mCoroutineAnimation1 = StartCoroutine(PlayAnimation(heart));
        }
        else
        {
            // Fade out the text
            heart.CrossFadeAlpha(0.0f, 0.5f, false);
            _mIndexSprite = 0;
        }
    }

    private IEnumerator LoseMultiplier()
    {
        multiplierText.text = "x0";
        multiplierText.color = Color.red;
        
        // Fade in the text instantly (since it's faded out from before)
        multiplierText.CrossFadeAlpha(1.0f, 0.0f, false);
        
        // Fade out the text
        multiplierText.CrossFadeAlpha(0.0f, 0.5f, false);
        
        // Make it x1 again
        multiplierText.text = "x" + Multiplier;
        multiplierText.CrossFadeAlpha(1.0f, 0.0f, false);
        
        // For plus one
        plusOneText.text = "-";
        plusOneText.color = Color.red;
        // Fade in the text instantly (since it's faded out from before)
        plusOneText.CrossFadeAlpha(1.0f, 0.0f, false);
        
        // Fade out the text
        plusOneText.CrossFadeAlpha(0.0f, 0.5f, false);
        yield return null;
    }
    
    private IEnumerator MultiplierActivated(int ballsHitIn)
    {
        var elapsedTime = 0f;
        var setActive = false;
        foreach (var threshold in _colorThresholds.Where(threshold => Multiplier == threshold.Key))
        {
            scoreText.color = threshold.Value;
            multiplierText.color = threshold.Value;
            break;
        }

        if (ballsHitIn != 0)
        {
            plusOneText.text = "+" + ballsHitIn;
            // Fade in the text instantly (since it's faded out from before)
            plusOneText.CrossFadeAlpha(1.0f, 0.0f, false);
        
            // Fade out the text
            plusOneText.CrossFadeAlpha(0.0f, 0.5f, false);
        }

        while (elapsedTime < 0.4f)
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
        scoreText.color = Color.white;
        
        gameOverScreen.Setup(Score);
    }
}

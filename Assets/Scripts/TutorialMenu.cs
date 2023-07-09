using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenu : MonoBehaviour
{
    public GameObject tutorialMenu;

    public static bool IsTutorial;
    // Start is called before the first frame update
    private void Awake()
    {
        if (PlayerPrefs.GetInt("HasDoneTutorial") == 0)
        {
            PlayerPrefs.SetInt("HasDoneTutorial", 1);
            Time.timeScale = 0f;
            IsTutorial = true;
        }
        else
        {
            tutorialMenu.SetActive(false);
            // TODO Uncomment on final build (this is used for testing purposes)
            PlayerPrefs.SetInt("HasDoneTutorial", 0);
        }
    }

    public void PlayButton()
    {
        tutorialMenu.SetActive(false);
        Time.timeScale = 1f;
        IsTutorial = false;
    }

}

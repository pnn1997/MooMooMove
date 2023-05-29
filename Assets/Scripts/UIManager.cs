using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public KingCowController player;
    public HerdManager herdManager;

    [SerializeField] private GameObject startGamePanel;
    [SerializeField] private GameObject gameOverPanel;
    private bool isGameOver = false;

    void Start()
    {
        startGamePanel.SetActive(true);
        gameOverPanel.SetActive(false);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver && Input.anyKey)
        {
            startGamePanel.SetActive(false);
            Time.timeScale = 1;
        }

        // Game over condition
        if (!isGameOver && (!player.IsAlive || herdManager.IsHerdGone))
        {
            isGameOver = true;

            StartCoroutine(GameOverSequence());
        }

        // Game restart handling
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void TriggerGameOver()
    {
        isGameOver = true;
    }

    private IEnumerator GameOverSequence()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;

        yield return new WaitForSeconds(5.0f);
    }
}
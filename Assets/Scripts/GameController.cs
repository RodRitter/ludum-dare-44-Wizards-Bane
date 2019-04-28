using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] monsters;
    public GameObject[] spawnPoints;
    public float spawnInterval = 2f;
    float lastSpawn;

    public GameObject overlayPanel;
    public GameObject tutorialPanel;
    public GameObject endGamePanel;
    public Text endGameScoreText;

    public bool isPaused = true;

    private void Start()
    {
        StartCoroutine(StartSpawner());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame();
        }
    }

    IEnumerator StartSpawner()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if(!isPaused) 
                Instantiate(monsters[Random.Range(0, monsters.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
        }
    }
    
    public void BeginGame()
    {
        isPaused = false;
        HideTutorial();
    }

    public void HideTutorial()
    {
        overlayPanel.SetActive(false);
        tutorialPanel.SetActive(false);
    }

    public void EndGame()
    {
        endGameScoreText.text = "Score: "+FindObjectOfType<Wizard>().score.ToString();
        isPaused = true;
        endGamePanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

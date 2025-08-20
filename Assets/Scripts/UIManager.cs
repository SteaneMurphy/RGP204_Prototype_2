using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public GameObject powerUpTextParent;
    public TextMeshProUGUI powerupText;
    public TextMeshProUGUI scoreText2;
    public TextMeshProUGUI timeText2;
    public GameObject powerUpTextParent2;
    public TextMeshProUGUI powerupText2;
    public float powerTextMax;
    public Color readyTextColourPlayer1;
    public Color readyTextColourPlayer2;
    public Color readyTextStartColour;
    public GameObject player1Overlay;
    public GameObject player2Overlay;
    public GameObject player1GameUI;
    public GameObject player2GameUI;
    public GameObject gameOverOverlay;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalScoreText2;
    public GameObject GameUIP1;
    public GameObject GameUIP2;

    private void Start()
    {
        powerUpTextParent.SetActive(false);
        powerUpTextParent2.SetActive(false);
        gameOverOverlay.SetActive(false);
        GameUIP1.SetActive(false);
        GameUIP2.SetActive(false);
    }

    public void UpdateScore(int score) 
    {
        scoreText.text = score.ToString();
    }

    public void UpdateScoreBot(int score)
    {
        scoreText2.text = score.ToString();
    }

    public void UpdateTime(string time) 
    {
        timeText.text = time;
    }

    public void UpdateTime2(string time)
    {
        timeText2.text = time;
    }

    public void UpdatePowerupText(string powerup) 
    {
        powerupText.text = powerup;
    }

    public IEnumerator PowerupText(string powerup) 
    {
        powerUpTextParent.SetActive(true);
        powerupText.text = powerup;
        powerupText.fontSize = Random.Range(51f ,73f);
        Vector3 rotation = powerupText.transform.eulerAngles;
        rotation.z = Random.Range(-10f, 10f);
        powerupText.transform.eulerAngles = rotation;
        Color c = powerupText.color;
        c.a = 1f;
        powerupText.color = c;
        float elapsed = 0f;

        while (elapsed < powerTextMax)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / powerTextMax;
            c.a = Mathf.Lerp(1f, 0f, t);
            powerupText.color = c;
            yield return null;
        }
        c.a = 0f;
        powerupText.color = c;
        powerUpTextParent.SetActive(false);
    }

    public void RemovePlayer1Overlay() 
    {
        player1Overlay.SetActive(false);
    }

    public void RemovePlayer2Overlay()
    {
        player2Overlay.SetActive(false);
    }

    public void TurnOnGameUIPlayer1() 
    {
        player1GameUI.SetActive(true);
    }

    public void TurnOnGameUIPlayer2() 
    {
        player2GameUI.SetActive(true);
    }

    public void DisplayGameOverScreen(string player, string score, string score2) 
    {
        player1GameUI.SetActive(false);
        player2GameUI.SetActive(false);
        gameOverOverlay.SetActive(true);
        winText.text = $"{player} WINS!";
        finalScoreText.text = $"SCORE: {score}";
        finalScoreText2.text = $"SCORE: {score2}";
    }

    public void MainMenuButton() 
    {
        SceneManager.LoadScene(0);
    }

    public void PlayerOverlayOn() 
    {
        GameUIP1.SetActive(true);
        GameUIP2.SetActive(true);
    }
}

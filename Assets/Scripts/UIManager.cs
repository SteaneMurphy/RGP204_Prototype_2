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
    public float powerTextMax;
    public Color readyTextColour;
    public Color readyTextStartColour;

    private void Start()
    {
        powerUpTextParent.SetActive(false);
    }

    public void UpdateScore(int score) 
    {
        scoreText.text = score.ToString();
    }

    public void UpdateTime(string time) 
    {
        timeText.text = time;
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
}

using System.Collections;
using UnityEngine;
using static UnityEngine.UI.Image;

public class GameManager : MonoBehaviour
{
    public ShapeManager shapeManager;
    public AudioManager audioManager;
    public UIManager UIManager;

    public GameObject tutorialOverlay;
    private bool gameStart;

    private int score = 0;

    private bool hasPowerUp;

    [Header("Powerup Variables")]
    [SerializeField] float freezeTimeMin;
    [SerializeField] float freezeTimeMax;
    [SerializeField] float slowDownTimeMin;
    [SerializeField] float slowDownTimeMax;
    [SerializeField] float slowDownSpeed;
    [SerializeField] float musicSlowSpeed;
    [SerializeField] float powerupTiming;
    [SerializeField] float powerupSoundDelay;
    private float timeElapsed;
    private float powerupSoundTimer;

    private void Start()
    {
        shapeManager.slowDownSpeed = slowDownSpeed;
        timeElapsed = powerupTiming;
        hasPowerUp = false;
        gameStart = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && gameStart == false)
        {
            tutorialOverlay.SetActive(false);
            shapeManager.SpawnTop();
            shapeManager.SpawnBot();
            gameStart = true;
        }

        if (!hasPowerUp && gameStart) 
        {
            PowerupTimer();
        }

        if (Input.GetKey(KeyCode.Q) && hasPowerUp && gameStart)
        {
            UsePowerup();
            hasPowerUp = false;
        }

        if (hasPowerUp) 
        {
            powerupSoundTimer += Time.deltaTime;
            if (powerupSoundTimer >= powerupSoundDelay)
            {
                audioManager.PlaySound("powerupReady");
                powerupSoundTimer = 0;
            }
        }
    }

    private IEnumerator FreezeBottomScreen()
    {
        StartCoroutine(UIManager.PowerupText("BOTTOM FREEZE!"));
        float time = Random.Range(freezeTimeMin, freezeTimeMax);
        shapeManager.botFreeze = true;

        yield return new WaitForSeconds(time);

        shapeManager.botFreeze = false;
    }

    private IEnumerator FreezeTopScreen()
    {
        StartCoroutine(UIManager.PowerupText("TOP FREEZE!"));
        float time = Random.Range(freezeTimeMin, freezeTimeMax);
        shapeManager.topFreeze = true;

        yield return new WaitForSeconds(time);

        shapeManager.topFreeze = false;
    }

    private IEnumerator GameSlowDown()
    {
        StartCoroutine(UIManager.PowerupText("SLOWDOWN!"));
        float time = Random.Range(slowDownTimeMin, slowDownTimeMax);
        shapeManager.slowDownGame = true;
        shapeManager.topTickSpeed = slowDownSpeed;
        shapeManager.botTickSpeed = slowDownSpeed;
        StartCoroutine(audioManager.SlowBGMusic(musicSlowSpeed));

        yield return new WaitForSeconds(time);

        shapeManager.slowDownGame = false;
        StartCoroutine(audioManager.NormalBGMusic(musicSlowSpeed));
    }

    public void RerollCurrentTop()
    {
        StartCoroutine(UIManager.PowerupText("REROLL TOP!"));
        shapeManager.DeleteTop();
        shapeManager.SpawnTop();
    }

    public void RerollCurrentBottom()
    {
        StartCoroutine(UIManager.PowerupText("REROLL BOTTOM!"));
        shapeManager.DeleteBot();
        shapeManager.SpawnBot();
    }

    private void PowerupTimer() 
    {
        if (timeElapsed <= 0f) 
        {

            hasPowerUp = true;
            UIManager.timeText.color = UIManager.readyTextColour;
            audioManager.PlaySound("powerupReady");
            UIManager.UpdateTime("READY!");
            timeElapsed = powerupTiming;
        }
        else 
        {
            timeElapsed -= Time.deltaTime;
            UIManager.UpdateTime((Mathf.Floor(timeElapsed * 100f) / 100f).ToString());
        }
    }

    private void UsePowerup() 
    {
        UIManager.timeText.color = UIManager.readyTextStartColour;
        audioManager.PlaySound("usePowerup");
        int rnd = Random.Range(0, 7);

        switch (rnd) 
        {
            case 0:
                StartCoroutine(FreezeBottomScreen());
                break;
            case 1:
                StartCoroutine(FreezeTopScreen());
                break;
            case 2:
                StartCoroutine(GameSlowDown());
                break;
            case 3:
                RerollCurrentTop();
                break;
            case 4:
                RerollCurrentBottom();
                break;
            case 5:
                StartCoroutine(UIManager.PowerupText("CLEAR TOP!"));
                shapeManager.ClearTop();
                break;
            case 6:
                StartCoroutine(UIManager.PowerupText("CLEAR BOTTOM!"));
                shapeManager.ClearBot();
                break;
            default:
                break;
        }
    }

    public void AdjustScore(string currentShape) 
    {
        switch (currentShape) 
        {
            case "LShapedLeft":
            case "LShapedRight":
                score += 20;
                break;
            case "Square":
            case "Straight":
                score += 10;
                break;
            case "Tee":
            case "ZigzagLeft":
            case "ZigzagRight":
                score += 40;
                break;
            default:
                break;
        }
        UIManager.UpdateScore(score);
    }
}

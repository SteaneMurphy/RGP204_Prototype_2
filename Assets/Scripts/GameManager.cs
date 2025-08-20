using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ShapeManager shapeManager;
    public AudioManager audioManager;
    public UIManager UIManager;
    private bool player1Ready;
    private bool player2Ready;
    private bool gameStart;

    private int score = 0;
    private int score2 = 0;

    private bool player1Powerup;
    private bool player2Powerup;

    [Header("Powerup Variables")]
    [SerializeField] float freezeTimeMin;
    [SerializeField] float freezeTimeMax;
    [SerializeField] float slowDownTimeMin;
    [SerializeField] float slowDownTimeMax;
    [SerializeField] float slowDownSpeed;
    [SerializeField] float musicSlowSpeed;
    [SerializeField] float powerupTiming;
    [SerializeField] float powerupSoundDelay;
    private float timeElapsedPlayer1;
    private float timeElapsedPlayer2;
    private float powerupSoundTimer;

    private int lastDiffScore = 0;

    private void Start()
    {
        shapeManager.slowDownSpeed = slowDownSpeed;
        timeElapsedPlayer1 = powerupTiming;
        timeElapsedPlayer2 = powerupTiming;
        player1Powerup = false;
        player2Powerup = false;
        player1Ready = false;
        player2Ready = false;
        gameStart = false;

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.AdjustFolder();
        shapeManager.LinkAudioManager();
        UIManager.UpdateScore(0);
        UIManager.UpdateScoreBot(0);
    }

    private void Update()
    {
        CalculateScoreDifference();

        if (Input.GetKeyDown(KeyCode.Q) && gameStart == false)
        {
            UIManager.RemovePlayer1Overlay();
            UIManager.TurnOnGameUIPlayer1();
            player1Ready = true;
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton4) && gameStart == false) 
        {
            UIManager.RemovePlayer2Overlay();
            UIManager.TurnOnGameUIPlayer2();
            player2Ready = true;
        }

        if (player1Ready && player2Ready && gameStart == false) 
        {
            UIManager.PlayerOverlayOn();
            shapeManager.SpawnTop();
            shapeManager.SpawnBot();
            gameStart = true;
        }

        if (!player1Powerup && gameStart) 
        {
            PowerupTimerPlayer1();
        }

        if (!player2Powerup && gameStart)
        {
            PowerupTimerPlayer2();
        }

        if (Input.GetKey(KeyCode.Q) && player1Powerup && gameStart)
        {
            UsePowerup();
            player1Powerup = false;
        }

        if (Input.GetKey(KeyCode.JoystickButton4) && player2Powerup && gameStart)
        {
            UsePowerup();
            player2Powerup = false;
        }

        if (player1Powerup) 
        {
            powerupSoundTimer += Time.deltaTime;
            if (powerupSoundTimer >= powerupSoundDelay)
            {
                audioManager.PlaySound("powerupReady");
                powerupSoundTimer = 0;
            }
        }

        if (player2Powerup)
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

    private void PowerupTimerPlayer1() 
    {
        if (timeElapsedPlayer1 <= 0f) 
        {

            player1Powerup = true;
            UIManager.timeText.color = UIManager.readyTextColourPlayer1;
            audioManager.PlaySound("powerupReady");
            UIManager.UpdateTime("READY!");
            timeElapsedPlayer1 = powerupTiming;
        }
        else 
        {
            timeElapsedPlayer1 -= Time.deltaTime;
            UIManager.UpdateTime((Mathf.Floor(timeElapsedPlayer1 * 100f) / 100f).ToString());
        }
    }

    private void PowerupTimerPlayer2()
    {
        if (timeElapsedPlayer2 <= 0f)
        {

            player2Powerup = true;
            UIManager.timeText2.color = UIManager.readyTextColourPlayer2;
            audioManager.PlaySound("powerupReady");
            UIManager.UpdateTime2("READY!");
            timeElapsedPlayer2 = powerupTiming;
        }
        else
        {
            timeElapsedPlayer2 -= Time.deltaTime;
            UIManager.UpdateTime2((Mathf.Floor(timeElapsedPlayer2 * 100f) / 100f).ToString());
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

    public void AdjustScore(string currentShape, string player) 
    {
        int tempScore = 0;

        switch (currentShape) 
        {
            case "LShapedLeft":
            case "LShapedRight":
                tempScore += 20;
                break;
            case "Square":
            case "Straight":
                tempScore += 10;
                break;
            case "Tee":
            case "ZigzagLeft":
            case "ZigzagRight":
                tempScore += 40;
                break;
            case "Clear":
                tempScore += 100;
                break;
            default:
                break;
        }

        if (player == "top")
        {
            score += tempScore;
            UIManager.UpdateScore(score);
        }
        else if (player == "bot") 
        {
            score2 += tempScore;
            UIManager.UpdateScoreBot(score2);
        }
    }

    private void CalculateScoreDifference()
    {
        int diff = Mathf.Abs(score - score2);
        int diffScore = diff / 100;
        string playerAdvantage = score > score2 ? "top" : "bot";

        // calculate change since last update
        int moveAmount = diffScore - lastDiffScore;

        if (moveAmount > 0)
        {
            shapeManager.MoveMidpoint(moveAmount, playerAdvantage);
        }

        // update last difference
        lastDiffScore = diffScore;
    }

    public void GameOver(string player) 
    {
        UIManager.DisplayGameOverScreen(player, score.ToString(), score2.ToString());
    }
}

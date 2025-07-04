using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ShapeManager shapeManager;
    public AudioManager audioManager;

    public GameObject tutorialOverlay;
    private bool gameStart = false;

    [Header("Powerups")]
    public bool freezeBot = false;
    public bool freezeTop = false;
    public bool gameSlowDown = false;
    public bool rerollTop = false;
    public bool rerollBot = false;
    public bool clearTop = false;
    public bool clearBot = false;

    [Header("Powerup Variables")]
    [SerializeField] float freezeTimeMin;
    [SerializeField] float freezeTimeMax;
    [SerializeField] float slowDownTimeMin;
    [SerializeField] float slowDownTimeMax;
    [SerializeField] float slowDownSpeed;
    [SerializeField] float musicSlowSpeed;

    private void Start()
    {
        shapeManager.slowDownSpeed = slowDownSpeed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && gameStart == false) 
        {
            tutorialOverlay.SetActive(false);
            shapeManager.SpawnTop();
            shapeManager.SpawnBot();
        }

        if (freezeBot) 
        {
            freezeBot = false;
            StartCoroutine(FreezeBottomScreen());
        }

        if (freezeTop) 
        {
            freezeTop = false;
            StartCoroutine(FreezeTopScreen());
        }

        if (gameSlowDown) 
        {
            gameSlowDown = false;
            StartCoroutine(GameSlowDown());
        }

        if (rerollTop) 
        {
            RerollCurrentTop();
        }

        if (rerollBot) 
        {
            RerollCurrentBottom();
        }

        if (clearTop) 
        {
            clearTop = false;
            shapeManager.ClearTop();
        }

        if (clearBot) 
        {
            clearBot = false;
            shapeManager.ClearBot();
        }

        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            StartCoroutine(GameSlowDown());
        }
    }

    private IEnumerator FreezeBottomScreen() 
    {
        float time = Random.Range(freezeTimeMin, freezeTimeMax);
        shapeManager.botFreeze = true;

        yield return new WaitForSeconds(time);

        shapeManager.botFreeze = false;
    }

    private IEnumerator FreezeTopScreen() 
    {
        float time = Random.Range(freezeTimeMin, freezeTimeMax);
        shapeManager.topFreeze = true;

        yield return new WaitForSeconds(time);

        shapeManager.topFreeze = false;
    }

    private IEnumerator GameSlowDown() 
    {
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
        rerollTop = false;
        shapeManager.DeleteTop();
        shapeManager.SpawnTop();
    }

    public void RerollCurrentBottom() 
    {
        rerollBot = false;
        shapeManager.DeleteBot();
        shapeManager.SpawnBot();
    }
}

using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private bool settingsActive = false;
    private bool postProcessingActive = true;

    public GameObject settingsMenu;
    public GameObject mainMenu;
    public Camera mainCamera;
    public Slider musicSlider;
    public Slider effectsSlider;

    private void Start()
    {
        musicSlider.value = 1;
        effectsSlider.value = 1;
    }

    public void NewGame()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().StopLoop();
        SceneManager.LoadScene(1);              //main game scene
    }

    public void Settings()
    {
        settingsActive = true;
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Back() 
    {
        settingsActive = false;
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void PostProcessingToggle() 
    {
        mainCamera.GetUniversalAdditionalCameraData().renderPostProcessing = !mainCamera.GetUniversalAdditionalCameraData().renderPostProcessing;
    }
}

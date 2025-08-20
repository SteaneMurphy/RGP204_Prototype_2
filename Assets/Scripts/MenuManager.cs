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

    public void NewGame()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().StopLoop();
        mainCamera.transform.Rotate(0f, 0f, -90f);
        mainCamera.fieldOfView = 15f;
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

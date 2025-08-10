using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene(1);              //main game scene
    }

    public void Settings()
    {
        print("Settings button clicked");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

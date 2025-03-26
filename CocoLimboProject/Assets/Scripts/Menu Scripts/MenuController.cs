using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject buttons;

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        buttons.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsMenu?.SetActive(false);
        buttons?.SetActive(true);
    }
}

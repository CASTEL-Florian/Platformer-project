using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseFirstSelected;
    [SerializeField] private GameObject settingsFirstSelected;
    
    private InputSettings inputs;
    
    private void OnEnable(){
        inputs = new InputSettings();
        inputs.MainMenu.Enable();
        inputs.MainMenu.Echap.performed += ctx => Pause();
    }
    
    public void Pause()
    {
        if(pauseMenu.activeSelf)
            pauseMenu.SetActive(false);
        else {
            if (settingsMenu.activeSelf)
                settingsMenu.SetActive(false);
            pauseMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(pauseFirstSelected);
        }
    }

    public void Settings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(settingsFirstSelected);
    }
}
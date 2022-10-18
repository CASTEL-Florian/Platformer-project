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
        inputs.Menus.Enable();
        inputs.Menus.Echap.performed += ctx => Pause();
    }
    
    private void OnDisable(){
        inputs.Menus.Echap.performed -= ctx => Pause();
        inputs.Menus.Disable();
    }

    public void Pause()
    {
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            if (settingsMenu.activeSelf)
                settingsMenu.SetActive(false);
            else
                Time.timeScale = 0;
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
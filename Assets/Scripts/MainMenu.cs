using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Fader fader;
    [SerializeField] private SelectionArrow arrow;
    private InputSettings inputs;

    private void OnEnable()
    {
        inputs = new InputSettings();
        inputs.MainMenu.Enable();
        inputs.MainMenu.MenuUp.performed += ctx => arrow.GoUp();
        inputs.MainMenu.MenuDown.performed += ctx => arrow.GoDown();
        inputs.MainMenu.MenuSelect.performed += ctx => arrow.PressButton();
    }

    public void LoadScene(int sceneIndex)
    {
        fader.TransitionToScene(sceneIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnDisable()
    {
        inputs.MainMenu.Disable();
        inputs.MainMenu.MenuUp.performed -= ctx => arrow.GoUp();
        inputs.MainMenu.MenuDown.performed -= ctx => arrow.GoDown();
        inputs.MainMenu.MenuSelect.performed -= ctx => arrow.PressButton();
    }
}

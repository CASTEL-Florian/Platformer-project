using UnityEngine;

namespace deprecated
{
    public class SettingsMenu : MainMenu
    {
        [SerializeField] private SettingsArrow settingsArrow;
        protected override void OnEnable()
        {
            inputs = new InputSettings();
            inputs.MainMenu.Enable();
            inputs.MainMenu.MenuUp.performed += ctx => settingsArrow.GoUp();
            inputs.MainMenu.MenuDown.performed += ctx => settingsArrow.GoDown();
            inputs.MainMenu.MenuRight.performed += ctx => settingsArrow.GoRight();
            inputs.MainMenu.MenuLeft.performed += ctx => settingsArrow.GoLeft();
            inputs.MainMenu.MenuSelect.performed += ctx => settingsArrow.PressButton();
        }

        protected override void OnDisable()
        {
            inputs.MainMenu.Disable();
            inputs.MainMenu.MenuUp.performed -= ctx => settingsArrow.GoUp();
            inputs.MainMenu.MenuDown.performed -= ctx => settingsArrow.GoDown();
            inputs.MainMenu.MenuRight.performed -= ctx => settingsArrow.GoRight();
            inputs.MainMenu.MenuLeft.performed -= ctx => settingsArrow.GoLeft();
            inputs.MainMenu.MenuSelect.performed -= ctx => settingsArrow.PressButton();
        }
    }
}
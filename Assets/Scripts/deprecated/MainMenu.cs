using UnityEngine;

namespace deprecated
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Fader fader;
        [SerializeField] private SelectionArrow arrow;
        protected InputSettings inputs;

        protected virtual void OnEnable()
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

        protected virtual void OnDisable()
        {
            inputs.MainMenu.Disable();
            inputs.MainMenu.MenuUp.performed -= ctx => arrow.GoUp();
            inputs.MainMenu.MenuDown.performed -= ctx => arrow.GoDown();
            inputs.MainMenu.MenuSelect.performed -= ctx => arrow.PressButton();
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class ControlsMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject controlsMenu2;
    [SerializeField] private GameObject mainFirstSelected;
    
    private InputSettings inputs;
    
    private void OnEnable(){
        inputs = new InputSettings();
        inputs.Menus.Enable();
        inputs.Menus.Echap.performed += ctx => Controls();
        inputs.Menus.Back.performed += ctx => Controls();
    }
    
    private void OnDisable(){
        inputs.Menus.Echap.performed -= ctx => Controls();
        inputs.Menus.Back.performed -= ctx => Controls();
        inputs.Menus.Disable();
    }

    public void Controls()
    {
        if (controlsMenu.activeSelf)
        {
            mainMenu.SetActive(true);
            controlsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(mainFirstSelected);
        }
        
        
        if (controlsMenu2.activeSelf)
        {
            mainMenu.SetActive(true);
            controlsMenu2.SetActive(false);
            EventSystem.current.SetSelectedGameObject(mainFirstSelected);
        }
    }
}

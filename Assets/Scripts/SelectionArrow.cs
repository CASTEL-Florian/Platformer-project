using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [System.Serializable]
    public class Tab
    {
        public List<Button> buttons;
        public List<float> offsets;
    }
    [SerializeField] private List<Tab> tabs;
    private int tabIndex = 0;
    private int buttonIndex = 0;
    private void Start()
    {
        UpdatePos();
    }
    public void GoUp()
    {
        buttonIndex -= 1;
        if (buttonIndex < 0)
            buttonIndex = tabs[tabIndex].buttons.Count - 1;
        UpdatePos();
    }
    public void GoDown()
    {
        buttonIndex += 1;
        if (buttonIndex >= tabs[tabIndex].buttons.Count)
            buttonIndex = 0;
        UpdatePos();
    }

    public void GoToButton(Button button)
    {
        buttonIndex = tabs[tabIndex].buttons.IndexOf(button);
        UpdatePos();
    }

    private void UpdatePos()
    {
        transform.position = new Vector2(tabs[tabIndex].buttons[buttonIndex].transform.position. x + tabs[tabIndex].offsets[buttonIndex], tabs[tabIndex].buttons[buttonIndex].transform.position.y);
    }

    public void PressButton()
    {
        tabs[tabIndex].buttons[buttonIndex].onClick.Invoke();
    }

    public void SwitchTab(int newTabIndex)
    {
        tabIndex = newTabIndex;
        buttonIndex = 0;
        UpdatePos();
    }
}

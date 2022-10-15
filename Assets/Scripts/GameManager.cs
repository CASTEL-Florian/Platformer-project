using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Fader fader;
    [SerializeField] private bool fadeInOnStart = true;

    public void Start()
    {
        if(fadeInOnStart) fader.FadeIn();
    }

    public void LoadScene(int sceneIndex)
    {
        fader.TransitionToScene(sceneIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

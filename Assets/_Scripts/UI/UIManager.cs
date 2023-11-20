using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void LoadScene(int index)
    {
        SceneController.LoadScene(index);
    }

    public void LoadMainMenu()
    {
        SceneController.LoadScene(0);
    }

    public void Restart()
    {
        SceneController.RestartScene();
    }
}

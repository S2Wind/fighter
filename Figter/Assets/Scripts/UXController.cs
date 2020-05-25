using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UXController : MonoBehaviour
{

    [SerializeField] SceneLoader sceneloader;

    [SerializeField] Canvas thisCanvas;

    public void PressPlayButton()
    {
        sceneloader.LoadNextScene();
        thisCanvas.enabled = false;
    }

    public void PressExitButton()
    {
        Application.Quit();
    }
}

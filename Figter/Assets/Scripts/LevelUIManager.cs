using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUIManager : MonoBehaviour
{

    [SerializeField] GameObject levelUiCanvas;

    public void SettingCanvasOn()
    {
        levelUiCanvas.SetActive(true);
        levelUiCanvas.GetComponent<Canvas>().sortingOrder = 2;
    }

    public void SettingCanvasOff()
    {
        levelUiCanvas.SetActive(false);
        levelUiCanvas.GetComponent<Canvas>().sortingOrder = 0;
    }
}

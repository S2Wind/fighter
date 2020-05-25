using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader:MonoBehaviour
{

    [SerializeField] Animator anmt;

    [SerializeField] float timeTrans;

    private static int curScene;


    private void Start()
    {
        if (CurScene == null)
            CurScene = 0;
        anmt = GetComponent<Animator>();
    }

    public static int CurScene { get => curScene; set => curScene = value; }

    public void LoadNextScene()
    {
        CurScene++;
        StartCoroutine(TransitionBetweenScene());
    }

    public void ReLoadScene()
    {
        StartCoroutine(TransitionBetweenScene());
    }

    public void LoadMenuScene()
    {
        CurScene = 0;
        StartCoroutine(TransitionBetweenScene());
    }

    IEnumerator TransitionBetweenScene()
    {
        anmt.SetTrigger("start");
        yield return new WaitForSeconds(timeTrans);
        SceneManager.LoadScene(CurScene);
    }
}

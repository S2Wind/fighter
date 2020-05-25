using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    [SerializeField] AudioClip backtrackClip1;

    AudioSource audioSound;

    void Start()
    {
        audioSound = GetComponent<AudioSource>();
        audioSound.clip = backtrackClip1;
        audioSound.Play();
    }
}

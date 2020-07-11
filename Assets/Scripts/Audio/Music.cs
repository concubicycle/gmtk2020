using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Music : MonoBehaviour
{
    FMOD.Studio.EventInstance MusicEvent;
    private Sanity Sanity;
    private float Intensity = 0f;
    private int currentLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        Sanity = GetComponent<Sanity> ();
        MusicEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Level 1");
        MusicEvent.start();
    }

    // Update is called once per frame
    void Update()
    {
        MusicEvent.setParameterByName("Intensity", Intensity);
        MusicEvent.setParameterByName("Level", currentLevel);
    }
}

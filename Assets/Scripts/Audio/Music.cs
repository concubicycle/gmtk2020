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
		var sanityAvg = FindObjectOfType<SanityAverage>();
		Intensity = 1-sanityAvg.AverageSanity/100;
		print(Intensity);
        MusicEvent.setParameterByName("Intensity", Intensity);
        MusicEvent.setParameterByName("Level", currentLevel);
    }
}


// var sanityAvg = FindObjectOfType<SanityAverage>();
// var val = sanityAvg.AverageSanity;
   
// var robot = FindObjectOfType<StandardRobot>();
// var sanity = robot.GetComponent<Sanity>();
// var points = sanity.SanityPoints;
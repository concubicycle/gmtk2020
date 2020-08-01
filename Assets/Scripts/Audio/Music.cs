using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;

public class Music : MonoBehaviour
{
    FMOD.Studio.EventInstance MusicEvent;
    private Sanity Sanity;
    private float Intensity = 0f;
    private int currentLevel = 1;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        Sanity = GetComponent<Sanity> ();
        MusicEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Soundtrack");
        MusicEvent.start();
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene ();
        currentLevel = getSceneNumberByName(currentScene.name);

        var sanityAvg = FindObjectOfType<SanityAverage>();
		Intensity = 1-sanityAvg.AverageSanity/100;

        if (Intensity!=1) {
            MusicEvent.setParameterByName("Intensity", Intensity);
        }

        MusicEvent.setParameterByName("Level", currentLevel);
    }

    int getSceneNumberByName(string name) {
    	switch (name) {
    		case "LevelOne":
    			return 1;    			
    		case "LevelTwo":
    			return 2;    			
    		case "LevelThree":
    			return 3;    			
    		case "LevelFour":
    			return 4;    			
    		case "LevelFive":
    			return 5;    			
    		default:
    			return 1;
    	}
    }
}

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
        MusicEvent.setParameterByName("Intensity", Intensity);
        MusicEvent.setParameterByName("Level", currentLevel);
    }

    int getSceneNumberByName(string name) {
    	switch (name) {
    		case "LevelOne":
    			return 1;
    			break;
    		case "LevelTwo":
    			return 2;
    			break;
    		case "LevelThree":
    			return 3;
    			break;
    		case "LevelFour":
    			return 4;
    			break;
    		case "LevelFive":
    			return 5;
    			break;
    		default:
    			return 1;
    			break;
    	}
    }
}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// // Object.DontDestroyOnLoad example.
// //
// // This script example manages the playing audio. The GameObject with the
// // "music" tag is the BackgroundMusic GameObject. The AudioSource has the
// // audio attached to the AudioClip.

// public class DontDestroy : MonoBehaviour
// {
//     void Awake()
//     {
//         GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

//         if (objs.Length > 1)
//         {
//             Destroy(this.gameObject);
//         }

//         DontDestroyOnLoad(this.gameObject);
//     }
// }
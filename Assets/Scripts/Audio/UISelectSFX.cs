using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class UISelectSFX : MonoBehaviour
{
    FMOD.Studio.EventInstance UISFXEvent;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void OnClick()
    {
        UISFXEvent = FMODUnity.RuntimeManager.CreateInstance("event:/UI/UI Select");
        UISFXEvent.start();
    }
}

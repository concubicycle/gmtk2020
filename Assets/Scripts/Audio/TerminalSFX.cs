using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class TerminalSFX : MonoBehaviour
{
    public const int RobotLayer = 9;

	FMOD.Studio.EventInstance TerminalSFXEvent;

    // Start is called before the first frame update
    void Start()
    {
        TerminalSFXEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Hacking Terminal");
        // TerminalSFXEvent.start();
    }

    void update() {
    	// if (!someGameObject.GetComponent<this.gameObject>IsHacked) {
    	// 	print("It's a Me!");
    	// }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    	Terminal terminal = this.gameObject.GetComponent<Terminal>();
    	bool isRobot = collision.gameObject.layer == RobotLayer;
    	bool isHacked = collision.gameObject.GetComponent<StandardRobot>().IsHacked;
    	if (isRobot && isHacked && !terminal.IsHacked) {
    		TerminalSFXEvent.start();
    	}

    	// print(terminal.GetComponent<Terminal>());
        // if (collision.gameObject.layer == 9)
        // {
        //     var rb = collision.gameObject.GetComponent<StandardRobot>();

        //     if (rb.IsHacked) {
        //         TerminalSFXEvent.setParameterByName("isHacked", 1f);
        // 		TerminalSFXEvent.setParameterByName("isBeingHacked", 1f);
        //     } else {
        //     	TerminalSFXEvent.start();
        //     }
        // }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // if (collision.gameObject.layer == 9)
        // {
        // 	TerminalSFXEvent.setParameterByName("isBeingHacked", 0f);
        // }
    }
}

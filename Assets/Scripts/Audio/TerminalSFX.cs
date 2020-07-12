using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts
{
	public class TerminalSFX : MonoBehaviour
	{

		FMOD.Studio.EventInstance TerminalSFXEvent;

	    public const int RobotLayer = 9;

	    // Start is called before the first frame update
	    void Start()
	    {
	        TerminalSFXEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Hacking Terminal");
	        TerminalSFXEvent.start();
	    }

	    private void OnTriggerEnter2D(Collider2D collision)
	    {
	        if (collision.gameObject.layer == RobotLayer)
	        {
	            var rb = collision.gameObject.GetComponent<StandardRobot>();

	            if (rb.IsHacked) {
	                TerminalSFXEvent.setParameterByName("isHacked", 1f);
	        		TerminalSFXEvent.setParameterByName("isBeingHacked", 1f);
	            } else {
	            	TerminalSFXEvent.start();
	            }
	        }
	    }
	    private void OnTriggerExit2D(Collider2D collision)
	    {
	        if (collision.gameObject.layer == RobotLayer)
	        {
	        	TerminalSFXEvent.setParameterByName("isBeingHacked", 0f);
	        }
	    }
	}
}

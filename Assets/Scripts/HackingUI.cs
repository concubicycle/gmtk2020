using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackingUI : MonoBehaviour
{
    public GameObject connectedRobot;
    StandardRobot robotScript;
    GameObject thisButton;
    HackingUIController controlScript;
    bool isActiveOne = false;

    public Image SelectedImage = null;

    private Image _originalButtonImage;

    private Button _button;

    void Start()
    {
        _button = GetComponent<Button>();
        thisButton = this.gameObject;
        robotScript = connectedRobot.GetComponent<StandardRobot>();
        controlScript = FindObjectOfType<HackingUIController>();

        _originalButtonImage = _button.image;
    }

    void OnButtonClick()
    {
        if(connectedRobot != null)
        {
            if(!isActiveOne && controlScript.IsButtonActive())
            {
                controlScript.SendMessage("DeconnectActive");
                _button.image = _originalButtonImage;
            }

            if (!isActiveOne && !controlScript.IsButtonActive()) { 
                isActiveOne = true;
                //mark connected robot somehow as being active, colored light etc
                controlScript.SendMessage("SetButton", this);
                robotScript.SendMessage("SetConnected", true);
                //if we get that far, open camera/room on which the robot is

                _button.image = SelectedImage ?? _originalButtonImage;
            } 
            else
            {
                DeconnectRobot();
            }
        }
    }

    void DeconnectRobot()
    {
        isActiveOne = false;
        //unmark connected robot
        robotScript.SendMessage("SetConnected", false);
        if (controlScript.IsThisActive(this))
        {
            controlScript.SendMessage("NullButton");
        }

    }

    void OnInsane()
    {
        DeconnectRobot();
        thisButton.SetActive(false);
    }
}

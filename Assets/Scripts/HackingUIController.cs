using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingUIController : MonoBehaviour
{

    HackingUI currentButton;

    private void SetButton(HackingUI cB)
    {
        currentButton = cB;
    }

    private void NullButton()
    {
        currentButton = null;
    }

    private void DeconnectActive()
    {
        currentButton.SendMessage("DeconnectRobot");
    }

    public bool IsButtonActive()
    {
        return currentButton != null;
    }

    public bool IsThisActive(HackingUI tB)
    {
        return tB == currentButton;
    }

    
}

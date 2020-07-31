using UnityEngine;

public class HackingUIController : MonoBehaviour
{
    HackableItemButton currentButton;

    private void SetButton(HackableItemButton cB)
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

    public bool IsThisActive(HackableItemButton tB)
    {
        return tB == currentButton;
    }

    
}

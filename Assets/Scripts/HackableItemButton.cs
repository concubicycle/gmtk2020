using Assets.Scripts;

using UnityEngine;
using UnityEngine.UI;

public class HackableItemButton : MonoBehaviour
{
    public  IHackable _connectedItem;
    public Text text;

    private Button _button;    

    public IHackable ConnectedItem
    {
        get => _connectedItem;
        set
        {
            _connectedItem = value;
            text.text = value.Name;
        }
    }

    void Start()
    {
        _button = GetComponent<Button>();        
    }

    void OnButtonClick()
    {
      
    }

    void DisconnectItem()
    {   

    }

    void OnInsane()
    {
        DisconnectItem();
        gameObject.SetActive(false);
    }
}

using Assets.Scripts;

using UnityEngine;
using UnityEngine.UI;


public class HackableItemButton : MonoBehaviour
{
    public event GameObjectEvent ButtonPressed;

    public Hackable _connectedItem;
    public Text text;

    private Button _button;    

    public Hackable ConnectedItem
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
        _button.onClick.AddListener(() => ButtonPressed?.Invoke(_connectedItem.gameObject));
    }
}

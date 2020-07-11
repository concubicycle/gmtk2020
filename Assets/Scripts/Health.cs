using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public delegate void OnKillHandler();
public delegate void OnHealthChanged(float hp, float hpMax);

public class Health : MonoBehaviour
{        
    public event OnKillHandler EntityKilledListeners;
    public event OnHealthChanged HealthChangedListeners;
        
    public float maxHp = 100.0f;

    [SerializeField]
    private float _healthPoints = 100;

    public float HealthPoints
    {
        get
        {
            return _healthPoints;
        }
        set
        {
            if (_healthPoints <= 0 && value <= 0)
                return;

            if (_healthPoints > 0 && value <= 0 && EntityKilledListeners != null)
            {
                _healthPoints = value;
                EntityKilledListeners();
                return;
            }

            _healthPoints = value;
          
            if (HealthChangedListeners != null)
                HealthChangedListeners(_healthPoints, maxHp);                
        }
    }

    void Update()
    {
        HealthPoints = _healthPoints;                
    }   
}

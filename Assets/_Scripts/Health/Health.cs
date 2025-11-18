using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    private int _startingHealth;
    private int _currentHealth;

    // private bool _isDamageble = false;

    
    public void SetStartingHealth(int startingHealth)
    {
        _startingHealth = startingHealth;
        _currentHealth = startingHealth;
    }

    public int GetStartingHealth()
    {
        return _startingHealth;
    }

    public int TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }
        return _currentHealth;
    }

}
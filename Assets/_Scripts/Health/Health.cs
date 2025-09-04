using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    private int _startingHealth;
    private int _currentHealth;

    public void SetStartingHealth(int startingHealth)
    {
        _startingHealth = startingHealth;
        _currentHealth = startingHealth;
    }

    public int GetStartingHealth()
    {
        return _startingHealth;
    }


}
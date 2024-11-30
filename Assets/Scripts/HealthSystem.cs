using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealthPoints = 100;
    [SerializeField] private UnityEvent OnDeath;
    private int currentHealthPoints;

    private void Awake()
    {
        currentHealthPoints = maxHealthPoints;
    }

    public void TakeDamage(int damage)
    {
        currentHealthPoints -= damage;
        if (currentHealthPoints <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealthPoints += healAmount;
        if (currentHealthPoints > maxHealthPoints)
        {
            currentHealthPoints = maxHealthPoints;
        }
    }

    private void Die()
    {
        OnDeath.Invoke();
    }

    public int GetCurrentHealth()
    {
        return currentHealthPoints;
    }

    public int GetMaxHealth()
    {
        return maxHealthPoints;
    }
}

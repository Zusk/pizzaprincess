using UnityEngine;

// Base class
public class EnemyManager : MonoBehaviour
{
    // Public field to set health from the inspector
    public int Health;

    // Virtual method to handle the enemy's death
    public virtual void OnHealthDepleted()
    {
        Debug.Log("Enemy defeated!");
    }

    // Method to reduce health and check for depletion
    public void ReduceHealth(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            OnHealthDepleted();
        }
    }
}
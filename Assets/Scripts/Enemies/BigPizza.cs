using UnityEngine;
using System.Collections.Generic;

public class BigPizza : EnemyManager
{
    public GameObject particleObject; // GameObject to instantiate as a particle effect
    public GameObject revealObject;   // GameObject to reveal when health is depleted
    public bool revealOnHeal = true; // Set default to true for game logic

    public override void OnHealthDepleted()
    {
        Debug.Log("Big Pizza is healed!");

        // Instantiate the particle effect at the current position with no rotation
        if (particleObject != null)
        {
            Instantiate(particleObject, transform.position, Quaternion.identity);
        }

        // Check if we should reveal another object
        if (revealOnHeal && revealObject != null)
        {
            revealObject.SetActive(true); // Reveal the specified object
        }

        // Hide Big Pizza itself
        GameObject.Destroy(this.gameObject);
    }
}

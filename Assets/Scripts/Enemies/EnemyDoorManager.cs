using UnityEngine;

// Derived class for door-specific behavior
public class EnemyDoorManager : EnemyManager
{
    // Reference to a transform that should be enabled when the door's health is depleted
    public Transform partToEnable;

    public override void OnHealthDepleted()
    {
        Debug.Log("Door breaks open!");
        
        if (partToEnable != null)
        {
            partToEnable.gameObject.SetActive(true); // Enable the referenced transform
        }

        // Disable this gameObject's transform
        this.transform.gameObject.SetActive(false);
    }
}

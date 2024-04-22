using UnityEngine;
using System.Collections.Generic;  // Required for List

// Derived class for Pizzite-specific behavior
public class EnemyPizziteManager : EnemyManager
{
    // List of GameObjects to potentially instantiate when the Pizzite's health is depleted
    public List<GameObject> objectsToInstantiate;

    public override void OnHealthDepleted()
    {
        Debug.Log("Pizzite is healed!");

        if (objectsToInstantiate != null && objectsToInstantiate.Count > 0)
        {
            GameObject selectedObject;

            if (objectsToInstantiate.Count == 1)
            {
                // If there is only one object in the list, use that object
                selectedObject = objectsToInstantiate[0];
            }
            else
            {
                // If there are multiple objects, pick a random one from the list
                int randomIndex = Random.Range(0, objectsToInstantiate.Count);
                selectedObject = objectsToInstantiate[randomIndex];
            }

            // Instantiate the selected object at the current position raised by half a unit upwards, using its original rotation
            Instantiate(selectedObject, transform.position + Vector3.up * 0.5f, selectedObject.transform.rotation);
        }

        // Disable this gameObject's transform
        this.gameObject.SetActive(false);
    }
}

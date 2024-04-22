using UnityEngine;
using System.Collections.Generic;

public class MinigameEnemy : EnemyManager
{
    public static int enemiesDefeated = 0; // Static counter to track defeated enemies
    public static int defeatTarget = 50;   // Target number of enemies to defeat
    public List<GameObject> objectsToInstantiate;
    private GameObject unlockObject;        // GameObject to enable upon minigame completion

    public override void OnHealthDepleted()
    {
        Debug.Log("Minigame enemy is healed!");

        if (objectsToInstantiate != null && objectsToInstantiate.Count > 0)
        {
            GameObject selectedObject;

            if (objectsToInstantiate.Count == 1)
            {
                selectedObject = objectsToInstantiate[0];
            }
            else
            {
                int randomIndex = Random.Range(0, objectsToInstantiate.Count);
                selectedObject = objectsToInstantiate[randomIndex];
            }

            Instantiate(selectedObject, transform.position + Vector3.up * 0.5f, selectedObject.transform.rotation);
        }

        gameObject.SetActive(false);
        enemiesDefeated++;
        CheckForUnlockCondition();
    }

    private void CheckForUnlockCondition()
    {
        if (enemiesDefeated >= defeatTarget)
        {
            Debug.Log("Minigame finished!");
            PerformUnlockActions();
        }
    }

    private void PerformUnlockActions()
    {
        Debug.Log("Performing unlock actions...");
        unlockObject = GameObject.Find("rewardKey");
        if (unlockObject != null)
        {
            unlockObject.SetActive(true);
            Debug.Log("Unlock object enabled!");
        }
        else
        {
            Debug.Log("No unlock object assigned!");
        }
    }
}

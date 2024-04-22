using UnityEngine;
//using UnityStandardAssets.Characters.FirstPerson;

public class PlayerTeleporter : MonoBehaviour
{
    public GameObject teleportTarget; // Assign this in the Inspector
    //private FirstPersonController firstPersonController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //In this version of unity, it doesn't like when you teleport a character controller like this? Have to disable the component first.
            CharacterController charController = other.GetComponent<CharacterController>();
            if (charController != null)
            {
                charController.enabled = false;
                other.transform.position = teleportTarget.transform.position;
                charController.enabled = true;
            }
            else{
                // Teleport the player to the target position
                other.transform.position = teleportTarget.transform.position;
            }

            // Adjust the player's camera to look in the direction of the target's rotation
            if (Camera.main != null)
            {
                Camera.main.transform.rotation = Quaternion.LookRotation(-teleportTarget.transform.forward);
            }

            // Reset the player's movement/velocity in the FirstPersonController script
            //if (firstPersonController != null)
            //{
            //    firstPersonController.ResetMovement();
            //}
        }
    }
}

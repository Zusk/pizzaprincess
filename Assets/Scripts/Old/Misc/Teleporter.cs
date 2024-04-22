//Basic script to teleport an object to a different location.
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform profab;

    private void LateUpdate()
    {
        TeleportTo();
    }

    private void TeleportTo()
    {
        if (profab != null)
        {
            Debug.Log("This fired?");
            transform.position = profab.position;
        }
        else
        {
            Debug.LogError("Profab not assigned!");
        }
    }
}


using UnityEngine;
using System.Collections;

public class Carousel_Activate : MonoBehaviour
{
    // Reference to the GameObject to activate; the Carousel
    public GameObject objectToActivate;

    // Reference to the GameObject to deactivate; the teddy
    public GameObject objectToDeactivate;

    // Reference to the AudioSource component; the song.
    public AudioSource audioSource;
    private bool init = false;

    void OnTriggerEnter(Collider other)
    {
        // Check if the GameObject entering the trigger has the tag "Player"; If its the player.
        //Also check if we have done this before - with init.
        if (other.CompareTag("Player") && !init)
        {
            //Start init, this only fires once.
            init = true;
            // Play the audio clip
            if (audioSource != null)
            {
                //Play the song first thing.
                audioSource.Play();
            }

            // Start the coroutine for delay. This is because no matter what there always seemed to be about a 0.3 second delay between an audio source getting the 'play' request, and
            //actually playing. I needed the eyeballs and the song to start at about the same time for spooky factor.
            StartCoroutine(ChangeObjectStatesAfterDelay(0.35f));
        }
    }

    IEnumerator ChangeObjectStatesAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Activate the specified GameObject
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }

        // Deactivate the specified GameObject
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
        }
        //Turns this gameobject off, its done its job.
        this.gameObject.SetActive(false);
    }
}

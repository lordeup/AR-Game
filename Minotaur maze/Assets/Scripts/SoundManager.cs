using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip walkingClip;
    [SerializeField] private AudioClip fightClip;

    public void PlayWalkingSound()
    {
        // audioSource.PlayOneShot(walkingClip);
    }

    public void PlayFightSound()
    {
        // audioSource.PlayOneShot(fightClip);
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}

using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private AudioClip _walkingClip;
    private AudioClip _fightClip;
    private AudioClip _deathClip;
    private AudioClip _threadClip;
    private AudioClip _winClip;

    private void Start()
    {
        _walkingClip = Resources.Load<AudioClip>("Sounds/Walking");
        _fightClip = Resources.Load<AudioClip>("Sounds/Fight");
        _deathClip = Resources.Load<AudioClip>("Sounds/Death");
        _threadClip = Resources.Load<AudioClip>("Sounds/Thread");
        _winClip = Resources.Load<AudioClip>("Sounds/Win");
    }

    public void PlayWalkingSound()
    {
        if (!IsPlaying())
        {
            PlaySound(_walkingClip);
        }
    }

    public void PlayFightSound()
    {
        PlaySound(_fightClip);
    }

    public void PlayDeathSound()
    {
        PlaySound(_deathClip);
    }

    public void PlayThreadSound()
    {
        PlaySound(_threadClip);
    }

    public void PlayWinSound()
    {
        PlaySound(_winClip);
    }

    public void Stop()
    {
        if (IsPlaying())
        {
            audioSource.Stop();
        }
    }

    private void PlaySound(AudioClip audioClip)
    {
        if (SceneController.IsNull(audioSource) || SceneController.IsNull(audioClip)) return;

        Stop();
        audioSource.PlayOneShot(audioClip);
    }

    private bool IsPlaying()
    {
        return audioSource.isPlaying;
    }
}

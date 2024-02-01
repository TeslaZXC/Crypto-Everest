using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Audio : MonoBehaviour
{
    public static Audio singleton { get; private set; }
    [SerializeField] private AudioClip[] clips;

    private void Start() => singleton = this;

    public void PlayCachedAudio(string clipName, float volume = 1, float pitch = 1)
    {
        AudioClip clip = FindClipByName(clipName);
        if (clip == null) return;

        AudioSource audioSource = new GameObject($"{clip.name}SFX", typeof(AudioSource)).GetComponent<AudioSource>();
        audioSource.transform.parent = transform;

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip, volume);
        Destroy(audioSource.gameObject, clip.length);
    }

    private AudioClip FindClipByName(string clipName)
    {
        foreach (AudioClip clip in clips)
        {
            if(clip.name == clipName)
                return clip;
        }

        return null;
    }
}

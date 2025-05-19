
using UnityEngine;

public static class AudioManager
{
    // Plays a sound effect on the given position
    // Pitch gets slightly changed every time to avoid repeating sounds
    // Volume can be changed aswell 
    
    public static void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f, float pitchRange = 0.1f)
    {
        if (clip == null) return;

        GameObject tempGO = new GameObject("TempSFX");
        tempGO.transform.position = position;

        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;

        float randomPitch = Random.Range(1f - pitchRange, 1f + pitchRange);
        audioSource.pitch = randomPitch;

        audioSource.Play();
        Object.Destroy(tempGO, clip.length / randomPitch); // destroy after it's done
    }
}


using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    private AudioSource audio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void Play(AudioClip clip) {
        audio.PlayOneShot(clip);
    }
}

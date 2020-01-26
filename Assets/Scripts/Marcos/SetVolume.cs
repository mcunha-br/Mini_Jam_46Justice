using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVolume : MonoBehaviour {

	public AudioSource audioSource;

    private void Update() {
        audioSource.volume = PlayerPrefs.GetFloat("global_volumes");
    }

}

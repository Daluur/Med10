using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : Singleton<AudioHandler> {

	public AudioClip attack;
	public AudioClip takeDamage;
	public AudioClip die;
	public AudioClip move;
	public AudioClip click;

	public AudioSource source;

	public void PlayAttack() {
		source.clip = attack;
		source.Play();
	}

	public void PlayTakeDamage() {
		source.clip = takeDamage;
		source.Play();
	}

	public void PlayDie() {
		source.clip = die;
		source.Play();
	}

	public void PlayMove() {
		source.clip = move;
		source.Play();
	}

	public void PlayClick() {
		source.clip = click;
		source.Play();
	}
}

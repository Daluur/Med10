using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : Singleton<AudioHandler> {

	public AudioClip[] attack;
	public AudioClip[] takeDamage;
	public AudioClip die;
	public AudioClip[] move;
	public AudioClip[] click;

	public AudioSource source;

	protected override void Awake() {
		base.Awake();
		DontDestroyOnLoad (gameObject);
	}

	public void PlayAttack() {
		source.clip = attack[Random.Range(0, attack.Length)];
		source.Play();
	}

	public void PlayAttack(int i) {
		source.clip = attack[i];
		source.Play();
	}

	public void PlayTakeDamage() {
		source.clip = takeDamage[Random.Range(0, takeDamage.Length)];
		source.Play();
	}

	public void PlayTakeDamage(int i) {
		source.clip = takeDamage[i];
		source.Play();
	}

	public void PlayDie() {
		source.clip = die;
		source.Play();
	}

	public void PlayMove() {
		source.clip = move[Random.Range(0, move.Length)];
		source.Play();
	}

	public void PlayClick() {
		source.clip = click[Random.Range(0, click.Length)];
		source.Play();
	}

	public void PlayClick(int i) {
		source.clip = click[i];
		source.Play();
	}

	public void PlayInteraction() {
		Debug.Log("Interaction needs a sound");
	}

	public void PlayEnterCombat() {
		Debug.Log("Enter combat needs a sound");
	}

}
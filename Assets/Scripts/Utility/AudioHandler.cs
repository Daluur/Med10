using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : Singleton<AudioHandler> {

	public AudioClip[] attack;
	public AudioClip[] takeDamage;
	public AudioClip die;
	public AudioClip[] move;
	public AudioClip[] click;
	public AudioClip combatMusic;
	public AudioClip overWorldMusic;
	public AudioClip[] inventory;
	public AudioClip purchase;
	public AudioClip[] window;


	public AudioSource effectSource;
	public AudioSource BGSource;
	public AudioSource specificSource;

	protected override void Awake() {
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	public void PlayAttack() {
		effectSource.clip = attack[Random.Range(0, attack.Length)];
		effectSource.Play();
	}

	public void PlayAttack(int i) {
		effectSource.clip = attack[i];
		effectSource.Play();
	}

	public void PlayTakeDamage() {
		effectSource.clip = takeDamage[Random.Range(0, takeDamage.Length)];
		effectSource.Play();
	}

	public void PlayTakeDamage(int i) {
		effectSource.clip = takeDamage[i];
		effectSource.Play();
	}

	public void PlayDie() {
		effectSource.clip = die;
		effectSource.Play();
	}

	public void PlayMove() {
		specificSource.clip = move[Random.Range(0, move.Length)];
		specificSource.Play();
	}

	public void PlayClick() {
		effectSource.clip = click[Random.Range(0, click.Length)];
		effectSource.Play();
	}

	public void PlayClick(int i) {
		effectSource.clip = click[i];
		effectSource.Play();
	}

	public void PlayInteraction() {
		Debug.Log("Interaction needs a sound");
	}

	public void PlayEnterCombat() {
		Debug.Log("Enter combat needs a sound");
	}

	public void PlayWinSound() {
		Debug.Log("needs a sound");
	}

	public void PlayLoseSound() {
		Debug.Log("needs a sound");
	}

	public void PlayQuestComplete() {
		Debug.Log("needs a sound");
	}

	public void PlayCollectGold() {
		Debug.Log("needs a sound");
	}

	public void PlayCollectKeyItem() {
		Debug.Log("needs a sound");
	}

	public void PlayOpenShop() {
		Debug.Log("needs a sound");
	}

	/// <summary>
	/// Quest / Inventory
	/// </summary>
	public void PlayOpenWindow() {
		effectSource.clip = window[0];
		effectSource.Play();
	}

	public void PlayCloseWindow() {
		effectSource.clip = window[1];
		effectSource.Play();
	}

	public void PlayQuestUpdate() {
		Debug.Log("needs a sound");
	}

	public void MoveInventorySound() {
		effectSource.clip = inventory[0];
		effectSource.Play();
	}

	public void SwapInventorySound() {
		effectSource.clip = inventory[1];
		effectSource.Play();
	}

	public void PurchaseSound() {
		effectSource.clip = purchase;
		effectSource.Play();
	}

	public void StartOWBGMusic() {
		BGSource.clip = overWorldMusic;
		BGSource.Play();
	}

	public void StartCWBGMusic() {
		BGSource.clip = combatMusic;
		BGSource.Play();
	}
}
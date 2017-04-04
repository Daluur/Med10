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

	public AudioSource[] effectSource;
	public AudioSource BGSource;
	public AudioSource specificSource;

	//public Dictionary<int, int> sourcesUsed = new Dictionary<int, int>(); //Only used for debug.

	protected override void Awake() {
		base.Awake();
		DontDestroyOnLoad(gameObject);
		/*for (int i = 0; i < effectSource.Length; i++) { //Only used for debug.
			sourcesUsed.Add(i, 0);
		}*/
	}

	//Only used for debug.
	/*void Update() {
		if (Input.GetKeyDown(KeyCode.S)) {
			for (int i = 0; i < effectSource.Length; i++) {
				Debug.Log(i + " used: " + sourcesUsed[i] +" times");
			}
		}
	}*/

	void PlayEffectSound(AudioClip clip) {
		for (int i = 0; i < effectSource.Length; i++) {
			if (!effectSource[i].isPlaying) {
				effectSource[i].clip = clip;
				effectSource[i].Play();
				//sourcesUsed[i] = sourcesUsed[i] + 1; //Only used for debug.
				return;
			}
		}
		Debug.LogWarning("Tried to play an effect sound, but no sources were avaible. We might need to add more.");
	}

	public void PlayAttack() {
		PlayEffectSound(attack[Random.Range(0, attack.Length)]);
	}

	public void PlayAttack(int i) {
		PlayEffectSound(attack[i]);
	}

	public void PlayTakeDamage() {
		PlayEffectSound(takeDamage[Random.Range(0, takeDamage.Length)]);
	}

	public void PlayTakeDamage(int i) {
		PlayEffectSound(takeDamage[i]);
	}

	public void PlayDie() {
		PlayEffectSound(die);
	}

	public void PlayMove() {
		specificSource.clip = move[Random.Range(0, move.Length)];
		specificSource.Play();
	}

	public void PlayClick() {
		PlayEffectSound(click[Random.Range(0, click.Length)]);
	}

	public void PlayClick(int i) {
		PlayEffectSound(click[i]);
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
		PlayEffectSound(window[0]);
	}

	public void PlayCloseWindow() {
		PlayEffectSound(window[1]);
	}

	public void PlayQuestUpdate() {
		Debug.Log("needs a sound");
	}

	public void MoveInventorySound() {
		PlayEffectSound(inventory[0]);
	}

	public void SwapInventorySound() {
		PlayEffectSound(inventory[1]);
	}

	public void PurchaseSound() {
		PlayEffectSound(purchase);
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
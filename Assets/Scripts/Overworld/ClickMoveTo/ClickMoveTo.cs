using UnityEngine;

public class ClickMoveTo : MonoBehaviour {

	public Animator animator;

	private void Update() {
		if (!AnimatorIsPlaying()) {
			Destroy(gameObject);
		}
	}

	bool AnimatorIsPlaying(){
		return animator.GetCurrentAnimatorStateInfo(0).length >
		       animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
	}
}

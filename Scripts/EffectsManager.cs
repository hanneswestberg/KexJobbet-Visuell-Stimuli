using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EffectsManager : MonoBehaviour {

	public GameObject[] starExplodeEffects;
	public GameObject[] rankEffects;
	public Button[] allAnimButtons;

	[HideInInspector] public Vector3 starCountEffectPosition = new Vector3(9.18f, -3.7f, 0f);

	[HideInInspector] private Vector3 rankEffectPosition = new Vector3(0f, 0.7f, 0f);
	[HideInInspector] private Vector3 rankEffectPositionLower = new Vector3(0f, -1f, 0f);

	public Vector3 RankEffectPositionLower {
		get {
			return rankEffectPositionLower;
		}
	}

	public Vector3 RankEffectPosition {
		get {
			return rankEffectPosition;
		}
	}



	public bool effectsEnabled = true;

	void Start(){

	}

	public void AllButtonsEffectSetActive(bool active){
		if(effectsEnabled == true){
			if(active == true){
				EnableEffectsOnButtons();
			}else{
				DisableEffectsOnButtons();
			}
		}else{
			DisableEffectsOnButtons();
		}
	}

	void EnableEffectsOnButtons(){
		foreach(Button theButton in allAnimButtons){
			theButton.GetComponent<Animator>().enabled = true;
			theButton.transition = Selectable.Transition.Animation;
		}
	}

	void DisableEffectsOnButtons(){
		foreach(Button theButton in allAnimButtons){
			theButton.GetComponent<Animator>().enabled = false;
			theButton.transition = Selectable.Transition.ColorTint;
		}
	}

	// Creates an effect and places it at a certain position
	public void CreateEffectAt(GameObject effect_GO, Vector3 pos, Quaternion rot = default(Quaternion)){
		if (effectsEnabled == true){
			GameObject effect = (GameObject)GameObject.Instantiate(effect_GO, pos, rot);
			effect.transform.SetParent(this.transform);
			effect.name = "Particle_Effect_"+effect_GO.name;
			effect.layer = 10;
			StartCoroutine(DestroyEffectWhenDone(effect, effect.GetComponent<ParticleSystem>().duration));
		}
	}

	// Destroys an effect when it is done
	IEnumerator DestroyEffectWhenDone(GameObject effect, float effectDuration){
		yield return new WaitForSeconds(effectDuration);
		Destroy(effect);
	}
}

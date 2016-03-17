using UnityEngine;
using System.Collections;

public class EffectsManager : MonoBehaviour {

	public GameObject[] starExplodeEffects;
	public GameObject[] rankEffects;

	public bool effectsEnabled = true;


	// Creates an effect and places it at a certain position
	public void CreateEffectAt(GameObject effect_GO, Vector3 pos, Quaternion rot){
		if (effectsEnabled == true){
			GameObject effect = (GameObject)GameObject.Instantiate(effect_GO, pos, rot);
			effect.transform.SetParent(this.transform);
			effect.name = "Particle_Effect";
			StartCoroutine(DestroyEffectWhenDone(effect, effect.GetComponent<ParticleSystem>().duration));
		}
	}

	// Destroys an effect when it is done
	IEnumerator DestroyEffectWhenDone(GameObject effect, float effectDuration){
		yield return new WaitForSeconds(effectDuration);
		Destroy(effect);
	}
}

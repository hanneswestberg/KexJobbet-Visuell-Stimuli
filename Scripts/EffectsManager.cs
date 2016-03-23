using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EffectsManager : MonoBehaviour {

	public GameObject[] starExplodeEffects;
	public GameObject[] rankEffects;

	[HideInInspector] public Vector3 starCountEffectPosition = new Vector3(9.18f, -3.7f, 0f);

	public bool effectsEnabled = true;

	void Start(){

	}

	// Creates an effect and places it at a certain position
	public void CreateEffectAt(GameObject effect_GO, Vector3 pos, Quaternion rot = default(Quaternion)){
		if (effectsEnabled == true){
			GameObject effect = (GameObject)GameObject.Instantiate(effect_GO, pos, rot);
			effect.transform.SetParent(this.transform);
			effect.name = "Particle_Effect";
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

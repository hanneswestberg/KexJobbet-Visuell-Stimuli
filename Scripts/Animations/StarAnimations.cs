using UnityEngine;
using System.Collections;

public class StarAnimations : MonoBehaviour {

	Transform starGatherPointOnCompletedTask;
	EffectsManager effectsManager;
	UITaskScriptManager ui_task;

	bool readyToAnimateToGatherPoint = false;

	// Position
	float posX;
	float posY;
	Vector3 currentPos;
	Vector3 lastPos;

	//Rotation
	float rot;
	Vector3 rotVector;

	// Animations
	bool isFading = false;

	void Start(){
		starGatherPointOnCompletedTask = GameObject.Find("StarHolderGatherPoint").transform;
		effectsManager = GameObject.Find("EffectsManager").GetComponent<EffectsManager>();
		ui_task = GameObject.Find("TaskManager").GetComponent<UITaskScriptManager>();
	}

	public void StartEffects(string animationName){
		starGatherPointOnCompletedTask = GameObject.Find("StarHolderGatherPoint").transform;
		effectsManager = GameObject.Find("EffectsManager").GetComponent<EffectsManager>();
		ui_task = GameObject.Find("TaskManager").GetComponent<UITaskScriptManager>();

		if(effectsManager.effectsEnabled == true){
			GetComponent<Animator>().SetTrigger(animationName);
			//GetComponent<Animator>().Play("", 0, Random.Range(0f, 0.5f));
		}
	}

	public void PopOutEffect(){
		effectsManager.CreateEffectAt(effectsManager.starExplodeEffects[0], transform.position); 
		GetComponent<Animator>().SetTrigger("PopOut");
		StartCoroutine(WaitForPopOut());
	}

	public void DestroyStar(){
		ui_task.AddStarsToTotal(1);
		Destroy(gameObject);
	}
		

	void Update () {
		if (readyToAnimateToGatherPoint == true){
			// ANIMATE POSITION
			lastPos = transform.position;
			posX = Mathf.Lerp(transform.position.x, starGatherPointOnCompletedTask.position.x, 0.03f);
			posY = Mathf.Lerp(transform.position.y, starGatherPointOnCompletedTask.position.y, 0.03f);
			currentPos = new Vector3(posX, posY,transform.position.z);
			transform.position = currentPos;

			//ANIMATE ROTATION
			rot = Vector3.Distance(lastPos, currentPos);
			rotVector = new Vector3(0, 0, Mathf.Clamp(-rot*15, -30f, 30f));
			transform.rotation = Quaternion.Euler(rotVector);

			//DESTROY WHEN NEAR
			if (Vector3.Distance(transform.position, starGatherPointOnCompletedTask.position) <= 5f && isFading == false){
				isFading = true;
				GetComponent<Animator>().SetTrigger("FadeOut");
				StartCoroutine(DestroyAfterFadeOut());
			}
		}
	}

	IEnumerator WaitForPopOut(){
		yield return new WaitForSeconds(0.3f);
		readyToAnimateToGatherPoint = true;
	}

	IEnumerator DestroyAfterFadeOut(){
		effectsManager.CreateEffectAt(effectsManager.starExplodeEffects[1], effectsManager.starCountEffectPosition);
		yield return new WaitForSeconds(0.5f);
		ui_task.AddStarsToTotal(1);
		Destroy(this.gameObject);
	}
}

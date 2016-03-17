using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TaskStarManager : MonoBehaviour {

	public EffectsManager effectManager;
	public Text taskText;
	public GameObject starPrefab;
	public Transform starHolder;
	[SerializeField] private bool isTomorrowsTask;

	private List<Vector3[]> starPosList = new List<Vector3[]>(){
		new Vector3[]{new Vector3(0, 0, 0)}, 
		new Vector3[]{new Vector3(-16, 0, 0), new Vector3(17, 0, 0)},
		new Vector3[]{new Vector3(-15, 8, 0), new Vector3(16, 8, 0), new Vector3(0, -15, 0)}};

	// Checks if effects are enabled, if not, then we want to disable animations
	void Start(){
		if(effectManager.effectsEnabled == false){
			transform.GetComponent<Animator>().enabled = false;
			transform.GetComponent<Button>().transition = Selectable.Transition.ColorTint;
		}
	}

	public void StartStarAnimations(string animationName){
		foreach(Transform child in starHolder){
			child.GetComponent<StarAnimations>().StartEffects(animationName);
		}
	}

	// Spawns stars for this task
	public void CreateTaskStars(int starCount){
		for (int i = 0; i < starCount; i++) {
			GameObject star = (GameObject)GameObject.Instantiate(starPrefab, this.transform.position, Quaternion.identity);
			star.transform.SetParent(starHolder);
			star.name = "Star_" + (i+1).ToString();
			star.transform.localPosition = starPosList[starCount-1][i];
		}
	}

	public void AnimateTaskStar(){
		int numberOfChilds = 0;
		foreach(Transform child in starHolder){
			if(effectManager.effectsEnabled == true){
				StartCoroutine(PopOutQue(child, numberOfChilds));
				numberOfChilds++;
			}else{
				child.GetComponent<StarAnimations>().DestroyStar();
			}
		}
	}

	public void DestroyAllStarsConnectedToTask(){
		foreach(Transform child in starHolder){
			child.GetComponent<StarAnimations>().DestroyStar();
		}
	}

	IEnumerator PopOutQue(Transform child, int childNumber){
		yield return new WaitForSeconds(childNumber*0.2f);
		child.GetComponent<StarAnimations>().PopOutEffect();
	}
}

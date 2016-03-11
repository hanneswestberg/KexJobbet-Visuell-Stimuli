using UnityEngine;
using System.Collections;

public class TaskStarManager : MonoBehaviour {

	public EffectsManager effectManager;
	public GameObject starPrefab;
	public Transform starHolder;
	int numberOfChilds = 0;

	Vector3[] starPos = new Vector3[]{new Vector3(200, 0, 0), new Vector3(215, 0, 0), new Vector3(230, 0, 0)};

	public void CreateTaskStars(int starCount){
		for (int i = 0; i < starCount; i++) {
			GameObject star = (GameObject)GameObject.Instantiate(starPrefab, this.transform.position, Quaternion.identity);
			star.transform.SetParent(starHolder);
			star.transform.localPosition = starPos[i];
		}
	}

	public void AnimateTaskStar(){
		numberOfChilds = 0;
		foreach(Transform child in starHolder){
			if(effectManager.effectsEnabled == true){
				StartCoroutine(PopOutQue(child, numberOfChilds));
				numberOfChilds++;
			}else{
				child.GetComponent<StarAnimations>().DestroyStar();
			}
		}
	}

	IEnumerator PopOutQue(Transform child, int childNumber){
		yield return new WaitForSeconds(childNumber*0.2f);
		child.GetComponent<StarAnimations>().PopOutEffect();
	}
}

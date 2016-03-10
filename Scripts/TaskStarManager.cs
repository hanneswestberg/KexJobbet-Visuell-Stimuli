using UnityEngine;
using System.Collections;

public class TaskStarManager : MonoBehaviour {

	public GameObject starPrefab;
	public Transform starHolder;

	Vector2[] starPos = new Vector2[]{new Vector2(185, 0), new Vector2(150, 0), new Vector2(115, 0)};


	public void CreateTaskStars(int starCount){
		for (int i = 0; i < starCount; i++) {
			GameObject star = (GameObject)GameObject.Instantiate(starPrefab, this.transform.position, Quaternion.identity);
			star.transform.SetParent(starHolder);
			star.transform.localPosition = starPos[i];
		}
	}


	public void DestroyTaskStar(){
		foreach(Transform child in starHolder){
			Destroy(child.gameObject);
		}
	}
}

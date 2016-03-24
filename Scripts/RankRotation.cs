using UnityEngine;
using System.Collections;

public class RankRotation : MonoBehaviour {

	public EffectsManager effectMan;
	public GameObject rankPlane;
	public LayerMask mask;

	[SerializeField] private float maxRotAngle = 15f;
	[SerializeField] private float reactDistance = 5f;
	[Range(1f, 50f)] public float reactSpeed = 1f;
	float currentReactSpeed = 0f;

	Vector3 orginalRot;
	float planeRotX;
	float planeRotY;
	Vector3 point;
	Ray ray;
	RaycastHit hit;

	private bool rotationIsOn;

	void Start(){
		orginalRot = transform.rotation.eulerAngles;
	}

	void Update () {
		if(rotationIsOn == true) {

			currentReactSpeed = reactSpeed*Time.deltaTime;

			// Casts a ray to the cursorPlane and calculates the rotation of the rank
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit, 100f, mask)){
				point = hit.transform.InverseTransformPoint(hit.point);

				// Rank Rotation:
				if (point.magnitude <= reactDistance){
					planeRotX = orginalRot.x + Mathf.Clamp(point.z*maxRotAngle*10, -maxRotAngle, maxRotAngle);
					planeRotY = orginalRot.y + Mathf.Clamp(point.x*maxRotAngle*10, -maxRotAngle, maxRotAngle);

					rankPlane.transform.rotation = Quaternion.Lerp(rankPlane.transform.rotation, Quaternion.Euler(planeRotX, planeRotY, 0f), currentReactSpeed);
				}else{
					rankPlane.transform.rotation = Quaternion.Lerp(rankPlane.transform.rotation, Quaternion.Euler(orginalRot), currentReactSpeed);
				}
			}
		}
	}

	public void RankShadowsSetActive(bool active){
		if(active == true){
			rankPlane.transform.GetChild(0).gameObject.SetActive(true);
		}else {
			rankPlane.transform.GetChild(0).gameObject.SetActive(false);
		}
	}

	public void RankEffectsSetActive(bool active){
		if(active == true){
			rotationIsOn = true;
		} else{
			rotationIsOn = false;

		}
	}


}

using UnityEngine;
using System.Collections;

public class RankRotation : MonoBehaviour {

	public GameObject rankPlane;
	public LayerMask mask;

	[SerializeField] private float maxRotAngle = 15f;
	[SerializeField] private float reactDistance = 0.25f;
	[Range(0.01f, 1f)] public float reactSpeed = 0.05f;

	Vector3 orginalRot;
	float planeRotX;
	float planeRotY;
	Vector3 point;
	Ray ray;
	RaycastHit hit;

	void Start(){
		orginalRot = transform.rotation.eulerAngles;
	}

	void Update () {
		// Casts a ray to the cursorPlane and calculates the rotation of the rank
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit, 100f, mask)){
			point = hit.transform.InverseTransformPoint(hit.point);

			// Rank Rotation:
			if (point.magnitude <= reactDistance){
				planeRotX = orginalRot.x + Mathf.Clamp(point.z*maxRotAngle*10, -maxRotAngle, maxRotAngle);
				planeRotY = orginalRot.y + Mathf.Clamp(point.x*maxRotAngle*10, -maxRotAngle, maxRotAngle);

				rankPlane.transform.rotation = Quaternion.Lerp(rankPlane.transform.rotation, Quaternion.Euler(planeRotX, planeRotY, 0f), reactSpeed);
			}else{
				rankPlane.transform.rotation = Quaternion.Lerp(rankPlane.transform.rotation, Quaternion.Euler(orginalRot), reactSpeed);
			}
		}
	}
}

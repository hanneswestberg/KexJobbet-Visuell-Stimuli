using UnityEngine;
using System.Collections;

public class RankRotation : MonoBehaviour {

	public GameObject rankPlane;
	public LayerMask mask;
	public GameObject rankLight;


	Vector3 orginalRot = new Vector3(90f, 200f, 0f);
	float planeRotX;
	float planeRotY;
	Vector3 planeRotVector;
	Vector3 point;
	Ray ray;
	RaycastHit hit;
	float maxRotAngle = 15f;


	Vector3 lightPos;

	void Update () {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit, 100f, mask)){
			point = hit.transform.InverseTransformPoint(hit.point);

			// Rank Rotation:
			if (point.magnitude <= 0.25f){
				planeRotX = orginalRot.x + Mathf.Clamp(point.z*maxRotAngle*10, -maxRotAngle, maxRotAngle);
				planeRotY = orginalRot.y + Mathf.Clamp(point.x*maxRotAngle*10, -maxRotAngle, maxRotAngle);

				rankPlane.transform.rotation = Quaternion.Lerp(rankPlane.transform.rotation, Quaternion.Euler(planeRotX, planeRotY, 0f), 0.05f);
			}else{
				rankPlane.transform.rotation = Quaternion.Lerp(rankPlane.transform.rotation, Quaternion.Euler(orginalRot), 0.05f);
			}





			// Light Position:
			//lightPos = new Vector3(point.x, 30f,point.z);
			//lightPos = hit.transform.TransformPoint(lightPos);

			//rankLight.transform.position = lightPos;
		}
	}
}

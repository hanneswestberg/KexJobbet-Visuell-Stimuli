using UnityEngine;
using System.Collections;

public class WWWtest : MonoBehaviour {

	string database_url = "http://xml.csc.kth.se/~thomasvp/DM2517/kex/index.php";
	string userName = "asd";

	
	IEnumerator Start(){
		WWWForm form = new WWWForm();

		form.AddField("userName", userName);

		WWW download = new WWW(database_url, form);
		yield return download;

		if(!string.IsNullOrEmpty(download.error)){
			print("Error downloading: " + download.error);
		}else{
			Debug.Log(download.text);
		}
	}
}

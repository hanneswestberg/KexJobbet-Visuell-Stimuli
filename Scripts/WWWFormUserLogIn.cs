using UnityEngine;
using System.Collections;

public class WWWFormUserLogIn : MonoBehaviour {

	string database_url = "http://xml.csc.kth.se/~thomasvp/DM2517/kex/index.php";
	string userName = "";
	string userPassword = "";

	public void UpdatePassword (string value) {
		userPassword = value;
	}

	public void UpdateUserName (string value) {
		userName = value;
	}

	public IEnumerator SendLogInInfo(){

		WWWForm form = new WWWForm();

		form.AddField("userName", userName);
		form.AddField("userPassword", userPassword); //EJ KRYPTERAD

		Debug.Log(form.ToString());

		WWW download = new WWW(database_url, form);

		yield return download;

		if(!string.IsNullOrEmpty(download.error)){
			print("Error downloading: " + download.error);
		}else{
			Debug.Log(download.text);
		}
	}
}

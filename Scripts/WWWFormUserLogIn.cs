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

	public void SendLogInInfo(){
		StartCoroutine(SendLogInInfoIEnumerator());
	}


	IEnumerator SendLogInInfoIEnumerator(){
		WWWForm form = new WWWForm();

		form.AddField("userName", userName);
		form.AddField("userPassword", userPassword); //EJ KRYPTERAD

		WWW download = new WWW(database_url, form);

		yield return download;

		if(!string.IsNullOrEmpty(download.error)){
			print("Error downloading: " + download.error);
		}else{
			if(download.text == "False"){
				this.GetComponent<UILogInScriptManager>().WrongUsernameOrPasswordTextPopUp();
				this.GetComponent<UILogInScriptManager>().LogInToApplication();
			}else{
				Debug.Log(download.text);
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class WWWFormUserLogIn : MonoBehaviour {

	string database_url = "http://xml.csc.kth.se/~thomasvp/DM2517/kex/index.php?login=1";
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

	public string UserName {
		get {
			return userName;
		}
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
			if(download.text == "True"){
				this.GetComponent<UILogInScriptManager>().LogInToApplication();
				Debug.Log(download.text);
				GameObject.Find("TaskManager").GetComponent<WWWFormTasks>().RequestTasksFromDatabase();
			}else{
				Debug.Log(download.text);
				this.GetComponent<UILogInScriptManager>().WrongUsernameOrPasswordTextPopUp();
			}
		}
	}
}

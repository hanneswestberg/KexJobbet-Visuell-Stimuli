using UnityEngine;
using System.Collections;

public class WWWFormUserLogIn : MonoBehaviour {

	public EffectsManager effectMan;
	public bool effectsInverted = false;

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

	void SetEffectsEnabled(bool active){
		if(effectsInverted == false){
			effectMan.effectsEnabled = active;
		} else{
			if(active == true){
				effectMan.effectsEnabled = false;
			} else{
				effectMan.effectsEnabled = true;
			}
		}
	}


	IEnumerator SendLogInInfoIEnumerator(){
		WWWForm form = new WWWForm();

		form.AddField("userName", userName);
		form.AddField("userPassword", userPassword); //EJ KRYPTERAD

		WWW download = new WWW(database_url, form);

		this.GetComponent<UILogInScriptManager>().ButtonLogInSetActive(false);
		yield return download;

		this.GetComponent<UILogInScriptManager>().ButtonLogInSetActive(true);


		if(!string.IsNullOrEmpty(download.error)){
			print("Error downloading: " + download.error);
		}else{
			if(download.text == "True, 1"){
				SetEffectsEnabled(true);
				this.GetComponent<UILogInScriptManager>().LogInToApplication();
				Debug.Log("Login Successful");
				GameObject.Find("TaskManager").GetComponent<WWWFormTasks>().RequestTasksFromDatabase();
			}else if(download.text == "True, 0"){
				SetEffectsEnabled(false);
				this.GetComponent<UILogInScriptManager>().LogInToApplication();
				Debug.Log("Login Successful");
				GameObject.Find("TaskManager").GetComponent<WWWFormTasks>().RequestTasksFromDatabase();
			}else{
				this.GetComponent<UILogInScriptManager>().WrongUsernameOrPasswordTextPopUp();
			}
		}
	}
}

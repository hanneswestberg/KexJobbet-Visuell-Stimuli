using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScriptManager : MonoBehaviour {

	public Text inputFieldUserName;
	public Text inputFieldPassword;

	void Start(){
		
	}


	public void InputFieldChangeUserName(){
		this.GetComponent<WWWFormUserLogIn>().UpdateUserName(inputFieldUserName.text);
	}

	public void InputFieldChangePassword(){
		this.GetComponent<WWWFormUserLogIn>().UpdatePassword(inputFieldPassword.text);
	}

	public void ButtonLogIn(){
		this.GetComponent<WWWFormUserLogIn>().SendLogInInfo();
	}
}

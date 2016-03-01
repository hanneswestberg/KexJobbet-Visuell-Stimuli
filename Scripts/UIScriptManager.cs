﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIScriptManager : MonoBehaviour {
	
	public Text inputFieldUserName;
	public InputField inputFieldPassword;
	public GameObject wrongUsernameOrPasswordText_GO;
	public GameObject logInButton_GO;
	public GameObject rememberInfo_GO;
	public GameObject autoLogIn_GO;

	EventSystem system;

	//LOCAL COOKIE SAVES
	string username = "";
	string password = "";
	int autoLogIn_int = 0;
	int rememberInfo_int = 0;

	void Start(){
		system = EventSystem.current;
		LoadUserInfo();
	}

	//HERE WE LOAD AND SAVE OUR COOKIES!!!

	void LoadUserInfo(){
		rememberInfo_int = (int)PlayerPrefs.GetInt("rememberInfo");

		if(rememberInfo_int == 1){
			username = (string)PlayerPrefs.GetString("username");
			password = (string)PlayerPrefs.GetString("password");
			autoLogIn_int = (int)PlayerPrefs.GetInt("autoLogIn");

			// We need to update our text on field inputs:

			inputFieldUserName.transform.parent.GetComponent<InputField>().text = username;
			inputFieldPassword.text = password;

			if(rememberInfo_int == 1){
				rememberInfo_GO.GetComponent<Toggle>().isOn = true;
			}
				
			if(autoLogIn_int == 1){
				autoLogIn_GO.GetComponent<Toggle>().isOn = true;

				ButtonLogIn();
			}
		}
	}

	void SaveChangedInfo(){
		PlayerPrefs.SetString("username", (string)inputFieldUserName.text);
		PlayerPrefs.SetString("password", (string)inputFieldPassword.text);
		PlayerPrefs.SetInt("rememberInfo", (int)rememberInfo_int);
		PlayerPrefs.SetInt("autoLogIn", (int)autoLogIn_int);

	}
		
	void ResetInfo(){
		PlayerPrefs.SetString("username", "");
		PlayerPrefs.SetString("password", "");
		PlayerPrefs.SetInt("rememberInfo", 0);
		PlayerPrefs.SetInt("autoLogIn", 0);
	}


	// Manual keybindings

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.Tab)){
			Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

			if (next != null)
			{
				InputField inputfield = next.GetComponent<InputField>();
				if (inputfield != null)
					inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

				system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
			}
		}

		if(Input.GetKeyDown(KeyCode.Return)){
			ButtonLogIn();
		}
	}

	// GUI callbacks

	public void WrongUsernameOrPasswordTextPopUp(){
		wrongUsernameOrPasswordText_GO.GetComponent<Animator>().Play("WrongUsernameOrPasswordAnimation", -1, 0f);
		logInButton_GO.SetActive(enabled);
	}

	public void InputFieldChangeUserName(){
		this.GetComponent<WWWFormUserLogIn>().UpdateUserName(inputFieldUserName.text);
	}

	public void InputFieldChangePassword(){
		this.GetComponent<WWWFormUserLogIn>().UpdatePassword(inputFieldPassword.text);
	}

	public void ToggleRememberInfo(){
		if(rememberInfo_int == 0){
			rememberInfo_int = 1;
		}else{
			rememberInfo_int = 0;
		}
	}

	public void ToggleAutoLogIn(){
		if(autoLogIn_int == 0){
			autoLogIn_int = 1;
		}else{
			autoLogIn_int = 0;
		}
	}
		
	public void ButtonLogIn(){
		//logInButton_GO.SetActive(false);
		this.GetComponent<WWWFormUserLogIn>().SendLogInInfo();

		if(rememberInfo_GO.GetComponent<Toggle>().isOn == true)
		{
			rememberInfo_int = 1;
			SaveChangedInfo();
		}else{
			ResetInfo();
		}
	}
}
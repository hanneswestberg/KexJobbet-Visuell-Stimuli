﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UILogInScriptManager : MonoBehaviour {

	public RankManager rankManager;
	public UITaskScriptManager uiTaskManager;
	public EffectsManager effectMan;
	public Text welcomeText;
	public Text inputFieldUserName;
	public InputField inputFieldPassword;
	public CanvasGroup[] canvasGroupArray;
	public CanvasGroup[] logInGroups;
	public GameObject[] cheatButtons;
	public GameObject wrongUsernameOrPasswordText_GO;
	public GameObject logInButton_GO;
	public GameObject rememberInfo_GO;

	EventSystem system;

	//LOCAL COOKIE SAVES
	string username = "";
	string password = "";
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

			// We need to update our text on field inputs:

			inputFieldUserName.transform.parent.GetComponent<InputField>().text = username;
			inputFieldPassword.text = password;

			// We also need to update our username and password in our WWWForm script

			this.GetComponent<WWWFormUserLogIn>().UpdateUserName(username);
			this.GetComponent<WWWFormUserLogIn>().UpdatePassword(password);

			if(rememberInfo_int == 1){
				rememberInfo_GO.GetComponent<Toggle>().isOn = true;
			}
		}
	}

	void SaveChangedInfo(){
		PlayerPrefs.SetString("username", (string)inputFieldUserName.text);
		PlayerPrefs.SetString("password", (string)inputFieldPassword.text);
		PlayerPrefs.SetInt("rememberInfo", (int)rememberInfo_int);
	}
		
	void ResetInfo(){
		PlayerPrefs.SetString("username", "");
		PlayerPrefs.SetString("password", "");
		PlayerPrefs.SetInt("rememberInfo", 0);
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
		
	public void ButtonLogIn(){
		this.GetComponent<WWWFormUserLogIn>().SendLogInInfo();

		if(rememberInfo_GO.GetComponent<Toggle>().isOn == true)
		{
			rememberInfo_int = 1;
			SaveChangedInfo();
		}else{
			ResetInfo();
		}
	}

	public void ButtonQuitApplication(){
		Application.Quit ();
	}


	void CanvasGroupsSetAlpha(CanvasGroup[] canvasG, float newAlpha){
		foreach(CanvasGroup canvasGroup in canvasG){
			canvasGroup.alpha = newAlpha;
		}
	}

	void CanvasGroupSetVisible(CanvasGroup canvasGroup, bool setVisible){
		if(setVisible == true){
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;

			if(effectMan.effectsEnabled == true){
				canvasGroup.GetComponent<Animator>().SetTrigger("LogIn");
			}else{
				canvasGroup.GetComponent<Animator>().enabled = false;
				canvasGroup.alpha = 1;
			}

		} else{
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;

			if(effectMan.effectsEnabled == true){
				canvasGroup.GetComponent<Animator>().SetTrigger("LogOut");
			}else{
				canvasGroup.GetComponent<Animator>().enabled = false;
				canvasGroup.alpha = 0;
			}
		}
	}

	public void LogInToApplication(){
		welcomeText.text = "Välkommen " + inputFieldUserName.text.ToUpperInvariant() + "!";

		foreach(CanvasGroup logInGroup in logInGroups){
			CanvasGroupSetVisible(logInGroup, false);
		}

		foreach(CanvasGroup canvasGroup in canvasGroupArray){
			CanvasGroupSetVisible(canvasGroup, true);
		}

		CheckIfTestUser (inputFieldUserName.text);
	}

	public void LogOutFromApplication(){
		welcomeText.text = "Hej då " + inputFieldUserName.text.ToUpperInvariant() + "!";

		foreach(CanvasGroup logInGroup in logInGroups){
			CanvasGroupSetVisible(logInGroup, true);
		}

		foreach(CanvasGroup canvasGroup in canvasGroupArray){
			CanvasGroupSetVisible(canvasGroup, false);
		}

		// Rank and stars reset
		ToggleCheatButtons(false);
		rankManager.DeSpawnTheRank();
		uiTaskManager.DestroyAllStars();
		uiTaskManager.DeSpawnTheTaskButtons();
	}

	// Checks if the user is able to use the cheat buttons
	void CheckIfTestUser(string user){
		if (user == "Hannes") {
			ToggleCheatButtons (true);
		}
	}

	void ToggleCheatButtons(bool active){
		foreach(GameObject cheatButton in cheatButtons){
			cheatButton.SetActive (active);
		}
	}
}

using UnityEngine;
using System.Collections;

public class WWWFormTasks : MonoBehaviour {

	public WWWFormUserLogIn userInfo;
	int currentDay;
	string database_url = "http://xml.csc.kth.se/~thomasvp/DM2517/kex/index.php";
	string downloadedTaskText = "";
	string downloadedCompletedTasksText = "";


	public void SendCompletedTaskData(int buttonNumber){
		StartCoroutine(SendCompletedTaskDataIEnumerator(buttonNumber));
	}

	public void RequestTasksFromDatabase(){
		StartCoroutine(RequestTasksFromDatabaseIEnumerator());
	}

	public void RequestCompletedTasksFromDatabase(){
		StartCoroutine(GetCompletedTasksFromDatabaseIEnumerator());
	}

	public void ResetAllTasks(){
		StartCoroutine(ResetAllTodosIEnumerator());
	}

	public void SendMotivationCheck(int motivationAmount){
		StartCoroutine(SendMotivationCheckIEnumerator(motivationAmount));
	}
		
	IEnumerator RequestTasksFromDatabaseIEnumerator(){
		WWW downloadedTaskData = new WWW(database_url + "?getTodos=1");
		yield return downloadedTaskData;
		downloadedTaskText = downloadedTaskData.text;
		this.GetComponent<UITaskScriptManager>().UpdateTaskText(downloadedTaskText);
	}

	IEnumerator SendCompletedTaskDataIEnumerator(int buttonNumber){
		WWWForm form = new WWWForm();
		form.AddField("userName", userInfo.UserName);

		WWW confirmedSentData = new WWW(database_url + "?done=" + buttonNumber, form);
		yield return confirmedSentData;

		if(confirmedSentData.text == "Updated"){
			this.GetComponent<UITaskScriptManager>().ConfirmedCompletedTaskFromDatabase(buttonNumber);
		}else{
			Debug.LogError("Unknown database response when sending completed task information. Expected Updated but got: " + confirmedSentData.text);
		}
	}

	IEnumerator GetCompletedTasksFromDatabaseIEnumerator(){
		WWWForm form = new WWWForm();
		form.AddField("userName", userInfo.UserName);

		WWW completedTasks = new WWW(database_url + "?getCompletedTodos=1", form);
		yield return completedTasks;


		downloadedCompletedTasksText = completedTasks.text;
		this.GetComponent<UITaskScriptManager>().UpdateCompletedTasksFromDatabase(downloadedCompletedTasksText);
	}

	IEnumerator SendMotivationCheckIEnumerator(int motivationAmount){
		WWWForm form = new WWWForm();
		form.AddField("userName", userInfo.UserName);
		form.AddField("motivation", motivationAmount);

		WWW confirmedSentMotivationData = new WWW(database_url + "?setMotivation=1", form);
		yield return confirmedSentMotivationData;

		if(confirmedSentMotivationData.text == "Updated"){
			this.GetComponent<UITaskScriptManager>().MotivationGroupSetActive(false);
		}else{
			Debug.LogError("Unknown database response when sending completed task information. Expected Updated but got: " + confirmedSentMotivationData.text);
		}
	}

	IEnumerator ResetAllTodosIEnumerator(){
		WWWForm form = new WWWForm();
		form.AddField("userName", userInfo.UserName);

		WWW completedTasks = new WWW(database_url + "?undone=4", form);

		yield return completedTasks;
	}


}

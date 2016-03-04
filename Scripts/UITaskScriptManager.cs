using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITaskScriptManager : MonoBehaviour {

	public GameObject[] originalTasks;
	char splitter = ',';

	public void UpdateTaskText(string taskString){

		string[] splittedTasks = taskString.Split(splitter);

		for (int i = 0; i < (splittedTasks.Length-1); i++) {
			originalTasks[i].transform.GetChild(0).GetComponent<Text>().text = ("\t* " + splittedTasks[i]);
		}
		this.GetComponent<WWWFormTasks>().RequestCompletedTasksFromDatabase();
	}

	public void UpdateCompletedTasksFromDatabase(string completedTaskString){
		string[] splittedCompletedTasks = completedTaskString.Split(splitter);

		for (int i = 0; i < splittedCompletedTasks.Length; i++) {
			if (splittedCompletedTasks[i] == "0"){
				originalTasks[i].GetComponent<Button>().interactable = true;
			}else{
				originalTasks[i].GetComponent<Button>().interactable = false;
			}
		}
	}

	public void DebugResetAllTodos(){
		this.GetComponent<WWWFormTasks>().ResetAllTasks();
		for (int i = 0; i < 3; i++) {
			originalTasks[i].GetComponent<Button>().interactable = true;
		}
	}

	public void BottonClickOnTask(int buttonNumber){
		this.GetComponent<WWWFormTasks>().SendCompletedTaskData(buttonNumber);
	}

	public void ConfirmedCompletedTaskFromDatabase(int buttonNumber){
		originalTasks[buttonNumber-1].GetComponent<Button>().interactable = false;
	}


}

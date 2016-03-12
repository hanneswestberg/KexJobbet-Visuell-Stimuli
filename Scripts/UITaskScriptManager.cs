using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITaskScriptManager : MonoBehaviour {

	public EffectsManager effectManager;
	public RankManager rankManager;
	public GameObject[] originalTasks;
	public Text scoreCount;
	int currentScore;
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

		for (int i = 0; i < (splittedCompletedTasks.Length-1); i++) {
			if (splittedCompletedTasks[i] == "0"){
				originalTasks[i].GetComponent<Button>().interactable = true;
				originalTasks[i].GetComponent<TaskStarManager>().CreateTaskStars(i+1);
			}else{
				originalTasks[i].GetComponent<Button>().interactable = false;
			}
		}

		// We have got the score from the database, we need to save it and update the rank
		currentScore = int.Parse(splittedCompletedTasks[splittedCompletedTasks.Length-1]);
		scoreCount.text = splittedCompletedTasks[splittedCompletedTasks.Length-1];
		rankManager.SpawnTheRank(currentScore);
	}

	public void DebugResetAllTodos(){
		this.GetComponent<WWWFormTasks>().ResetAllTasks();
		for (int i = 0; i < 3; i++) {
			if(originalTasks[i].GetComponent<Button>().interactable == false){
				originalTasks[i].GetComponent<Button>().interactable = true;
				originalTasks[i].GetComponent<TaskStarManager>().CreateTaskStars(i+1);
			}
		}
		scoreCount.text = "0";
		currentScore = 0;
		rankManager.ResetRank();
	}

	public void ButtonClickOnTask(int buttonNumber){
		this.GetComponent<WWWFormTasks>().SendCompletedTaskData(buttonNumber);
	}

	public void ConfirmedCompletedTaskFromDatabase(int buttonNumber){
		originalTasks[buttonNumber-1].GetComponent<Button>().interactable = false;

		originalTasks[buttonNumber-1].GetComponent<TaskStarManager>().AnimateTaskStar();

		currentScore += buttonNumber;
		rankManager.CheckForRankUpgrade(currentScore);
	}

	public void AddStarsToTotal(int score = 0){
		scoreCount.text = (int.Parse(scoreCount.text) + score).ToString();
	}


}

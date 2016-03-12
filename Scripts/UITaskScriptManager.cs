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

	// We have recieved task information from the database
	public void UpdateTaskText(string taskString){

		string[] splittedTasks = taskString.Split(splitter);

		for (int i = 0; i < (splittedTasks.Length-1); i++) {
			originalTasks[i].transform.GetComponent<TaskStarManager>().taskText.GetComponent<Text>().text = ("* " + splittedTasks[i]);
		}
		this.GetComponent<WWWFormTasks>().RequestCompletedTasksFromDatabase();
	}

	// We have recieved information from the database about which task we already have done
	public void UpdateCompletedTasksFromDatabase(string completedTaskString){
		string[] splittedCompletedTasks = completedTaskString.Split(splitter);

		// Spawn stars for the uncompleted tasks
		for (int i = 0; i < (splittedCompletedTasks.Length-1); i++) {
			if (splittedCompletedTasks[i] == "0"){
				originalTasks[i].GetComponent<Button>().interactable = true;
				originalTasks[i].GetComponent<TaskStarManager>().CreateTaskStars(i+1);
			}else{
				originalTasks[i].GetComponent<Button>().interactable = false;
			}
		}

		// Spawn stars for tomorrows tasks
		for (int i = 3; i < 6; i++) {
			originalTasks[i].GetComponent<TaskStarManager>().CreateTaskStars(i-2);
		}

		// We have got the score from the database, we need to save it and update the rank
		currentScore = int.Parse(splittedCompletedTasks[splittedCompletedTasks.Length-1]);
		scoreCount.text = splittedCompletedTasks[splittedCompletedTasks.Length-1];
		rankManager.SpawnTheRank(currentScore);

		// We need to spawn the Task buttons
		SpawnTheTaskButtons();
	}

	// Time to spawn the buttons
	void SpawnTheTaskButtons(){
		if (effectManager.effectsEnabled == true){
			StartCoroutine(PlayAnimationsQue(new Animator[]{originalTasks[0].GetComponent<Animator>(), originalTasks[1].GetComponent<Animator>(), originalTasks[2].GetComponent<Animator>(),
				originalTasks[3].GetComponent<Animator>(), originalTasks[4].GetComponent<Animator>(), originalTasks[5].GetComponent<Animator>(),}
				, "TaskButtons_PopInRight", 0.2f));
		} else{
			for (int i = 0; i < 6; i++) {
				if(i < 3){
					ToggleButtonVisible(originalTasks[i].transform.GetComponent<CanvasGroup>(), true, false);
				} else{
					ToggleButtonVisible(originalTasks[i].transform.GetComponent<CanvasGroup>(), true, true);
				}
			}
		}
	}

	public void DeSpawnTheTaskButtons(){
		for (int i = 0; i < 6; i++) {
			if(i < 3){
				ToggleButtonVisible(originalTasks[i].transform.GetComponent<CanvasGroup>(), false, false);
			} else{
				ToggleButtonVisible(originalTasks[i].transform.GetComponent<CanvasGroup>(), false, true);
			}
		}
	}

	void ToggleButtonVisible(CanvasGroup canvasGroup, bool setVisible, bool isTomorrowsTask){
		if(setVisible == true){
			canvasGroup.alpha = 1f;
			if(isTomorrowsTask == false){
				canvasGroup.interactable = true;
				canvasGroup.blocksRaycasts = true;
			}
		}
		else{
			canvasGroup.alpha = 0f;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		}
	}

	// Resets all tasks and creates new stars. Also resets our database
	public void DebugResetAllTodos(){
		this.GetComponent<WWWFormTasks>().ResetAllTasks();

		// First reset all todays-tasks
		for (int i = 0; i < 3; i++) {
			if(originalTasks[i].GetComponent<Button>().interactable == false){
				originalTasks[i].GetComponent<Button>().interactable = true;
				originalTasks[i].GetComponent<TaskStarManager>().CreateTaskStars(i+1);
			}

			ToggleButtonVisible(originalTasks[i].GetComponent<CanvasGroup>(), false, false);
			originalTasks[i].GetComponent<TaskStarManager>().StartStarAnimations("Reset");
		}

		// Secondly reset all tomorrows-tasks
		for (int i = 3; i < 6; i++) {
			ToggleButtonVisible(originalTasks[i].GetComponent<CanvasGroup>(), false, true);
			originalTasks[i].GetComponent<TaskStarManager>().StartStarAnimations("Reset");
		}

		// Resets our internal score and the visual rank
		scoreCount.text = "0";
		currentScore = 0;
		rankManager.ResetRank();
		SpawnTheTaskButtons();
	}

	// Triggers when we have pressed a button
	public void ButtonClickOnTask(int buttonNumber){
		this.GetComponent<WWWFormTasks>().SendCompletedTaskData(buttonNumber);
	}

	// Triggers when we have recieved "Updated" data from the database when we have pressed a button
	public void ConfirmedCompletedTaskFromDatabase(int buttonNumber){
		originalTasks[buttonNumber-1].GetComponent<Button>().interactable = false;
		originalTasks[buttonNumber-1].GetComponent<TaskStarManager>().AnimateTaskStar();

		// We also need to keep track of score and update the visual rank
		currentScore += buttonNumber;
		rankManager.CheckForRankUpgrade(currentScore);
	}

	// Deleting all stars when logging out
	public void DestroyAllStars(){
		for (int i = 0; i < 3; i++) {
			originalTasks[i].GetComponent<TaskStarManager>().DestroyAllStarsConnectedToTask();
		}
	}

	// Updating our current score visually, occurs when a star has finished it's animation
	public void AddStarsToTotal(int score = 0){
		scoreCount.text = (int.Parse(scoreCount.text) + score).ToString();
	}


	IEnumerator PlayAnimationsQue(Animator[] animators, string triggerName, float waitTime){
		for (int i = 0; i < 6; i++) {
			if(i < 3){
				ToggleButtonVisible(animators[i].transform.GetComponent<CanvasGroup>(), true, false);
			} else{
				ToggleButtonVisible(animators[i].transform.GetComponent<CanvasGroup>(), true, true);
			}
			animators[i].SetTrigger(triggerName);
			animators[i].GetComponent<TaskStarManager>().StartStarAnimations("Effects_enabled");
			yield return new WaitForSeconds(waitTime);
		}
	}
}

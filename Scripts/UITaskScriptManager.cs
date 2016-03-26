using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITaskScriptManager : MonoBehaviour {

	public EffectsManager effectManager;
	public RankManager rankManager;
	public XPBarAnimator xpBarAnim;

	public GameObject[] originalTasks;
	public CanvasGroup[] tutorialGroup;
	public CanvasGroup tutorialParentGroup;
	public CanvasGroup motivationParentGroup;
	public GameObject motivationButton;
	public Text scoreCount;
	public Text DayCount;
	char splitter = ',';

	Animator[] taskAnimators = new Animator[]{};

	// Creates an array of all the task animators
	void Start(){
		taskAnimators = new Animator[]{originalTasks[0].GetComponent<Animator>(), originalTasks[1].GetComponent<Animator>(), originalTasks[2].GetComponent<Animator>(),
			originalTasks[3].GetComponent<Animator>(), originalTasks[4].GetComponent<Animator>(), originalTasks[5].GetComponent<Animator>()};
	}

	// We have recieved task information from the database
	public void UpdateTaskText(string taskString){

		string[] splittedTasks = taskString.Split(splitter);

		for (int i = 0; i < 5; i++) {
			originalTasks[i].transform.GetComponent<TaskStarManager>().taskText.GetComponent<Text>().text = ("* " + splittedTasks[i]);
		}

		DayCount.text = "Dag: " + splittedTasks[6];

		this.GetComponent<WWWFormTasks>().RequestCompletedTasksFromDatabase();
	}

	// We have recieved information from the database about which task we already have done
	public void UpdateCompletedTasksFromDatabase(string completedTaskString){
		string[] splittedCompletedTasks = completedTaskString.Split(splitter);

		// Activates the buttons and spawn stars for the uncompleted tasks
		for (int i = 0; i < 3; i++) {
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
		rankManager.CurrentScore = int.Parse(splittedCompletedTasks[3]);
		scoreCount.text = splittedCompletedTasks[3];
		rankManager.SpawnTheRank();

		// We need to spawn the Task buttons
		if(effectManager.effectsEnabled == true){
			StartCoroutine(SpawnWaitForTaskWindowToEndAnimation(0.6f));
		}else{
			SpawnTheTaskButtons();
		}

		// We have recieved the motivationCheck variable, if 0 we can send current motivation data
		if(splittedCompletedTasks[4] == "0"){
			motivationButton.SetActive(true);
			motivationButton.GetComponent<Animator>().SetBool("MotivationDone", false);
		} else{
			motivationButton.SetActive(false);
		}

		// Animation triggers:
		effectManager.AllButtonsEffectSetActive(true);
		xpBarAnim.StartXPBarStarAnimations();
	}

	// Activates the buttons and plays the spawn animation if we have effects enabled
	void SpawnTheTaskButtons(){
		if (effectManager.effectsEnabled == true){ // If we have effects enabled, then we want to start animation
			StartCoroutine(PlayAnimationsQue(taskAnimators, "TaskButtons_PopInRight", 0.2f));

			/// BugFix, Tomorrows task too low alpha
			for (int i = 3; i < 6; i++) {
				originalTasks[i].transform.GetChild(0).GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
			}
		} else{
			for (int i = 0; i < 6; i++) {
				originalTasks[i].GetComponent<TaskStarManager>().SpawnCheckEffects();
				originalTasks[i].transform.GetChild(0).GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);

				if(i < 3){
					ToggleButtonVisible(originalTasks[i].transform.GetComponent<CanvasGroup>(), true, false);
				} else{
					ToggleButtonVisible(originalTasks[i].transform.GetComponent<CanvasGroup>(), true, true);
				}
			}
		}
	}

	// Deactivates the buttons
	public void DeSpawnTheTaskButtons(){
		StartCoroutine(DeSpawnWaitForTaskWindowToEndAnimation(1f));
	}

	// Toggles a canvasgroup visibility
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

		rankManager.CurrentScore = 0;
		rankManager.ResetRank();
		SpawnTheTaskButtons();
		motivationButton.SetActive(true);

		if(effectManager.effectsEnabled == true){
			motivationButton.GetComponent<Animator>().enabled = true;
			motivationButton.GetComponent<Animator>().SetBool("MotivationDone", false);
			motivationButton.GetComponent<Animator>().SetTrigger("MotivationReset");
		}
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
		rankManager.CurrentScore += buttonNumber;
	}

	// Deleting all stars when logging out
	public void DestroyAllStars(){
		StartCoroutine(DestroyAllStarsWait(1f));
	}

	// Updating our current score visually, occurs when a star has finished it's animation
	public void AddStarsToTotal(int score = 0){
		int newScore = (int.Parse (scoreCount.text) + score);
		if(effectManager.effectsEnabled == true){
			if(rankManager.CurrentScore >= newScore){ // We need to check this so we don't add more stars than we should have, when animating
				
				// We want the star total text to update instantly, but the rank and xp bar should be animated
				scoreCount.text = newScore.ToString();
				scoreCount.GetComponent<Animator>().SetTrigger("AddScore");
				rankManager.CheckForRankUpgrade(newScore);
			}
		} else{
			scoreCount.text = newScore.ToString();
			rankManager.CheckForRankUpgrade(newScore);
		}
	}

	// Cheats are only avaliable to admins
	public void CheatAddStars(){
		rankManager.CurrentScore += 1;
		AddStarsToTotal (1);
	}

	public void MotivationQuestionStart (){
		if (motivationParentGroup.interactable == false) {
			motivationParentGroup.interactable = true;
			motivationParentGroup.blocksRaycasts = true;

			if(effectManager.effectsEnabled == true){
				motivationParentGroup.GetComponent<Animator>().SetTrigger("PopIn");
			}else{
				motivationParentGroup.GetComponent<Animator>().enabled = false;
				motivationParentGroup.alpha = 1;
			}
		}
	}

	public void MotivationQuestionQuit(){
		if (motivationParentGroup.interactable == true) {
			motivationParentGroup.interactable = false;
			motivationParentGroup.blocksRaycasts = false;

			if(effectManager.effectsEnabled == true){
				motivationParentGroup.GetComponent<Animator>().SetTrigger("PopOut");
			}else{
				motivationParentGroup.GetComponent<Animator>().enabled = false;
				motivationParentGroup.alpha = 0;
			}
		}
	}

	public void MotivationQuestionSave(int motivationAmount){
		this.GetComponent<WWWFormTasks>().SendMotivationCheck(motivationAmount);
		MotivationQuestionQuit ();
	}

	public void MotivationGroupSetActive(bool active){
		if (active == true){
			motivationParentGroup.interactable = true;
			motivationParentGroup.blocksRaycasts = true;
			motivationParentGroup.alpha = 1f;
		}else{
			motivationParentGroup.interactable = false;
			motivationParentGroup.blocksRaycasts = false;
			motivationParentGroup.alpha = 0f;
			motivationButton.GetComponent<Animator>().SetBool("MotivationDone", true);

			if(effectManager.effectsEnabled == true){
				StartCoroutine(DisableMotivationGroup());
			}else{
				motivationButton.SetActive(false);
			}
		}
	}

	public void TutorialStart(){
		if (tutorialParentGroup.interactable == false) {
			tutorialParentGroup.interactable = true;
			tutorialParentGroup.blocksRaycasts = true;
			TutorialShowTip (0);
		}
	}

	public void TutorialShowTip(int tipNumber){
		foreach (CanvasGroup canvasGroup in tutorialGroup) {
			if (canvasGroup.interactable == true) {
				TutorialSetVisible (canvasGroup, false);
			}
		}

		TutorialSetVisible(tutorialGroup[tipNumber], true);
	}

	void TutorialSetVisible( CanvasGroup canvasGroup, bool active){
		if(active == true){
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;

			if(effectManager.effectsEnabled == true){
				canvasGroup.GetComponent<Animator>().SetTrigger("PopIn");
			}else{
				canvasGroup.GetComponent<Animator>().enabled = false;
				canvasGroup.alpha = 1;
			}

		} else{
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;

			if(effectManager.effectsEnabled == true){
				canvasGroup.GetComponent<Animator>().SetTrigger("PopOut");
			}else{
				canvasGroup.GetComponent<Animator>().enabled = false;
				canvasGroup.alpha = 0;
			}
		}
	}

	public void TutorialQuit(){
		tutorialParentGroup.interactable = false;
		tutorialParentGroup.blocksRaycasts = false;

		foreach (CanvasGroup canvasGroup in tutorialGroup) {
			if (canvasGroup.interactable == true) {
				TutorialSetVisible (canvasGroup, false);
			}
		}
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

	IEnumerator SpawnWaitForTaskWindowToEndAnimation(float waitTime){
		yield return new WaitForSeconds(waitTime);
		SpawnTheTaskButtons();
	}

	IEnumerator DeSpawnWaitForTaskWindowToEndAnimation(float waitTime){
		if(effectManager.effectsEnabled == true){
			yield return new WaitForSeconds(waitTime);
		}

		for (int i = 0; i < 6; i++) {
			if(i < 3){
				ToggleButtonVisible(originalTasks[i].transform.GetComponent<CanvasGroup>(), false, false);
			} else{
				ToggleButtonVisible(originalTasks[i].transform.GetComponent<CanvasGroup>(), false, true);
			}
		}
		rankManager.ResetRank();
	}

	IEnumerator DestroyAllStarsWait(float waitTime){
		if(effectManager.effectsEnabled == true){
			yield return new WaitForSeconds(waitTime);
		}

		for (int i = 0; i < 6; i++) {
			originalTasks[i].GetComponent<TaskStarManager>().DestroyAllStarsConnectedToTask();
		}
	}

	IEnumerator DisableMotivationGroup(){
		yield return new WaitForSeconds(1f);
		motivationButton.SetActive(false);
	}
		
}

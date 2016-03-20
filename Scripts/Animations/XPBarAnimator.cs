using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class XPBarAnimator : MonoBehaviour {

	public EffectsManager effectMan;

	private float fillAmount;
	public Image xpFiller;
	public Image xpGlas;

	private bool hasChanged;
	private bool firstStart;
	private int currentXPBarQue = 0;
	private int currentScore = 0;
	private float tempFillAmount = 0f;

	void Start(){
		firstStart = true;
	}

	void Update () 
	{
		if(effectMan.effectsEnabled == true){
			xpFiller.fillAmount = Mathf.Lerp(xpFiller.fillAmount, fillAmount, 0.05f);
			xpGlas.fillAmount = Mathf.Lerp(xpGlas.fillAmount, 1f - fillAmount, 0.05f);
		} else{
			xpFiller.fillAmount = fillAmount;
			xpGlas.fillAmount = 1f - fillAmount;
		}

	}

	public void ResetFillAmount(){
		fillAmount = 0;
		firstStart = true;
	}

	public void SetFillAmount(float amount){
		if (fillAmount != 1) {
			fillAmount = amount;
			hasChanged = true;
		} else {
			tempFillAmount = amount;
		}

		if(amount == 0 && firstStart == false){
			hasChanged = false;
			StartCoroutine(LevelUpAnimation());
		}
		firstStart = false;
	}

	public void AddToXPBarQue(int newScore){
		if (currentXPBarQue == 0) {
			currentXPBarQue = (newScore - currentScore);
			StartCoroutine (XPBarQueAnimator ());
		} else {
			currentXPBarQue = (newScore - currentScore);
		}
	}

	// Calculates the XP - Bar fill amount
	void CalculateXpBar(int score){
		SetFillAmount((score % 4)/4f);
	}

	IEnumerator LevelUpAnimation(){
		fillAmount = 1f;
		effectMan.CreateEffectAt (effectMan.rankEffects [0], transform.position);
		yield return new WaitForSeconds(1f);
		
			if(hasChanged == false){
			xpFiller.fillAmount = 0f;
			xpGlas.fillAmount = 1f;
			fillAmount = tempFillAmount;
			tempFillAmount = 0f;
			}
	}

	IEnumerator XPBarQueAnimator(){
		currentScore++;
		CalculateXpBar (currentScore);

		yield return new WaitForSeconds (0.3f);
		currentXPBarQue--;

		Debug.Log ("Current tracked score is: " + currentScore + "\t Current que is: " + currentXPBarQue);
		if (currentXPBarQue > 0) {
			StartCoroutine(XPBarQueAnimator());
		}
	}
}

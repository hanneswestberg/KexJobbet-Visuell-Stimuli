using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class XPBarAnimator : MonoBehaviour {

	public EffectsManager effectMan;

	private float fillAmount;
	public Image xpFiller;
	public Image xpGlas;
	public Transform xpLevelUpAnimationPoint;
	public Animator[] starLines;
	public Animator[] xpBarStars;

	private bool firstStart;
	private bool currentlyAnimating = false;
	private bool leveledTwice = false;
	private bool hasChanged = false;

	private float currentFillSpeed = 0f;
	private float currentFillFactor = 5f;
	private float tempFillAmount = 0f;

	void Start(){
		firstStart = true;
		currentFillFactor = 5f;
	}

	void Update () 
	{
		if(effectMan.effectsEnabled == true){
			currentFillSpeed = currentFillFactor*Time.deltaTime;
			xpFiller.fillAmount = Mathf.Lerp(xpFiller.fillAmount, fillAmount, currentFillSpeed);
			xpGlas.fillAmount = Mathf.Lerp(xpGlas.fillAmount, 1f - fillAmount, currentFillSpeed);
		} else{
			xpFiller.fillAmount = fillAmount;
			xpGlas.fillAmount = 1f - fillAmount;
		}

		if(leveledTwice == true && currentlyAnimating == false){ // We have leveled twice and need to animate the next level up animation
			leveledTwice = false;
			tempFillAmount = fillAmount;
			StartCoroutine(LevelUpAnimation());
		}

		// Animate the XP  stars and lines
		if(effectMan.effectsEnabled == true){
			for (int i = 0; i < 4; i++) {
				if(xpFiller.fillAmount >= (((i+1)*0.20f)+(0.05f*i))){
					starLines[i].SetBool("StarLine_Fill", true);
				} else{
					starLines[i].SetBool("StarLine_Fill", false);
				}
			}
		}
	}

	public void StartXPBarStarAnimations(){
		if(effectMan.effectsEnabled == true){
			foreach(Animator anim in xpBarStars){
				anim.SetTrigger("Effects_enabled");
			}
		}
	}

	public void ResetFillAmount(){
		fillAmount = 0;
		firstStart = true;
	}

	// Sets the fill amount of the XP - Bar, 0 is no fill, 1 is all filled up. 
	public void SetFillAmount(float amount){
		hasChanged = true;

		if (currentlyAnimating == false) { // Checks if we are currently animating for a level up
			fillAmount = amount;
		} else {
			tempFillAmount = amount;
		}

		if(amount == 0 && firstStart == false){ // If we have recieved a 0. Of which only happen at the start OR we have just leveled up! We need to animate
			if(currentlyAnimating == true){
				leveledTwice = true;
			}else if (effectMan.effectsEnabled == true){
				StartCoroutine(LevelUpAnimation());
			} else{
				hasChanged = false;
				StartCoroutine(LevelUpAnimationNoEffects());
			}
		}
		firstStart = false;
	}

	// Calculates the XP - Bar fill amount
	void CalculateXpBar(int score){
		SetFillAmount((score % 4)/4f);
	}

	IEnumerator LevelUpAnimation(){
		currentlyAnimating = true;
		fillAmount = 1f;
		yield return new WaitForSeconds(0.5f);
		effectMan.CreateEffectAt (effectMan.rankEffects [1], xpLevelUpAnimationPoint.position);
		effectMan.CreateEffectAt (effectMan.rankEffects [0], xpLevelUpAnimationPoint.position);
		yield return new WaitForSeconds(0.5f);
		fillAmount = 0f;
		currentFillFactor = 3f;
		yield return new WaitForSeconds(2.5f);

		currentFillFactor = 5f;

		fillAmount = tempFillAmount;
		tempFillAmount = 0f;
		currentlyAnimating = false;
	}

	IEnumerator LevelUpAnimationNoEffects(){
		fillAmount = 1f;
		yield return new WaitForSeconds(1f);

		if(hasChanged == false){
			fillAmount = 0f;
		}
	}
}

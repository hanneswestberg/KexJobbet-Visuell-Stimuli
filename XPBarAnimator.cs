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
		fillAmount = amount;
		hasChanged = true;

		if(amount == 0 && firstStart == false){
			hasChanged = false;
			StartCoroutine(LevelUpAnimation());
		}
		firstStart = false;
	}

	IEnumerator LevelUpAnimation(){
		fillAmount = 1f;
		yield return new WaitForSeconds(0.5f);
		
			if(hasChanged == false){
				fillAmount = 0f;
			}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RankManager : MonoBehaviour {

	public EffectsManager effectManager;
	public Text rankUIText;
	public GameObject rankHolder;
	public GameObject rankPlane;
	public RankRotation rankRot;
	public Material rankMaterial;
	public Material rankShadowMaterial;
	public XPBarAnimator xpBarAnim;
	public Texture[] rankTextures;
	public Texture[] rankNormalMaps;
	public Texture[] rankHeightMaps;
	public Light rankLight;

	[SerializeField] private int[] rankScores = new int[]{0,6,12,18,24,30,36,42,48,54,60};
	private string[] rankDescription = new string[]{"Ickestuderande NØllan", "Talanglös Musklickare", "Lortig Distansstuderare", "Vredig Medietekniksinspirerare", 
		"Tillfälligt Förekommande Skolvisiterare", "Medelmåttig Uppgiftsavklarare", "Medioker Applikationsstartare", "Andfådd Föreläsningsgåare", 
		"Gynnsam Lunchätare", "Lovande Applikationsanvändare", "Förtroendeingivande Kexjobbsdeltagare", "Eftersträvande Ingenjörsefterföljare", "Aspirerande Höginkomsttagare", 
		"Intellektuell Akademiutövare","Nästan Fulländad Sysslo-utförare", "Ultimat Att-göragörare"};
	private string standard_rank = "";

	private int currentRank = 0;
	private int currentScore = 0;
	private int upgradeCheckTempRank;

	private float currentScale = 1f;
	private bool currentlyAnimating = false;
	private bool waitingForAnimationToEndThenRankUp = false;

	// Returns our current score
	public int CurrentScore {
		get {
			return currentScore;
		}
		set {
			currentScore = value;
		}
	}

	void Update(){
		if(waitingForAnimationToEndThenRankUp == true && currentlyAnimating == false){
			waitingForAnimationToEndThenRankUp = false;
			StartCoroutine(RankUpgradeWaitForXPBarAnimation(currentRank));
		}
	}

	// Activates the rank, triggers when logging in
	public void SpawnTheRank(){
		currentRank = CalculateRank(currentScore);
		UpdateRankVisuals(currentRank);
		rankHolder.SetActive(true);
		CalculateXpBar(currentScore);

		if(effectManager.effectsEnabled == true) {
			rankRot.RankShadowsSetActive(true);
			rankRot.RankEffectsSetActive(true);
		}else{
			rankRot.RankShadowsSetActive(false);
			rankRot.RankEffectsSetActive(false);
			rankLight.intensity = 0.3f;
		}
	}

	public void CalculateXpBar(int score){
		xpBarAnim.SetFillAmount((score % 4)/4f);
	}

	// Deactivates the rank, triggers when logging out
	public void DeSpawnTheRank(){
		rankHolder.SetActive(false);
	}

	// Checks if we have reached a new rank, triggers when we have completed a new task
	public void CheckForRankUpgrade(int score){
		upgradeCheckTempRank = currentRank;
		currentRank = CalculateRank(score);

		CalculateXpBar(score);

		if(currentRank > upgradeCheckTempRank){ // We should upgrade our rank!!
			if(effectManager.effectsEnabled == true){
				if(rankPlane.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle") && currentlyAnimating == false){
					StartCoroutine(RankUpgradeWaitForXPBarAnimation(currentRank));
				} else{
					
					// We are currently animating an upgrade and cannot disrupt. We need to wait until it is done.
					waitingForAnimationToEndThenRankUp = true;
				}
			} else{
				UpdateRankVisuals(currentRank);
			}
		}
	}

	// Resets the rank to the lowest, triggers when clicking on 'DEBUG_RESETALL' button
	public void ResetRank(){
		rankHolder.transform.localScale = Vector3.one;
		UpdateRankVisuals(0);
		xpBarAnim.ResetFillAmount();
		currentScore = 0;
	}

	// Updates the visuals of the rank to represent our new rank
	public void UpdateRankVisuals(int rank){
		// Text
		rankUIText.text = "" + (rank+1).ToString() + ": " + standard_rank + rankDescription[rank];

		if((rank+1) >= 5 && effectManager.effectsEnabled == true){
			rankUIText.GetComponent<Animator>().SetBool("Over5", true);

			if((rank+1) >= 10){
				rankUIText.GetComponent<Animator>().SetBool("Over10", true);

				if((rank+1) >= 16){
					rankUIText.GetComponent<Animator>().SetBool("Over16", true);
				} else{
					rankUIText.GetComponent<Animator>().SetBool("Over16", false);
				}
			}else{
				rankUIText.GetComponent<Animator>().SetBool("Over10", false);
			}
		}else{
			rankUIText.GetComponent<Animator>().SetBool("Over5", false);
		}

		// Material Textures
		rankMaterial.mainTexture = rankTextures[rank];
		rankMaterial.SetTexture("_BumpMap", rankNormalMaps[rank]);
		rankMaterial.SetTexture("_ParallaxMap", rankHeightMaps[rank]);

		rankShadowMaterial.mainTexture = rankTextures[rank];

		// Scale
		currentScale = 1f + 0.02f*rank;
		rankHolder.transform.localScale = new Vector3 (currentScale, currentScale, currentScale);
	}

	// Calculates the current rank based on what score we have
	int CalculateRank(int score = 0){
		for (int i = rankScores.Length-1; i > -1; i--) {
			if(score >= rankScores[i]){
				return i;
			}
		}
		return 0;
	}

	IEnumerator RankUpgradeWaitForXPBarAnimation(int rank){
		currentlyAnimating = true;
		yield return new WaitForSeconds(0.5f);
		rankPlane.GetComponent<Animator>().SetTrigger("RankUpgradeWait");
		rankUIText.GetComponent<Animator>().SetTrigger("RankUp");
		effectManager.CreateEffectAt (effectManager.rankEffects [2], transform.position);
		yield return new WaitForSeconds(2f);

		string rankUpgradeString = "RankUpgrade_" + (rank+1).ToString();

		rankPlane.GetComponent<Animator>().SetTrigger(rankUpgradeString);

		yield return new WaitForSeconds(1f);
		currentlyAnimating = false;
	}
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RankManager : MonoBehaviour {

	public EffectsManager effectManager;
	public Text[] rankUITexts;
	public GameObject rankHolder;
	public Material rankMaterial;
	public Material rankShadowMaterial;
	public XPBarAnimator xpBarAnim;
	public Texture[] rankTextures;
	public Texture[] rankNormalMaps;
	public Texture[] rankHeightMaps;

	[SerializeField] private int[] rankScores = new int[]{0,6,12,18,24,30,36,42,48,54,60};
	private string[] rankDescription = new string[]{"Ickestuderande NØllan", "Talanglös Musklickare", "Lortig Distansstuderare", "Vredig Medietekniksinspirerare", 
		"Tillfälligt Förekommande Skolvisiterare", "Medelmåttig Uppgiftsavklarare", "Medioker Applikationsstartare", "Andfådd Föreläsningsgåare", 
		"Gynnsam Lunchätare", "Lovande Applikationsanvändare", "Förtroendeingivande Kexjobbsdeltagare", "Eftersträvande Ingenjörsefterföljare", "Aspirerande Höginkomsttagare", 
		"Intellektuell Akademiutövare","Nästan Fulländad Sysslo-utförare", "Ultimat Att-göragörare"};
	private string standard_rank = "";

	private int currentRank = 0;
	private int currentScore = 0;

	// Returns our current score
	public int CurrentScore {
		get {
			return currentScore;
		}
		set {
			currentScore = value;
		}
	}

	// Activates the rank, triggers when logging in
	public void SpawnTheRank(){
		if (effectManager.effectsEnabled == true){
			currentRank = CalculateRank(currentScore);
			UpdateRankVisuals(currentRank);
			rankHolder.SetActive(true);
		}
		CalculateXpBar(currentScore);
	}

	public void CalculateXpBar(int score){
		xpBarAnim.SetFillAmount((score % 4)/4f);
	}

	public void AddToXPBarQue(int newScore){
		xpBarAnim.AddToXPBarQue (newScore);
	}

	// Deactivates the rank, triggers when logging out
	public void DeSpawnTheRank(){
		rankHolder.SetActive(false);
	}

	// Checks if we have reached a new rank, triggers when we have completed a new task
	public void CheckForRankUpgrade(int score){
		int temp = currentRank;
		currentRank = CalculateRank(score);

		if(currentRank > temp){
			effectManager.CreateEffectAt(effectManager.rankEffects[0], rankHolder.transform.GetChild(0).position);
			UpdateRankVisuals(currentRank);
			rankHolder.transform.localScale = new Vector3 (rankHolder.transform.localScale.x + 0.02f, rankHolder.transform.localScale.y + 0.02f,
				rankHolder.transform.localScale.z + 0.02f);
		}
	}

	// Resets the rank to the lowest, triggers when clicking on 'DEBUG_RESETALL' button
	public void ResetRank(){
		rankHolder.transform.localScale = Vector3.one;
		UpdateRankVisuals(0);
		xpBarAnim.ResetFillAmount();

	}

	// Updates the visuals of the rank to represent our new rank
	void UpdateRankVisuals(int rank){
		currentRank = rank;
		rankUITexts[0].text = "(" + (rank+1).ToString() + ") " + standard_rank + rankDescription[rank];
		rankUITexts[1].text = "(" + (rank+1).ToString() + ") " + standard_rank + rankDescription[rank];
		rankUITexts[2].text = "(" + (rank+1).ToString() + ") " + standard_rank + rankDescription[rank];
		rankMaterial.mainTexture = rankTextures[rank];
		rankMaterial.SetTexture("_BumpMap", rankNormalMaps[rank]);
		rankMaterial.SetTexture("_ParallaxMap", rankHeightMaps[rank]);

		rankShadowMaterial.mainTexture = rankTextures[rank];
		rankShadowMaterial.SetTexture("_BumpMap", rankNormalMaps[rank]);
		rankShadowMaterial.SetTexture("_ParallaxMap", rankHeightMaps[rank]);
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
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RankManager : MonoBehaviour {

	public EffectsManager effectManager;
	public Text rankUIText;
	public GameObject rankHolder;
	public Material rankMaterial;
	public Texture[] rankTextures;
	public Texture[] rankNormalMaps;

	public int[] rankScores = new int[]{0,6,12,18,24,30,36,42,48,54,60};
	string[] rankDescription = new string[]{"Icke-studerande Pleb", "Talanglös Musklickare", "Medioker Uppgiftsavklarare", "Aspirerande Höginkomsttagare"};
	int currentRank = 0;
	string standard_rank = "Du är rank: ";


	public void SpawnTheRank(int score = 0){
		if (effectManager.effectsEnabled == true){
			currentRank = CalculateRank(score);
			UpdateRankVisuals(currentRank);
			rankHolder.SetActive(true);
		}
	}

	public void DeSpawnTheRank(){
		rankHolder.SetActive(false);
	}

	public void CheckForRankUpgrade(int score){
		int temp = currentRank;
		currentRank = CalculateRank(score);

		if(currentRank > temp){
			effectManager.CreateEffectAt(effectManager.rankEffects[0], rankHolder.transform.GetChild(0).position, Quaternion.identity);
			UpdateRankVisuals(currentRank);
		}
	}

	public void ResetRank(){
		UpdateRankVisuals(0);
	}

	void UpdateRankVisuals(int rank){
		currentRank = rank;
		rankUIText.text = standard_rank + rankDescription[rank] + " (" + (rank+1).ToString() + ")";
		rankMaterial.mainTexture = rankTextures[rank];
		rankMaterial.SetTexture("_BumpMap", rankNormalMaps[rank]);
	}

	int CalculateRank(int score = 0){
		if(score >= rankScores[10]){
			return 10;
		} else if(score >= rankScores[9]){
			return 9;
		}else if(score >= rankScores[8]){
			return 8;
		}else if(score >= rankScores[7]){
			return 7;
		}else if(score >= rankScores[6]){
			return 6;
		}else if(score >= rankScores[5]){
			return 5;
		}else if(score >= rankScores[4]){
			return 4;
		}else if(score >= rankScores[3]){
			return 3;
		}else if(score >= rankScores[2]){
			return 2;
		}else if(score >= rankScores[1]){
			return 1;
		}else{
			return 0;
		}
	}





}

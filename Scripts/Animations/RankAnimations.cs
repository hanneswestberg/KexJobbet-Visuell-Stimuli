using UnityEngine;
using System.Collections;

public class RankAnimations : MonoBehaviour {

	public RankManager rankMan;
	public EffectsManager effectMan;

	public void UpdateVisualsToRank(int rankNumber){
		rankMan.UpdateRankVisuals(rankNumber-1);
	} 

	public void CreateExplodeEffect(){
		effectMan.CreateEffectAt(effectMan.rankEffects[0], transform.position);
	}


	public void RankUpgrade2(int stage){
		if(stage == 1){
			effectMan.CreateEffectAt(effectMan.rankEffects[0], transform.position);

		}else if(stage == 2){

		}
	}

	public void RankUpgrade3(int stage){

	}

	public void RankUpgrade4(int stage){

	}

	public void RankUpgrade5(int stage){

	}

	public void RankUpgrade6(int stage){

	}

	public void RankUpgrade7(int stage){

	}

	public void RankUpgrade8(int stage){

	}

	public void RankUpgrade9(int stage){

	}

	public void RankUpgrade10(int stage){

	}

	public void RankUpgrade11(int stage){

	}

	public void RankUpgrade12(int stage){

	}

	public void RankUpgrade13(int stage){

	}

	public void RankUpgrade14(int stage){

	}

	public void RankUpgrade15(int stage){

	}

	public void RankUpgrade16(int stage){

	}
}

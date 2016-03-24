using UnityEngine;
using System.Collections;

public class RankAnimations : MonoBehaviour {

	public RankManager rankMan;
	public EffectsManager effectMan;

	public void UpdateVisualsToRank(int rankNumber){
		rankMan.UpdateRankVisuals(rankNumber-1);
	} 

	public void CreateExplodeEffect(){
		effectMan.CreateEffectAt(effectMan.rankEffects[3], effectMan.RankEffectPosition);
	}

	public void RankUpEffect(int rank){
		effectMan.CreateEffectAt(effectMan.rankEffects[rank+1], effectMan.RankEffectPosition);
	}
}

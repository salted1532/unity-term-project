using UnityEngine;
using System.Collections;

namespace SlimUI.ModernMenu{
	public class CheckSFXVolume : MonoBehaviour {
		public void  Start (){

		}

		public void UpdateVolume (){
			SoundManager.Instance.SetVolume(SoundType.EFFECT, PlayerPrefs.GetFloat("SFXVolume"));
		}
	}
}
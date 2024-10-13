using UnityEngine;
using System.Collections;

namespace SlimUI.ModernMenu{
	public class CheckSFXVolume : MonoBehaviour {
		public void  Start (){
			SoundManager.Instance.SetVolume(SoundType.EFFECT, PlayerPrefs.GetFloat("SFXVolume"));
		}

		public void UpdateVolume (){
			SoundManager.Instance.SetVolume(SoundType.EFFECT, PlayerPrefs.GetFloat("SFXVolume"));
		}
	}
}
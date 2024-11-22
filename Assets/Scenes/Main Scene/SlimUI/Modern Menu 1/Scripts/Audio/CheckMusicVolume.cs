using UnityEngine;
using System.Collections;

namespace SlimUI.ModernMenu{
	public class CheckMusicVolume : MonoBehaviour {
		public void  Start (){

		}

		public void UpdateVolume (){
			SoundManager.Instance.SetVolume(SoundType.BGM, PlayerPrefs.GetFloat("MusicVolume"));
		}
	}
}
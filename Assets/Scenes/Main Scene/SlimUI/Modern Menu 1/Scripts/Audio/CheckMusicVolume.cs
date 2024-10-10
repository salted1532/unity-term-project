using UnityEngine;
using System.Collections;

namespace SlimUI.ModernMenu{
	public class CheckMusicVolume : MonoBehaviour {
		public void  Start (){
			SoundManager.Instance.SetVolume(SoundType.BGM, PlayerPrefs.GetFloat("MusicVolume"));
		}

		public void UpdateVolume (){
			SoundManager.Instance.SetVolume(SoundType.BGM, PlayerPrefs.GetFloat("MusicVolume"));
		}
	}
}
using UnityEngine;
using System.Collections;

namespace SlimUI.ModernMenu{
	public class CheckMasterVolume : MonoBehaviour {
		public void  Start (){
			SoundManager.Instance.SetVolume(SoundType.MASTER, PlayerPrefs.GetFloat("MasterVolume"));
		}

		public void UpdateVolume (){
			SoundManager.Instance.SetVolume(SoundType.MASTER, PlayerPrefs.GetFloat("MasterVolume"));
		}
	}
}
using System;
using UnityEngine;

namespace BattleCity
{
	public class AudioManager : MonoBehaviour
	{
		[SerializeField]
		private AudioSource MainAudioSource;
		[SerializeField]
		private AudioSource SecondaryAudioSource;
		[SerializeField]
		private AudioSource PlayerAudioSource;

		public static event EventHandler FinishPlayMainSoundEvent;
		public static event EventHandler FinishPlaySecondarySoundEvent;

		private static AudioManager _instance;
		private PlayerAudioType _player1;
		private PlayerAudioType _player2;

		public static void PlaySoundPlayer(AudioClip clip, bool loop, Block player,
			PlayerAudioType type)
		{
			if (clip == null)
				return;

			var audio = _instance.PlayerAudioSource;
			if (!audio.enabled)
				return;

			if (player == Block.Player1)
				_instance._player1 = type;
			else if (player == Block.Player2)
				_instance._player2 = type;

			if (type == PlayerAudioType.Idle)
				if (_instance._player1 == PlayerAudioType.Move ||
					_instance._player2 == PlayerAudioType.Move)
					return;

			if (audio.clip == clip)
				return;

			audio.loop = loop;
			audio.clip = clip;
			audio.Play();
		}

		public static void PlayMainSound(AudioClip clip)
		{
			if (clip == null)
				return;

			_instance.SecondaryAudioSource.enabled = false;
			_instance.PlayerAudioSource.enabled = false;

			var audio = _instance.MainAudioSource;
			audio.PlayOneShot(clip);
			FinishPlayMainSoundEvent += (s, e) =>
			{
				_instance.SecondaryAudioSource.enabled = true;
				_instance.PlayerAudioSource.enabled = true;
				FinishPlayMainSoundEvent = null;
			};
		}

		public static void PlaySecondarySound(AudioClip clip)
		{
			if (clip == null)
				return;

			var audio = _instance.SecondaryAudioSource;
			if (!audio.enabled)
				return;

			audio.PlayOneShot(clip);
		}

		private void Awake()
		{
			_instance = this;
		}

		private void Update()
		{
			if (!MainAudioSource.isPlaying)
			{
				if (FinishPlayMainSoundEvent != null)
					FinishPlayMainSoundEvent(this, EventArgs.Empty);
			}
			if (!SecondaryAudioSource.isPlaying)
			{
				if (FinishPlaySecondarySoundEvent != null)
					FinishPlaySecondarySoundEvent(this, EventArgs.Empty);
			}
		}

		public enum PlayerAudioType
		{
			Idle,
			Move
		}
	}
}
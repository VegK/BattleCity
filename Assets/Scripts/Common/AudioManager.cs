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

		public static bool EnablePlayerSound
		{
			get
			{
				return _instance.PlayerAudioSource.enabled;
			}
			set
			{
				_instance.PlayerAudioSource.enabled = value;
			}
		}

		public static bool EnableSecondarySound
		{
			get
			{
				return _instance._enableSecondarySound;
			}
			set
			{
				_instance._enableSecondarySound = value;

				if (value)
					_instance.SecondaryAudioSource.enabled = true;
				else
					FinishPlaySecondarySoundEvent += _instance.OnDisableSecondaryAudio;
			}
		}

		private static AudioManager _instance;
		private PlayerAudioType _player1;
		private PlayerAudioType _player2;
		private bool _enableSecondarySound;

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

			if (audio.clip == clip && audio.isPlaying)
				return;

			audio.loop = loop;
			audio.clip = clip;
			audio.Play();
		}

		public static void StopSoundPlayer(Block player)
		{
			if (player == Block.Player1)
				_instance._player1 = PlayerAudioType.Idle;
			else if (player == Block.Player2)
				_instance._player2 = PlayerAudioType.Idle;

			_instance.PlayerAudioSource.Stop();
		}

		public static void PlayMainSound(AudioClip clip)
		{
			if (clip == null)
				return;

			_instance.PlayerAudioSource.enabled = false;

			var audio = _instance.MainAudioSource;
			audio.PlayOneShot(clip);
			FinishPlayMainSoundEvent += OnEnablePlayerAudio;
		}

		public static void PlaySecondarySound(AudioClip clip)
		{
			if (clip == null)
				return;
			if (!_instance._enableSecondarySound)
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
				{
					FinishPlayMainSoundEvent(this, EventArgs.Empty);
					FinishPlayMainSoundEvent = null;
				}
			}
			if (!SecondaryAudioSource.isPlaying)
			{
				if (FinishPlaySecondarySoundEvent != null)
				{
					FinishPlaySecondarySoundEvent(this, EventArgs.Empty);
					FinishPlaySecondarySoundEvent = null;
				}
			}
		}

		private static void OnEnablePlayerAudio(object sender, EventArgs e)
		{
			_instance.PlayerAudioSource.enabled = true;
			FinishPlayMainSoundEvent -= OnEnablePlayerAudio;
		}

		private void OnDisableSecondaryAudio(object sender, EventArgs e)
		{
			SecondaryAudioSource.enabled = false;
			FinishPlaySecondarySoundEvent -= OnDisableSecondaryAudio;
		}

		public enum PlayerAudioType
		{
			Idle,
			Move
		}
	}
}
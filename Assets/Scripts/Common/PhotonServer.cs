using System;
using UnityEngine;

namespace BattleCity.Net
{
	public class PhotonServer : Photon.MonoBehaviour
	{
		public static event EventHandler ConnectedToMasterEvent;
		public static event EventHandler JoinedLobbyEvent;
		public static event EventHandler JoinedRoomEvent;
		
		public static bool Connect()
		{
			if (!PhotonNetwork.connected)
				PhotonNetwork.ConnectUsingSettings(Consts.VERSION);
			return PhotonNetwork.connected;
		}

		public static void Disconnect()
		{
			PhotonNetwork.Disconnect();
		}

		public static bool CreateRoom(string roomName)
		{
			if (!PhotonNetwork.connected)
				return false;
			
			var options = new RoomOptions
			{
				isOpen = true,
				isVisible = true,
				maxPlayers = 2
			};
			PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
			return true;
		}

		public static bool JoinRoom(string roomName)
		{
			if (!PhotonNetwork.connected)
				return false;
			
			PhotonNetwork.JoinRoom(roomName);
			return true;
		}

		private void Start()
		{
			PhotonNetwork.autoJoinLobby = false;
		}

		private void OnConnectedToMaster()
		{
			if (ConnectedToMasterEvent != null)
				ConnectedToMasterEvent(this, EventArgs.Empty);
			PhotonNetwork.JoinLobby();
		}

		private void OnJoinedLobby()
		{
			if (JoinedLobbyEvent != null)
				JoinedLobbyEvent(this, EventArgs.Empty);
		}

		private void OnJoinedRoom()
		{
			if (JoinedRoomEvent != null)
				JoinedRoomEvent(this, EventArgs.Empty);
		}
	}
}
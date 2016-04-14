using UnityEditor;
using UnityEngine;

namespace BattleCity.Net
{
	public class PhotonServer : Photon.MonoBehaviour
	{
		private static bool _connected;

		public static bool Connect()
		{
			_connected = PhotonNetwork.ConnectUsingSettings(Consts.VERSION);
			return _connected;
		}

		public static void CreateRoom(string roomName)
		{
			if (!_connected)
				Connect();

			var options = new RoomOptions
			{
				isOpen = true,
				isVisible = true,
				maxPlayers = 2
			};
			PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
		}

		public static void JoinRoom(string roomName)
		{
			if (!_connected)
				Connect();
			
			PhotonNetwork.JoinRoom(roomName);
		}
	}
}
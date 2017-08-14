using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class RandomMatchMaker : Photon.PunBehaviour
{
	private GameObject	_player;
	private float		_speed = 1f;
	private float		_deltaSpeed = 0f;

	void Start()
	{
		//PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	/*
	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString() + "Is Mastrer : " + PhotonNetwork.isMasterClient);
	}
	*/

	void Update()
	{
		_deltaSpeed += Time.deltaTime;
		if (_deltaSpeed > 1f / _speed)
		{
			Debug.Log(PhotonNetwork.connectionStateDetailed.ToString() + " | Is Mastrer : " + PhotonNetwork.isMasterClient);
			_deltaSpeed = 0f;
		}
	}

	public override void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		PhotonNetwork.CreateRoom(null);
	}

	override public void OnCreatedRoom()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	override public void OnJoinedRoom()
	{
		Debug.Log("P" + PhotonNetwork.player.ID);

		if (PhotonNetwork.player.ID == 1)
		{
			_player = PhotonNetwork.Instantiate("PlayerNetwork", new Vector3(32, 32, 0), Quaternion.identity, 0);
		}
		else
		{
			_player = PhotonNetwork.Instantiate("Player2TEOTH", Vector3.zero, Quaternion.identity, 0);
		}
	}
}

using UnityEngine;
using Photon;

public class NetworkPlayer : Photon.MonoBehaviour
{
	private Player				_player;
	private SpriteManager		_sprite;

	private Vector3				_position;
	private Vector2				_movement;
	private Vector2				_side;
	private Player.PlayerState	_state;

	void Start()
	{
		_player = GetComponent<Player>();
		_sprite = GetComponent<SpriteManager>();
		_player._isMine = photonView.isMine;
	}

	void Update()
	{
		if (!photonView.isMine)
		{
			if (_movement != Vector2.zero)
				_player._movement = _movement;
			else
			{
				Vector2 movement = _position - transform.position;
				movement.x = Mathf.Clamp((int)movement.x * 1000, -1, 1);
				movement.y = Mathf.Clamp((int)movement.y * 1000, -1, 1);
				_player._movement = movement;
				_player.transform.position = _position;
				_sprite.SetSide(_side);
				_player._state = _state;
			}
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(_player._movement);
			stream.SendNext(_sprite.GetSide());
			stream.SendNext(_player._state);
		}
		else
		{
			// Network player, receive data
			_position = (Vector3)stream.ReceiveNext();
			_movement = (Vector2)stream.ReceiveNext();
			_side = (Vector2)stream.ReceiveNext();
			_state = (Player.PlayerState)stream.ReceiveNext();
		}
	}
}
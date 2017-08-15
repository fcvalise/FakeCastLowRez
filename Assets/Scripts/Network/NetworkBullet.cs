using UnityEngine;
using Photon;

public class NetworkBullet : Photon.MonoBehaviour
{
	private Bullet				_bullet;
	private Vector3				_position;

	void Start()
	{
		_bullet = GetComponent<Bullet>();
		//_bullet._isMine = photonView.isMine;
	}

	void Update()
	{
		//if (!_bullet._isMine)
		//	transform.position = Vector3.Lerp(_position, this.transform.position, Time.deltaTime * 5f);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(transform.position);
		}
		else
		{
			// Network player, receive data
			_position = (Vector3)stream.ReceiveNext();
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class ShotProjectile : ASkill
{
	public GameObject	_bullet;
	public int			_deltaSpawn = 5;
	private static PhotonView ScenePhotonView;
	private GameObject	_currentBullet;


	void Start ()
	{
		base.Init(2.0f, 0.1f, _owner._cast);
		ScenePhotonView = this.GetComponent<PhotonView>();
	}

	public override void Cast(Player p_owner)
	{
		Vector3 delta = p_owner.GetComponent<Player>()._side * _deltaSpawn;
		//GameObject bullet = Instantiate(_bullet, p_owner.transform.position + delta, p_owner.transform.rotation);
		//bullet.GetComponent<Bullet>().Setup();
		//bullet.GetComponent<Bullet>().SetSideAndTarget(p_owner.GetTarget(), p_owner.GetComponent<Player>()._side);
		ScenePhotonView.RPC("Spawn", PhotonTargets.All, p_owner.transform.position + delta, p_owner.GetTarget().transform.position, p_owner.GetComponent<Player>()._side);
	}

	[PunRPC]
	private void Spawn(Vector3 position, Vector3 target, Vector2 side)
	{
		_currentBullet = Instantiate(_bullet, position, Quaternion.identity);
		_currentBullet.GetComponent<Bullet>().Setup();
		_currentBullet.GetComponent<Bullet>().SetSideAndTarget(target, side);
	}

	/*
	[PunRPC]
	private void UpdateTarget(Vector3 target, Vector2 side)
	{
		if (_currentBullet != null)
		_currentBullet.GetComponent<Bullet>().SetSideAndTarget(target, side);
	}

	void Update()
	{
		ScenePhotonView.RPC("UpdateTarget", PhotonTargets.All, _owner.GetTarget().transform.position, _owner.GetComponent<Player>()._side);
	}
	*/
}

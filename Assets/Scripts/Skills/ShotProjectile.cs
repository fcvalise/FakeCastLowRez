using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotProjectile : ASkill
{
	public GameObject	_bullet;
	public int			_deltaSpawn = 5;

	void Start ()
	{
		base.Init(2.0f, 0.1f, _owner._cast);
	}

	public override void Cast(Player p_owner)
	{
		Vector3 delta = p_owner.GetComponent<Player>()._side * _deltaSpawn;
		GameObject bullet = Instantiate(_bullet, p_owner.transform.position + delta, p_owner.transform.rotation);
		bullet.GetComponent<Bullet>().Setup();
		bullet.GetComponent<Bullet>().SetSideAndTarget(p_owner.GetTarget(), p_owner.GetComponent<Player>()._side);
	}
}

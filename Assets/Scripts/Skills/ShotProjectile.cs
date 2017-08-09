using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit;

public class ShotProjectile : ASkill {

	public GameObject _bullet;

	void Start () {
		base.Init(1.0f, 5.0f, "space");
	}

	public override void Cast(Player p_owner) {
		GameObject bullet = Instantiate(_bullet, p_owner.transform.position, p_owner.transform.rotation);
		bullet.GetComponent<Bullet>()._target = p_owner.GetTarget();
	}
}

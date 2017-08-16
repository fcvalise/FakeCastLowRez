using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastDamage : ASkill
{
	public GameObject	_damage;
	public ColorHSV		_damageColor = new ColorHSV(Color.red);
	private int			_deltaSpawn = 5;

	void Start ()
	{
		base.Init(2.0f, 0.1f, _owner._damageKey, _damageColor);
	}

	public override void Cast(Player p_owner)
	{
		Vector3 delta = p_owner.GetComponent<Player>()._side * _deltaSpawn;
		GameObject damageObject = Instantiate(_damage, p_owner.transform.position + delta, p_owner.transform.rotation);
		Damage damage = damageObject.GetComponent<Damage>();
		damage.Setup();
		damage.SetSideAndTarget(p_owner.GetTarget(), p_owner.GetComponent<Player>()._side);
		damage._damageColor = _damageColor;
	}
}
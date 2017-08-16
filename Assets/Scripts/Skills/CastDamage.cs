using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastDamage : ASkill
{
	public GameObject	_damage;
	public ColorHSV		_damageColor = new ColorHSV(319f / 360f, 0.47f, 1f, 0.6f);

	void Start ()
	{
		base.Init(2.0f, 0.1f, _owner._damageKey, _damageColor);
	}

	public override void Cast(Player p_owner)
	{
		GameObject damageObject = Instantiate(_damage, p_owner.transform.position, p_owner.transform.rotation);
		Damage damage = damageObject.GetComponent<Damage>();
		damage.SetSideAndTarget(p_owner.GetTarget(), p_owner.GetComponent<Player>()._side);
		damage._damageColor = _damageColor;
	}
}
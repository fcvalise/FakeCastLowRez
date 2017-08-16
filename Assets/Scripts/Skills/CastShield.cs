using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastShield : ASkill
{
	public GameObject	_shield;
	public ColorHSV		_shieldColor = new ColorHSV(Color.blue);

	void Start ()
	{
		base.Init(1.0f, 2.0f, _owner._shieldKey, _shieldColor);
	}

	public override void Cast(Player p_owner)
	{
		GameObject shield = Instantiate(_shield, p_owner.transform.position, p_owner.transform.rotation);
		shield.GetComponent<Shield>().Setup();
		shield.GetComponent<Shield>()._colorShield = _shieldColor;
	}
}
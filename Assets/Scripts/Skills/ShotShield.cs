using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotShield : ASkill
{
	public GameObject	_shield;
	public ColorHSV		_shieldColor = new ColorHSV(Color.blue);

	void Start ()
	{
		base.Init(1.0f, 0.1f, _owner._shield, _shieldColor);
	}

	public override void Cast(Player p_owner)
	{
		GameObject shield = Instantiate(_shield, p_owner.transform.position, p_owner.transform.rotation);
		shield.GetComponent<Shield>().Setup();
		shield.GetComponent<Shield>()._colorShield = _shieldColor;
	}
}

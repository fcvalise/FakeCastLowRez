using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastShield : ASkill
{
	public GameObject					_shield;
	public ColorHSV						_shieldColor = new ColorHSV(219f / 360f, 1f, 1f, 0.6f);
	[HideInInspector] public GameObject	_currentShield;
	private GameObject					_lastShield;

	void Start ()
	{
		base.Init(1.0f, 2.0f, _owner._shieldKey, _shieldColor);
	}

	public override void Cast(Player p_owner)
	{
		if (_currentShield != null)
		{
			_currentShield.GetComponent<Shield>()._isDestroy = true;
			_lastShield = _currentShield;
		}
		_currentShield = Instantiate(_shield, p_owner.transform.position, p_owner.transform.rotation);
		_currentShield.GetComponent<Shield>()._colorShield = _shieldColor;
	}
}
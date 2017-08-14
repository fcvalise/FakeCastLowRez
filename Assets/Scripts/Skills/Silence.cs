using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silence : ASkill
{
	public float	_duration;

	void Start ()
	{
		base.Init(1.0f, 5.0f, _owner._silence);
	}

	public override void Cast(Player p_owner)
	{
		p_owner.GetTarget().GetComponent<Player>().IsSilence = true;
		StartCoroutine(SilenceCo(p_owner));
	}

	IEnumerator SilenceCo(Player p_owner)
	{
		yield return new WaitForSeconds(_duration);
		p_owner.GetTarget().GetComponent<Player>().IsSilence = false;
	}
}

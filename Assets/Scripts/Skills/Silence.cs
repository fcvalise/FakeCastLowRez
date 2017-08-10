using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit;

public class Silence : ASkill
{
	public float _duration;

	void Start ()
	{
		base.Init(1.0f, 5.0f, "space");
	}

	public override void Cast(Player p_owner)
	{
		p_owner.GetTarget().GetComponent<Player>().Silence();
		StartCoroutine(SilenceCo());
	}

	IEnumerator SilenceCo()
	{
		yield return new WaitForSeconds(_duration);
	}
}

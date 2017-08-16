using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSilence : ASkill
{
	public float	_duration;
	public ColorHSV	_silenceColor = new ColorHSV(Color.green);

	void Start ()
	{
		base.Init(0.4f, 5.0f, _owner._silenceKey, _silenceColor);
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
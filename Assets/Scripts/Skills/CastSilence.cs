using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSilence : ASkill
{
	public GameObject	_silence;
	public ColorHSV		_silenceColor = new ColorHSV(134.0f / 360.0f, 0.55f, 1.0f, 0.6f);

	private float		_duration = 5.0f;
	private GameObject	_targetObject;

	void Start ()
	{
		base.Init(0.4f, 5.0f, _owner._silenceKey, _silenceColor);
	}

	public override void Cast(Player p_owner)
	{
		_targetObject = p_owner.GetTarget();
		Player target = _targetObject.GetComponent<Player>();

		if (target.CanCastSkill())
			_targetObject = p_owner.gameObject;

		target = _targetObject.GetComponent<Player>();

		target.IsSilence = true;
		StartCoroutine(SilenceCo(target));

		GameObject damageObject = Instantiate(_silence, target.transform.position, target.transform.rotation);
		Silence silence = damageObject.GetComponent<Silence>();
		silence.SetTarget(_targetObject);
		silence._silenceColor = _silenceColor;
	}

	IEnumerator SilenceCo(Player target)
	{
		yield return new WaitForSeconds(_duration);
		target.GetComponent<Player>().IsSilence = false;
	}
}
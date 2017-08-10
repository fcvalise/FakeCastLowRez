using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit;

public abstract class ASkill : MonoBehaviour
{
	enum SkillState
	{
		Waiting,
		Casting,
		Cooldown
	}

	Player _owner;
	float _castDuration = 0.0f;
	float _cooldownDuration = 0.0f;
	float _castTimer = 0.0f;
	float _cooldown = 0.0f;
	SkillState _state;

	string _key;

	void Awake()
	{
		_state = SkillState.Waiting;
		_owner = GetComponent<Player>();
	}

	protected void Init(float p_castDuration, float p_cooldownDuration, string key)
	{
		_castDuration = p_castDuration;
		_cooldownDuration = p_cooldownDuration;
		_key = key;
	}

	// TODO update cast bar
	// Add input in player
	void Update()
	{
		switch (_state)
		{
		case SkillState.Waiting:
			if (_owner.CanCastSkill() && Input.GetKeyDown(_key))
			{
				_state = SkillState.Casting;
				_castTimer = _castDuration;
				_owner.SetState(Player.PlayerState.CastSkill);
			}
			break;

		case SkillState.Casting:
			_castTimer -= Time.deltaTime;

			if (_owner.IsSilence())
			{
				_cooldown = _cooldownDuration;
				_state = SkillState.Cooldown;
				_owner.SetState(Player.PlayerState.None);
			}
			else if (_castTimer <= 0.0f)
			{
				Cast(_owner);
				_cooldown = _cooldownDuration;
				_state = SkillState.Cooldown;
				_owner.SetState(Player.PlayerState.None);
			}
			break;

		case SkillState.Cooldown:
			_cooldown = Mathf.Clamp(_cooldown - Time.deltaTime, 0.0f, _cooldownDuration);
			if (_cooldown <= 0.0f)
			{
				_state = SkillState.Waiting;
			}
			break;

		default:
			break;
		}
	}

	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 500, 20), "Skill : ");
		GUI.Label(new Rect(10, 150, 500, 20), "Remaining cooldown " + _cooldown.ToString());
		GUI.Label(new Rect(10, 170, 500, 20), "Casting Time " + _castTimer.ToString() + "/" + _castDuration.ToString());
	}

	public abstract void Cast(Player p_owner);
}
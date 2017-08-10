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

	protected Player	_owner;
	private float		_castDuration = 0.0f;
	private float		_cooldownDuration = 0.0f;
	private float		_castTimer = 0.0f;
	private float		_cooldown = 0.0f;
	private SkillState	_state;

	private KeyCode		_key;

	private ColorHSV	_guiColor = new ColorHSV(219f / 360f, 1f, 1f);
	private GUIStyle	_guiStyle = new GUIStyle();

	void Awake()
	{
		_state = SkillState.Waiting;
		_owner = GetComponent<Player>();
	}

	protected void Init(float p_castDuration, float p_cooldownDuration, KeyCode key)
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
				_owner.SetState(Player.PlayerState.Shoot);
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
		_guiStyle.fontSize = 2;
		string str = "";
		if (_state == SkillState.Casting)
		{
			float coef = _castTimer / _castDuration;
			int progress = (int)((1 - coef) * Core._width);

			_guiColor.s = coef;
			_guiColor.a = 1 - coef;
			_guiStyle.normal.textColor = _guiColor.ToColor();

			for (int i = 0; i < progress; i++)
				str += "█";
		}
		else if (_state == SkillState.Cooldown)
		{
			_guiStyle.normal.textColor = Color.white;

			int progress = (int)((_cooldown / _cooldownDuration) * Core._width);
			for (int i = 0; i < progress; i++)
				str += "█";
		}
		GUI.Label(new Rect(0, 0, 64, 64), str, _guiStyle);
	}

	public abstract void Cast(Player p_owner);
}
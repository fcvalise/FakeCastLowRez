using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class ASkill : MonoBehaviour
{
	enum SkillState
	{
		Waiting,
		Casting,
		Cooldown
	}

	public Buff			_ui;
	protected Player	_owner;
	protected ColorHSV	_castColor;
	private float		_castDuration = 0.0f;
	private float		_cooldownDuration = 0.0f;
	private float		_castTimer = 0.0f;
	private float		_cooldown = 0.0f;
	private SkillState	_state;

	private KeyCode		_key;
	public AudioClip	_sound;
	private AudioSource	_audioSource;

	void Awake()
	{
		_state = SkillState.Waiting;
		_owner = GetComponent<Player>();
		_audioSource = gameObject.AddComponent<AudioSource>();
	}

	protected void Init(float p_castDuration, float p_cooldownDuration, KeyCode key, ColorHSV color)
	{
		_castDuration = p_castDuration;
		_cooldownDuration = p_cooldownDuration;
		_key = key;
		_castColor = color;
	}

	void Update()
	{
		if (_ui != null)
			_ui._number = (int)Mathf.Ceil(_cooldown);
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
			_owner._castColor = _castColor;
			_owner.GetComponent<UIPlayer>().SetCastPercent(1.0f - (_castTimer / _castDuration));
			_owner.SetCastingPercent(1.0f - (_castTimer / _castDuration));
			if (_owner.IsSilence || _owner.IsMoving())
			{
				_state = SkillState.Cooldown;
				_owner.SetState(Player.PlayerState.None);
				_owner.SetCastingPercent(0.0f);
			}
			else if (_castTimer <= 0.0f)
			{
				_owner.GetComponent<UIPlayer>().SetCastPercent(0.0f);
				_owner.SetCastingPercent(0.0f);
				Cast(_owner);
				_audioSource.PlayOneShot(_sound, 1.0f);
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

	public float GetCDPercent()
	{
		return _cooldown / _cooldownDuration;
	}

	public abstract void Cast(Player p_owner);
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(SpriteManager))]
public class Player : ACellObject
{
	public enum PlayerState
	{
		None,
		CastSkill,
		Shoot
	}

	public GameObject			_target;
	public ProgressBar			_lifeBar;
	public ProgressBar			_castBar;
	public ColorHSV				_castColor = new ColorHSV(Color.white);

	public KeyCode				_upKey;
	public KeyCode				_leftKey;
	public KeyCode				_downKey;
	public KeyCode				_rightKey;
	public KeyCode				_damageKey;
	public KeyCode				_shieldKey;
	public KeyCode				_silenceKey;

	private Cell[,]				_cells;
	private SpriteManager		_spriteManager;
	private Vector2				_size;
	private int					_lifeMax = 100;
	private int					_life = 100;
	private GUIStyle			_guiStyle = new GUIStyle();

	private Vector2				_movement = Vector2.down;
	private Vector2				_lastMovement = Vector2.down;
	[HideInInspector]
	public Vector2				_side;

	private bool				_isSilence = false;
	private PlayerState			_state = PlayerState.None;
	private float				_colorFactor;
	private ColorHSV			_colorDamage;
	public Font					_font;

	public AudioClip	_sound;
	private AudioSource	_audioSource;

	public override int GetZIndex() { return 6; }

	private void Awake()
	{
		_colorDamage = new ColorHSV(new Color(1.0f, 0.0f, 0.0f, 1.0f));
		_spriteManager = GetComponent<SpriteManager>();
		//TODO : To get from SpriteManager
		_size = new Vector2(10, 10);
		transform.position = new Vector2(64 / 2, 64 / 2);
		_cells = new Cell[(int)_size.x, (int)_size.y];
		_colorDamage = GetComponent<CastDamage>()._damageColor;
		_audioSource = gameObject.AddComponent<AudioSource>();
	}

	public override void Simulate()
	{
		UpdatePosition();
		UpdateSpriteManager();
		_lastMovement = _movement;
		_lifeBar._percent = (float)(_life) / (float)_lifeMax;
		AddAim();
	}

	private void Update()
	{
		_movement = Vector2.zero;

		if (Input.GetKey(_rightKey) || Input.GetKey(_leftKey) || Input.GetKey(_upKey) || Input.GetKey(_downKey))
			_state = PlayerState.None;

		if (_state == PlayerState.None || _state == PlayerState.Shoot)
		{
			if (Input.GetKey(_rightKey))
				_movement += Vector2.right;
			if (Input.GetKey(_leftKey))
				_movement += Vector2.left;
			if (Input.GetKey(_upKey))
				_movement += Vector2.up;
			if (Input.GetKey(_downKey))
				_movement += Vector2.down;
			GetComponent<UIPlayer>().SetCastPercent(0.0f);
		}
	}

	private void UpdatePosition()
	{
		Vector2 position = transform.position;

		if (Mathf.Abs(_lastMovement.x) == 1 && Mathf.Abs(_lastMovement.y) == 1)
			_movement = Vector2.zero;

		position += _movement;
		position.x = Mathf.Clamp(position.x, 1, Core._width - _size.x);
		position.y = Mathf.Clamp(position.y, 1, Core._height - _size.y);

		transform.position = position;
	}

	private void UpdateSpriteManager()
	{
		SetSpriteSide();

		switch (_state)
		{
			case PlayerState.CastSkill:
			{
				if (_spriteManager.GetState() != "player_cast_incant")
					_spriteManager.PlayNext("player_cast_start");
				if (_spriteManager.isFinished())
					_spriteManager.PlayNext("player_cast_incant");
				break;
			}
			case PlayerState.Shoot:
			{
				_spriteManager.PlayNext("player_cast_shoot");
				if (_spriteManager.isFinished())
					_state = PlayerState.None;
				break;
			}
			default:
			{
				if (_movement != Vector2.zero || _lastMovement != Vector2.zero)
					_spriteManager.PlayNext("player_walk");
				else if (_life <= 0)
				{
					if (_spriteManager.GetState() != "player_lay")
						_audioSource.PlayOneShot(_sound, 1.0f);
					_colorDamage = new ColorHSV(Color.black);
					_spriteManager.PlayNext("player_lay");
				}
				else
					_spriteManager.PlayNext("player_idle");
			}
			break;
		}

		_spriteManager.Simulate(_cells);
	}

	private void SetSpriteSide()
	{
		//Follow target
		if (_state == PlayerState.CastSkill)
		{
			Vector2 targetPosition = _target.transform.position;

			// TODO: Generalize that behavior
			if (_target.GetComponent<CastShield>()._currentShield != null)
				targetPosition = _target.GetComponent<CastShield>()._currentShield.transform.position;
			Vector2 position = transform.position;
			position = position + _size / 2;
			_side = targetPosition - position;

			if (Mathf.Abs(_side.x) > Mathf.Abs(_side.y))
			{
				_side.x = Mathf.Clamp(_side.x, -1, 1);
				_side.y = 0;
			}
			else
			{
				_side.y = Mathf.Clamp(_side.y, -1, 1);
				_side.x = 0;
			}
		}
		else
			_side = _movement;

		_spriteManager.SetSide(_side);

	}

	private void AddAim()
	{
		if (_state == PlayerState.CastSkill)
		{
			Vector2 targetPosition = (Vector2)_target.transform.position + _size / 2;
			ColorHSV color = new ColorHSV(Color.white);

			// TODO: Generalize that behavior
			if (_target.GetComponent<CastShield>()._currentShield != null)
			{
				targetPosition = (Vector2)_target.GetComponent<CastShield>()._currentShield.transform.position + _size / 2;
				color = _target.GetComponent<CastShield>()._shieldColor;
			}
			Vector2 position = transform.position;
			position = position + _size / 2;
			Vector2 direction = targetPosition - position;
			direction.x = Mathf.Clamp(direction.x, -4, 4);
			direction.y = Mathf.Clamp(direction.y, -4, 4);
			_cells[5 + (int)direction.x, 5 + (int)direction.y].color = color;
			_cells[5 + (int)direction.x, 5 + (int)direction.y].value = 1f;
			_cells[5 + (int)direction.x, 5 + (int)direction.y].state = Cell.State.Alive;
		}
	}

	public override void Add(Cell[,] p_automaton, Cell[,] p_staticGrid)
	{
		Vector2Int position = new Vector2Int((int)transform.position.x, (int)transform.position.y);
		bool isOnAliveCell = false;
		
		for (int x = 0; x < _size.x; x++)
		{
			for (int y = 0; y < _size.y; y++)
			{
				if (x + transform.position.x < Core._width && y + transform.position.y < Core._height)
				{
					if (_cells[x, y].state == Cell.State.Alive)
					{
						p_staticGrid[x + position.x, y + position.y].value = _cells[x, y].value;
						if (_cells[x, y].color == new ColorHSV(Color.white))
							p_staticGrid[x + position.x, y + position.y].color = _castColor;
						else
							p_staticGrid[x + position.x, y + position.y].color = ColorHSV.Lerp(_cells[x, y].color, _colorDamage, _colorFactor);
						p_staticGrid[x + position.x, y + position.y].state = _cells[x, y].state;
					}

					if (_cells[x, y].state == Cell.State.Alive && p_automaton[x + position.x, y + position.y].state == Cell.State.Alive)
					{
						isOnAliveCell = true;
					}
				}
			}
		}
		if (isOnAliveCell)
		{
			_life -= 1;
			StartCoroutine(DamageColorCo());
		}
	}

	public void SetState(Player.PlayerState p_state)
	{
		_state = p_state;
	}

	public bool CanCastSkill()
	{
		return _state == PlayerState.None && !_isSilence;
	}

	public bool IsSilence
	{
		get { return _isSilence; }
		set { _isSilence = value; }
	}

	public bool IsMoving()
	{
		return _movement != Vector2.zero;
	}

	public GameObject GetTarget()
	{
		return _target; //TODO get the other player
	}

	public void SetCastingPercent(float p_percent)
	{
		_castBar._percent = p_percent;
	}

	public float GetLifePercent()
	{
		return (float)_life / (float)_lifeMax;
	}

	private IEnumerator DamageColorCo()
	{
		float timerMax = 0.5f;
		float timer = timerMax;
		while (timer > 0.0f)
		{
			timer -= Time.deltaTime;
			_colorFactor = Mathf.Clamp(timer / timerMax, 0.0f, 1.0f);
			yield return new WaitForEndOfFrame();
		}
	}

	void OnGUI()
	{
		_guiStyle.normal.textColor = Color.blue;
		_guiStyle.font = _font;
		_guiStyle.fontSize = 50;
		if (_life <= 0 && GUI.Button(new Rect(120, 200, 400, 400), "Replay!", _guiStyle))
			SceneManager.LoadScene("Game");
	}

	/*
	 * Debug
	 * 
	private void Fill()
	{
		for (int x = 0; x < _size.x; x++)
		{
			for (int y = 0; y < _size.y; y++)
			{
				_cells[x, y].state = Cell.State.Alive;
				_cells[x, y].value = 1f;
				_cells[x, y].color = new ColorHSV(200f / 360f, 1f, 1f);
			}
		}
	}
	*/
}
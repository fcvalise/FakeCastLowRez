using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	public GameObject			_lifeBar;
	public GameObject			_castBar;

	public KeyCode				_up;
	public KeyCode				_down;
	public KeyCode				_right;
	public KeyCode				_left;
	public KeyCode				_cast;
	public KeyCode				_silence;

	private Cell[,]				_cells;
	private SpriteManager		_spriteManager;
	private Vector2				_size;
	private int					_lifeMax = 64;
	private int					_life = 64;
	private GUIStyle			_guiStyle = new GUIStyle();

	private Vector2				_movement = Vector2.down;
	private Vector2				_lastMovement = Vector2.down;
	[HideInInspector]
	public Vector2				_side;

	private bool				_isSilence = false;
	private PlayerState			_state = PlayerState.None;

	public override void Setup()
	{
		_spriteManager = GetComponent<SpriteManager>();
		//TODO : To get from SpriteManager
		_size = new Vector2(10, 10);
		transform.position = new Vector2(64 / 2, 64 / 2);
		_cells = new Cell[(int)_size.x, (int)_size.y];
	}

	public override void Simulate()
	{
		UpdatePosition();
		UpdateSpriteManager();
		_lastMovement = _movement;
		_lifeBar.GetComponent<UnityEngine.UI.Image>().fillAmount = ((float)(_life + 8 - (_life % 8)) / (float)_lifeMax);
	}

	private void Update()
	{
		_movement = Vector2.zero;

		if (Input.GetKey(_right) || Input.GetKey(_left) || Input.GetKey(_up) || Input.GetKey(_down))
			_state = PlayerState.None;

		if (_state == PlayerState.None || _state == PlayerState.Shoot)
		{
			if (Input.GetKey(_right))
				_movement += Vector2.right;
			if (Input.GetKey(_left))
				_movement += Vector2.left;
			if (Input.GetKey(_up))
				_movement += Vector2.up;
			if (Input.GetKey(_down))
				_movement += Vector2.down;
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
					_spriteManager.PlayNext("player_lay");
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
			Vector2 position = transform.position;
			position = position + _size / 2;
			Vector2 targetPosition = _target.transform.position;
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
						p_staticGrid[x + position.x, y + position.y].color = _cells[x, y].color;
						p_staticGrid[x + position.x, y + position.y].state = _cells[x, y].state;
					}

					if (_cells[x, y].state == Cell.State.Alive && p_automaton[x + position.x, y + position.y].state == Cell.State.Alive)
						isOnAliveCell = true;
					//	p_automaton[x + position.x, y + position.y].state = _cells[x, y].state;
				}
			}
		}
		if (isOnAliveCell)
			_life -= 1;
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

	public GameObject GetTarget()
	{
		return _target; //TODO get the other player
	}

	public void SetCastingPercent(float p_percent)
	{
		float max = _castBar.GetComponent<RectTransform>().sizeDelta.x + 8;
		int percent = (int)(p_percent * max);
		_castBar.GetComponent<UnityEngine.UI.Image>().fillAmount = ((float)(percent - 8 - (percent % 8)) / max);
	}

	void OnGUI()
	{
		_guiStyle.fontSize = 2;
		string str = "";
		_guiStyle.normal.textColor = Color.blue;

		for (int i = 0; i < _life; i++)
			str += "█";

		GUI.Label(new Rect(0, 63, 64, 64), str, _guiStyle);

		_guiStyle.fontSize = 15;
		if (_life <= 0 && GUI.Button(new Rect(8, 20, 56, 40), "Replay!", _guiStyle))
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

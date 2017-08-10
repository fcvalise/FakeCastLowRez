using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteManager))]
public class Player : ACellObject
{
	public enum PlayerState
	{
		None,
		CastSkill
	}

	private Cell[,]				_cells;
	public Vector2				_bounds;

	private SpriteManager		_spriteManager;

	private Vector2				_size;
	[HideInInspector]
	public Vector2				_position;

	private Vector2				_movement = Vector2.down;
	private Vector2				_lastMovement = Vector2.down;

	private bool				_isSilence = false;
	private PlayerState			_state = PlayerState.None;

	public override void Setup()
	{
		_spriteManager = GetComponent<SpriteManager>();
		//TODO : To get from SpriteManager
		_size = new Vector2(10, 10);
		_position = new Vector2(64 / 2, 64 / 2);
		_cells = new Cell[(int)_size.x, (int)_size.y];
	}

	public override void Simulate()
	{
		switch (_state)
		{
		case PlayerState.None:
			UpdatePosition();
			break;
		case PlayerState.CastSkill:
			break;
		default:
			break;
		}

		UpdateSpriteManager();
		_lastMovement = _movement;
	}

	private void Update()
	{
		_movement = Vector2.zero;

		if (Input.anyKeyDown)
			_state = PlayerState.None;
		if (Input.GetKeyDown(KeyCode.Space))
			_state = PlayerState.CastSkill;

		if (_state == PlayerState.None)
		{
			if (Input.GetKey(KeyCode.RightArrow))
				_movement += Vector2.right;
			if (Input.GetKey(KeyCode.LeftArrow))
				_movement += Vector2.left;
			if (Input.GetKey(KeyCode.UpArrow))
				_movement += Vector2.up;
			if (Input.GetKey(KeyCode.DownArrow))
				_movement += Vector2.down;
		}
	}

	private void UpdatePosition()
	{
		if (Mathf.Abs(_lastMovement.x) == 1 && Mathf.Abs(_lastMovement.y) == 1)
			_movement = Vector2.zero;

		_position += _movement;
		_position.x = Mathf.Clamp(_position.x, 1, _bounds.x - _size.x);
		_position.y = Mathf.Clamp(_position.y, 1, _bounds.y - _size.y);
	}

	private void UpdateSpriteManager()
	{
		_spriteManager.SetSide(_movement);

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
			default:
			{
				if (_movement != Vector2.zero || _lastMovement != Vector2.zero)
					_spriteManager.PlayNext("player_walk");
				else
					_spriteManager.PlayNext("player_lay");
			}
			break;
		}

		_spriteManager.Simulate(_cells);
	}

	public override void Add(Cell[,] p_automaton, Cell[,] p_staticGrid)
	{
		for (int x = 0; x < _size.x; x++)
		{
			for (int y = 0; y < _size.y; y++)
			{
				if (x + _position.x < _bounds.x && y + _position.y < _bounds.y)
				{
					p_staticGrid[x + (int)_position.x, y + (int)_position.y].value = _cells[x, y].value;
					p_staticGrid[x + (int)_position.x, y + (int)_position.y].state = _cells[x, y].state;
					p_staticGrid[x + (int)_position.x, y + (int)_position.y].color = _cells[x, y].color;

					if (_cells[x, y].state == Cell.State.Alive)
						p_automaton[x + (int)_position.x, y + (int)_position.y].state = _cells[x, y].state;
				}
			}
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

	public void Silence()
	{
		_isSilence = true;
	}

	public bool IsSilence()
	{
		return _isSilence;
	}

	public GameObject GetTarget()
	{
		return null; //TODO get the other player
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

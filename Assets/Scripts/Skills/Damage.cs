using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : ACellObject
{
	public CellSprite			_sprite;
	public ColorHSV				_damageColor;

	private int					_speed = 2;
	private GameObject			_target;
	private Cell[,]				_cells;
	private Vector2				_movement;

	public override int GetZIndex() { return 6; }

	private void Awake()
	{
		_sprite = _sprite.AddSprite(gameObject);
		_cells = new Cell[_sprite._size.x, _sprite._size.y];
	}

	//TODO : Really not a good way...
	public void SetSideAndTarget(GameObject target, Vector2 side)
	{
		_target = target;
		if (_target.GetComponent<CastShield>()._currentShield != null)
			_target = _target.GetComponent<CastShield>()._currentShield;
		_sprite.SetSide(side);
	}

	public override void Simulate()
	{
		UpdatePosition();
		UpdateSprite();
		UpdateCollision();
	}

	private void UpdatePosition()
	{
		Vector2 position = transform.position;
		if (_target != null)
		{
			Vector2 targetPosition = _target.transform.position;
			_movement = (targetPosition - position).normalized;
			position += _movement * _speed;
			position.x = Mathf.Clamp(position.x, 1, Core._width - _sprite._size.x);
			position.y = Mathf.Clamp(position.y, 1, Core._height - _sprite._size.y);
			transform.position = position;
		}
		else
			Destroy(gameObject);
	}

	private void UpdateSprite()
	{
		_sprite.SetSide(_movement);
		_sprite.Simulate(_cells);
	}

	private void UpdateCollision()
	{
		if (Vector2.Distance(transform.position, _target.transform.position) < 2.0f)
		{
			Destroy(gameObject);
			if (_target.GetComponent<Shield>() != null)
				_target.GetComponent<Shield>()._isDestroy = true;
		}
	}

	public override void Add(Cell[,] p_automaton, Cell[,] p_staticGrid)
	{
		Vector2Int position = new Vector2Int((int)transform.position.x, (int)transform.position.y);

		for (int x = 0; x < _sprite._size.x; x++)
		{
			for (int y = 0; y < _sprite._size.y; y++)
			{
				if (_cells[x, y].state == Cell.State.Alive)
				{
					p_staticGrid[x + position.x, y + position.y].value = _cells[x, y].value;
					p_staticGrid[x + position.x, y + position.y].state = _cells[x, y].state;
					p_staticGrid[x + position.x, y + position.y].color = _damageColor;
					if (Vector2.Distance(transform.position, _target.transform.position) < 15.0f)
						p_automaton[x + position.x, y + position.y].state = Cell.State.Alive;
					else
						p_automaton[x + position.x, y + position.y].state = Cell.State.Dead;
				}
			}
		}
	}
}
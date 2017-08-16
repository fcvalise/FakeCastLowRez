using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ACellObject
{
	public int					_speed;
	public CellSprite			_sprite;
	public ColorHSV				_bulletColor;

	private GameObject			_target;
	private Cell[,]				_cells;
	private Vector2				_movement;

	public override void Setup()
	{
		_sprite = _sprite.AddSprite(gameObject);
		_cells = new Cell[_sprite._size.x, _sprite._size.y];
	}

	//TODO : Really not a good way...
	public void SetSideAndTarget(GameObject target, Vector2 side)
	{
		_target = target;
		_sprite.SetSide(side);
	}

	public override void Simulate()
	{
		UpdatePosition();
		UpdateSprite();
		//Find a proper way to get collision
		int accuracy = 3; // For accuracy = 1 the position need to be exactly the same
		if ((int)(transform.position.x / accuracy) == (int)(_target.transform.position.x / accuracy) &&
			(int)(transform.position.y / accuracy) == (int)(_target.transform.position.y / accuracy))
			Destroy(gameObject);
	}

	private void UpdatePosition()
	{
		Vector2 position = transform.position;
		Vector2 targetPosition = _target.transform.position;
		_movement = (targetPosition - position).normalized;
		position += _movement * _speed;
		position.x = Mathf.Clamp(position.x, 1, Core._width - _sprite._size.x);
		position.y = Mathf.Clamp(position.y, 1, Core._height - _sprite._size.y);
		transform.position = position;
	}

	private void UpdateSprite()
	{
		_sprite.SetSide(_movement);
		_sprite.Simulate(_cells);
	}

	public override void Add(Cell[,] p_automaton, Cell[,] p_staticGrid)
	{
		Vector2Int position = new Vector2Int((int)transform.position.x, (int)transform.position.y);

		for (int x = 0; x < _sprite._size.x; x++)
		{
			for (int y = 0; y < _sprite._size.y; y++)
			{
				p_staticGrid[x + position.x, y + position.y].value = _cells[x, y].value;
				p_staticGrid[x + position.x, y + position.y].state = _cells[x, y].state;
				p_staticGrid[x + position.x, y + position.y].color = _cells[x, y].color;
				//TODO : Rework, the SpriteCell should take car of that
				if (_sprite._printOnAutomaton && _cells[x, y].state == Cell.State.Alive)
				{
					p_staticGrid[x + position.x, y + position.y].color = _bulletColor;
					p_automaton[x + position.x, y + position.y].state = _cells[x, y].state;
				}
			}
		}
	}
}
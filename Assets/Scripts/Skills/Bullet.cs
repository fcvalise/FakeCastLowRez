using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ACellObject
{
	public int					_speed;
	public GameObject			_target;
	public Vector2				_targetPosition;
	public CellSprite			_sprite;

	public Vector2				_bounds;

	private Cell[,]				_cells;
	private SpriteManager		_cellSprite;
	private Vector2				_size;
	private Vector2				_position;

	public override void Setup()
	{
		_position = new Vector2(10, 10);
		_sprite.Create();
		//TODO : To get from SpriteManager
		_size = new Vector2(10, 10);
		_cells = new Cell[(int)_size.x, (int)_size.y];
	}

	public override void Simulate()
	{
		Vector2 direction = (_targetPosition - _position).normalized;
		_position += direction * _speed;
		_sprite.Simulate(_cells);
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
				}
			}
		}
	}
}
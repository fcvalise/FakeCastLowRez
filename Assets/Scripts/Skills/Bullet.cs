using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ACellObject
{
	public int					_speed;
	public GameObject			_target;
	public CellSprite			_sprite;

	private Cell[,]				_cells;
	private SpriteManager		_cellSprite;
	private Vector2				_size;

	public override void Setup()
	{
		_sprite.Create();
		//TODO : To get from SpriteManager
		_size = new Vector2(10, 10);
		_cells = new Cell[(int)_size.x, (int)_size.y];
	}

	public override void Simulate()
	{
		UpdatePosition();
		_sprite.Simulate(_cells);
	}

	private void UpdatePosition()
	{
		Vector2 position = transform.position;
		Vector2 targetPosition = _target.transform.position;
		Vector2 direction = (targetPosition - position).normalized;
		position += direction * _speed;
		transform.position = position;
	}

	public override void Add(Cell[,] p_automaton, Cell[,] p_staticGrid)
	{
		Vector2Int position = new Vector2Int((int)transform.position.x, (int)transform.position.y);

		for (int x = 0; x < _size.x; x++)
		{
			for (int y = 0; y < _size.y; y++)
			{
				if (x + position.x < Core._width && y + position.y < Core._height)
				{
					p_staticGrid[x + position.x, y + position.y].value = _cells[x, y].value;
					p_staticGrid[x + position.x, y + position.y].state = _cells[x, y].state;
					p_staticGrid[x + position.x, y + position.y].color = _cells[x, y].color;
				}
			}
		}
	}
}
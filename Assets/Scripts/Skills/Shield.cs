using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ACellObject
{
	public CellSprite			_sprite;
	public ColorHSV				_colorShield;
	private Cell[,]				_cells;

	public override void Setup()
	{
		_sprite = _sprite.AddSprite(gameObject);
		_cells = new Cell[_sprite._size.x, _sprite._size.y];
	}

	public override void Simulate()
	{
		UpdateSprite();
	}

	private void UpdateSprite()
	{
		_sprite.Simulate(_cells);
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
					//p_staticGrid[x + position.x, y + position.y].value = _cells[x, y].value;
					p_staticGrid[x + position.x, y + position.y].state = _cells[x, y].state;
					p_staticGrid[x + position.x, y + position.y].color = _colorShield;
					p_automaton[x + position.x, y + position.y].state = Cell.State.Dead;
				}
			}
		}
	}
}
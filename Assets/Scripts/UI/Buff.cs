using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : ACellObject {

	public CellSprite			_sprite;
	private Cell[,]				_cells;

	public int _number = 0;

	public override void Setup()
	{
		_sprite = _sprite.AddSprite(gameObject);
		_cells = new Cell[_sprite._size.x, _sprite._size.y];
	}

	public override void Simulate()
	{
		_sprite._index = _number;
		_sprite.Simulate(_cells);
	}

	public override void Add(Cell[,] p_automaton, Cell[,] p_staticGrid)
	{
		Vector2Int position = new Vector2Int((int)transform.position.x, (int)transform.position.y);
		for (int x = 0; x < _sprite._size.x; x++)
		{
			for (int y = 0; y < _sprite._size.y; y++)
			{
				if (_sprite._printOnAutomaton && _cells[x, y].state == Cell.State.Alive)
				{
					p_staticGrid[x + position.x, y + position.y].value = _cells[x, y].value;
					p_staticGrid[x + position.x, y + position.y].color = _cells[x, y].color;
					p_staticGrid[x + position.x, y + position.y].state = _cells[x, y].state;
				}
			}
		}
	}
}

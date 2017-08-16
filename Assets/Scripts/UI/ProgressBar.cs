using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : ACellObject {

	public CellSprite			_sprite;
	private Cell[,]				_cells;

	public CellSprite			_spriteFill;
	private Cell[,]				_cellsFill;

	public float _percent = 0.5f;

	public override int GetZIndex() { return 1; }

	private void Awake()
	{
		_sprite = _sprite.AddSprite(gameObject);
		_cells = new Cell[_sprite._size.x, _sprite._size.y];
		_spriteFill = _spriteFill.AddSprite(gameObject);
		_cellsFill = new Cell[_spriteFill._size.x, _spriteFill._size.y];
	}

	public override void Simulate()
	{
		_sprite.Simulate(_cells);
		_spriteFill.Simulate(_cellsFill);
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
					if (x > 0 && x < 15 && y > 0 && y < 3 && (float)(x - 1) / 14.0f < _percent)
						p_staticGrid[x + position.x, y + position.y].color = _cellsFill[x, y].color;
					else
						p_staticGrid[x + position.x, y + position.y].color = _cells[x, y].color;
				}
			}
		}
	}
}

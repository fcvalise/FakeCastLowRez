using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayer : ACellObject
{
	private Cell[,]					_cells;
	private Vector2Int				_size;
	private float					_castPercent;

	public override int GetZIndex() { return 7; }

	private void Awake()
	{
		_size = new Vector2Int(10, 2);
		_cells = new Cell[_size.x, _size.y];
	}

	public override void Simulate()
	{
		for (int x = 0; x < _size.x; x++)
		{
			if (x < (int)(_castPercent * _size.x))
			{
				_cells[x, 0].color = new ColorHSV(Color.white);
				_cells[x, 0].state = Cell.State.Alive;
				_cells[x, 0].value = 1f;
			}
		}
	}

	public void SetCastPercent(float percent)
	{
		_castPercent = percent;
	}

	public override void Add(Cell[,] p_automaton, Cell[,] p_staticGrid)
	{
		Vector2Int position = new Vector2Int((int)transform.position.x, (int)transform.position.y);

		for (int x = 0; x < _size.x; x++)
		{
			for (int y = 0; y < _size.y; y++)
			{
				if (x + transform.position.x < Core._width && y + transform.position.y < Core._height)
				{
					if (_cells[x, y].state == Cell.State.Alive)
					{
						p_staticGrid[x + position.x, y + position.y].state = _cells[x, y].state;
						p_staticGrid[x + position.x, y + position.y].color = _cells[x, y].color;
					}
				}
			}
		}
	}
}

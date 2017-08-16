using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ACellObject
{
	public CellSprite				_sprite;
	public ColorHSV					_colorShield;

	[HideInInspector]public bool	_isDestroy = false;

	private int						_maxLife = 200;
	private int						_currentLife;
	private Cell[,]					_cells;

	public override int GetZIndex() { return 2; }

	private void Awake()
	{
		_sprite = _sprite.AddSprite(gameObject);
		_currentLife = _maxLife;
		_cells = new Cell[_sprite._size.x, _sprite._size.y];
	}

	public override void Simulate()
	{
		_currentLife -= 1;
		if (_currentLife == (int)(_maxLife / 5))
			_sprite._index -= 1;
		if (_currentLife <= 0)
			_isDestroy = true;
		UpdateSprite();
	}

	private void UpdateSprite()
	{
		UpdateDestroyShield();
		_sprite.Simulate(_cells);
	}

	public void UpdateDestroyShield()
	{
		if (_isDestroy)
		{
			if (_sprite._mode == CellSprite.Mode.Pause)
			{
				int index = _sprite._index - 1;

				if (index < 0)
					Destroy(gameObject);
				else
					_sprite._index = index;
			}
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
					p_staticGrid[x + position.x, y + position.y].state = _cells[x, y].state;
					p_staticGrid[x + position.x, y + position.y].color = _colorShield;
					p_automaton[x + position.x, y + position.y].state = Cell.State.Dead;
				}
			}
		}
	}
}
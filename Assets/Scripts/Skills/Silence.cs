using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silence : ACellObject
{
	public CellSprite			_sprite;
	public ColorHSV				_silenceColor;

	private GameObject			_target;
	private Cell[,]				_cells;
	private Vector2				_movement;

	public override void Setup()
	{
		_sprite = _sprite.AddSprite(gameObject);
		_cells = new Cell[_sprite._size.x, _sprite._size.y];
	}

	public void SetTarget(GameObject target)
	{
		_target = target;
	}

	public override void Simulate()
	{
		UpdatePosition();
		UpdateSprite();
	}

	private void UpdateSprite()
	{
		_sprite.Simulate(_cells);
		if (!_target.GetComponent<Player>().IsSilence)
			Destroy(gameObject);
	}

	private void UpdatePosition()
	{
		transform.position = _target.transform.position;
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
					p_staticGrid[x + position.x, y + position.y].color = _silenceColor;
					//p_automaton[x + position.x, y + position.y].state = _cells[x, y].state;
				}
			}
		}
	}
}
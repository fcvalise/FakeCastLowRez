using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayer : ACellObject
{
	private Cell[,]					_cells;
	private Cell[,]					_copy;
	private Vector2Int				_size;
	private Vector2Int				_delta;
	private float					_castPercent;
	private float					_shieldCdPercent;
	private float					_silenceCdPercent;

	public override int GetZIndex() { return 5; }

	private void Awake()
	{
		_size = new Vector2Int(20, 4);
		_delta = new Vector2Int(-5, 7 + _size.y);
		_cells = new Cell[_size.x, _size.y];
		_copy = new Cell[_size.x, _size.y];

		for (int x = 0; x < _size.x; x++)
		{
			_cells[x, 0].state = Cell.State.Alive;
			_cells[x, 1].state = Cell.State.Alive;
			_cells[x, 2].state = Cell.State.Alive;
			_cells[x, 3].state = Cell.State.Alive;
		}
	}

	public override void Simulate()
	{
		ColorHSV none = new ColorHSV(Color.black);

		for (int x = 0; x < _size.x; x++)
		{
			if ((float)x < _castPercent * _size.x)
			{
				_cells[x, 1].color = new ColorHSV(Color.white);
				_cells[x, 2].color = new ColorHSV(Color.white);
				_cells[x, 3].color = new ColorHSV(Color.white);

				_cells[x, 1].state = Cell.State.Alive;
				_cells[x, 2].state = Cell.State.Alive;
				_cells[x, 3].state = Cell.State.Alive;
			}
			else
			{
				_cells[x, 1].state = Cell.State.Dead;
				_cells[x, 2].state = Cell.State.Dead;
				_cells[x, 3].state = Cell.State.Dead;
			}
			if ((float)x < gameObject.GetComponent<CastShield>().GetCDPercent() * _size.x)
			{
				_cells[x, 1].state = Cell.State.Alive;
				_cells[x, 1].color = gameObject.GetComponent<CastShield>()._shieldColor;
			}
			if ((float)x < gameObject.GetComponent<CastSilence>().GetCDPercent() * _size.x)
			{
				_cells[x, 2].state = Cell.State.Alive;
				_cells[x, 2].color = gameObject.GetComponent<CastSilence>()._silenceColor;
			}

			if ((float)x < gameObject.GetComponent<Player>().GetLifePercent() * _size.x)
				_cells[x, 0].color = gameObject.GetComponent<CastDamage>()._damageColor;
			else
				_cells[x, 0].color = none;
		}
		_copy = _cells;
		//FollowPlayer();
	}

	private void FollowPlayer()
	{
		Vector2 side = gameObject.GetComponent<SpriteManager>().GetSide();
		if (side == Vector2.left)
			PTUtils.CopyLeft(_copy, _cells, _size);
		else if (side == Vector2.right)
			PTUtils.CopyRight(_copy, _cells, _size);
		else if (side == Vector2.down)
			PTUtils.CopyDown(_copy, _cells, _size);
		else if (side == Vector2.up)
			PTUtils.CopyUp(_copy, _cells, _size);
		_delta = new Vector2Int((int)(side.x * 9), (int)(side.y * 9));
	}

	public void SetCastPercent(float percent)
	{
		_castPercent = percent;
	}

	public override void Add(Cell[,] p_automaton, Cell[,] p_staticGrid)
	{
		Vector2Int position = new Vector2Int((int)transform.position.x, (int)transform.position.y);
		Vector2Int delta = _delta;
		Vector2Int positionUI = position + _delta;
		if (positionUI.x <= 0)
			delta.x += Mathf.Abs(positionUI.x);
		if (positionUI.x + _size.x >= Core._width)
			delta.x -= Mathf.Abs(positionUI.x + _size.x - Core._width);
		if (positionUI.y + _size.y >= Core._height)
			delta.y -= Mathf.Abs(positionUI.y + _size.y - Core._height);

		for (int x = 0; x < _size.x; x++)
		{
			for (int y = 0; y < _size.y; y++)
			{
				if (_copy[x, y].state == Cell.State.Alive)
				{
					p_staticGrid[x + position.x + delta.x, y + position.y + delta.y].state = _copy[x, y].state;
					p_staticGrid[x + position.x + delta.x, y + position.y + delta.y].color = _copy[x, y].color;
				}
			}
		}
	}
}

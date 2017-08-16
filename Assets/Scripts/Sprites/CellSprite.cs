using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSprite : MonoBehaviour
{
	public enum Mode
	{
		Play,
		Pause
	}

	public Texture2D					_spriteSheet;
	public int							_numberOfSprites;
	public bool							_isLoop;
	public bool							_printOnAutomaton;

	[HideInInspector] public Vector2Int	_size;
	[HideInInspector] public Mode		_mode = Mode.Play;

	private List<Cell[,]>				_cells;
	private	int							_spriteCount;
	private Vector2						_side = Vector2.down;
	private int							_index = 0;

	public void Create(CellSprite sprite)
	{
		_spriteSheet = sprite._spriteSheet;
		_numberOfSprites = sprite._numberOfSprites;
		_isLoop = sprite._isLoop;
		_printOnAutomaton = sprite._printOnAutomaton;

		_cells = new List<Cell[,]>();
		_size = new Vector2Int(_spriteSheet.width / _numberOfSprites, _spriteSheet.height);
		if (_size.x != _size.y)
			Debug.LogError("ERROR : CellSprite \"" + _spriteSheet.name + "\" width different of height not supported yet");
		FillCells();
	}

	public CellSprite AddSprite(GameObject gameObject)
	{
		CellSprite sprite = gameObject.AddComponent<CellSprite>();
		sprite.Create(this);
		return sprite;
	}

	private void FillCells()
	{
		int i = 0;
		for (int x = 0 ; x < _spriteSheet.width; x++)
		{
			if (x % _size.x == 0)
			{
				_cells.Add(new Cell[_size.x, _size.y]);
				i = x / _size.x;
			}

			for (int y = 0; y < _size.y; y++)
			{
				Color color = _spriteSheet.GetPixel(x, _size.y - y - 1);
				//TODO : Add possibility to fill in a different way
				FillOneCell(ref _cells[i][x - i * _size.x, y], color);
			}
		}
	}

	private void FillOneCell(ref Cell p_cell, Color p_color)
	{
		if (p_color.a != 0)
		{
			p_cell.color = new ColorHSV(p_color);
			p_cell.state = Cell.State.Alive;
			p_cell.value = 1f;
		}
		else
		{
			p_cell.state = Cell.State.Dead;
		}
	}

	private void PrintToCells(Cell[,] p_cells)
	{
		if (_side == Vector2.left)
			PTUtils.CopyLeft(p_cells, _cells[_index], _size);
		else if (_side == Vector2.right)
			PTUtils.CopyRight(p_cells, _cells[_index], _size);
		else if (_side == Vector2.down)
			PTUtils.CopyDown(p_cells, _cells[_index], _size);
		else if (_side == Vector2.up)
			PTUtils.CopyUp(p_cells, _cells[_index], _size);
	}

	public void Simulate(Cell[,] p_cells)
	{
		PrintToCells(p_cells);
		if (_index < _numberOfSprites - 1)
			_index++;
		else if (_isLoop)
			_index = 0;
		else
			_mode = Mode.Pause;
	}

	public void Play()
	{
		_index = 0;
		_mode = Mode.Play;
	}

	public bool isFinished()
	{
		return _index == _numberOfSprites - 1;
	}

	public void SetSide(Vector2 p_side)
	{
		if (p_side == Vector2.left || p_side == Vector2.right || p_side == Vector2.up || p_side == Vector2.down)
			_side = p_side;
	}

	public void SetIndex(int index)
	{
		_index = index;
	}

	/*
	private void DebugCells()
	{
		foreach (Cell[,] cell in _cells)
		{
			string s = "";

			for (int y = 0; y < _size.y; y++)
			{
				for (int x = 0 ; x < _size.x; x++)
				{
					if (cell[x, y].color.a == 0)
						s += "o";
					else
						s += "x";
				}
				s += "\n";
			}
			Debug.Log(s);
		}
	}
	*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSprite : MonoBehaviour
{
	public Texture2D				_spriteSheet;
	public int						_numberOfSprites;
	public bool						_isLoop;
	public bool						_printOnAutomaton;

	[HideInInspector]
	public Vector2Int				_size;
	private List<Cell[,]>			_cells;
	private	int						_spriteCount;
	private Vector2					_side = Vector2.down;
	public int						_index = 0;

	public void Create()
	{
		_cells = new List<Cell[,]>();
		_size = new Vector2Int(_spriteSheet.width / _numberOfSprites, _spriteSheet.height);
		if (_size.x != _size.y)
			Debug.LogError("ERROR : CellSprite \"" + _spriteSheet.name + "\" width different of height not supported yet");
		FillCells();
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

	public void Simulate(Cell[,] p_cells)
	{
		PrintToCells(p_cells);
		if (_index < _numberOfSprites - 1)
			_index++;
		else if (_isLoop)
			_index = 0;
	}

	public void Play()
	{
		_index = 0;
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

	private void PrintToCells(Cell[,] p_cells)
	{
		if (_side == Vector2.left)
			CopyLeft(p_cells);
		else if (_side == Vector2.right)
			CopyRight(p_cells);
		else if (_side == Vector2.down)
			CopyDown(p_cells);
		else if (_side == Vector2.up)
			CopyUp(p_cells);
	}

	private void CopyUp(Cell[,] p_array)
	{
		for (int y = 0; y < _size.y; y++)
		{
			for (int x = 0 ; x < _size.x; x++)
			{
				p_array[x, y] = _cells[_index][x, y];
			}
		}
	}

	private void CopyDown(Cell[,] p_array)
	{
		for (int y = 0; y < _size.y; y++)
		{
			for (int x = 0 ; x < _size.x; x++)
			{
				p_array[x, y] = _cells[_index][x, _size.y - 1 - y];
			}
		}
	}

	private void CopyLeft(Cell[,] p_array)
	{
		//TODO : Only working with square textures
		for (int y = 0; y < _size.y; y++)
		{
			for (int x = 0 ; x < _size.x; x++)
			{
				p_array[y, x] = _cells[_index][x, _size.y - 1 - y];
			}
		}
	}

	private void CopyRight(Cell[,] p_array)
	{
		//TODO : Only working with square textures
		for (int y = 0; y < _size.y; y++)
		{
			for (int x = 0 ; x < _size.x; x++)
			{
				p_array[y, x] = _cells[_index][x, y];
			}
		}
	}
		
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSprite
{
	public	Texture2D				_spriteSheet;
	public int						_cellSizeX;
	public int						_numberOfSprites;
	public bool						_isLoop;

	private List<CellularCell[,]>	_cells;
	private	int						_spriteCount;
	private Vector2					_side = Vector2.down;
	private int						_index = 0;

	public CellSprite(Texture2D p_spriteSheet, int p_numberOfSprites, bool p_isLoop)
	{
		_spriteSheet = p_spriteSheet;
		_numberOfSprites = p_numberOfSprites;
		_cells = new List<CellularCell[,]>();
		_cellSizeX = _spriteSheet.width / p_numberOfSprites;
		_isLoop = p_isLoop;

		FillCells();
	}

	private void FillCells()
	{
		int i = 0;
		for (int x = 0 ; x < _spriteSheet.width; x++)
		{
			if (x % _cellSizeX == 0)
			{
				_cells.Add(new CellularCell[_cellSizeX, _spriteSheet.height]);
				i = x / _cellSizeX;
			}

			for (int y = 0; y < _spriteSheet.height; y++)
			{
				Color color = _spriteSheet.GetPixel(x, _spriteSheet.height - y - 1);
				//TODO : Add possibility to fill in a different way
				FillOneCell(ref _cells[i][x - i * _cellSizeX, y], color);
			}
		}
	}

	private void FillOneCell(ref CellularCell p_cell, Color p_color)
	{
		if (p_color.a != 0)
		{
			p_cell.color = new ColorHSV(p_color);
			p_cell.state = CellularCell.State.Alive;
			p_cell.value = 1f;
		}
		else
		{
			p_cell.state = CellularCell.State.Dead;
		}
	}

	public void Simulate(CellularCell[,] p_cells)
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

	private void PrintToCells(CellularCell[,] p_cells)
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

	private void CopyUp(CellularCell[,] p_array)
	{
		for (int y = 0; y < _spriteSheet.height; y++)
		{
			for (int x = 0 ; x < _cellSizeX; x++)
			{
				p_array[x, y] = _cells[_index][x, y];
			}
		}
	}

	private void CopyDown(CellularCell[,] p_array)
	{
		for (int y = 0; y < _spriteSheet.height; y++)
		{
			for (int x = 0 ; x < _cellSizeX; x++)
			{
				p_array[x, y] = _cells[_index][x, _spriteSheet.height - 1 - y];
			}
		}
	}

	private void CopyLeft(CellularCell[,] p_array)
	{
		//TODO : Only working with square textures
		for (int y = 0; y < _spriteSheet.height; y++)
		{
			for (int x = 0 ; x < _cellSizeX; x++)
			{
				p_array[y, x] = _cells[_index][x, _spriteSheet.height - 1 - y];
			}
		}
	}

	private void CopyRight(CellularCell[,] p_array)
	{
		//TODO : Only working with square textures
		for (int y = 0; y < _spriteSheet.height; y++)
		{
			for (int x = 0 ; x < _cellSizeX; x++)
			{
				p_array[y, x] = _cells[_index][x, y];
			}
		}
	}

	/*
	 * DEBUG
	 * 
	private void DebugCells()
	{
		foreach (CellularCell[,] cell in _cells)
		{
			string s = "";

			for (int y = 0; y < _spriteSheet.height; y++)
			{
				for (int x = 0 ; x < _cellSizeX; x++)
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

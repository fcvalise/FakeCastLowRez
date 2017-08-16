using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automaton : MonoBehaviour
{
	public Cell[,]		_cells;
	public Cell[,]		_copy;

	private Vector2Int	_size;
	//TODO : Implement alive borders
	private bool		_aliveBorders;

	public Automaton(int width, int height, Ruleset ruleset, bool aliveBorders)
	{
		_size.x = width;
		_size.y = height;
		_aliveBorders = aliveBorders;
		_cells = new Cell[width, height];
		_copy = new Cell[width, height];
	}

	public Automaton(int width, int height, Ruleset ruleset, float startNoise, bool aliveBorders) : this(width, height, ruleset, aliveBorders)
	{
		FillWithNoise(startNoise);
	}

	public void SetRuleset(Ruleset ruleset)
	{
		for (int x = 0; x < _size.x; x++)
		{
			for (int y = 0; y < _size.y; y++)
			{
				_cells[x, y].rulset = ruleset;
				_copy[x, y].rulset = ruleset;
			}
		}
	}

	public void FillWithNoise(float noise)
	{
		for (int x = 0; x < _size.x; x++)
		{
			for (int y = 0; y < _size.y; y++)
			{
				_cells[x, y].state = Random.value < noise ? Cell.State.Alive : Cell.State.Dead;
				_cells[x, y].rulset = RulesetList.Life;
				_copy[x, y].rulset = RulesetList.Life;
			}
		}
	}

	public void Simulate()
	{
		PTUtils.Swap(ref _cells, ref _copy);

		for (int x = 1; x < _size.x - 1; x++)
		{
			for (int y = 1; y < _size.y - 1; y++)
			{
				int aliveCells = CountAliveNeighbourCells(x, y);
				ComputeState(ref _cells[x, y], ref _copy[x, y], aliveCells);
				ComputeValue(ref _cells[x, y]);

				if (_cells[x, y].value >= 1f)
					_cells[x, y].state = Cell.State.Dead;

				_copy[x, y].value = _cells[x, y].value;
			}
		}
	}

	private int CountAliveNeighbourCells(int x, int y)
	{
		int sum = 
			(int)_copy[x + 0, y + 1].state +
			(int)_copy[x + 0, y - 1].state +
			(int)_copy[x + 1, y - 1].state +
			(int)_copy[x + 1, y + 0].state +
			(int)_copy[x + 1, y + 1].state +
			(int)_copy[x - 1, y - 1].state +
			(int)_copy[x - 1, y + 0].state +
			(int)_copy[x - 1, y + 1].state;
		return 1 << sum;
	}

	private void ComputeState(ref Cell p_cell, ref Cell p_copy, int p_aliveCells)
	{
		if (p_copy.state == Cell.State.Dead)
		{
			if ((p_aliveCells & p_cell.rulset._birth) != 0)
				p_cell.state = Cell.State.Alive;
			else
				p_cell.state = Cell.State.Dead;
		}
		else
		{
			if ((p_aliveCells & p_cell.rulset._survival) != 0)
				p_cell.state = Cell.State.Alive;
			else
				p_cell.state = Cell.State.Dead;
		}
	}

	private void ComputeValue(ref Cell cell)
	{
		if (cell.state == Cell.State.Alive)
			cell.value = Mathf.Clamp(cell.value + Time.deltaTime * 10f, 0f, 10f);
		else
			cell.value = Mathf.Max(cell.value - Time.deltaTime * 5f, 0f);
	}
}

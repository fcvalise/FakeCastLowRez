﻿using System;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ProceduralToolkit
{
	/// <summary>
	/// Generic cellular automaton for two-state rulesets
	/// </summary>

	public class CellularAutomaton
	{
		public CellularCell[,]				_cells;
		public CellularCell[,]				_copy;

		public Vector2Int					_size;
		private bool						_aliveBorders;

		private int							_aliveNeighbours;
		private readonly Action<int, int>	_visitAliveBorders;
		private readonly Action<int, int>	_visitDeadBorders;

		public CellularAutomaton(int width, int height, Ruleset ruleset, bool aliveBorders)
		{
			_size.x = width;
			_size.y = height;
			_aliveBorders = aliveBorders;
			_cells = new CellularCell[width, height];
			_copy = new CellularCell[width, height];

			_visitAliveBorders = (int neighbourX, int neighbourY) =>
			{
				if (_copy.IsInBounds(neighbourX, neighbourY))
				{
					if (_copy[neighbourX, neighbourY].state == CellularCell.State.Alive)
					{
						_aliveNeighbours++;
					}
				}
				else
				{
					_aliveNeighbours++;
				}
			};
			_visitDeadBorders = (int neighbourX, int neighbourY) =>
			{
				if (_copy[neighbourX, neighbourY].state == CellularCell.State.Alive)
				{
					_aliveNeighbours++;
				}
			};

			SetRuleset(ruleset);
		}

		public CellularAutomaton(int width, int height, Ruleset ruleset, float startNoise, bool aliveBorders) : this(width, height, ruleset, aliveBorders)
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
					_cells[x, y].state = Random.value < noise ? CellularCell.State.Alive : CellularCell.State.Dead;
				}
			}
		}

		public void Simulate(int generations)
		{
			for (int i = 0; i < generations; i++)
			{
				Simulate();
			}
		}

		public void Simulate()
		{
			PTUtils.Swap(ref _cells, ref _copy);
			for (int x = 0; x < _size.x; x++)
			{
				for (int y = 0; y < _size.y; y++)
				{
					int alive_cells = CountAliveNeighbour_cells(x, y);

					ComputeState(ref _cells[x, y], ref _copy[x, y], alive_cells);
					ComputeValue(ref _cells[x, y]);

					if (_cells[x, y].value >= 10f)
						_cells[x, y].state = CellularCell.State.Dead;

					_copy[x, y].value = _cells[x, y].value;
				}
			}
		}

		private void ComputeState(ref CellularCell cell, ref CellularCell _copy, int alive_cells)
		{					
			if (_copy.state == CellularCell.State.Dead)
			{
				if (cell.rulset.CanSpawn(alive_cells))
					cell.state = CellularCell.State.Alive;
				else
					cell.state = CellularCell.State.Dead;
			}
			else
			{
				if (!cell.rulset.CanSurvive(alive_cells))
					cell.state = CellularCell.State.Dead;
				else
					cell.state = CellularCell.State.Alive;
			}
		}

		private void ComputeValue(ref CellularCell cell)
		{
			if (cell.state == CellularCell.State.Alive)
				cell.value = Mathf.Clamp(cell.value + Time.deltaTime * 10f, 0f, 10f);
			else
				cell.value = Mathf.Clamp(cell.value - Time.deltaTime * 2f, 0f, 1f);
		}

		private int CountAliveNeighbour_cells(int x, int y)
		{
			_aliveNeighbours = 0;
			if (_aliveBorders)
			{
				_copy.VisitMooreNeighbours(x, y, false, _visitAliveBorders);
			}
			else
			{
				_copy.VisitMooreNeighbours(x, y, true, _visitDeadBorders);
			}
			return _aliveNeighbours;
		}
	}
}
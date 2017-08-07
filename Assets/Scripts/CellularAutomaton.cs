using System;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ProceduralToolkit
{
	/// <summary>
	/// Generic cellular automaton for two-state rulesets
	/// </summary>
	public class CellularAutomaton
	{
		public CellularCell[,]				m_cells;
		public CellularCell[,]				m_copy;

		public Vector2Int					m_size;
		private bool						m_aliveBorders;

		private int							m_aliveNeighbours;
		private readonly Action<int, int>	m_visitAliveBorders;
		private readonly Action<int, int>	m_visitDeadBorders;

		public CellularAutomaton(int width, int height, Ruleset ruleset, bool aliveBorders)
		{
			m_size.x = width;
			m_size.y = height;
			m_aliveBorders = aliveBorders;
			m_cells = new CellularCell[width, height];
			m_copy = new CellularCell[width, height];

			m_visitAliveBorders = (int neighbourX, int neighbourY) =>
			{
				if (m_copy.IsInBounds(neighbourX, neighbourY))
				{
					if (m_copy[neighbourX, neighbourY].state == CellularCell.State.Alive)
					{
						m_aliveNeighbours++;
					}
				}
				else
				{
					m_aliveNeighbours++;
				}
			};
			m_visitDeadBorders = (int neighbourX, int neighbourY) =>
			{
				if (m_copy[neighbourX, neighbourY].state == CellularCell.State.Alive)
				{
					m_aliveNeighbours++;
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
			for (int x = 0; x < m_size.x; x++)
			{
				for (int y = 0; y < m_size.y; y++)
				{
					m_cells[x, y].rulset = ruleset;
					m_copy[x, y].rulset = ruleset;
				}
			}
		}

		public void FillWithNoise(float noise)
		{
			for (int x = 0; x < m_size.x; x++)
			{
				for (int y = 0; y < m_size.y; y++)
				{
					m_cells[x, y].state = Random.value < noise ? CellularCell.State.Alive : CellularCell.State.Dead;
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
			PTUtils.Swap(ref m_cells, ref m_copy);
			for (int x = 0; x < m_size.x; x++)
			{
				for (int y = 0; y < m_size.y; y++)
				{
					int alivem_cells = CountAliveNeighbourm_cells(x, y);

					ComputeState(ref m_cells[x, y], ref m_copy[x, y], alivem_cells);
					ComputeValue(ref m_cells[x, y]);

					if (m_cells[x, y].value >= 10f)
						m_cells[x, y].state = CellularCell.State.Dead;

					m_copy[x, y].value = m_cells[x, y].value;
				}
			}
		}

		private void ComputeState(ref CellularCell cell, ref CellularCell m_copy, int alivem_cells)
		{					
			if (m_copy.state == CellularCell.State.Dead)
			{
				if (cell.rulset.CanSpawn(alivem_cells))
					cell.state = CellularCell.State.Alive;
				else
					cell.state = CellularCell.State.Dead;
			}
			else
			{
				if (!cell.rulset.CanSurvive(alivem_cells))
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

		private int CountAliveNeighbourm_cells(int x, int y)
		{
			m_aliveNeighbours = 0;
			if (m_aliveBorders)
			{
				m_copy.VisitMooreNeighbours(x, y, false, m_visitAliveBorders);
			}
			else
			{
				m_copy.VisitMooreNeighbours(x, y, true, m_visitDeadBorders);
			}
			return m_aliveNeighbours;
		}
	}
}
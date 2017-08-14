using System;
using Random = UnityEngine.Random;
using UnityEngine;

/*
namespace ProceduralToolkit
{
	// J'avait teste de mettre le cellular automaton dans un thread.
	// Ca fonctionne mais complique les choses et c'est pas forcement necessaire

	public class CellularAutomatonThread : ThreadedJob
	{
		public Cell[,]				cells;
		public Cell[,]				copy;

		public Vector2Int					size;
		private bool						aliveBorders;

		private int							aliveNeighbours;
		private readonly Action<int, int>	visitAliveBorders;
		private readonly Action<int, int>	visitDeadBorders;

		public CellularAutomatonThread(int width, int height, Ruleset ruleset, bool aliveBorders)
		{
			this.size.x = width;
			this.size.y = height;
			this.aliveBorders = aliveBorders;
			cells = new Cell[width, height];
			copy = new Cell[width, height];

			visitAliveBorders = (int neighbourX, int neighbourY) =>
			{
				if (copy.IsInBounds(neighbourX, neighbourY))
				{
					if (copy[neighbourX, neighbourY].state == Cell.State.Alive)
					{
						aliveNeighbours++;
					}
				}
				else
				{
					aliveNeighbours++;
				}
			};
			visitDeadBorders = (int neighbourX, int neighbourY) =>
			{
				if (copy[neighbourX, neighbourY].state == Cell.State.Alive)
				{
					aliveNeighbours++;
				}
			};

			SetRuleset(ruleset);
		}

		public CellularAutomatonThread(int width, int height, Ruleset ruleset, float startNoise, bool aliveBorders) : this(width, height, ruleset, aliveBorders)
		{
			FillWithNoise(startNoise);
		}

		public void SetRuleset(Ruleset ruleset)
		{
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					cells[x, y].rulset = ruleset;
					copy[x, y].rulset = ruleset;
				}
			}
		}

		public void FillWithNoise(float noise)
		{
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					cells[x, y].state = Random.value < noise ? Cell.State.Alive : Cell.State.Dead;
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

		protected override void ThreadFunction()
		{
			PTUtils.Swap(ref cells, ref copy);
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					int aliveCells = CountAliveNeighbourCells(x, y);

					ComputeState(ref cells[x, y], ref copy[x, y], aliveCells);
					ComputeValue(ref cells[x, y]);
					copy[x, y].value = cells[x, y].value;
				}
			}
		}

		protected override void OnFinished() { }

		public void Simulate()
		{
			PTUtils.Swap(ref cells, ref copy);
			for (int x = 0; x < size.x; x++)
			{
				for (int y = 0; y < size.y; y++)
				{
					int aliveCells = CountAliveNeighbourCells(x, y);

					ComputeState(ref cells[x, y], ref copy[x, y], aliveCells);
					ComputeValue(ref cells[x, y]);

					//if (cells[x, y].value >= 10f)
					//	cells[x, y].state = Cell.State.Dead;

					copy[x, y].value = cells[x, y].value;
				}
			}
		}

		private void ComputeState(ref Cell cell, ref Cell copy, int aliveCells)
		{					
			if (copy.state == Cell.State.Dead)
			{
				if (cell.rulset.CanSpawn(aliveCells))
					cell.state = Cell.State.Alive;
				else
					cell.state = Cell.State.Dead;
			}
			else
			{
				if (!cell.rulset.CanSurvive(aliveCells))
					cell.state = Cell.State.Dead;
				else
					cell.state = Cell.State.Alive;
			}
		}

		private void ComputeValue(ref Cell cell)
		{
			if (cell.state == Cell.State.Alive)
				cell.value = Mathf.Clamp(cell.value + 0.05f, 0f, 10f);
			else
				cell.value = Mathf.Clamp(cell.value - 0.05f, 0f, 1f);
		}

		private int CountAliveNeighbourCells(int x, int y)
		{
			aliveNeighbours = 0;
			if (aliveBorders)
			{
				copy.VisitMooreNeighbours(x, y, false, visitAliveBorders);
			}
			else
			{
				copy.VisitMooreNeighbours(x, y, true, visitDeadBorders);
			}
			return aliveNeighbours;
		}
	}
}
*/
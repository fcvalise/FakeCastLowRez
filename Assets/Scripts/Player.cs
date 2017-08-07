using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
	public class Player : MonoBehaviour, ICellularObject
	{
		private CellularCell[,]		m_cells;
		public Vector2Int			m_bounds;

		private Vector2Int			m_size;
		private Vector2Int			m_position;

		public void Setup()
		{
			m_size = new Vector2Int(4, 4);
			m_position = new Vector2Int(64 / 2, 64 / 2);
			m_cells = new CellularCell[m_size.x, m_size.y];
			Fill();
		}

		private void Fill()
		{
			for (int x = 0; x < m_size.x; x++)
			{
				for (int y = 0; y < m_size.y; y++)
				{
					m_cells[x, y].state = CellularCell.State.Alive;
					m_cells[x, y].value = 1f;
					m_cells[x, y].color = new ColorHSV(200f / 360f, 1f, 1f);
				}
			}
		}

		// Ca c'est ce qui te sert d'update
		public void Simulate()
		{
			UpdatePosition();
		}

		private void UpdatePosition()
		{
			if (Input.GetKey(KeyCode.RightArrow))
				m_position.x = Mathf.Min(m_position.x + 1, m_bounds.x - m_size.x);
			if (Input.GetKey(KeyCode.LeftArrow))
				m_position.x = Mathf.Max(m_position.x - 1, 0);
			if (Input.GetKey(KeyCode.UpArrow))
				m_position.y = Mathf.Min(m_position.y + 1, m_bounds.y - m_size.y);
			if (Input.GetKey(KeyCode.DownArrow))
				m_position.y = Mathf.Max(m_position.y - 1, 0);
		}

		public void Add(ref CellularAutomaton automaton)
		{
			for (int x = 0; x < m_size.x; x++)
			{
				for (int y = 0; y < m_size.y; y++)
				{
					if (x + m_position.x < automaton.m_size.x && y + m_position.y < automaton.m_size.y)
					{
						automaton.m_cells[x + m_position.x, y + m_position.y].value = m_cells[x, y].value;
						//automaton.m_cells[x + m_position.x, y + m_position.y].state = m_cells[x, y].state;
						automaton.m_cells[x + m_position.x, y + m_position.y].color = m_cells[x, y].color;
					}
				}
			}
		}
	}
}
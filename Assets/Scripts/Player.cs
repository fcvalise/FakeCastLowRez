using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
	[RequireComponent(typeof(SpriteManager))]
	public class Player : MonoBehaviour, ICellularObject
	{
		private CellularCell[,]		m_cells;
		public Vector2Int			m_bounds;

		private SpriteManager		m_spriteManager;

		private Vector2Int			m_size;
		private Vector2Int			m_position;
		private Vector2Int			m_lastMovement;

		public void Setup()
		{
			m_spriteManager = GetComponent<SpriteManager>();
			//TODO : To get from SpriteManager
			m_size = new Vector2Int(10, 10);
			m_position = new Vector2Int(64 / 2, 64 / 2);
			m_cells = new CellularCell[m_size.x, m_size.y];
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

		public void Simulate()
		{
			UpdatePosition();
			m_spriteManager.Simulate(m_cells);
		}

		private void UpdatePosition()
		{
			Vector2Int movement = Vector2Int.zero;

			if (Input.GetKey(KeyCode.RightArrow))
				movement += Vector2Int.right;
			if (Input.GetKey(KeyCode.LeftArrow))
				movement += Vector2Int.left;
			if (Input.GetKey(KeyCode.UpArrow))
				movement += Vector2Int.up;
			if (Input.GetKey(KeyCode.DownArrow))
				movement += Vector2Int.down;

			if (movement != Vector2Int.zero)
			{
				m_spriteManager.SetSide(movement);
				m_spriteManager.PlayNext(SpriteManager.SpriteState.Walk);
			}
			else if (Input.GetKey(KeyCode.Space))
				m_spriteManager.PlayNext(SpriteManager.SpriteState.Cast);
			else
				m_spriteManager.PlayNext(SpriteManager.SpriteState.Lay);

			if (Mathf.Abs(m_lastMovement.x) == 1 && Mathf.Abs(m_lastMovement.y) == 1)
				movement = Vector2Int.zero;
			m_lastMovement = movement;

			m_position += movement;
			m_position.x = Mathf.Clamp(m_position.x, 1, m_bounds.x - m_size.x);
			m_position.y = Mathf.Clamp(m_position.y, 1, m_bounds.y - m_size.y);
		}

		public void Add(CellularCell[,] automaton, CellularCell[,] staticGrid)
		{
			for (int x = 0; x < m_size.x; x++)
			{
				for (int y = 0; y < m_size.y; y++)
				{
					if (x + m_position.x < m_bounds.x && y + m_position.y < m_bounds.y)
					{
						staticGrid[x + m_position.x, y + m_position.y].value = m_cells[x, y].value;
						staticGrid[x + m_position.x, y + m_position.y].state = m_cells[x, y].state;
						staticGrid[x + m_position.x, y + m_position.y].color = m_cells[x, y].color;

						if (m_cells[x, y].state == CellularCell.State.Alive)
							automaton[x + m_position.x, y + m_position.y].state = m_cells[x, y].state;
					}
				}
			}
		}
	}
}
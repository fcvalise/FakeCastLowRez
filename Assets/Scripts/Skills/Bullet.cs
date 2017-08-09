using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit
{
	public class Bullet : MonoBehaviour, ICellularObject
	{
		public int					_speed;
		public GameObject			_target;
		public Vector2Int			m_targetPosition;
		public Texture2D			m_spriteSheet;

		public Vector2Int			m_bounds;

		private CellSprite			m_sprite;
		private CellularCell[,]		m_cells;
		private SpriteManager		m_cellSprite;
		private Vector2Int			m_size;
		private Vector2Int			m_position;

		public void Setup()
		{
			m_position = new Vector2Int(10, 10);
			m_sprite = new CellSprite(m_spriteSheet, 3, false);
			//TODO : To get from SpriteManager
			m_size = new Vector2Int(10, 10);
			m_cells = new CellularCell[m_size.x, m_size.y];
		}

		public void Simulate()
		{
			Vector2 a = new Vector2(m_targetPosition.x, m_targetPosition.y);
			Vector2 b = new Vector2(m_position.x, m_position.y);
			Vector2 c = (a - b).normalized;

			Vector2Int direction = new Vector2Int((int)(c.x + 1), (int)(c.y + 1));
			m_position += direction * _speed;
			m_sprite.Simulate(m_cells);
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
					}
				}
			}
		}
	}
}
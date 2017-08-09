using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProceduralToolkit
{
	public class CellSprite
	{
		private List<CellularCell[,]>	m_cells;
		private	Texture2D				m_spriteSheet;
		private int						m_cellSizeX;
		private int						m_numberOfSprites;
		private bool					m_isLoop;

		private	int						m_spriteCount;
		private Vector2Int				m_side = Vector2Int.down;
		private int						m_index = 0;

		public CellSprite(Texture2D spriteSheet, int numberOfSprites, bool isLoop)
		{
			m_spriteSheet = spriteSheet;
			m_numberOfSprites = numberOfSprites;
			m_cells = new List<CellularCell[,]>();
			m_cellSizeX = m_spriteSheet.width / numberOfSprites;
			m_isLoop = isLoop;

			FillCells();
		}

		private void FillCells()
		{
			int i = 0;
			for (int x = 0 ; x < m_spriteSheet.width; x++)
			{
				if (x % m_cellSizeX == 0)
				{
					m_cells.Add(new CellularCell[m_cellSizeX, m_spriteSheet.height]);
					i = x / m_cellSizeX;
				}

				for (int y = 0; y < m_spriteSheet.height; y++)
				{
					Color color = m_spriteSheet.GetPixel(x, m_spriteSheet.height - y - 1);
					//TODO : Add possibility to fill in a different way
					FillOneCell(ref m_cells[i][x - i * m_cellSizeX, y], color);
				}
			}
		}

		private void FillOneCell(ref CellularCell cell, Color color)
		{
			if (color.a != 0)
			{
				cell.color = new ColorHSV(color);
				cell.state = CellularCell.State.Alive;
				cell.value = 1f;
			}
			else
			{
				cell.state = CellularCell.State.Dead;
			}
		}

		public void Simulate(CellularCell[,] cells)
		{
			PrintToCells(cells);
			if (m_index < m_numberOfSprites - 1)
				m_index++;
			else if (m_isLoop)
				m_index = 0;
		}

		public void Play()
		{
			m_index = 0;
		}

		public bool isFinished()
		{
			return m_index == m_numberOfSprites - 1;
		}

		public void SetSide(Vector2Int side)
		{
			if (side == Vector2Int.left || side == Vector2Int.right || side == Vector2Int.up || side == Vector2Int.down)
				m_side = side;
		}

		private void PrintToCells(CellularCell[,] cells)
		{
			if (m_side == Vector2Int.left)
				CopyLeft(cells);
			else if (m_side == Vector2Int.right)
				CopyRight(cells);
			else if (m_side == Vector2Int.down)
				CopyDown(cells);
			else if (m_side == Vector2Int.up)
				CopyUp(cells);
		}

		private void CopyUp(CellularCell[,] array)
		{
			for (int y = 0; y < m_spriteSheet.height; y++)
			{
				for (int x = 0 ; x < m_cellSizeX; x++)
				{
					array[x, y] = m_cells[m_index][x, y];
				}
			}
		}

		private void CopyDown(CellularCell[,] array)
		{
			for (int y = 0; y < m_spriteSheet.height; y++)
			{
				for (int x = 0 ; x < m_cellSizeX; x++)
				{
					array[x, y] = m_cells[m_index][x, m_spriteSheet.height - 1 - y];
				}
			}
		}

		private void CopyLeft(CellularCell[,] array)
		{
			//TODO : Only working with square textures
			for (int y = 0; y < m_spriteSheet.height; y++)
			{
				for (int x = 0 ; x < m_cellSizeX; x++)
				{
					array[y, x] = m_cells[m_index][x, m_spriteSheet.height - 1 - y];
				}
			}
		}

		private void CopyRight(CellularCell[,] array)
		{
			//TODO : Only working with square textures
			for (int y = 0; y < m_spriteSheet.height; y++)
			{
				for (int x = 0 ; x < m_cellSizeX; x++)
				{
					array[y, x] = m_cells[m_index][x, y];
				}
			}
		}

		/*
		 * DEBUG
		 * 
		private void DebugCells()
		{
			foreach (CellularCell[,] cell in m_cells)
			{
				string s = "";

				for (int y = 0; y < m_spriteSheet.height; y++)
				{
					for (int x = 0 ; x < m_cellSizeX; x++)
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
}

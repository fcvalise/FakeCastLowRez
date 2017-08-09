using UnityEngine;
using UnityEngine.UI;

//
// MAIN LOOP OF THE GAME
//

namespace ProceduralToolkit
{
	public class CellularCore : MonoBehaviour
	{
		public RawImage		m_image;
		public Player		m_player;

		private const int	m_width = 64;
		private const int	m_height = 64;
		private Color[]		m_pixels = new Color[m_width * m_height];
		private Texture2D	m_texture;

		private float		m_speed = 30f;
		private float		m_deltaSpeed = 0f;

		private MainGrid	m_mainGrid;

		private void Awake()
		{
			m_texture = new Texture2D(m_width, m_height, TextureFormat.ARGB32, false, true)
			{
				filterMode = FilterMode.Point
			};
			m_texture.Apply();
			m_image.texture = m_texture;

			m_mainGrid = new MainGrid(m_width, m_height);
			m_mainGrid.Setup();
			m_player.Setup();
		}

		private void Update()
		{
			m_deltaSpeed += Time.deltaTime;
			if (m_deltaSpeed > 1f / m_speed)
			{
				m_deltaSpeed = 0f;
				m_player.Simulate();
				m_mainGrid.Simulate();
				m_player.Add(m_mainGrid.m_automaton.m_cells, m_mainGrid.m_staticGrid);
				Draw();
			}
		}

		private void Draw()
		{
			for (int x = 0; x < m_width; x++)
			{
				for (int y = 0; y < m_height; y++)
				{
					m_mainGrid.m_automaton.m_cells[x, y].color.s = 1f - m_mainGrid.m_automaton.m_cells[x, y].value;
					m_mainGrid.m_automaton.m_cells[x, y].color.a = Mathf.Clamp(m_mainGrid.m_automaton.m_cells[x, y].value, 0f, 1f);
					if (m_mainGrid.m_staticGrid[x, y].state == CellularCell.State.Alive)
					{
						m_pixels[y * m_width + x] = m_mainGrid.m_staticGrid[x, y].color.ToColor();
					}
					else
						m_pixels[y * m_width + x] = m_mainGrid.m_automaton.m_cells[x, y].color.ToColor();
				}
			}
			m_texture.SetPixels(m_pixels);
			m_texture.Apply();
		}
	}
}
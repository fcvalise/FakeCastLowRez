using UnityEngine;

namespace ProceduralToolkit
{
	public class MainGrid : ICellularObject
	{
		public CellularAutomaton	m_automaton;

		private Ruleset				m_ruleset = Ruleset.coral;
		private float				m_startNoise = 0.25f;
		private bool				m_aliveBorders = false;
		private Texture2D			m_texture;

		public MainGrid(int width, int height)
		{
			m_texture = (Texture2D)Resources.Load("Sprites/test_map_cpu");
			m_automaton = new CellularAutomaton(m_texture.width, m_texture.height, m_ruleset, m_startNoise, m_aliveBorders);
		}

		public void Setup()
		{
			FillOneRuleset(Ruleset.life);
		}

		public void Simulate()
		{
			UpdateRuleset();
			m_automaton.Simulate();
		}

		public void Add(ref CellularAutomaton automaton) { }

		private void UpdateRuleset()
		{
			if (Input.GetKey(KeyCode.Alpha1))
				SetRuleset(Ruleset.anneal);
			if (Input.GetKey(KeyCode.Alpha2))
				SetRuleset(Ruleset.coagulations);
			if (Input.GetKey(KeyCode.Alpha3))
				SetRuleset(Ruleset.coral);
			if (Input.GetKey(KeyCode.Alpha4))
				SetRuleset(Ruleset.life);
			if (Input.GetKey(KeyCode.Alpha5))
				SetRuleset(Ruleset.majority);
			if (Input.GetKey(KeyCode.Alpha6))
				SetRuleset(Ruleset.mazectric);
			if (Input.GetKey(KeyCode.Alpha7))
				SetRuleset(Ruleset.walledCities);
		}

		private void SetRuleset(Ruleset ruleset)
		{
			m_ruleset = ruleset;
			m_automaton.SetRuleset(ruleset);
			FillOneRuleset(ruleset);
			Debug.Log(ruleset.ToString());
		}

		private void FillOneRuleset(Ruleset ruleset)
		{
			for (int y = m_texture.height - 1; y >= 0; y--)
			{
				for (int x = m_texture.width - 1; x >= 0; x--)
				{
					m_automaton.m_cells[x, y].rulset = ruleset;
					m_automaton.m_cells[x, y].color = new ColorHSV(200f / 360f, 1f, 1f);
					m_automaton.m_cells[x, y].state = CellularCell.State.Dead;
				}
			}
		}

		/*
		 * Not to remove
		 * 
		private void FillFromTexture()
		{
			for (int y = m_texture.height - 1; y >= 0; y--)
			{
				for (int x = m_texture.width - 1; x >= 0; x--)
				{
					if (m_texture.GetPixel(x, y).a != 0)
					{
						m_automaton.m_cells[x, y].rulset = Ruleset.life;
						m_automaton.m_cells[x, y].color = new ColorHSV(200f / 360f, 1f, 1f);

						m_automaton.m_copy[x, y].rulset = Ruleset.life;
						m_automaton.m_copy[x, y].color = new ColorHSV(200f / 360f, 1f, 1f);
					}
					else
					{
						m_automaton.m_cells[x, y].color = new ColorHSV(58f / 360f, 1f, 1f);
						m_automaton.m_copy[x, y].color = new ColorHSV(58f / 360f, 1f, 1f);
					}
				}
			}
		}
		*/
	}
}
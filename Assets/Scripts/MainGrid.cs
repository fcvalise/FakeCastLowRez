using UnityEngine;

namespace ProceduralToolkit
{
	public class MainGrid : ICellularObject
	{
		public CellularAutomaton	_automaton;
		public CellularCell[,]		_staticGrid;

		private Ruleset				_ruleset = Ruleset.coral;
		private float				_startNoise = 0.25f;
		private bool				_aliveBorders = false;
		private Texture2D			_texture;

		public MainGrid(int p_width, int p_height)
		{
			_texture = (Texture2D)Resources.Load("Sprites/test_map_cpu");
			_automaton = new CellularAutomaton(_texture.width, _texture.height, _ruleset, _startNoise, _aliveBorders);
			_staticGrid = new CellularCell[_texture.width, _texture.height];
		}

		public void Setup()
		{
			FillOneRuleset(Ruleset.life);
		}

		public void Simulate()
		{
			UpdateRuleset();
			_automaton.Simulate();
		}

		public void Add(CellularCell[,] p_automaton, CellularCell[,] p_staticGrid) { }

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

		private void SetRuleset(Ruleset p_ruleset)
		{
			_ruleset = p_ruleset;
			_automaton.SetRuleset(_ruleset);
			FillOneRuleset(_ruleset);
		}

		private void FillOneRuleset(Ruleset p_ruleset)
		{
			for (int y = _texture.height - 1; y >= 0; y--)
			{
				for (int x = _texture.width - 1; x >= 0; x--)
				{
					_automaton._cells[x, y].rulset = p_ruleset;
					_automaton._cells[x, y].color = new ColorHSV(200f / 360f, 1f, 1f);
					_automaton._cells[x, y].state = CellularCell.State.Dead;
				}
			}
		}

		/*
		 * Not to remove
		 * 
		private void FillFromTexture()
		{
			for (int y = _texture.height - 1; y >= 0; y--)
			{
				for (int x = _texture.width - 1; x >= 0; x--)
				{
					if (_texture.GetPixel(x, y).a != 0)
					{
						_automaton._cells[x, y].rulset = Ruleset.life;
						_automaton._cells[x, y].color = new ColorHSV(200f / 360f, 1f, 1f);

						_automaton._copy[x, y].rulset = Ruleset.life;
						_automaton._copy[x, y].color = new ColorHSV(200f / 360f, 1f, 1f);
					}
					else
					{
						_automaton._cells[x, y].color = new ColorHSV(58f / 360f, 1f, 1f);
						_automaton._copy[x, y].color = new ColorHSV(58f / 360f, 1f, 1f);
					}
				}
			}
		}
		*/
	}
}
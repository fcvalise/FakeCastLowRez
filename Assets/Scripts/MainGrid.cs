using UnityEngine;

public class MainGrid : ACellObject
{
	public Automaton			_automaton;
	public Cell[,]				_staticGrid;

	private Ruleset				_ruleset = RulesetList.Anneal;
	private float				_startNoise = 0.05f;
	private bool				_aliveBorders = false;
	private Texture2D			_map;

	public override void Setup()
	{
		_map = (Texture2D)Resources.Load("Textures/map_empty");
		_automaton = new Automaton(Core._width, Core._height, _ruleset, _startNoise, _aliveBorders);
		_staticGrid = new Cell[Core._width, Core._height];
		//FillOneRuleset(Ruleset.life);
		FillFromTexture(RulesetList.Anneal);
	}

	public override void Simulate()
	{
		UpdateRuleset();
		_automaton.Simulate();
	}

	public override void Add(Cell[,] p_automaton, Cell[,] p_staticGrid) { }

	private void UpdateRuleset()
	{
		if (Input.GetKey(KeyCode.Alpha1))
			SetRuleset(RulesetList.Anneal);
		if (Input.GetKey(KeyCode.Alpha2))
			SetRuleset(RulesetList.Coagulations);
		if (Input.GetKey(KeyCode.Alpha3))
			SetRuleset(RulesetList.Coral);
		if (Input.GetKey(KeyCode.Alpha4))
			SetRuleset(RulesetList.Life);
		if (Input.GetKey(KeyCode.Alpha5))
			SetRuleset(RulesetList.Majority);
		if (Input.GetKey(KeyCode.Alpha6))
			SetRuleset(RulesetList.Mazectric);
		if (Input.GetKey(KeyCode.Alpha7))
			SetRuleset(RulesetList.WalledCities);
	}

	private void SetRuleset(Ruleset p_ruleset)
	{
		_ruleset = p_ruleset;
		//_automaton.SetRuleset(_ruleset);
		//FillOneRuleset(_ruleset);
		FillFromTexture(p_ruleset);
	}

	private void FillOneRuleset(Ruleset p_ruleset)
	{
		for (int y = Core._width - 1; y >= 0; y--)
		{
			for (int x = Core._height - 1; x >= 0; x--)
			{
				_automaton._cells[x, y].rulset = p_ruleset;
				_automaton._cells[x, y].color = new ColorHSV(219f / 360f, 1f, 1f);
				_automaton._cells[x, y].state = Cell.State.Dead;
				_automaton._copy[x, y].rulset = p_ruleset;
				_automaton._copy[x, y].color = new ColorHSV(219f / 360f, 1f, 1f);
				_automaton._copy[x, y].state = Cell.State.Dead;
			}
		}
	}
		
	private void FillFromTexture(Ruleset rulset)
	{
		for (int y = _map.height - 1; y >= 0; y--)
		{
			for (int x = _map.width - 1; x >= 0; x--)
			{
				if (_map.GetPixel(x, y).a != 0)
				{
					_automaton._cells[x, y].rulset = RulesetList.Death;
					_automaton._copy[x, y].rulset = RulesetList.Death;
				}
				else
				{
					_automaton._cells[x, y].rulset = rulset;
					_automaton._copy[x, y].rulset = rulset;
				}
				_automaton._cells[x, y].color = new ColorHSV(200f / 360f, 1f, 1f);
				_automaton._copy[x, y].color = new ColorHSV(200f / 360f, 1f, 1f);
			}
		}
	}
}
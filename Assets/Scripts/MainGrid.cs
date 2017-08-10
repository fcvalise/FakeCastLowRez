﻿using UnityEngine;

public class MainGrid : ACellObject
{
	public CellularAutomaton	_automaton;
	public Cell[,]				_staticGrid;

	private Ruleset				_ruleset = Ruleset.coral;
	private float				_startNoise = 0.25f;
	private bool				_aliveBorders = false;
	private Texture2D			_texture;

	public override void Setup()
	{
		_texture = (Texture2D)Resources.Load("Textures/test_map_cpu");
		_automaton = new CellularAutomaton(_texture.width, _texture.height, _ruleset, _startNoise, _aliveBorders);
		_staticGrid = new Cell[_texture.width, _texture.height];
		FillOneRuleset(Ruleset.life);
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
				_automaton._cells[x, y].state = Cell.State.Dead;
				_automaton._copy[x, y].rulset = p_ruleset;
				_automaton._copy[x, y].color = new ColorHSV(200f / 360f, 1f, 1f);
				_automaton._copy[x, y].state = Cell.State.Dead;
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
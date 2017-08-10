using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpriteManager : MonoBehaviour
{
	public CellSprite[]						_spritesData;
	public string							_initial;

	private Dictionary<string, CellSprite>	_sprites;
	private string							_current;
	private Vector2							_side;

	void Start()
	{
		_sprites = new Dictionary<string, CellSprite>();
		foreach (CellSprite sprite in _spritesData)
		{
			//TODO : Consider dynamic enum generation
			_sprites.Add(sprite.name, sprite);
			_sprites[sprite.name].Create();
		}
		_current = _initial;
		_side = Vector2.down;
	}

	public void Simulate(Cell[, ] p_cells)
	{
		_sprites[_current].SetSide(_side);
		_sprites[_current].Simulate(p_cells);
	}

	public string GetState()
	{
		return _current;
	}

	public void SetSide(Vector2 p_side)
	{
		if (p_side != Vector2.zero)
			_side = p_side;
	}

	public void PlayNext(string p_spriteState)
	{
		if (_current != p_spriteState)
		{
			_current = p_spriteState;
			_sprites[_current].Play();
		}
	}

	public bool isFinished()
	{
		return _sprites[_current].isFinished();
	}
}
﻿using UnityEngine;
using UnityEngine.UI;

//
// CORE LOOP OF THE GAME
//

public class Core : MonoBehaviour
{
	public ACellObject[] _ui;
	public const int	_width = 64;
	public const int	_height = 64;

	public RawImage		_image;

	private Color[]		_pixels = new Color[_width * _height];
	private Texture2D	_texture;

	private float		_speed = 30f;
	private float		_deltaSpeed = 0f;

	private GameObject	_players;

	private MainGrid	_mainGrid;

	private void Awake()
	{
		_texture = new Texture2D(_width, _height, TextureFormat.ARGB32, false, true)
		{
			filterMode = FilterMode.Point
		};
		_texture.Apply();
		_image.texture = _texture;

		_mainGrid = new MainGrid();
		_mainGrid.Setup();

		foreach (ACellObject obj in _ui)
			obj.Setup();
	}

	private void Update()
	{
		_deltaSpeed += Time.deltaTime;
		if (_deltaSpeed > 1f / _speed)
		{
			Bullet[] bullets = FindObjectsOfType(typeof(Bullet)) as Bullet[];
			_deltaSpeed = 0f;
			Player[] players = FindObjectsOfType(typeof(Player)) as Player[];
			foreach (Player player in players)
				player.Simulate();
			// Manage collision between bullets
			/*
			for (int i = bullets.Length - 1; i >= 0; i--) {
				for (int j = bullets.Length - 1; j >= 0; j--) {
					if (bullets[i] != bullets[j]) {
						if (Vector3.Distance(bullets[i].transform.position, bullets[j].transform.position) < 5.0f) {
							Destroy(bullets[i].gameObject);
							Destroy(bullets[j].gameObject);
						}
					}
				}
			}
			*/
			foreach (Bullet bullet in bullets)
				bullet.Simulate();
			foreach (ACellObject obj in _ui)
				obj.Simulate();
			_mainGrid.Simulate();
			foreach (Player player in players)
				player.Add(_mainGrid._automaton._cells, _mainGrid._staticGrid);
			foreach (Bullet bullet in bullets)
				bullet.Add(_mainGrid._automaton._cells, _mainGrid._staticGrid);
			foreach (ACellObject obj in _ui)
				obj.Add(_mainGrid._automaton._cells, _mainGrid._staticGrid);
			Draw();
		}
	}

	private void Draw()
	{
		for (int x = 0; x < _width; x++)
		{
			for (int y = 0; y < _height; y++)
			{
				_mainGrid._automaton._cells[x, y].color.s = 1f - _mainGrid._automaton._cells[x, y].value;
				_mainGrid._automaton._cells[x, y].color.a = Mathf.Clamp(_mainGrid._automaton._cells[x, y].value, 0f, 1f);
				if (_mainGrid._staticGrid[x, y].state == Cell.State.Alive)
					_pixels[y * _width + x] = _mainGrid._staticGrid[x, y].color.ToColor();
				else
					_pixels[y * _width + x] = _mainGrid._automaton._cells[x, y].color.ToColor();
				_mainGrid._staticGrid[x, y].state = Cell.State.Dead;
			}
		}
		_texture.SetPixels(_pixels);
		_texture.Apply();
	}
}
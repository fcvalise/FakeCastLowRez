using UnityEngine;
using UnityEngine.UI;

//
// CORE LOOP OF THE GAME
//

public class Core : MonoBehaviour
{
	public const int	_width = 64;
	public const int	_height = 64;

	public RawImage		_image;
	public Player		_player;

	private Color[]		_pixels = new Color[_width * _height];
	private Texture2D	_texture;

	private float		_speed = 30f;
	private float		_deltaSpeed = 0f;

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
		_player.Setup();
	}

	private void Update()
	{
		Bullet[] bullets = FindObjectsOfType(typeof(Bullet)) as Bullet[];
		_deltaSpeed += Time.deltaTime;
		if (_deltaSpeed > 1f / _speed)
		{
			_deltaSpeed = 0f;
			_player.Simulate();
			foreach (Bullet bullet in bullets)
				bullet.Simulate();
			_mainGrid.Simulate();
			_player.Add(_mainGrid._automaton._cells, _mainGrid._staticGrid);
			foreach (Bullet bullet in bullets)
				bullet.Add(_mainGrid._automaton._cells, _mainGrid._staticGrid);
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
				{
					_pixels[y * _width + x] = _mainGrid._staticGrid[x, y].color.ToColor();
				}
				else
					_pixels[y * _width + x] = _mainGrid._automaton._cells[x, y].color.ToColor();
			}
		}
		_texture.SetPixels(_pixels);
		_texture.Apply();
	}
}
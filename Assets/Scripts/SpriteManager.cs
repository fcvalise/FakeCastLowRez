using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProceduralToolkit
{
	public class SpriteManager : MonoBehaviour
	{
		public enum SpriteState
		{
			Idle,
			Walk,
			Lay,
			CastStart,
			CastIncant,
			CastShoot,
			None
		}

		[System.Serializable]
		public struct SpriteData
		{
			public SpriteState	state;
			public Texture2D	spriteSheet;
			public int			numberOfSprites;
			public bool			isLoop;
			[HideInInspector]
			public CellSprite	cellSprite;
		}

		public SpriteData[]							_spritesData;

		private Dictionary<SpriteState, CellSprite>	_sprites;
		private SpriteState							_state;
		private Vector2								_side;

		void Start()
		{
			_sprites = new Dictionary<SpriteState, CellSprite>();
			foreach (SpriteData data in _spritesData)
			{
				CellSprite cellSprite = new CellSprite(data.spriteSheet, data.numberOfSprites, data.isLoop);
				_sprites.Add(data.state, cellSprite);
			}
			_state = SpriteState.Idle;
			_side = Vector2.down;
		}

		public void Simulate(CellularCell[, ] p_cells)
		{
			_sprites[_state].SetSide(_side);
			_sprites[_state].Simulate(p_cells);
		}

		public SpriteState GetState()
		{
			return _state;
		}

		public void SetSide(Vector2 p_side)
		{
			if (p_side != Vector2.zero)
				_side = p_side;
		}

		public void PlayNext(SpriteState p_spriteState)
		{
			if (_state != p_spriteState)
			{
				_state = p_spriteState;
				_sprites[_state].Play();
			}
		}

		public bool isFinished()
		{
			return _sprites[_state].isFinished();
		}
	}
}
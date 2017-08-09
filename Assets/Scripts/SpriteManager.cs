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

		public SpriteData[]							m_spritesData;

		private Dictionary<SpriteState, CellSprite>	m_sprites;
		private SpriteState							m_state;
		private Vector2Int							m_side;

		void Start()
		{
			m_sprites = new Dictionary<SpriteState, CellSprite>();
			foreach (SpriteData data in m_spritesData)
			{
				CellSprite cellSprite = new CellSprite(data.spriteSheet, data.numberOfSprites, data.isLoop);
				m_sprites.Add(data.state, cellSprite);
			}
			m_state = SpriteState.Idle;
			m_side = Vector2Int.down;
		}

		public void Simulate(CellularCell[, ] cells)
		{
			m_sprites[m_state].SetSide(m_side);
			m_sprites[m_state].Simulate(cells);
		}

		public SpriteState GetState()
		{
			return m_state;
		}

		public void SetSide(Vector2Int side)
		{
			if (side != Vector2Int.zero)
				m_side = side;
		}

		public void PlayNext(SpriteState spriteState)
		{
			if (m_state != spriteState)
			{
				m_state = spriteState;
				m_sprites[m_state].Play();
			}
		}

		public bool isFinished()
		{
			return m_sprites[m_state].isFinished();
		}
	}
}
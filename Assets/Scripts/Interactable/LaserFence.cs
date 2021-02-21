using System;
using PlayerCore;
using UnityEngine;

namespace Interactable
{
	/// <summary>
	/// Представление лазерной сетки
	/// </summary>
	[ExecuteAlways]
	public class LaserFence : InteractableObject
	{
#pragma warning disable CS0649
		[Header("Лазеры")]
		[SerializeField] private LineRenderer[] _beams;

		[SerializeField] private Transform _leftSide;
		[SerializeField] private Transform _rightSide;

		[Header("Свет")]
		[SerializeField] private GameObject _lightsContainer;
#pragma warning restore CS0649
		
		// do nothing...
		public override void Interact() {}
		
		private void Start()
		{
			DrawBeams();
		}
		
		private void ExternalSourceOnStateOn()
		{
			SetFenceState(false);
		}
		
		private void ExternalSourceOnStateOff()
		{
			SetFenceState(true);
		}
		
		private void OnEnable()
		{
			// Подписка на события внешнего триггера
			if (ExternalSource != null)
			{
				ExternalSource.StateOn += ExternalSourceOnStateOn;
				ExternalSource.StateOff += ExternalSourceOnStateOff;
			}
		}

		private void OnDisable()
		{
			if (ExternalSource != null)
			{
				ExternalSource.StateOn -= ExternalSourceOnStateOn;
				ExternalSource.StateOff -= ExternalSourceOnStateOff;
			}
		}

		// Отрисовка лучей между панелями
		private void DrawBeams()
		{
			float sideHeight = _leftSide.transform.localScale.y / _beams.Length;
			
			var start = _leftSide.transform.position;
			var end = _rightSide.transform.position;
			
			for(int i = _beams.Length - 1; i >= 0; i--)
			{
				var beam = _beams[i];
				var beamHeight = start.y - sideHeight * i - 0.2f;
				beam.positionCount = 2;
				
				beam.SetPosition(0, new Vector3( start.x, beamHeight, start.z));
				beam.SetPosition(1, new Vector3( end.x, beamHeight, end.z));
			}
		}

		// Включение/отключение лазеров
		private void SetFenceState(bool state)
		{
			foreach (var beam in _beams)
				beam.gameObject.SetActive(state);
			
			_lightsContainer.SetActive(state);
			State = state;
		}

		private void OnTriggerEnter(Collider other)
		{
			if(State == false)
				return;
			
			// Урон игроку если лазерная сетка активирована
			if (other.TryGetComponent(out Player player))
				player.TakeDamage(100);

			Debug.Log($"<b><color=red>Игрок получил {100} урона</color></b>");
		}
	}
}

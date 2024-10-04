using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickSpeed : MonoBehaviour {
    [SerializeField] private float hitTickRate = 0.1f;
	[SerializeField] private float behaviourTickRate = 0.5f;

	private float _hitTickrate = 0;
	private float _behaviourTickRate = 0;

	public delegate void Tick();
    public static Tick HitTickEvent;
	public static Tick BehaviourTickEvent;

	void Update() {
		_hitTickrate += Time.deltaTime;

		if (_hitTickrate > hitTickRate) {
            _hitTickrate = 0;

			HitTickEvent?.Invoke();
		}

		_behaviourTickRate += Time.deltaTime;

		if (_behaviourTickRate > behaviourTickRate) {
			_behaviourTickRate = 0;

			BehaviourTickEvent?.Invoke();
		}
	}
}

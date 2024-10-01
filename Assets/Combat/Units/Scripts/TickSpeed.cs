using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickSpeed : MonoBehaviour {
    [SerializeField] private float hitTickRate = 0.1f;

    private float _hitTickrate = 0;

    public delegate void Tick();
    public static Tick HitTickEvent;

    void Update() {
		_hitTickrate += Time.deltaTime;

		if (_hitTickrate > hitTickRate) {
            _hitTickrate = 0;

			HitTickEvent?.Invoke();
		}
    }
}

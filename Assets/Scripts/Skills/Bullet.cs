using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float _speed;
	public GameObject _target;

	void Update() {
		Vector3 direction = (_target.transform.position - transform.position).normalized;
		transform.position += direction * _speed * Time.deltaTime;
	}
}

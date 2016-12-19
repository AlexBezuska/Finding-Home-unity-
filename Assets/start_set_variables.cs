using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class start_set_variables : MonoBehaviour {

  private ExampleVariableStorage _storage;

  void Start(){
  	_storage = Object.FindObjectOfType<ExampleVariableStorage>();
    //_storage.SetValue("$shoud_see_mama", true);
		_storage.SetValue("$should_see_mama", new Yarn.Value((bool)true));
    Debug.Log(_storage.GetValue("$should_see_mama"));
	}

	// Update is called once per frame
	void Update () {

	}
}

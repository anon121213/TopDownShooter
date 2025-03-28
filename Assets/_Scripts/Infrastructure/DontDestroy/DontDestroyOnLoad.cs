﻿using UnityEngine;

namespace _Scripts.Infrastructure.DontDestroy
{
  public class DontDestroyOnLoad : MonoBehaviour
  {
    private void Awake() =>
      DontDestroyOnLoad(gameObject);
  }
}
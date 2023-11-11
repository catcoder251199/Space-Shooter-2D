using System;
using UnityEngine.Events;
using UnityEngine;

[Serializable]
public class IntIntEvent : UnityEvent<int, int> { }

[Serializable]
public class IntEvent : UnityEvent<int> { }

[Serializable]
public class IntBoolEvent : UnityEvent<int, bool> { }

[Serializable]
public class BoolEvent : UnityEvent<bool> { }

[Serializable]
public class Collider2D_Event : UnityEvent<Collider2D> { }




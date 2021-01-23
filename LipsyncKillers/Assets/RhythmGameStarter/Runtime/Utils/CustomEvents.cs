using UnityEngine.Events;
using UnityEngine;
using System;

namespace RhythmGameStarter
{
    [Serializable] public class StringEvent : UnityEvent<string> { }
    [Serializable] public class BoolEvent : UnityEvent<bool> { }
    [Serializable] public class FloatEvent : UnityEvent<float> { }
    [Serializable] public class TouchEvent : UnityEvent<Touch> { }
}

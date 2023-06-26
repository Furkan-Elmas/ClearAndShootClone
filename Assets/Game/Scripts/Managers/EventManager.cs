using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static Action OnPlayerTapToStart;
    public static Action OnCleanObjectPerfectly;
    public static Action OnCleanObjectImperfectly;
    public static Action OnCleaningStatePassed;
    public static Action OnStatePassed;
}

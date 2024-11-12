using System;
using System.Collections.Generic;
using UnityEngine;

#region Commands
[Serializable]
public class Command : Object
{
    public int index;
    public string description;
    public TransformData transformData;
    public string url;
}

[Serializable]
public class ObjectCommand : Command
{
}

[Serializable]
public class PresentationCommand : Command
{
    public string videoUrl;
}

[Serializable]
public class TeleportCommand : Command
{
}

[Serializable]
public class PollCommand : Command
{
    public string message;
    public string creator;
    public List<Option> options;
    public Dictionary<string, string> responseDictionary;
}

[Serializable]
public class Option
{
    public string id;
    public string text;
    public bool isCorrect;
}
#endregion

#region Helpers
[Serializable]
public class TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}

[Serializable]
public enum ClipboardActionType
{
    None,
    Object,
    Presentation,
    Teleport,
    Poll
}

[Serializable]
public enum Classroom
{
    Classroom1,
    Classroom2
}
#endregion

[Serializable]
public class Clipboard
{
    public string version;
    public List<ClipboardData> data;
}

[Serializable]
public class ClipboardData
{
    public ClipboardActionType actionType;
    public Command commandData;
    public ObjectCommand objectData;
    public PresentationCommand presentationData;
    public TeleportCommand teleportData;
    public PollCommand pollData;
}
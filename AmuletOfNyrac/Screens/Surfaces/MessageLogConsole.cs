﻿using System.Diagnostics.CodeAnalysis;
using SadConsole;
using SadRogue.Primitives;

namespace AmuletOfNyrac.Screens.Surfaces;

/// <summary>
/// A very basic SadConsole Console subclass that acts as a game message log.
/// </summary>
public class MessageLogConsole : Console
{
    private ColoredString _lastMessage;
    private int _lastMessageCount;

    public MessageLogConsole(int width, int height)
        : base(width, height)
    {
        Initialize();
    }

    public MessageLogConsole(int width, int height, int bufferWidth, int bufferHeight)
        : base(width, height, bufferWidth, bufferHeight)
    {
        Initialize();
    }

    public MessageLogConsole(ICellSurface surface, IFont? font = null, Point? fontSize = null)
        : base(surface, font, fontSize)
    {
        Initialize();
    }

    [MemberNotNull(nameof(_lastMessage))]
    private void Initialize()
    {
        Cursor.AutomaticallyShiftRowsUp = true;
        _lastMessage = new("");
    }

    public void AddMessage(ColoredString message)
    {
        // For now, we'll just blend messages with different colors but same content
        if (_lastMessage.String == message.String)
            _lastMessageCount++;
        else
        {
            _lastMessageCount = 1;
            _lastMessage = message;
        }

        if (_lastMessageCount > 1)
        {
            Cursor.Position = Cursor.Position.Translate(0, -1);
            Cursor.Print(_lastMessage + " (x" + _lastMessageCount.ToString() + ")");
        }
        else
            Cursor.Print(_lastMessage);

        Cursor.NewLine();
    }
}
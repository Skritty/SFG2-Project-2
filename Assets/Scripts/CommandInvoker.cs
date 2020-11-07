using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    private static Stack<ICommand> commandHistory = new Stack<ICommand>();

    public static void ExecuteCommand(ICommand command)
    {
        commandHistory.Push(command);
        command.Execute();
    }

    public static void UndoCommand()
    {
        if (commandHistory.Count <= 0) return;
        commandHistory.Pop().Undo();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler
{
    private List<KeyValuePair<Func<bool>, ICommand>> commandPairList = new List<KeyValuePair<Func<bool>, ICommand>>();

    public void BindCommand(Func<bool> predicate, ICommand command)
    {            
        commandPairList.Add(new KeyValuePair<Func<bool>, ICommand>(predicate, command));
    }
    
    public void UnbindCommand(Func<bool> predicate, ICommand command)
    {
        commandPairList.Remove(new KeyValuePair<Func<bool>, ICommand>(predicate, command));
    }

    // Loop through the predicates within the PairList and
    // return a list of commands where their corresponding predicates return true
    public List<ICommand> ReceiveCommands()
    {
        List<ICommand> commandList = new List<ICommand>();
        foreach (var pair in commandPairList)
        {
            ICommand command = pair.Value;
            
            if (pair.Key.Invoke())
                commandList.Add(command);
        }
        
        return commandList;
    }
    
    // Executes the given commands
    public static void ExecuteCommands(List<ICommand> commandList)
    {
        foreach (ICommand command in commandList)
        {
            command?.Execute();
        }
    }
}
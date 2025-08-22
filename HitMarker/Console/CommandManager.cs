using HitMarker.Console.Commands.Give;
using HitMarker.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HitMarker.Console
{
    public class CommandManager : MonoBehaviour
    {
        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();
        private int index = 0;

        private void Awake()
        {
            LoggerUtils.LogDebug("CommandManager", "Awake called. Initializing CommandManager...");
            // Registra il comando "give"
            RegisterCommand(new GiveCommand());


        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K) && index <= 15)
            {
                ExecuteCommand("give", index.ToString());
                index++;
            }
        }
        public void RegisterCommand(ICommand command)
        {
            if (command == null || string.IsNullOrEmpty(command.Name))
            {
                throw new ArgumentException("Command or command name cannot be null or empty.");
            }
            if (_commands.ContainsKey(command.Name))
            {
                throw new InvalidOperationException($"Command '{command.Name}' is already registered.");
            }
            _commands[command.Name] = command;
        }
        public void ExecuteCommand(string commandName, string args)
        {
            if (_commands.TryGetValue(commandName, out var command))
            {
                command.Execute(args);
            }
            else
            {
                throw new KeyNotFoundException($"Command '{commandName}' not found.");
            }
        }
    }
}

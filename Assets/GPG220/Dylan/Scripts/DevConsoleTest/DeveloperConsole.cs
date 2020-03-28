using System;
using System.Collections.Generic;
using System.Linq;

namespace DevConsoleTest
{
    public class DeveloperConsole
    {
        private readonly string prefix;
        private readonly IEnumerable<IConsoleCommand> commands;
        
        public DeveloperConsole(string prefix, IEnumerable<IConsoleCommand> commands)
        {
            this.prefix = prefix;
            this.commands = commands;
        }

        public void ProcessCommand(string inputValue)
        {
            if (!inputValue.StartsWith(prefix))
            {
                return;
            }

            inputValue = inputValue.Remove(0, prefix.Length);

            string[] inputSplit = inputValue.Split(' ');

            string commandInput = inputSplit[0];
            string[] args = inputSplit.Skip(1).ToArray();
            
            CheckValidCommand(commandInput,args);
        }

        public void CheckValidCommand(string commandInput, string[] args)
        {
            foreach (var command in commands)
            {
                //using equals ensures that commands ignore case sensitivity 
                if (!commandInput.Equals(command.CommandWord, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (command.Process(args))
                {
                    return;
                    
                }
            }
        }
        
    }
}

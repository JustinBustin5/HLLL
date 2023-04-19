using System;
using System.IO;
using Spectre.Console;

namespace HLLL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AnsiConsole.MarkupLine("[bold yellow]HLLL[/]");
            AnsiConsole.WriteLine("v1.0");
            AnsiConsole.WriteLine();
            if (args.Length >= 1)
            {
                if (args[0].ToLower().Trim() == "-r")
                {
                    if (args.Length >= 2)
                    {
                        if (File.Exists(args[1]))
                        {
                            RunProgram(args[1]);
                        }
                        else AnsiConsole.MarkupLine("[red]Error: File doesn't exist[/]");
                    }
                    else
                    {
                        HelpMsg("-r");
                    }
                }
                else HelpMsg();
            }
            else
            {
                HelpMsg();
            }
        }

        static void RunProgram(string progPath)
        {
            AnsiConsole.MarkupLine("[lime]Running [/][bold underline white]" + progPath + "[/][lime]...[/]");
            AnsiConsole.WriteLine();
            string[] lines = File.ReadAllLines(progPath);
            HLLLProgram program = new HLLLProgram(lines);
            program.Execute();
        }

        static void HelpMsg()
        {
            AnsiConsole.MarkupLine("[lime]HLLL Command Line Arguments:[/]");
            AnsiConsole.WriteLine("-r <path> : Run program");
        }
        static void HelpMsg(string topic)
        {
            if (topic == "-r")
            {
                AnsiConsole.MarkupLine("[lime]-r <path> : Run program[/]");
                AnsiConsole.WriteLine("<path> : Path to the .hlll file (ex: HelloWorld.hlll; C:\\HLLLPrograms\\Choice.hlll)");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]So apparently I'm bad at coding because this message shouldn't show up[/]");
            }
        }
    }
}
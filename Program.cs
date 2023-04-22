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
            AnsiConsole.WriteLine("v1.1");
            AnsiConsole.WriteLine();
            if (args.Length >= 1)
            {
                if (File.Exists(args[0])) RunProgram(args[0]);
                else if (File.Exists(args[0] + ".hlll")) RunProgram(args[0] + ".hlll");
                else AnsiConsole.MarkupLine("[red]Error: File doesn't exist[/]");
            }
            else HelpMsg();
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
            AnsiConsole.MarkupLine("[lime]HLLL: An assembly-like scripting language.[/]");
            AnsiConsole.WriteLine("Pass a path to a .hlll file as a command line argument to run it.");
        }
    }
}
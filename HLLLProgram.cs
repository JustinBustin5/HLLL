using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLLL
{
    public class HLLLProgram
    {
        public string[] lines { get; private set; }
        public int sofIndex { get; private set; }
        public List<HLLLFlag> flags { get; private set; } = new List<HLLLFlag>();
        public List<HLLLInstruction> instructions { get; private set; } = new List<HLLLInstruction>();
        public List<HLLLVariable> variables { get; private set; } = new List<HLLLVariable>();

        private bool halt = false;
        private Stack<string> stack = new Stack<string>();
        private int executingIndex = 0;

        public HLLLProgram(string[] _lines)
        {
            lines = _lines;
            int index = 0;
            foreach (string line in lines)
            {
                if (line.Trim() == "") continue;
                HLLLInstruction instruction = new HLLLInstruction(line, index);
                instructions.Add(instruction);

                if (instruction.opcode == "sof") sofIndex = index;
                else if (instruction.opcode == "flg")
                {
                    flags.Add(new HLLLFlag(instruction.arguments[0], index + 1));
                }
                index++;
            }
        }

        public void Execute()
        {
            bool executing = true;
            executingIndex = sofIndex + 1;
            while (executing && !halt)
            {
                ExecuteInstruction(instructions[executingIndex]);
            }
        }

        private void ExecuteInstruction(HLLLInstruction instruction)
        {
            bool manualExecIndex = false;
            if (instruction.opcode == "wrt")
            {
                string? topStack = stack.Pop();
                if (topStack == null) ErrorOut("Stack is empty");
                else Console.Write(topStack);
            }
            else if (instruction.opcode == "wln")
            {
                string? topStack = stack.Pop();
                if (topStack == null) ErrorOut("Stack is empty");
                else Console.WriteLine(topStack);
            }
            else if (instruction.opcode == "rln")
            {
                string? lineRead = Console.ReadLine();
                if (lineRead == null) ErrorOut("Read line is null");
                else stack.Push(lineRead);
            }
            else if (instruction.opcode == "stv")
            {
                string? topStack = stack.Pop();
                if (topStack == null) ErrorOut("Stack is empty");
                else if (instruction.arguments.Count < 1) ErrorOut("Argument required at 'stv'");
                else
                {
                    HLLLVariable? var = variables.Where(x => x.name == instruction.arguments[0]).FirstOrDefault();
                    if (var == default || var == null) variables.Add(new HLLLVariable(instruction.arguments[0], topStack));
                    else var.value = topStack;
                }
            }
            else if (instruction.opcode == "ldv")
            {
                if (instruction.arguments.Count < 1) ErrorOut("Argument required at 'ldv'");
                HLLLVariable? var = variables.Where(x => x.name == instruction.arguments[0]).FirstOrDefault();
                if (var == default || var == null) ErrorOut($"Variable '{instruction.arguments[0]}' not found");
                else stack.Push(var.value);
            }
            else if (instruction.opcode == "dlv")
            {
                if (instruction.arguments.Count < 1) ErrorOut("Argument required at 'dlv'");
                HLLLVariable? var = variables.Where(x => x.name == instruction.arguments[0]).First();
                if (var == null) ErrorOut($"Variable '{instruction.arguments[0]}' not found");
                else variables.Remove(var);
            }
            else if (instruction.opcode == "psh")
            {
                if (instruction.arguments.Count < 1) ErrorOut("Argument required at 'psh'");
                else stack.Push(instruction.arguments[0]);
            }
            else if (instruction.opcode == "pop")
            {
                stack.Pop();
            }
            else if (instruction.opcode == "add")
            {
                string? topStack = stack.Pop();
                if (topStack == null) ErrorOut("Stack is empty");
                else
                {
                    string? newTopStack = stack.Pop();
                    if (topStack == null) ErrorOut("Not enough stack");
                    else
                    {
                        float? a = float.Parse(topStack);
                        if (a == null) ErrorOut("Arithmetic operand A is not a float");
                        else
                        {
                            float? b = float.Parse(newTopStack);
                            if (b == null) ErrorOut("Arithmetic operand B is not a float");
                            string? result = (a + b).ToString();
                            if (result == null) ErrorOut("Arithmetic result returned null");
                            else stack.Push(result);
                        }
                    }
                }
            }
            else if (instruction.opcode == "sub")
            {
                string? topStack = stack.Pop();
                if (topStack == null) ErrorOut("Stack is empty");
                else
                {
                    string? newTopStack = stack.Pop();
                    if (topStack == null) ErrorOut("Not enough stack");
                    else
                    {
                        float? a = float.Parse(topStack);
                        if (a == null) ErrorOut("Arithmetic operand A is not a float");
                        else
                        {
                            float? b = float.Parse(newTopStack);
                            if (b == null) ErrorOut("Arithmetic operand B is not a float");
                            string? result = (a - b).ToString();
                            if (result == null) ErrorOut("Arithmetic result returned null");
                            else stack.Push(result);
                        }
                    }
                }
            }
            else if (instruction.opcode == "mul")
            {
                string? topStack = stack.Pop();
                if (topStack == null) ErrorOut("Stack is empty");
                else
                {
                    string? newTopStack = stack.Pop();
                    if (topStack == null) ErrorOut("Not enough stack");
                    else
                    {
                        float? a = float.Parse(topStack);
                        if (a == null) ErrorOut("Arithmetic operand A is not a float");
                        else
                        {
                            float? b = float.Parse(newTopStack);
                            if (b == null) ErrorOut("Arithmetic operand B is not a float");
                            string? result = (a * b).ToString();
                            if (result == null) ErrorOut("Arithmetic result returned null");
                            else stack.Push(result);
                        }
                    }
                }
            }
            else if (instruction.opcode == "div")
            {
                string? topStack = stack.Pop();
                if (topStack == null) ErrorOut("Stack is empty");
                else
                {
                    string? newTopStack = stack.Pop();
                    if (topStack == null) ErrorOut("Not enough stack");
                    else
                    {
                        float? a = float.Parse(topStack);
                        if (a == null) ErrorOut("Arithmetic operand A is not a float");
                        else
                        {
                            float? b = float.Parse(newTopStack);
                            if (b == null) ErrorOut("Arithmetic operand B is not a float");
                            string? result = (a / b).ToString();
                            if (result == null) ErrorOut("Arithmetic result returned null");
                            else stack.Push(result);
                        }
                    }
                }
            }
            else if (instruction.opcode == "jmp")
            {
                if (instruction.arguments.Count < 1) ErrorOut("Argument required at 'jmp'");
                else
                {
                    HLLLFlag? flag = flags.Where(x => x.name == instruction.arguments[0]).First();
                    if (flag == null) ErrorOut($"No flag found named '{instruction.arguments[0]}'");
                    else
                    {
                        manualExecIndex = true;
                        executingIndex = flag.startIndex;
                    }
                }
            }
            else if (instruction.opcode == "bie")
            {
                if (instruction.arguments.Count < 1) ErrorOut("Argument required at 'bie'");
                else
                {
                    string? topStack = stack.Pop();
                    if (topStack == null) ErrorOut("Stack is empty");
                    else
                    {
                        string? newTopStack = stack.Pop();
                        if (topStack == null) ErrorOut("Not enough stack");
                        else
                        {
                            string a = topStack;
                            string b = newTopStack;
                            if (a == b)
                            {
                                HLLLFlag? flag = flags.Where(x => x.name == instruction.arguments[0]).First();
                                if (flag == null) ErrorOut($"No flag found named '{instruction.arguments[0]}'");
                                else
                                {
                                    manualExecIndex = true;
                                    executingIndex = flag.startIndex;
                                }
                            }
                        }
                    }
                }
            }
            else if (instruction.opcode == "big")
            {
                if (instruction.arguments.Count < 1) ErrorOut("Argument required at 'big'");
                else
                {
                    string? topStack = stack.Pop();
                    if (topStack == null) ErrorOut("Stack is empty");
                    else
                    {
                        string? newTopStack = stack.Pop();
                        if (topStack == null) ErrorOut("Not enough stack");
                        else
                        {
                            float? a = float.Parse(topStack);
                            if (a == null) ErrorOut("Comparison operand A is not a float");
                            else
                            {
                                float? b = float.Parse(newTopStack);
                                if (b == null) ErrorOut("Comparison operand B is not a float");
                                else
                                {
                                    if (a > b)
                                    {
                                        HLLLFlag? flag = flags.Where(x => x.name == instruction.arguments[0]).First();
                                        if (flag == null) ErrorOut($"No flag found named '{instruction.arguments[0]}'");
                                        else
                                        {
                                            manualExecIndex = true;
                                            executingIndex = flag.startIndex;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (instruction.opcode == "bge")
            {
                if (instruction.arguments.Count < 1) ErrorOut("Argument required at 'bge'");
                else
                {
                    string? topStack = stack.Pop();
                    if (topStack == null) ErrorOut("Stack is empty");
                    else
                    {
                        string? newTopStack = stack.Pop();
                        if (topStack == null) ErrorOut("Not enough stack");
                        else
                        {
                            float? a = float.Parse(topStack);
                            if (a == null) ErrorOut("Comparison operand A is not a float");
                            else
                            {
                                float? b = float.Parse(newTopStack);
                                if (b == null) ErrorOut("Comparison operand B is not a float");
                                else
                                {
                                    if (a >= b)
                                    {
                                        HLLLFlag? flag = flags.Where(x => x.name == instruction.arguments[0]).First();
                                        if (flag == null) ErrorOut($"No flag found named '{instruction.arguments[0]}'");
                                        else
                                        {
                                            manualExecIndex = true;
                                            executingIndex = flag.startIndex;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (instruction.opcode == "bil")
            {
                if (instruction.arguments.Count < 1) ErrorOut("Argument required at 'bil'");
                else
                {
                    string? topStack = stack.Pop();
                    if (topStack == null) ErrorOut("Stack is empty");
                    else
                    {
                        string? newTopStack = stack.Pop();
                        if (topStack == null) ErrorOut("Not enough stack");
                        else
                        {
                            float? a = float.Parse(topStack);
                            if (a == null) ErrorOut("Comparison operand A is not a float");
                            else
                            {
                                float? b = float.Parse(newTopStack);
                                if (b == null) ErrorOut("Comparison operand B is not a float");
                                else
                                {
                                    if (a < b)
                                    {
                                        HLLLFlag? flag = flags.Where(x => x.name == instruction.arguments[0]).First();
                                        if (flag == null) ErrorOut($"No flag found named '{instruction.arguments[0]}'");
                                        else
                                        {
                                            manualExecIndex = true;
                                            executingIndex = flag.startIndex;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (instruction.opcode == "ble")
            {
                if (instruction.arguments.Count < 1) ErrorOut("Argument required at 'ble'");
                else
                {
                    string? topStack = stack.Pop();
                    if (topStack == null) ErrorOut("Stack is empty");
                    else
                    {
                        string? newTopStack = stack.Pop();
                        if (topStack == null) ErrorOut("Not enough stack");
                        else
                        {
                            float? a = float.Parse(topStack);
                            if (a == null) ErrorOut("Comparison operand A is not a float");
                            else
                            {
                                float? b = float.Parse(newTopStack);
                                if (b == null) ErrorOut("Comparison operand B is not a float");
                                else
                                {
                                    if (a <= b)
                                    {
                                        HLLLFlag? flag = flags.Where(x => x.name == instruction.arguments[0]).First();
                                        if (flag == null) ErrorOut($"No flag found named '{instruction.arguments[0]}'");
                                        else
                                        {
                                            manualExecIndex = true;
                                            executingIndex = flag.startIndex;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (instruction.opcode == "biz")
            {
                if (instruction.arguments.Count < 1) ErrorOut("Argument required at 'biz'");
                else
                {
                    string? topStack = stack.Pop();
                    if (topStack == null) ErrorOut("Stack is empty");
                    else
                    {
                        float? a = float.Parse(topStack);
                        if (a == null) ErrorOut("Comparison operand A is not a float");
                        else
                        {
                            if (a == 0)
                            {
                                HLLLFlag? flag = flags.Where(x => x.name == instruction.arguments[0]).First();
                                if (flag == null) ErrorOut($"No flag found named '{instruction.arguments[0]}'");
                                else
                                {
                                    manualExecIndex = true;
                                    executingIndex = flag.startIndex;
                                }
                            }
                        }
                    }
                }
            }
            else if (instruction.opcode == "hlt")
            {
                if (instruction.arguments.Count < 1) ErrorOut("Argument required at 'hlt'");
                else
                {
                    int? exitCode = int.Parse(instruction.arguments[0]);
                    if (exitCode == null) ErrorOut("Exit code is not an int");
                    else Exit((int)exitCode);
                }
            }
            else if (instruction.opcode == "flg" || instruction.opcode == "sof" || instruction.opcode == "nop") { }
            else ErrorOut("Invalid instruction '" + instruction.opcode + "'");
            if (!manualExecIndex) executingIndex++;
        }

        private void ErrorOut(string msg)
        {
            halt = true;
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[red]Error: " + msg + "[/]");
        }
        private void Exit(int exitCode)
        {
            halt = true;
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[lime]Program exited with code " + exitCode.ToString() + "[/]");
        }

        public class HLLLFlag
        {
            public string name { get; private set; }
            public int startIndex { get; private set; }

            public HLLLFlag(string _name, int _startIndex)
            {
                name = _name;
                startIndex = _startIndex;
            }
        }
        public class HLLLVariable
        {
            public string name { get; set; }
            public string value { get; set; }

            public HLLLVariable(string _name, string _value)
            {
                name = _name;
                value = _value;
            }
        }
        public class HLLLInstruction
        {
            public string opcode { get; private set; }
            public List<string> arguments { get; private set; } = new List<string>();
            public int instructionIndex { get; private set; }

            public HLLLInstruction(string line, int _instructionIndex)
            {
                instructionIndex = _instructionIndex;
                string[] splitLine = line.Trim().Split(" ");
                if (splitLine.Length > 0)
                {
                    int lineSplitterIndex = 0;
                    foreach (string str in splitLine)
                    {
                        if (lineSplitterIndex == 0) opcode = str.Trim();
                        else arguments.Add(str.Replace("\\s", " "));
                        lineSplitterIndex++;
                    }
                }
                if (opcode == null)
                {
                    opcode = "nop";
                    arguments.Clear();
                }
            }
        }
    }
}
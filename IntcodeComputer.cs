using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019;

public class IntcodeComputer
{
    private readonly List<long> Memory;
    public IReadOnlyList<long> ReadOnlyMemory => Memory;

    private int PC = 0;
    public long Output {get; private set;}
    private readonly Queue<long> Input = new();
    
    public IntcodeComputer(IReadOnlyList<long> initial)
    {
        Memory = initial.ToList();
    }

    public void ProvideInput(long value)
    {
        Input.Enqueue(value);
    }

    public void RunToHalt()
    {
        while (true)
        {
            var x = Run();
            if (x == IntcodeResult.INPUT) throw new ApplicationException("RunToHalt() called but program needs input");
            if (x == IntcodeResult.HALT) return;
        }
    }

    public IntcodeResult Run()
    {
        var startPC = PC;
        while (true)
        {
            var parameterMode1 = (Memory[PC] / 100) % 10;
            var parameterMode2 = (Memory[PC] / 1000) % 10;
            switch (Memory[PC] % 100)
            {
                case 99: return IntcodeResult.HALT;
                case 1: 
                    Memory[(int)Memory[PC + 3]] = Access(parameterMode1, Memory[PC + 1]) + Access(parameterMode2, Memory[PC + 2]);
                    PC += 4;
                    break;
                case 2: 
                    Memory[(int)Memory[PC + 3]] = Access(parameterMode1, Memory[PC + 1]) * Access(parameterMode2, Memory[PC + 2]);
                    PC += 4;
                    break;
                case 3: // INPUT
                    if (Input.TryDequeue(out var input))
                    {
                        Memory[(int)Memory[PC + 1]] = input;
                        PC += 2;
                    }
                    else
                    {
                        if (startPC == PC) throw new ApplicationException("IntcodeComputer.Run() called while waiting for input but no input was provided.");
                        return IntcodeResult.INPUT;
                    }
                    break;
                case 4:
                    Output = Access(parameterMode1, Memory[PC + 1]);
                    PC += 2;
                    return IntcodeResult.OUTPUT;
                case 5: // JUMP IF TRUE
                    var result = Access(parameterMode1, Memory[PC + 1]);
                    if (result != 0) PC = (int)Access(parameterMode2, Memory[PC + 2]);
                    else PC += 3;
                    break;
                case 6: // JUMP IF FALSE
                    var result2 = Access(parameterMode1, Memory[PC + 1]);
                    if (result2 == 0) PC = (int)Access(parameterMode2, Memory[PC + 2]);
                    else PC += 3;
                    break;
                case 7: // LESS THAN
                    Memory[(int)Memory[PC + 3]] = Access(parameterMode1, Memory[PC + 1]) < Access(parameterMode2, Memory[PC + 2]) ? 1 : 0;
                    PC += 4;
                    break;
                case 8: // EQUAL
                    Memory[(int)Memory[PC + 3]] = Access(parameterMode1, Memory[PC + 1]) == Access(parameterMode2, Memory[PC + 2]) ? 1 : 0;
                    PC += 4;
                    break;
                default: throw new ApplicationException($"Encountered opcode {Memory[PC]} at position {PC}");
            }
        }
        throw new ApplicationException();
    }

    private long Access(long parameterMode, long value)
    {
        if (parameterMode == 1) return value;
        return Memory[(int)value];
    }
}

public enum IntcodeResult
{
    OUTPUT,
    INPUT,
    HALT
}
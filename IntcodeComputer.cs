using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019;

public class IntcodeComputer
{
    private readonly List<long> Memory;
    public IReadOnlyList<long> ReadOnlyMemory => Memory;

    private int PC = 0;
    private int RelativeBase = 0;
    public long Output {get; private set;}
    private readonly Queue<long> Input = new();
    
    public IntcodeComputer(IEnumerable<long> initial)
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

    public IntcodeResult RunToOutputOrHalt()
    {
        while (true)
        {
            var x = Run();
            if (x == IntcodeResult.INPUT) throw new ApplicationException("RunToOutputOrHalt() called but program needs input");
            return x;
        }
    }

    public long RunToOutput()
    {
        while (true)
        {
            var x = Run();
            if (x == IntcodeResult.INPUT) throw new ApplicationException("RunToOutput() called but program needs input");
            if (x == IntcodeResult.HALT) throw new ApplicationException("RunToOutput() called but program halted");
            return Output;
        }
    }

    public IntcodeResult Run()
    {
        var startPC = PC;
        while (true)
        {
            var parameterMode1 = (Memory[PC] / 100) % 10;
            var parameterMode2 = (Memory[PC] / 1000) % 10;
            var parameterMode3 = (Memory[PC] / 10000) % 10;
            switch (Memory[PC] % 100)
            {
                case 99: return IntcodeResult.HALT;
                case 1: 
                    Write(parameterMode3, PC + 3, Read(parameterMode1, PC + 1) + Read(parameterMode2, PC + 2));
                    PC += 4;
                    break;
                case 2: 
                    Write(parameterMode3, PC + 3, Read(parameterMode1, PC + 1) * Read(parameterMode2, PC + 2));
                    PC += 4;
                    break;
                case 3: // INPUT
                    if (Input.TryDequeue(out var input))
                    {
                        Write(parameterMode1, PC + 1, input);
                        PC += 2;
                    }
                    else
                    {
                        if (startPC == PC) throw new ApplicationException("IntcodeComputer.Run() called while waiting for input but no input was provided.");
                        return IntcodeResult.INPUT;
                    }
                    break;
                case 4:
                    Output = Read(parameterMode1, PC + 1);
                    PC += 2;
                    return IntcodeResult.OUTPUT;
                case 5: // JUMP IF TRUE
                    var result = Read(parameterMode1, PC + 1);
                    if (result != 0) PC = (int)Read(parameterMode2, PC + 2);
                    else PC += 3;
                    break;
                case 6: // JUMP IF FALSE
                    var result2 = Read(parameterMode1, PC + 1);
                    if (result2 == 0) PC = (int)Read(parameterMode2, PC + 2);
                    else PC += 3;
                    break;
                case 7: // LESS THAN
                    Write(parameterMode3, PC + 3, Read(parameterMode1, PC + 1) < Read(parameterMode2, PC + 2) ? 1 : 0);
                    PC += 4;
                    break;
                case 8: // EQUAL
                    Write(parameterMode3, PC + 3, Read(parameterMode1, PC + 1) == Read(parameterMode2, PC + 2) ? 1 : 0);
                    PC += 4;
                    break;
                case 9: // ADJUST RELATIVE BASE
                    RelativeBase += (int)Read(parameterMode1, PC + 1);
                    PC += 2;
                    break;
                default: throw new ApplicationException($"Encountered opcode {Memory[PC]} at position {PC}");
            }
        }
        throw new ApplicationException();
    }

    private void Write(long parameterMode, long writePC, long value)
    {
        Ensure(writePC);
        Ensure(Memory[(int)writePC] + (parameterMode == 2 ? RelativeBase : 0));
        Memory[(int)Memory[(int)writePC] + (parameterMode == 2 ? RelativeBase : 0)] = value;
    }

    private long Read(long parameterMode, long readPC)
    {
        Ensure(readPC);
        var readAddress = Memory[(int)readPC];
        if (parameterMode == 1) return readAddress;
        Ensure(readAddress + (parameterMode == 2 ? RelativeBase : 0));
        return Memory[(int)readAddress + (parameterMode == 2 ? RelativeBase : 0)];
    }

    private void Ensure(long position)
    {
        if (position >= Memory.Count)
        {
            Memory.AddRange(Enumerable.Repeat(0L, (int)(position - Memory.Count + 1)));
        }
    }
}

public enum IntcodeResult
{
    OUTPUT,
    INPUT,
    HALT
}
using System.Collections;

namespace DistFS.Tools;

public class ArraySplitter<T> : IEnumerable<T[]>
{
    private readonly T[] _array;
    private readonly int _blockSize;
    private int _arrayIndex;

    public ArraySplitter(T[] array, int blockSize = 65536)
    {
        _array = array;
        _blockSize = blockSize;
    }

    public IEnumerator<T[]> GetEnumerator()
    {
        _arrayIndex = 0;
        while (HasNextBlock())
        {
            yield return GetNextBlock();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    private bool HasNextBlock()
    {
        return _arrayIndex < _array.Length;
    }

    private T[] GetNextBlock()
    {
        if (_arrayIndex >= _array.Length)
            throw new Exception("All blocks were already read");

        var result = _array.Skip(_arrayIndex);
        if (_arrayIndex + _blockSize < _array.Length)
        {
            result = result.Take(_blockSize);
            _arrayIndex += _blockSize;
        }
        else
        {
            _arrayIndex = _array.Length;
        }

        return result.ToArray();
    }
}
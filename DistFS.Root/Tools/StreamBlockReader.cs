namespace DistFS.Tools;

public class StreamBlockReader
{
    private const int MinimalBlockSize = 65536;
    private readonly Stream _stream;
    private readonly long _length;
    private readonly int _blockSize;
    private int _index;
    private readonly byte[] _buffer;

    public StreamBlockReader(Stream stream, long length)
    {
        _stream = stream;
        _length = length;
        _blockSize = (int)Math.Max(MinimalBlockSize, Math.Pow(2, Math.Floor(Math.Log2(length/200))));
        _buffer = new byte[_blockSize];
    }

    public bool HasNextBlock()
    {
        return _index < _length;
    }

    public byte[] GetNextBlock()
    {
        if (_index >= _length)
            throw new Exception("All blocks were already read");
        
        if (_index + _blockSize < _length)
        {
            _stream.Read(_buffer, 0, _blockSize);
        }
        else
        {
            _stream.Read(_buffer, 0, (int)(_length - _index));
        }

        _index += _blockSize;
        return _buffer;
    }
}
/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.Serialization
{
    using System;
    using Utilities;

    public sealed class StringBuffer
    {
        private char[] _buffer;
        private int _position;

        public StringBuffer()
        {
            _buffer = Empty<char>.array;
        }

        public StringBuffer(int initalCapacity)
        {
            _buffer = new char[initalCapacity];
        }

        public int position
        {
            get { return _position; }
            set { _position = value; }
        }

        public void Append(char value)
        {
            if (_position == _buffer.Length)
            {
                Resize(1);
            }

            _buffer[_position++] = value;
        }

        public void Append(string value)
        {
            var length = value.Length;
            if (_position + length >= _buffer.Length)
            {
                Resize(length);
            }

            value.CopyTo(0, _buffer, _position, length);
            _position += length;
        }

        public void Append(string value, int startIndex, int count)
        {
            if (_position + count >= _buffer.Length)
            {
                Resize(count);
            }

            value.CopyTo(startIndex, _buffer, _position, count);
            _position += count;
        }

        public void Append(char[] value)
        {
            var length = value.Length;
            if (_position + length >= _buffer.Length)
            {
                Resize(length);
            }

            Array.Copy(value, 0, _buffer, _position, length);
            _position += length;
        }

        public void Append(char[] value, int startIndex, int count)
        {
            if (_position + count >= _buffer.Length)
            {
                Resize(count);
            }

            Array.Copy(value, startIndex, _buffer, _position, count);
            _position += count;
        }

        public void EnsureCapacity(int minimumSpace)
        {
            if (_position + minimumSpace >= _buffer.Length)
            {
                Resize(minimumSpace);
            }
        }

        public void Clear()
        {
            _buffer = Empty<char>.array;
            _position = 0;
        }

        public override string ToString()
        {
            return new string(_buffer, 0, _position);
        }

        public string Flush()
        {
            var val = new string(_buffer, 0, _position);
            _position = 0;
            return val;
        }

        private void Resize(int appendLength)
        {
            char[] newBuffer = new char[(_position + appendLength) * 2];

            Array.Copy(_buffer, newBuffer, _position);

            _buffer = newBuffer;
        }
    }
}

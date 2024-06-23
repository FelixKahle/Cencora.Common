// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Runtime.InteropServices;
using System.Text;

namespace Cencora.Common.Core
{
    /// <summary>
    /// A locally unique identifier.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(true)]
    public struct Luid : IEquatable<Luid>
    {
        private int _lowerPart;
        private int _upperPart;

        private static int Counter = int.MinValue;

        public Luid()
        {
            long currentTicks = DateTime.UtcNow.Ticks;

            _lowerPart = (int)(currentTicks & 0xFFFFFFFF);
            _upperPart = Interlocked.Increment(ref Counter);
        }

        public Luid(byte[] bytes)
        {
            ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
            if (bytes.Length != 8)
            {
                throw new ArgumentException("The byte array must be 8 bytes long.", nameof(bytes));
            }

            _lowerPart = BitConverter.ToInt32(bytes, 0);
            _upperPart = BitConverter.ToInt32(bytes, 4);
        }

        public Luid(string value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            if (value.Length != 8)
            {
                throw new ArgumentException("The value must be 8 characters long.", nameof(value));
            }

            byte[] bytes = Encoding.UTF8.GetBytes(value);
            _lowerPart = BitConverter.ToInt32(bytes, 0);
            _upperPart = BitConverter.ToInt32(bytes, 4);
        }

        /// <summary>
        /// Creates a new LUID.
        /// </summary>
        /// <returns>A new LUID.</returns>
        public static Luid NewLuid()
        {
            return new Luid();
        }

        /// <summary>
        /// Creates a new LUID.
        /// </summary>
        /// <param name="value">The value of the LUID.</param>
        /// <returns>A new LUID.</returns>
        public static Luid NewLuid(string value)
        {
            return new Luid(value);
        }

        /// <summary>
        /// Creates a new LUID.
        /// </summary>
        /// <param name="bytes">The byte array to initialize the LUID with.</param>
        /// <returns>A new LUID.</returns>
        public static Luid NewLuid(byte[] bytes)
        {
            return new Luid(bytes);
        }
        
        /// <summary>
        /// Converts the LUID to a byte array.
        /// </summary>
        public byte[] ToByteArray()
        {
            byte[] lowerPartBytes = BitConverter.GetBytes(_lowerPart);
            byte[] upperPartBytes = BitConverter.GetBytes(_upperPart);
            
            byte[] result = new byte[8];
            Array.Copy(lowerPartBytes, 0, result, 0, 4);
            Array.Copy(upperPartBytes, 0, result, 4, 4);
            return result;
        }

        public bool Equals(Luid other)
        {
            return _lowerPart.Equals(other._lowerPart) && _upperPart.Equals(other._upperPart);
        }

        public override bool Equals(object? obj)
        {
            return obj is Luid other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_lowerPart, _upperPart);
        }

        public override string ToString()
        {
            byte[] bytes = ToByteArray();
            return BitConverter.ToString(ToByteArray());
        }
    }
}
// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.Common.Core
{
    /// <summary>
    /// Represents a locally unique identifier (LUID).
    /// </summary>
    /// <remarks>
    /// A LUID is a 64-bit value that is guaranteed to be unique only on the system that generated it.
    /// </remarks>
    public struct Luid : IComparable<Luid>, IEquatable<Luid>, IComparable
    {
        private int _lowerPart;
        private int _upperPart;

        /// <summary>
        /// Initializes a new instance of the <see cref="Luid"/> struct from a byte array.
        /// </summary>
        /// <param name="bytes">The byte array representing the LUID.</param>
        public Luid(byte[] bytes)
        {
            _lowerPart = BitConverter.ToInt32(bytes, 0);
            _upperPart = BitConverter.ToInt32(bytes, 4);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Luid"/> struct from the lower and upper parts.
        /// </summary>
        /// <param name="lowerPart">The lower part of the LUID.</param>
        /// <param name="upperPart">The upper part of the LUID.</param>
        public Luid(int lowerPart, int upperPart)
        {
            _lowerPart = lowerPart;
            _upperPart = upperPart;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Luid"/> struct from a string representation.
        /// </summary>
        /// <param name="str">The string representation of the LUID. Must be 8 characters long.</param>
        /// <exception cref="ArgumentException">Thrown when the string representation is not 8 characters long.</exception>
        public Luid(string str)
        {
            if (str.Length != 8)
            {
                throw new ArgumentException("Luid string must be 8 characters long");
            }

            byte[] bytes = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                bytes[i] = (byte)str[i];
            }

            _lowerPart = BitConverter.ToInt32(bytes, 0);
            _upperPart = BitConverter.ToInt32(bytes, 4);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Luid"/> struct.
        /// </summary>
        /// <remarks>
        /// The lower part of the LUID is the current time in ticks and the upper part is a random number.
        /// </remarks>
        /// <seealso cref="DateTime.Ticks"/>
        /// <seealso cref="Random.Next()"/>
        public Luid()
        {
            var currentTicks = DateTime.UtcNow.Ticks;
            var random = new Random();
            _lowerPart = (int)(currentTicks & 0xFFFFFFFF);
            _upperPart = random.Next();
        }

        /// <summary>
        /// Compares the current instance with another <see cref="Luid"/> and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other <see cref="Luid"/>.
        /// </summary>
        /// <param name="other">The <see cref="Luid"/> to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(Luid other)
        {
            if (_upperPart == other._upperPart)
            {
                return _lowerPart.CompareTo(other._lowerPart);
            }

            return _upperPart.CompareTo(other._upperPart);
        }

        /// <summary>
        /// Determines whether the current <see cref="Luid"/> is equal to another <see cref="Luid"/>.
        /// </summary>
        /// <param name="other">The <see cref="Luid"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the current instance is equal to the other instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Luid other)
        {
            return _lowerPart == other._lowerPart && _upperPart == other._upperPart;
        }

        /// <summary>
        /// Compares the current instance with another object and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        /// <exception cref="ArgumentException">Thrown when the object is not a <see cref="Luid"/>.</exception>
        public int CompareTo(object? obj)
        {
            if (obj is Luid luid)
            {
                return CompareTo(luid);
            }

            throw new ArgumentException("Object is not a Luid");
        }

        /// <summary>
        /// Determines whether the current <see cref="Luid"/> is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><c>true</c> if the current instance is equal to the other object; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            return obj is Luid luid && Equals(luid);
        }

        /// <summary>
        /// Returns the hash code for this <see cref="Luid"/>.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(_lowerPart, _upperPart);
        }

        /// <summary>
        /// Returns a string representation of this <see cref="Luid"/>.
        /// </summary>
        /// <returns>A string representation of the <see cref="Luid"/>.</returns>
        public override string ToString()
        {
            return $"{_lowerPart:X8}{_upperPart:X8}";
        }

        /// <summary>
        /// Determines whether two <see cref="Luid"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="Luid"/> to compare.</param>
        /// <param name="right">The second <see cref="Luid"/> to compare.</param>
        /// <returns><c>true</c> if the two instances are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Luid left, Luid right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="Luid"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="Luid"/> to compare.</param>
        /// <param name="right">The second <see cref="Luid"/> to compare.</param>
        /// <returns><c>true</c> if the two instances are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Luid left, Luid right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Determines whether the first <see cref="Luid"/> is less than the second <see cref="Luid"/>.
        /// </summary>
        /// <param name="left">The first <see cref="Luid"/> to compare.</param>
        /// <param name="right">The second <see cref="Luid"/> to compare.</param>
        /// <returns><c>true</c> if the first instance is less than the second instance; otherwise, <c>false</c>.</returns>
        public static bool operator <(Luid left, Luid right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Determines whether the first <see cref="Luid"/> is greater than the second <see cref="Luid"/>.
        /// </summary>
        /// <param name="left">The first <see cref="Luid"/> to compare.</param>
        /// <param name="right">The second <see cref="Luid"/> to compare.</param>
        /// <returns><c>true</c> if the first instance is greater than the second instance; otherwise, <c>false</c>.</returns>
        public static bool operator >(Luid left, Luid right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Determines whether the first <see cref="Luid"/> is less than or equal to the second <see cref="Luid"/>.
        /// </summary>
        /// <param name="left">The first <see cref="Luid"/> to compare.</param>
        /// <param name="right">The second <see cref="Luid"/> to compare.</param>
        /// <returns><c>true</c> if the first instance is less than or equal to the second instance; otherwise, <c>false</c>.</returns>
        public static bool operator <=(Luid left, Luid right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Determines whether the first <see cref="Luid"/> is greater than or equal to the second <see cref="Luid"/>.
        /// </summary>
        /// <param name="left">The first <see cref="Luid"/> to compare.</param>
        /// <param name="right">The second <see cref="Luid"/> to compare.</param>
        /// <returns><c>true</c> if the first instance is greater than or equal to the second instance; otherwise, <c>false</c>.</returns>
        public static bool operator >=(Luid left, Luid right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
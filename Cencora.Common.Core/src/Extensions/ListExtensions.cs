// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.Common.Core
{
    /// <summary>
    /// Provides extension methods for the <see cref="List{T}"/> class.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Determines whether the specified index is valid for the given list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to check.</param>
        /// <param name="index">The index to check.</param>
        /// <returns><c>true</c> if the index is valid; otherwise, <c>false</c>.</returns>
        public static bool IsIndexValid<T>(this IList<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }
    }
}
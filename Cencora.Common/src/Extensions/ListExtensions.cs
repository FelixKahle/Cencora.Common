// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.Common.Extensions;

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

    /// <summary>
    /// Determines whether the elements in the list are unique.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to check for uniqueness.</param>
    /// <returns><c>true</c> if all elements in the list are unique; otherwise, <c>false</c>.</returns>
    public static bool IsUnique<T>(this IList<T> list)
    {
        return list.Distinct().Count() == list.Count;
    }

    /// <summary>
    /// Determines whether the elements in a list are unique based on a specified key.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <typeparam name="TKey">The type of the key used for comparison.</typeparam>
    /// <param name="list">The list to check for uniqueness.</param>
    /// <param name="keySelector">A function to extract the key from each element.</param>
    /// <returns><c>true</c> if the elements in the list are unique based on the specified key; otherwise, <c>false</c>.</returns>
    public static bool IsUniqueBy<T, TKey>(this IList<T> list, Func<T, TKey> keySelector)
    {
        return list.DistinctBy(keySelector).Count() == list.Count;
    }
}
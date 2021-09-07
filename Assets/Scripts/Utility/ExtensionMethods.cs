using System.Collections.Generic;

/// <summary>
/// Extension methods for Lists.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Removes the last element of the given list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns>The last element</returns>
    public static T RemoveLast<T>(this List<T> list)
    {
        T lastItem = list[list.Count - 1];
        list.RemoveAt(list.Count-1);
        return lastItem;
    }
}
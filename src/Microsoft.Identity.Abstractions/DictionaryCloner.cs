// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Identity.Abstractions
{
    internal static class DictionaryCloner
    {
        internal static IDictionary<string, TValue>? Clone<TValue>(IDictionary<string, TValue>? source)
        {
            return source switch
            {
                null => null,
                Dictionary<string, TValue> dictionary => new Dictionary<string, TValue>(dictionary, dictionary.Comparer),
                SortedDictionary<string, TValue> dictionary => new SortedDictionary<string, TValue>(dictionary, dictionary.Comparer),
                SortedList<string, TValue> dictionary => new SortedList<string, TValue>(dictionary, dictionary.Comparer),
                _ => new Dictionary<string, TValue>(source),
            };
        }
    }
}

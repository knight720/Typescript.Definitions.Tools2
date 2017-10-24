﻿using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace Typescript.Definitions.Tools2.TsModels
{
    /// <summary>
    /// Represents a collection in the code model.
    /// </summary>
    [DebuggerDisplay("TsCollection - ItemsType={ItemsType}")]
    public class TsCollection : TsType
    {
        /// <summary>
        /// Gets or sets type of the items in the collection.
        /// </summary>
        /// <remarks>
        /// If the collection isn't strongly typed, the ItemsType property is initialized to TsType.Any.
        /// </remarks>
        public TsType ItemsType { get; set; }

        /// <summary>
        /// Gets or sets the dimension of the collection.
        /// </summary>
        public int Dimension { get; set; }

        /// <summary>
        /// Initializes a new instance of the TsCollection class with the specific CLR type.
        /// </summary>
        /// <param name="type">The CLR collection represented by this instance of the TsCollection.</param>
        public TsCollection(Type type)
            : base(type)
        {

            var enumerableType = GetEnumerableType(Type);
            if (enumerableType != null)
            {
                ItemsType = Create(enumerableType);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(Type))
            {
                ItemsType = Any;
            }
            else
            {
                throw new ArgumentException(string.Format("The type '{0}' is not collection.", Type.FullName));
            }

            Dimension = GetCollectionDimension(type);
        }

        private static int GetCollectionDimension(Type t)
        {
            Type enumerableUnderlying = null;

            if (t.IsArray)
            {
                return GetCollectionDimension(t.GetElementType()) + 1;
            }
            else if (t != typeof(string) && (enumerableUnderlying = GetEnumerableType(t)) != null)
            {
                return GetCollectionDimension(enumerableUnderlying) + 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
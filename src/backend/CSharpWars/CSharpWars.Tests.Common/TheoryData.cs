using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharpWars.Tests.Common
{
    public abstract class TheoryData : IEnumerable<Object[]>
    {
        readonly List<Object[]> _data = new List<Object[]>();

        protected void AddRow(params object[] values)
        {
            _data.Add(values);
        }

        public IEnumerator<Object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class TheoryData<T1, T2> : TheoryData
    {
        /// <summary>
        /// Adds data to the theory data set.
        /// </summary>
        /// <param name="p1">The first data value.</param>
        /// <param name="p2">The second data value.</param>
        public void Add(T1 p1, T2 p2)
        {
            AddRow(p1, p2);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace TradeStats.Extensions
{
    public static class LinqExtensions
    {
        public static decimal WeightedAverage<T>(this IEnumerable<T> records, Func<T, decimal> value, Func<T, decimal> weight)
        {
            if (records == null)
                throw new ArgumentNullException(nameof(records), $"{nameof(records)} is null.");

            int count = 0;
            decimal valueSum = 0;
            decimal weightSum = 0;

            foreach (var record in records)
            {
                count++;
                decimal recordWeight = weight(record);

                valueSum += value(record) * recordWeight;
                weightSum += recordWeight;
            }

            if (count == 0)
                throw new ArgumentException($"{nameof(records)} is empty.");

            if (count == 1)
                return value(records.Single());

            if (weightSum != 0)
                return valueSum / weightSum;

            else throw new DivideByZeroException($"Division of {valueSum} by zero.");
        }
    }
}

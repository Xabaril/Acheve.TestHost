using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Acheve.TestHost.Routing.Models;

public sealed class ParamWithSeveralTypes : IEquatable<ParamWithSeveralTypes>
{
    public string StringValue { get; set; }
    public int IntValue { get; set; }
    public double DoubleValue { get; set; }
    public bool BooleanValue { get; set; }
    public DateTime DateTimeValue { get; set; }

    public IEnumerable<string> StringValues { get; set; }
    public IEnumerable<int> IntValues { get; set; }
    public IEnumerable<double> DoubleValues { get; set; }
    public IEnumerable<bool> BooleanValues { get; set; }
    public IEnumerable<DateTime> DateTimeValues { get; set; }

    public static ParamWithSeveralTypes CreateRandom()
    {
        var random = new Random();

        var getRandomEnumerable = () => Enumerable.Repeat(0, random.Next(1, 10));
        var getRandomString = () => Guid.NewGuid().ToString();
        var getRandomInt = () => random.Next();
        var getRandomDouble = () => random.NextDouble();
        var getRandomBool = () => random.Next() % 2 == 0;
        var getRandomDateTime = () => new DateTime(
           random.Next(1900, 2900),
           random.Next(1, 12),
           random.Next(1, 28),
           random.Next(23),
           random.Next(59),
           random.Next(59),
           random.Next(999));

        return new()
        {
            StringValue = getRandomString(),
            IntValue = getRandomInt(),
            DoubleValue = getRandomDouble(),
            BooleanValue = getRandomBool(),
            DateTimeValue = getRandomDateTime(),

            StringValues = getRandomEnumerable().Select(_ => getRandomString()).ToList(),
            IntValues = getRandomEnumerable().Select(_ => getRandomInt()).ToList(),
            DoubleValues = getRandomEnumerable().Select(_ => getRandomDouble()).ToList(),
            BooleanValues = getRandomEnumerable().Select(_ => getRandomBool()).ToList(),
            DateTimeValues = getRandomEnumerable().Select(_ => getRandomDateTime()).ToList(),
        };
    }

    public bool Equals(ParamWithSeveralTypes other)
        => other != null
            && StringValue == other.StringValue
            && IntValue == other.IntValue
            && (DoubleValue - other.DoubleValue) < 0.0001
            && BooleanValue == other.BooleanValue
            && DateTimeValue == other.DateTimeValue
            && StringValues.SequenceEqual(other.StringValues)
            && IntValues.SequenceEqual(other.IntValues)
            && DoubleValues.SequenceEqual(other.DoubleValues)
            && BooleanValues.SequenceEqual(other.BooleanValues)
            && DateTimeValues.SequenceEqual(other.DateTimeValues);

    public override bool Equals(object obj)
        => Equals(obj as ParamWithSeveralTypes);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(StringValue);
        hash.Add(IntValue);
        hash.Add(DoubleValue);
        hash.Add(BooleanValue);
        hash.Add(DateTimeValue);
        hash.Add(StringValues);
        hash.Add(IntValues);
        hash.Add(DoubleValues);
        hash.Add(BooleanValues);
        hash.Add(DateTimeValues);
        return hash.ToHashCode();
    }
}

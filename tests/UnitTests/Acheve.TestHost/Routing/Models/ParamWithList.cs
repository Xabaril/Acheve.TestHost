using System;
using System.Collections.Generic;

namespace UnitTests.Acheve.TestHost.Routing.Models;

public class ParamWithSeveralTypes
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
}

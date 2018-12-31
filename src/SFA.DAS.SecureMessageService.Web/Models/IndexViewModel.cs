using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

public class IndexViewModel
{
    public string Message { get; set; }
    public Dictionary<int, string> TtlValue { get; set; }
    public IndexViewModel()
    {
        TtlValue = new Dictionary<int, string>()
        {
            { 1, "Hour" },
            { 24, "Day" },
            { 168, "Week" }
        };
    }
}
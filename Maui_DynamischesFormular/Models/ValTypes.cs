using System;
using System.Collections.Generic;

namespace Common.Models.ValTypes;

public static class ValTypes
{
    public enum ValType
    {
        NotValid,
        ValString,
        ValFloat,
        ValTimeSpan
    };
}
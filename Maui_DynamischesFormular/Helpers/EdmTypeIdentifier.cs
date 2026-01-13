﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Data.Tables;
using static Common.Models.EdmTypes.EdmTypes;

namespace Maui_DynamischesFormular.Helpers;

public class EdmTypeIdentifier
{

    public static EdmType Detect(TableEntity entity, string property)
    {
        if (property == null)
        {
            return EdmType.NotSupported;
        }

        try
        {
            string? result = entity.GetString(property);
            return EdmType.EdmString;
        }
        catch { }

        try
        {
            int? result = entity.GetInt32(property);
            return EdmType.EdmInt32;
        }
        catch { }

        try
        {
            DateTimeOffset? result = entity.GetDateTimeOffset(property);
            return EdmType.EdmDateTime;
        }
        catch { }

        try
        {
            double? result = entity.GetDouble(property);
            return EdmType.EdmDouble;
        }
        catch { }

        try
        {
            bool? result = entity.GetBoolean(property);
            return EdmType.EdmBoolean;
        }
        catch { }

        try
        {
            Guid? result = entity.GetGuid(property);
            return EdmType.EdmGuid;
        }
        catch { }

        try
        {
            long? result = entity.GetInt64(property);
            return EdmType.EdmInt64;
        }
        catch { }

        try
        {
            byte[]? result = entity.GetBinary(property);
            return EdmType.EdmBinary;
        }
        catch { }

        return EdmType.NotSupported;

    }
}



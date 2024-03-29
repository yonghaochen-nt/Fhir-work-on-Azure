﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System.Threading;

namespace Microsoft.Health.Fhir.Store.Utils
{
    public class CancelRequest
    {
        private long _value;

        internal bool IsSet => Interlocked.Read(ref _value) == 1;

        internal void Set()
        {
            Interlocked.Exchange(ref _value, 1);
        }
    }
}

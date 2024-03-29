﻿// -------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;

namespace ResourceProcessorNamespace
{
    internal class ResourceProcedureAdapter : ResourceAdapterBase<Procedure.Rootobject, ResourceProcedureAdapter.ProcedureSibling>
    {
        public override ProcedureSibling CreateOriginal(ResourceGroupProcessor processor, Procedure.Rootobject json)
        {
            return default;
        }

        public override string GetId(Procedure.Rootobject json)
        {
            return json.id;
        }

        public override string GetResourceType(Procedure.Rootobject json)
        {
            return json.resourceType;
        }

        protected override void IterateReferences(bool clone, ResourceGroupProcessor processor, Procedure.Rootobject originalJson, Procedure.Rootobject cloneJson, int refSiblingNumber, ref int refSiblingNumberLimit)
        {
            cloneJson.subject.reference = CloneOrLimit(clone, originalJson, originalJson.subject.reference, refSiblingNumber, ref refSiblingNumberLimit);
            if (cloneJson.encounter != null)
            {
                cloneJson.encounter.reference = CloneOrLimit(clone, originalJson, originalJson.encounter.reference, refSiblingNumber, ref refSiblingNumberLimit);
            }
        }

        public override ProcedureSibling CreateClone(ResourceGroupProcessor processor, Procedure.Rootobject originalJson, Procedure.Rootobject cloneJson, int refSiblingNumber)
        {
            cloneJson.id = Guid.NewGuid().ToString();
            int unused = int.MinValue;
            IterateReferences(true, processor, originalJson, cloneJson, refSiblingNumber, ref unused);
            return default;
        }

        public override bool ValidateResourceRefsAndSelect(ResourceGroupProcessor processor, Procedure.Rootobject json, out bool select)
        {
            select = true;
            if (json.subject == null)
            {
                processor.LogWarning(processor.GetResourceGroupDir(), ResourceGroupProcessor.ProcedureStr, json.id, "Property 'subject' is null!");
                select = false;
                return false;
            }

            if (!processor.ValidateResourceRefAndSelect(json.id, ResourceGroupProcessor.ProcedureStr, json.subject.reference, ResourceGroupProcessor.PatientStr, processor.Patients, processor.PatientIdsRemoved, ref select))
            {
                select = false;
                return false;
            }

            if (json.encounter != null &&
                !processor.ValidateResourceRefAndSelect(json.id, ResourceGroupProcessor.ProcedureStr, json.encounter.reference, ResourceGroupProcessor.EncounterStr, processor.Encounters, processor.EncounterIdsRemoved, ref select))
            {
                select = false;
                return false;
            }

            return true;
        }

        internal struct ProcedureSibling
        {
        }
    }
}

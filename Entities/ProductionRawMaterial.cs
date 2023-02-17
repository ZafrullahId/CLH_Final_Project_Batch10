using System;
using System.Collections.Generic;
using Dansnom.Contracts;

namespace Dansnom.Entities
{

    public class ProductionRawMaterial : AuditableEntity
    {
        public int RawMaterialId { get; set; }
        public  RawMaterial RawMaterial { get; set; }
        public int ProductionId { get; set; }
        public Production Production { get; set; }
    }
}
using MesModels.VatLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_project.Models.Interfaces
{
    public interface IVatLayerRepository
    {
        Task<VatRoot> VATReponse(string vat);
    }
}

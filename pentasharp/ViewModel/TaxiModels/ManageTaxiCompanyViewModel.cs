using pentasharp.ViewModel.TaxiModels;
using System.Collections.Generic;

namespace pentasharp.ViewModel.Taxi
{
    public class ManageTaxiCompanyViewModel
    {
        public List<TaxiCompanyViewModel> TaxiCompanies { get; set; } = new List<TaxiCompanyViewModel>();
    }
}

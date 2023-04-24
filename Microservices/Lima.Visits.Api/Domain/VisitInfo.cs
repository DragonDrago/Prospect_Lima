using System;
using System.Collections.Generic;

namespace Lima.Visits.Api.Domain
{
    public class VisitInfo
    {
        public int VisitId { get; set; }
        public DateTime DateCreate { get; set; }
        public int VisitStatusId { get; set; }
        public string VisitStatus { get; set; }
        public decimal TotalSum { get; set; }
        public decimal PrepaymentSum => TotalSum / 100 * PrepaymentPercent;
        public decimal PrepaymentPercent { get; set; }
        public string Comment { get; set; }
        public Medrep Medrep { get; set; }
        public Doctor Doctor { get; set; }
        public Organization Organization { get; set; }
        public Organization Distributor { get; set; }
        public List<Drug> TalkedAboutDrugs { get; set; }
        public List<SelectedDrug> Drugs { get; set; }

        public VisitInfo()
        {
            TalkedAboutDrugs = new List<Drug>();
            Drugs = new List<SelectedDrug> ();
        }
    }
}

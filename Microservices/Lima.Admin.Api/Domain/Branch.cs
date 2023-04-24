namespace Lima.Admin.Api.Domain
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int? ChiefUserId { get; set; }
        public string ChiefUserName { get; set; }
        public int DepartmentsCount { get; set; }
        public int EmployeeCount { get; set; }

    }
}

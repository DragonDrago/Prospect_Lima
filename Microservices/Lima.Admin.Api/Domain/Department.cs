namespace Lima.Admin.Api.Domain
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BranchId { get; set; }
        public int ChiefUserId { get; set; }
        public string ChiefUserName { get; set; }
        public int EmployeeCount { get; set; }
    }
}

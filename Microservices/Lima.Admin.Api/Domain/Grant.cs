namespace Lima.Admin.Api.Domain
{
    public class Grant
    {
        public int Id { get; set; }
        public string GrantName { get; set; }
        public string GroupName { get; set; }
        public bool Enable { get; set; }
    }
}

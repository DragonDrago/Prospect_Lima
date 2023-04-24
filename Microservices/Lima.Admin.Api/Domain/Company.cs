namespace Lima.Admin.Api.Domain
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Inn { get; set; }
        public string Uid { get; set; }
        public string Prefix { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int NotificationMinutes { get; set; }
        public int RemindMinutes { get; set; }
        public string Lang { get; set; }
    }
}

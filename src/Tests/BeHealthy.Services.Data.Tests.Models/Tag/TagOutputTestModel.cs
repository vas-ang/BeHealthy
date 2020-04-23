namespace BeHealthy.Services.Data.Tests.Models.Tag
{
    using BeHealthy.Services.Mapping;
    using BeHealthy.Data.Models;

    public class TagOutputTestModel : IMapFrom<Tag>
    {
        public string Name { get; set; }
    }
}

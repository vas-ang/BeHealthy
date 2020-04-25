namespace BeHealthy.Services.Data.Tests.Models.Tags
{
    using BeHealthy.Services.Mapping;
    using BeHealthy.Data.Models;

    public class TagOutputTestModel : IMapFrom<Tag>
    {
        public string Name { get; set; }
    }
}

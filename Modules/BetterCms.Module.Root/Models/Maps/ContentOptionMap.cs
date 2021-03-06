using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ContentOptionMap : EntityMapBase<ContentOption>
    {
        public ContentOptionMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("ContentOptions");

            Map(x => x.Key, "[Key]").Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Type).Not.Nullable();
            Map(x => x.DefaultValue).Length(MaxLength.Max).Nullable().LazyLoad();

            References(x => x.Content).Cascade.SaveUpdate().LazyLoad();            
        }
    }
}
using Xunit;

namespace Fawry.EntityFrameworkCore;

[CollectionDefinition(FawryTestConsts.CollectionDefinitionName)]
public class FawryEntityFrameworkCoreCollection : ICollectionFixture<FawryEntityFrameworkCoreFixture>
{

}

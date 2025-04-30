using Fawry.Samples;
using Xunit;

namespace Fawry.EntityFrameworkCore.Applications;

[Collection(FawryTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<FawryEntityFrameworkCoreTestModule>
{

}

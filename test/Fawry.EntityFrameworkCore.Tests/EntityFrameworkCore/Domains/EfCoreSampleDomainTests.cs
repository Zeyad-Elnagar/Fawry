using Fawry.Samples;
using Xunit;

namespace Fawry.EntityFrameworkCore.Domains;

[Collection(FawryTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<FawryEntityFrameworkCoreTestModule>
{

}

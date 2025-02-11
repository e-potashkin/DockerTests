using System.Collections.Generic;
using DockerTestsSample.Build.Infrastructure;
using DockerTestsSample.Build.Infrastructure.Common;
using DockerTestsSample.Build.Infrastructure.Extensions;
using Nuke.Common;

class Build : NukeBuild, IDefaultBuildFlow
{
    public string ServiceName => "DockerTestsSample";

    public ApplicationVersion Version => this.UseSemanticVersion(major: 1, minor: 0);

    public bool ExecuteIntegrationTests => true;

    public IReadOnlyList<DockerImageInfo> DockerImages { get; } = new[]
    {
        new DockerImageInfo(DockerImageName: "docker-tests-sample", DockerfileName: "Dockerfile"),
    };

    private Target RunBuild => _ => _
        .DependsOn<IDefaultBuildFlow>(x => x.Default)
        .Executes(() =>
        {
        });

    public static int Main()
        => Execute<Build>(x => x.RunBuild);
}

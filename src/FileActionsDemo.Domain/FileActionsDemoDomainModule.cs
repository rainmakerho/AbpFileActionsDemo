using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using FileActionsDemo.MultiTenancy;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.IdentityServer;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.BlobStoring.FileSystem;
using Volo.Abp.BlobStoring;

namespace FileActionsDemo;

[DependsOn(
    typeof(FileActionsDemoDomainSharedModule),
    typeof(AbpAuditLoggingDomainModule),
    typeof(AbpBackgroundJobsDomainModule),
    typeof(AbpFeatureManagementDomainModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpIdentityServerDomainModule),
    typeof(AbpPermissionManagementDomainIdentityServerModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpTenantManagementDomainModule),
    typeof(AbpEmailingModule)
)]
//[DependsOn(typeof(BlobStoringDatabaseDomainModule))]
    [DependsOn(typeof(AbpBlobStoringFileSystemModule))]
    public class FileActionsDemoDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = MultiTenancyConsts.IsEnabled;
        });

        //Configure<AbpBlobStoringOptions>(options =>
        //{
        //    options.Containers.ConfigureDefault(container =>
        //    {
        //        container.UseFileSystem(fileSystem =>
        //        {
        //            fileSystem.BasePath = @"C:\abp\FileActionDemo\src\FileActionsDemo.Files\";
        //        });
        //    });
        //});

#if DEBUG
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif
    }
}

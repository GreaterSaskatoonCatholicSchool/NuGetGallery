namespace NuGetGallery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPackageStatusKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Packages", "PackageStatusKey", c => c.Int());

            Sql(@"
DECLARE @PerIteration INT = 1000;
DECLARE @Delay VARCHAR(8) = '00:00:01';

DECLARE @DeletedUpdateCount INT = -1;
WHILE @DeletedUpdateCount <> 0
BEGIN
	UPDATE TOP (@PerIteration) dbo.Packages
	SET PackageStatusKey = 1
	WHERE Deleted = 1 AND PackageStatusKey IS NULL

	SELECT @DeletedUpdateCount = @@ROWCOUNT

	WAITFOR DELAY @Delay
END

DECLARE @AvailableUpdateCount INT = -1;
WHILE @AvailableUpdateCount <> 0
BEGIN
	UPDATE TOP (@PerIteration) dbo.Packages
	SET PackageStatusKey = 0
	WHERE PackageStatusKey IS NULL

	SELECT @AvailableUpdateCount = @@ROWCOUNT

	WAITFOR DELAY @Delay
END
");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Packages", "PackageStatusKey");
        }
    }
}
